public class SridWktEntry
{
    public required string Authority { get; set; }
    public required int Code { get; set; }
    public required string Wkt { get; set; }
}

public class SridWktProvider
{
    private List<SridWktEntry>? _cachedEntries = null;

    public SridWktProvider()
    {
        var assembly = System.Reflection.Assembly.GetExecutingAssembly();
        using var stream = assembly.GetManifestResourceStream("SridWktProvider.data.srid_wkt_mapping.csv");
        if (stream == null)
            throw new InvalidOperationException("Embedded resource 'srid_wkt_mapping.csv' not found.");

        LoadFromCsv(stream);
    }

    public void LoadFromCsv(Stream stream)
    {
        var entries = new List<SridWktEntry>();

        using (var reader = new StreamReader(stream))
        {
            string line;
            while ((line = reader.ReadLine()!) != null)
            {
                var entry = ReadLine(line);
                if (entry != null)
                    entries.Add(entry);
            }
        }

        _cachedEntries = entries.Count > 0 ? entries : null;
    }

    public void LoadFromCsv(string filePath)
    {
        var entries = new List<SridWktEntry>();

        foreach (var l in File.ReadLines(filePath))
        {
            var entry = ReadLine(l);
            if (entry != null)
                entries.Add(entry);
        }

        _cachedEntries = entries.Count > 0 ? entries : null;
    }

    private SridWktEntry? ReadLine(string line)
    {
        var parts = line.Split(',', 3, StringSplitOptions.None);
        if (parts.Length == 3 && int.TryParse(parts[1], out int code))
        {
            return new SridWktEntry
            {
                Authority = parts[0],
                Code = code,
                Wkt = parts[2]
            };
        }
        else
        {
            return null;
        }
    }

    public List<SridWktEntry> GetAllEntries()
    {
        if (_cachedEntries == null)
            throw new InvalidOperationException("SRID WKT data not loaded. Call LoadFromCsv(path) first.");
        return _cachedEntries;
    }

    public string? GetWkt(int srid)
    {
        if (_cachedEntries == null)
            throw new InvalidOperationException("SRID WKT data not loaded. Call LoadFromCsv(path) first.");
        var entry = _cachedEntries.FirstOrDefault(e => e.Code == srid);
        return entry?.Wkt;
    }

    public void AddCustomMapping(int srid, string wkt)
    {
        if (_cachedEntries == null)
            _cachedEntries = new List<SridWktEntry>();

        if (_cachedEntries.Any(e => e.Code == srid))
            throw new InvalidOperationException($"SRID {srid} already exists in the mapping.");

        _cachedEntries.Add(new SridWktEntry
        {
            Authority = "CUSTOM",
            Code = srid,
            Wkt = wkt
        });
    }
}
