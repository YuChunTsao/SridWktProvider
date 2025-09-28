# SridWktProvider

[![Release](https://github.com/YuChunTsao/SridWktProvider/actions/workflows/release.yml/badge.svg)](https://github.com/YuChunTsao/SridWktProvider/actions/workflows/release.yml)
[![Test](https://github.com/YuChunTsao/SridWktProvider/actions/workflows/test.yml/badge.svg)](https://github.com/YuChunTsao/SridWktProvider/actions/workflows/test.yml)
![GitHub License](https://img.shields.io/github/license/YuChunTsao/SridWktProvider)
![NuGet Version](https://img.shields.io/nuget/v/SridWktProvider)

SridWktProvider is a lightweight C# library for loading and querying EPSG SRID to WKT1 mappings, using data exported from the [PROJ](https://github.com/OSGeo/PROJ) database. It supports reading mappings from CSV files or streams and provides simple APIs for retrieving WKT1 definitions by SRID code.

## Installation

```bash
dotnet add package SridWktProvider
```

## Usage

```csharp
var provider = new SridWktProvider();
string? wkt = provider.GetWkt(4326);
```

If the SRID is found, `wkt` will contain the corresponding WKT string; otherwise, it will be `null`.

## Data Source

This package only provides EPSG WKT1 information extracted from the PROJ database.
In the `scripts` folder, you will find a bash script that generates a CSV file containing the SRID to WKT mappings.

Run the script as follows:

```bash
cd SridWktProvider
sh ./scripts/generate_srid_wkt_csv.sh
```

> - You need to have PROJ installed and available in your PATH.
> - I exported the data using PROJ version 9.7.0.

## Testing

```bash
dotnet test
```

## Create NuGet package

```bash
dotnet pack -c Release
```

## License

MIT
