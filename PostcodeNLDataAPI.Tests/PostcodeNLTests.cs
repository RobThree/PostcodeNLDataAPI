using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using System.Threading.Tasks;
using System.IO;
using PostcodeNLDataAPI.Entities;

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
        public async Task ListAccountsAsync_ShouldParseCorrectly()
        {
            // Taken from documentation page 7
            var handler = new FakeResponseHandler()
                .AddJsonResponse(new Uri(PostcodeNL.DEFAULTURI, "subscription/accounts"), File.ReadAllText("responses/accounts.json"));

            var pcnl = new PostcodeNL("test", "test", handler);
            var accounts = await pcnl.ListAccountsAsync();

            Assert.AreEqual(2, accounts.Count());
        }

        [TestMethod]
        public async Task ListAccountsAsync_ProductFilter_ShouldBuildURICorrectly()
        {
            var handler = new FakeResponseHandler()
                .AddJsonResponse(new Uri(PostcodeNL.DEFAULTURI, "subscription/accounts?productCode=123"), File.ReadAllText("responses/accounts.json"));
            var pcnl = new PostcodeNL("test", "test", handler);

            var accounts = await pcnl.ListAccountsAsync("123");
            Assert.AreEqual(2, accounts.Count());
        }

        [TestMethod]
        public async Task GetAccountsAsync_ShouldParseCorrectly()
        {
            // Taken from documentation page 8
            var handler = new FakeResponseHandler()
                .AddJsonResponse(new Uri(PostcodeNL.DEFAULTURI, "subscription/accounts/12647"), File.ReadAllText("responses/account.json"));

            var pcnl = new PostcodeNL("test", "test", handler);
            var account = await pcnl.GetAccountAsync(12647);

            Assert.AreEqual(12647, account.Id);
        }

        [TestMethod]
        public async Task GetAccountsAsync_ShouldParseMinimalObjectCorrectly()
        {
            // Taken from documentation page 4; all nullable fields are not in the response/json
            var handler = new FakeResponseHandler()
                .AddJsonResponse(new Uri(PostcodeNL.DEFAULTURI, "subscription/accounts/123456789012"), File.ReadAllText("responses/minimalaccount.json"));

            var pcnl = new PostcodeNL("test", "test", handler);
            var account = await pcnl.GetAccountAsync(123456789012);

            Assert.AreEqual(123456789012, account.Id);
            Assert.AreEqual("X", account.ProductCode);
            Assert.AreEqual("Y", account.ProductName);
            Assert.AreEqual(new DateTime(2014, 3, 3), account.SubscriptionStart);
            Assert.AreEqual(new DateTime(2014, 3, 4), account.SubscriptionEnd);
            Assert.IsNull(account.LastDeliveryComplete);
            Assert.IsNull(account.LastDeliveryMutation);
            Assert.IsNull(account.NextDeliveryComplete);
            Assert.IsNull(account.NextDeliveryMutation);
        }

        [TestMethod]
        public async Task GetDeliveryAsync_ShouldParseMinimalObjectCorrectly()
        {
            // Taken from documentation page 4; all nullable fields are not in the response/json
            var handler = new FakeResponseHandler()
                .AddJsonResponse(new Uri(PostcodeNL.DEFAULTURI, "subscription/deliveries/abc"), File.ReadAllText("responses/minimaldelivery.json"));

            var pcnl = new PostcodeNL("test", "test", handler);
            var delivery = await pcnl.GetDeliveryAsync("abc");

            Assert.AreEqual("abc", delivery.Id);
            Assert.AreEqual(1, delivery.AccountId);
            Assert.AreEqual("X", delivery.ProductCode);
            Assert.AreEqual("Y", delivery.ProductName);
            Assert.AreEqual(DeliveryType.Mutation, delivery.DeliveryType);
            Assert.AreEqual(new DateTime(2015, 1, 1), delivery.DeliveryTarget);
            Assert.AreEqual(new Uri("https://retrieve.postcode.nl/abc"), delivery.DownloadUrl);
            Assert.AreEqual(5, delivery.DownloadCount);
        }

        [TestMethod]
        public async Task ListDeliveriesAsync_ShouldParseCorrectly()
        {
            // Taken from documentation page 9
            var handler = new FakeResponseHandler()
                .AddJsonResponse(new Uri(PostcodeNL.DEFAULTURI, "subscription/deliveries?deliveryType=mutation&after=20141229"), File.ReadAllText("responses/deliveries.json"));

            var pcnl = new PostcodeNL("test", "test", handler);
            var deliveries = await pcnl.ListDeliveriesAsync(new DeliveriesQuery { DeliveryType = DeliveryType.Mutation, After = new DateTime(2014, 12, 29) });

            Assert.AreEqual(2, deliveries.Count());
        }

        [TestMethod]
        public async Task GetDeliveryAsync_ShouldParseCorrectly()
        {
            // Taken from documentation page 10
            var handler = new FakeResponseHandler()
                .AddJsonResponse(new Uri(PostcodeNL.DEFAULTURI, "subscription/deliveries/a4ddb036f28f232b40be469bb692cc88"), File.ReadAllText("responses/delivery.json"));

            var pcnl = new PostcodeNL("test", "test", handler);
            var delivery = await pcnl.GetDeliveryAsync("a4ddb036f28f232b40be469bb692cc88");

            Assert.AreEqual("a4ddb036f28f232b40be469bb692cc88", delivery.Id);
        }

        [TestMethod]
        public async Task ErrorResponse_ShouldParse_AndBeThrown()
        {
            // Taken from documentation page 5
            var handler = new FakeResponseHandler()
                .AddJsonResponse(new Uri(PostcodeNL.DEFAULTURI, "subscription/accounts/1234567"), File.ReadAllText("responses/error.json"), HttpStatusCode.NotFound);

            var pcnl = new PostcodeNL("test", "test", handler);

            try
            {
                var accounts = await pcnl.GetAccountAsync(1234567);
                Assert.Fail("Exception should have been thrown");
            }
            catch (PostcodeNLException ex)
            {
                Assert.AreEqual("Cannot find subscription with id #1234567.", ex.Message);
                Assert.AreEqual("PostcodeNl_Controller_Subscription_UnknownSubscriptionException", ex.ExceptionId);
                Assert.AreEqual("EQJKVW", ex.RequestId);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_ShouldThrow_OnNullCredentials()
        {
            var pcnl = new PostcodeNL(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_ShouldThrow_OnNullUri()
        {
            var pcnl = new PostcodeNL("test", "test", (Uri)null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task GetDeliveryAsync_ShouldThrow_OnNullDeliveryId()
        {
            var pcnl = new PostcodeNL("test", "test");
            await pcnl.GetDeliveryAsync(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task ListDeliveriesAsync_ShouldThrow_OnNullQuery()
        {
            var pcnl = new PostcodeNL("test", "test");
            await pcnl.ListDeliveriesAsync(null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task ListDeliveriesAsync_ShouldThrow_OnNonQuery()
        {
            var pcnl = new PostcodeNL("test", "test");
            await pcnl.ListDeliveriesAsync(new DeliveriesQuery());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task DownloadDeliveryAsync_ShouldThrow_OnNullDelivery()
        {
            var pcnl = new PostcodeNL("test", "test");
            await pcnl.DownloadDeliveryAsync(null, "test");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task DownloadDeliveryAsync_ShouldThrow_OnNullFilename()
        {
            var pcnl = new PostcodeNL("test", "test");
            await pcnl.DownloadDeliveryAsync(new Delivery { DownloadUrl = new Uri("http://google.com") }, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task DownloadFileAsync_ShouldThrow_OnNullUri()
        {
            var pcnl = new PostcodeNL("test", "test");
            await pcnl.DownloadFileAsync(null, "test");
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task DownloadFileAsync_ShouldThrow_OnNullFilename()
        {
            var pcnl = new PostcodeNL("test", "test");
            await pcnl.DownloadFileAsync(new Uri("http://google.com"), null);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task PostcodeNL_ShouldThrow_OnInvalidRelativeUri()
        {
            var pcnl = new PostcodeNL("test", "test", new Uri("invalid/foo", UriKind.Relative));
            await pcnl.ListAccountsAsync();
        }

        [TestMethod]
        public async Task PostcodeNL_ShouldBuild_DeliveriesQuery_Correctly()
        {
            // We add no items to our FakeResponseHandler, resulting in everything throwing a 404
            // which is what we want; the exception will contain the requested URI and so we can
            // check if everything we expect in it is present
            var pcnl = new PostcodeNL("test", "test", new FakeResponseHandler());

            try
            {
                await pcnl.ListDeliveriesAsync(new DeliveriesQuery
                {
                    AccountId = 1234567890,
                    After = new DateTime(2077, 1, 1),
                    From = new DateTime(2088, 12, 31),
                    To = new DateTime(2099, 9, 10),
                    DeliveryType = DeliveryType.Complete
                });
            }
            catch (PostcodeNLException ex)
            {
                var absuri = ex.Uri.AbsoluteUri;

                Assert.IsTrue(absuri.Contains("accountId=1234567890"));
                Assert.IsTrue(absuri.Contains("deliveryType=complete"));    // Note that the enum is explicitly cast to lowercase
                Assert.IsTrue(absuri.Contains("from=20881231"));            // Ensure yyyyMMdd format
                Assert.IsTrue(absuri.Contains("to=2099091"));               // Ensure yyyyMMdd format
                Assert.IsTrue(absuri.Contains("after=20770101"));           // Ensure yyyyMMdd format
            }
        }

        [TestMethod]
        public async Task PostcodeNL_ShouldEncodeUriValues_Correctly()
        {
            // We add no items to our FakeResponseHandler, resulting in everything throwing a 404
            // which is what we want; the exception will contain the requested URI and so we can
            // check if everything we expect in it is present
            var pcnl = new PostcodeNL("test", "test", new FakeResponseHandler());

            try
            {
                await pcnl.GetDeliveryAsync("some@test?foo=bar&baz=foobar");
            }
            catch (PostcodeNLException ex)
            {
                var absuri = ex.Uri.AbsoluteUri;

                Assert.IsTrue(absuri.EndsWith("/some%40test%3Ffoo%3Dbar%26baz%3Dfoobar"));
            }
        }

        [TestMethod]
        public async Task PostcodeNL_ShouldOmit_UnspecifiedDeliveriesQueryValues_Correctly()
        {
            // We add no items to our FakeResponseHandler, resulting in everything throwing a 404
            // which is what we want; the exception will contain the requested URI and so we can
            // check if everything we expect (or DON'T expect) in it is (not) present
            var pcnl = new PostcodeNL("test", "test", new FakeResponseHandler());

            try
            {
                await pcnl.ListDeliveriesAsync(new DeliveriesQuery
                {
                    DeliveryType = DeliveryType.Complete
                });
            }
            catch (PostcodeNLException ex)
            {
                var absuri = ex.Uri.AbsoluteUri;

                Assert.IsTrue(absuri.EndsWith("?deliveryType=complete"));   // This should be the only queryvalue present in the uri
            }
        }
    }
}
