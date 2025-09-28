public class SridWktProviderTests
{
    [Fact]
    public void GetWkt_ReturnsWktString()
    {
        var provider = new SridWktProvider();
        var wkt = provider.GetWkt(4326);
        Assert.False(string.IsNullOrEmpty(wkt));
    }

    [Fact]
    public void GetWkt_ReturnsNullIfNotFound()
    {
        var provider = new SridWktProvider();
        var entry = provider.GetWkt(1234567);
        Assert.Null(entry);
    }

    [Fact]
    public void AddCustomMapping_AddsEntrySuccessfully()
    {
        var provider = new SridWktProvider();
        provider.AddCustomMapping(9999, "FAKE_WKT");

        string? entry = provider.GetWkt(9999);
        Assert.NotNull(entry);
        Assert.Equal("FAKE_WKT", entry);
    }

    [Fact]
    public void AddCustomMapping_ThrowsIfSridExists()
    {
        var provider = new SridWktProvider();
        provider.AddCustomMapping(1000, "WKT1");

        Assert.Throws<InvalidOperationException>(() =>
            provider.AddCustomMapping(1000, "WKT2")
        );
    }

    [Fact]
    public void GetAllEntries_ReturnsEntries_WhenLoaded()
    {
        var provider = new SridWktProvider();
        var entries = provider.GetAllEntries();
        Assert.NotNull(entries);
        Assert.NotEmpty(entries);
    }

    [Fact]
    public void LoadFromCsv_OverwritesCachedEntries()
    {
        var provider = new SridWktProvider();

        // Add a custom mapping first
        provider.AddCustomMapping(9999, "CUSTOM_WKT");
        Assert.NotNull(provider.GetWkt(9999));

        // Create a temp CSV and load it
        var tempCsv = System.IO.Path.GetTempFileName();
        System.IO.File.WriteAllText(tempCsv, "Authority,Code,Wkt\nEPSG,4326,WKT_4326");
        provider.LoadFromCsv(tempCsv);

        // After loading, custom mapping should be gone, only CSV entries present
        Assert.Null(provider.GetWkt(9999));
        string? entry = provider.GetWkt(4326);
        Assert.NotNull(entry);
        Assert.Equal("WKT_4326", entry);
    }
}
