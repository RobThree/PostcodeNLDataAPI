# ![Logo](https://raw.githubusercontent.com/RobThree/PostcodeNLDataAPI/master/postcode-logo-square.png) Postcode.nl DATA API
(Unofficial) A .Net Postcode.nl DATA API implementation (implemented version `1.0r83` of the official documentation).

# Usage

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
