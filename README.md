<p align="center">
  <picture>
    <source media="(prefers-color-scheme: dark)" srcset="assets/branding/open-barcode-logo-dark.svg">
    <img src="assets/branding/open-barcode-logo.svg" alt="Open Barcode by Floms" width="420">
  </picture>
</p>

<p align="center"><em>Barcodes, made open.</em></p>

# Open Barcode for C#

Barcode library for C# that creates Code 39, EAN-13 and UPC-A barcodes. It returns the bar and space modules and has no runtime dependencies. Drawing the modules is up to your application.

## Install

Install the package from NuGet:

```sh
dotnet add package OpenBarcode
```

The package targets .NET Standard 2.0 and .NET 8.

## Usage

```csharp
using OpenBarcode.Linear;

LinearBarcode ean = new EAN("400638133393");
string code = ean.Code();          // "4006381333931" (check digit added)
int[] modules = ean.Barcode();     // 95 values: 1 = bar, 0 = space

LinearBarcode upc = new UPC("036000291452");     // check digit verified
LinearBarcode code39 = new Code39("Hello world"); // Code() is "HELLO WORLD"
```

EAN and UPC accept digits only. They take the code with or without its check digit. If you include a check digit that does not match, the constructor throws `ArgumentException`.

Code 39 trims the input, converts it to upper case, and throws `ArgumentException` for characters it cannot encode, including `*`.

Each call to `Barcode()` returns a new array, so changing the array does not change the barcode.

## Testing

The tests use xUnit. They need the .NET 8 SDK:

```sh
dotnet test tests/OpenBarcode.Tests/OpenBarcode.Tests.csproj
```

## Other languages

Open Barcode is available in several languages:

- Java: https://github.com/openbarcode/java
- C#: https://github.com/openbarcode/csharp
- Python: https://github.com/openbarcode/python
- PHP: https://github.com/openbarcode/php
- JavaScript: https://github.com/openbarcode/javascript

Project homepage: https://openbarcode.dev

This repository was split from the original multi-language project at
https://github.com/floms/Open-Barcode. The commit history was preserved
with git-filter-repo; commit hashes differ from the original.

Copyright 2013 Floms, LLC (Yoel Nunez)

Copyright 2026 Floms, Inc.

Licensed under the Apache License, Version 2.0. See LICENSE.md for details.
