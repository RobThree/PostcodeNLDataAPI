using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using System.Threading.Tasks;
using System.IO;

namespace PostcodeNLDataAPI.Tests
{
    [TestClass]
    public class PostcodeNLTests
    {
        [TestMethod]
        [ExpectedException(typeof(PostcodeNLException))]
        public async Task IncorrectCredentials_Should_Throw()
        {
            var handler = new FakeResponseHandler()
                .AddEmptyResponse(new Uri(PostcodeNL.DEFAULTURI, "subscription/accounts"), HttpStatusCode.Unauthorized);
            
            var target = new PostcodeNL("unauth", "testunauth", handler);
            await target.ListAccountsAsync();
        }

        [TestMethod]
        public async Task CorrectCredentials_Should_Throw()
        {
            var handler = new FakeResponseHandler()
                .AddJsonResponse(new Uri(PostcodeNL.DEFAULTURI, "subscription/accounts"), File.ReadAllText("responses/accounts.json"));

            var pcnl = new PostcodeNL("test", "test", handler);
            var accounts = await pcnl.ListAccountsAsync();

            Assert.AreEqual(2, accounts.Count());
        }

    }
}
