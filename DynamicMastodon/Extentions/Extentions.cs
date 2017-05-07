using DynamicMastodon.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DynamicMastodon.Extentions
{
    public static class Extentions
    {
        private static IEnumerable<FastMember.Member> GetMember(this object obj, FastMember.TypeAccessor type)
        {
            return type
                .GetMembers()
                .Where(x => type[obj, x.Name] != null);
        }

        public static Dictionary<string, string> ToUrlSegment(this object obj)
        {
            var type = FastMember.TypeAccessor.Create(obj.GetType());
            return obj.GetMember(type)
                .ToDictionary(x => x.Name, x => type[obj, x.Name].ToString());
        }

        public static Dictionary<string, object> ToDictionary(this object obj)
        {
            var type = FastMember.TypeAccessor.Create(obj.GetType());
            return type
                .GetMembers()
                .ToDictionary(x => x.Name, x => type[obj, x.Name]);
        }

        private static readonly string MAXID = "max_id";
        private static readonly string SINCEID = "since_id";
        private static readonly string OPERATOR_NEXT = "next";
        private static readonly string OPERATOR_PREV = "prev";
        private static readonly string LINKPATTERN = "<(?<url>.*)>; *rel=\\\"(?<operator>.*)\\\"";

        public static (int? next, int? prev) GetHeader(this string header)
        {
            var headerGroup = GetUrlsOperation(header);
            int? next = null;
            int? prev = null;

            string tmp;
            if (headerGroup.TryGetValue(OPERATOR_NEXT, out tmp))
            {
                var queries = QueryParser(tmp);
                next = GetNextId(queries);
            }
            if (headerGroup.TryGetValue(OPERATOR_PREV, out tmp))
            {
                var queries = QueryParser(tmp);
                prev = GetPrevId(queries);
            }

            return (next, prev);
        }

        public static IDictionary<string, string> GetUrlsOperation(string header)
        {
            var re = new Regex(LINKPATTERN, RegexOptions.IgnoreCase);
            var headers = header.Split(',').Select(s => s.Trim());
            var headerPair = new Dictionary<string, string>();

            foreach (var h in headers)
            {
                var match = re.Match(h);
                if (!match.Success)
                {
                    continue;
                }

                headerPair.Add(match.Groups["operator"].ToString().ToLower(), match.Groups["url"].ToString().ToLower());
            }

            return headerPair;
        }

        private static IDictionary<string, string> QueryParser(string header)
            => new string(header.SkipWhile(c => !c.Equals('?')).Skip(1).ToArray())
                .Split('&').Select(q => q.Split('=')).Where(kvp => kvp.Length == 2).ToDictionary(k => k[0], v => v[1]);

        private static int? GetNextId(IDictionary<string, string> header)
        {
            string id;
            if (header.TryGetValue(MAXID, out id))
            {
                return int.Parse(id);
            }

            return null;
        }

        private static int? GetPrevId(IDictionary<string, string> header)
        {
            string id;
            if (header.TryGetValue(SINCEID, out id))
            {
                return int.Parse(id);
            }

            return null;
        }

        public static string ToParam(this Visibility visibility)
        {
            switch(visibility)
            {
                case Visibility.Direct:
                    return "direct";
                case Visibility.Private:
                    return "private";
                case Visibility.Public:
                    return "public";
                case Visibility.Unlisted:
                    return "unlisted";
                default:
                    throw new ArgumentException(visibility.ToString());
            }
        }

        public static string ToParam(this Scope scope)
        {
            switch (scope)
            {
                case Scope.Read:
                    return "read";
                case Scope.Write:
                    return "write";
                case Scope.Follow:
                    return "follow";
                default:
                    throw new ArgumentException(scope.ToString());
            }
        }
    }
}
