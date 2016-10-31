# ![Logo](https://raw.githubusercontent.com/RobThree/PostcodeNLDataAPI/master/postcode-logo-trimmed.png) Postcode.nl DATA API
An (**unofficial**) .Net Postcode.nl DATA API implementation (implemented version `1.0r83` of the official documentation). Available as [NuGet package](https://www.nuget.org/packages/PostcodeNLDataAPI).

# Usage

Install the nuget package:

```cmd
Install-Package PostcodeNLDataAPI
```

Add a `using` statement:

```c#
using PostcodeNLDataAPI;
```

Then you can use the `PostcodeNL` object:

```c#
// Create a new instance of the PostcodeNL class; this will be used
// to access the Postcode.nl DATA API.
var pcnl = new PostcodeNL("<your_key_here>", "<your_secret_here>");

// Now call any of the following methods:

// Retrieve all accounts (optionally filtered)
var accounts = await pcnl.ListAccountsAsync();        // Note you can also pass a product-filter argument;
                                                      // for example: pcnl.ListAccountsAsync("NP-MySQL-A-M")
// Retrieve specific account by Id
var account = await pcnl.GetAccountAsync(1234567890);

// Retrieve all deliveries based on filter
var deliveries = await pcnl.ListDeliveriesAsync(
    new DeliveriesQuery { DeliveryType = DeliveryType.Complete }  // Specify filter
  );

// Retrieve specific delivery
var delivery = await pcnl.GetDeliveryAsync("9b2b...dc89");

// Download specific (previously retrieved) delivery to specified filename
pcnl.DownloadDeliveryAsync(delivery, @"D:\foo\bar\file.zip");
```

Couldn't be more simple.

## PostcodeNLDownloader

Also in this repository is an "example application" which demonstrates a simple use-case of the PostcodeNLDataAPI. It is a Command Line Interface (CLI) utility to download the latest version of a database. You can download all your subscriptions or just the one specified (using `-p`) and you can choose between 'mutation' and 'complete'. Do keep in mind that this application is for demonstration purposes only an may change at any given time. If you run the application from the commandline you'll see it's usage explained a little more detailed:

```
Usage: PostcodeNLDownloader [arguments] [options]

Arguments:
  key           Your postcode.nl key (or username)
  secret        Your postcode.nl secret (or password)
  path          The destination download directory
  deliverytype  Either 'mutation' or 'complete'

Options:
  -p  | --productcode  Use to download specific productcode
  -o  | --overwrite    (Force) Overwrite existing files
  -vv | --verbose      Verbose
  -h  | --help         Show help information
  -v  | --version      Show version information
```

## Test Suite

[![Build status](https://ci.appveyor.com/api/projects/status/iq9vaikslv4ru67k?svg=true)](https://ci.appveyor.com/project/RobIII/postcodenldataapi)
                 
