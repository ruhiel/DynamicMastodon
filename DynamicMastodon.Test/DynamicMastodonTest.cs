using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using Codeplex.Data;
using DynamicMastodon.Extentions;
using System.Collections;

namespace DynamicMastodon.Test
{
    [TestClass]
    public class DynamicMastodonTest
    {
        private static string _AccessToken;
        private static string _Host;
        private static int _TestUserID;

        [AssemblyInitialize]
        public static void AssemblyInit(TestContext context)
        {
            // アセンブリ内のすべてのテストが実行される前に、アセンブリによって取得されるリソースを割り当てるために使用されるコードを含むメソッドを識別します。 
            Trace.WriteLine("AssemblyInit " + context.TestName);

            _AccessToken = Environment.GetEnvironmentVariable("TestAuthCode");

            _Host = Environment.GetEnvironmentVariable("TestHost");

            _TestUserID = int.Parse(Environment.GetEnvironmentVariable("TestUserID"));
        }

        [TestMethod]
        public async Task TestMethod1()
        {
            var client = new DynamicMastodonClient(_Host, _AccessToken);
            var id = _TestUserID;

            var result = await client.Account(id);

        }

        [TestMethod]
        public async Task TestMethod2()
        {
            var client = new DynamicMastodonClient(_Host, _AccessToken);
            var result = await client.PublicTimeline(limit: 20);

            Assert.AreEqual(20, ((object[])result.Statuses).Length);

            result = await client.HomeTimeline(limit: 20);

            Assert.AreEqual(20, ((object[])result.Statuses).Length);

            result = await client.HashtagTimeline(hashtag: "超会議", limit: 20);

            Assert.AreEqual(20, ((object[])result.Statuses).Length);

        }

        [TestMethod]
        public async Task TestMethod3()
        {
            var client = new DynamicMastodonClient(_Host, _AccessToken);
            var result = await client.Search("超会議");
            var statuses = (object[])result.hashtags;

            Assert.IsTrue(statuses.Any());
        }
    }
}
