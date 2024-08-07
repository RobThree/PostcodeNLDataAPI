﻿using Microsoft.Extensions.CommandLineUtils;
using PostcodeNLDataAPI;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace PostcodeNLDownloader;

internal class Program
{
    private static int Main(string[] args)
    {
        AppDomain.CurrentDomain.UnhandledException += (s, e) =>
        {
            if (e.ExceptionObject is Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Environment.Exit(-1);
        };

        var app = new CommandLineApplication
        {
            Name = "PostcodeNLDownloader",
            FullName = "Postcode.nl downloader",
            Description = "Downloads files to build your postcode.nl databases (complete and/or mutation)",
        };

        var keyArgument = app.Argument("key", "Your postcode.nl key (or username)", false);
        var secretArgument = app.Argument("secret", "Your postcode.nl secret (or password)", false);
        var pathArgument = app.Argument("path", "The destination download directory", false);
        var deliveryTypeArgument = app.Argument("deliverytype", "Either 'mutation' or 'complete'", false);

        var productfilterOption = app.Option("-p  | --productcode", "Use to download specific productcode", CommandOptionType.SingleValue);
        var overwriteOption = app.Option("-o  | --overwrite", "(Force) Overwrite existing files", CommandOptionType.NoValue);

        var verboseOption = app.Option("-vv | --verbose", "Verbose", CommandOptionType.NoValue);
        var helpOption = app.HelpOption("-h  | --help");
        var versionOption = app.VersionOption("-v  | --version", Assembly.GetExecutingAssembly().GetName().Version.ToString(), Assembly.GetExecutingAssembly().GetName().Version.ToString());

        app.OnExecute(async () =>
        {
            if (helpOption.HasValue() || args.Length == 0)
            {
                app.ShowHelp();
                return 1;
            }

            if (versionOption.HasValue())
            {
                app.ShowVersion();
                return 1;
            }

            if (Enum.TryParse<DeliveryType>(deliveryTypeArgument.Value, true, out var dt))
            {
                await DownloadFiles(
                    keyArgument.Value,
                    secretArgument.Value,
                    pathArgument.Value,
                    dt,
                    productfilterOption.Value(),
                    overwriteOption.HasValue(),
                    verboseOption.HasValue()
                );
            }
            else
            {
                Console.WriteLine($"Invalid deliverytype '{deliveryTypeArgument.Value}'. Valid values: '{string.Join("','", Enum.GetNames(typeof(DeliveryType)))}'");
            }


            return 0;
        });
        return app.Execute(args);
    }

    private static async Task DownloadFiles(string key, string secret, string path, DeliveryType deliveryType, string productcode, bool overwrite, bool verbose)
    {
        var pcnl = new PostcodeNL(key, secret);
        Log(verbose, "Retrieving accounts...");
        foreach (var acc in await pcnl.ListAccountsAsync(productcode))
        {
            Log(verbose, $"Getting latest for '{acc.ProductName}'...");
            var latest = (await pcnl.ListDeliveriesAsync(new DeliveriesQuery
            {
                AccountId = acc.Id,
                DeliveryType = deliveryType
            })).OrderByDescending(d => d.DeliveryTarget)
               .FirstOrDefault();

            if (latest != null)
            {
                var dest = Path.Combine(path, $"{latest.ProductCode}_{latest.DeliveryTarget:yyyyMMdd}_{latest.DeliveryType}.zip".ToLowerInvariant());
                if (!File.Exists(dest) || overwrite)
                {
                    Log(verbose, $"Downloading file {dest}");
                    await pcnl.DownloadDeliveryAsync(latest, dest);
                }
                else
                {
                    Log(verbose, $"Skipping file {dest}");
                }
            }
        }
    }

    private static void Log(bool verbose, string message)
    {
        if (verbose)
        {
            Console.WriteLine(message);
        }
    }
}
