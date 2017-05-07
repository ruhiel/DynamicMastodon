using DynamicMastodon.Extentions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicMastodon.Core
{
    public class OAuthScope
    {
        private Scope[] scopes;

        private OAuthScope(Scope[] scopes)
        {
            this.scopes = scopes;
        }

        public static OAuthScope of(params Scope[] scopes)
        {
            return new OAuthScope(scopes);
        }

        public override string ToString()
        {
            return string.Join(" ", scopes.Distinct().Select(x => x.ToParam()));
        }
    }
}
