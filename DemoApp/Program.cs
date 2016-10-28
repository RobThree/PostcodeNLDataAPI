using PostcodeNLDataAPI;
using PostcodeNLDataAPI.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DemoApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Task t = MainAsync(args);
            t.Wait();

            Console.WriteLine("Done. Press any key");
            Console.ReadKey();
        }

        static async Task MainAsync(string[] args)
        {
            var pcnl = new PostcodeNL("<your_key_here>", "<your_secret_here>");

            await ShowAccounts(pcnl);
            await ShowDeliveries(pcnl);
        }

        private static async Task ShowAccounts(PostcodeNL pcnl)
        {
            // Retrieve all accounts using/demonstrating ListAccountsAsync() method
            Console.WriteLine("ACCOUNTS:");
            var accounts = await pcnl.ListAccountsAsync();
            foreach (var acc in accounts)
                PrintAccount(acc);
            Console.WriteLine();

            // Retrieve single accounts using/demonstrating GetAccountAsync() method
            Console.WriteLine("SINGLE ACCOUNT:");
            PrintAccount(await pcnl.GetAccountAsync(accounts.First().Id));
            Console.WriteLine();
        }

        private static void PrintAccount(Account acc)
        {
            Console.WriteLine($"{acc.Id,-10} {acc.ProductCode,-20} {acc.ProductName}");
            Console.WriteLine($"\tSubscription start     : {acc.SubscriptionStart.ToString("O")}");
            Console.WriteLine($"\t               end     : {acc.SubscriptionEnd.ToString("O")}");
            Console.WriteLine($"\tLast delivery complete : {acc.LastDeliveryComplete?.ToString("O")}");
            Console.WriteLine($"\tLast delivery mutation : {acc.LastDeliveryMutation?.ToString("O")}");
            Console.WriteLine($"\tNext delivery complete : {acc.NextDeliveryComplete?.ToString("O")}");
            Console.WriteLine($"\tNext delivery mutation : {acc.NextDeliveryMutation?.ToString("O")}");
        }

        private static async Task ShowDeliveries(PostcodeNL pcnl)
        {
            // Retrieve all deliveries using/demonstrating ListDeliveriesAsync() method
            Console.WriteLine("DELIVERIES:");
            var deliveries = await pcnl.ListDeliveriesAsync(new DeliveriesQuery { DeliveryType = DeliveryType.Complete });
            foreach (var del in deliveries)
                PrintDelivery(del);
            Console.WriteLine();

            // Retrieve single delivery using/demonstrating GetDeliveryAsync() method
            Console.WriteLine("SINGLE DELIVERY:");
            PrintDelivery(await pcnl.GetDeliveryAsync(deliveries.First().Id));
            Console.WriteLine();
        }

        private static void PrintDelivery(Delivery del)
        {
            Console.WriteLine($"{del.Id,-33} AccountId: {del.AccountId,-10}\n\tType: {del.DeliveryType,-10} Code: {del.ProductCode,-20} Name: {del.ProductName}");

            Console.WriteLine($"\tDelivery source : {del.DeliverySource?.ToString("O")}");
            Console.WriteLine($"\tDelivery target : {del.DeliveryTarget.ToString("O")}");
            Console.WriteLine($"\tDownload URI    : {del.DownloadUrl}");
            Console.WriteLine($"\tDownloads       : {del.DownloadCount}");
            Console.WriteLine();
        }
    }
}
