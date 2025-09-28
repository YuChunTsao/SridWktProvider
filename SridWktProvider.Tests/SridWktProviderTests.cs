public class SridWktProviderTests
{
    static SridWktProviderTests()
    {
        SridWktProvider.Init();
    }

    [Fact]
    public void GetWkt_ReturnsWktString()
    {
        var wkt = SridWktProvider.GetWkt(4326);
        Assert.False(string.IsNullOrEmpty(wkt));
    }

    [Fact]
    public void GetWkt_ReturnsNullIfNotFound()
    {
        var entry = SridWktProvider.GetWkt(1234567);
        Assert.Null(entry);
    }

    [Fact]
    public void AddCustomMapping_AddsEntrySuccessfully()
    {
        SridWktProvider.AddCustomMapping(9999, "FAKE_WKT");

        string? entry = SridWktProvider.GetWkt(9999);
        Assert.NotNull(entry);
        Assert.Equal("FAKE_WKT", entry);
    }

    [Fact]
    public void AddCustomMapping_ThrowsIfSridExists()
    {
        SridWktProvider.AddCustomMapping(1000, "WKT1");

        Assert.Throws<InvalidOperationException>(() =>
            SridWktProvider.AddCustomMapping(1000, "WKT2")
        );
    }

    [Fact]
    public void GetAllEntries_ReturnsEntries_WhenLoaded()
    {
        var entries = SridWktProvider.GetAllEntries();
        Assert.NotNull(entries);
        Assert.NotEmpty(entries);
    }

    [Fact]
    public void LoadFromCsv_OverwritesCachedEntries()
    {
        // Add a custom mapping first
        SridWktProvider.AddCustomMapping(9999, "CUSTOM_WKT");
        Assert.NotNull(SridWktProvider.GetWkt(9999));

        // Create a temp CSV and load it
        var tempCsv = System.IO.Path.GetTempFileName();
        System.IO.File.WriteAllText(tempCsv, "Authority,Code,Wkt\nEPSG,4326,WKT_4326");
        SridWktProvider.LoadFromCsv(tempCsv);

        // After loading, custom mapping should be gone, only CSV entries present
        Assert.Null(SridWktProvider.GetWkt(9999));
        string? entry = SridWktProvider.GetWkt(4326);
        Assert.NotNull(entry);
        Assert.Equal("WKT_4326", entry);

        SridWktProvider.Init();  // Re-initialize to restore original data
    }
}
