public static class CsvDataMapper
{
    public static CsvMappingConfiguration<CsvDataExample> GetCsvMappingConfiguration()
    {
        var config = new CsvMappingConfiguration<CsvDataExample>();

        config.Map(
            "Identifier",
            (currentData, value) =>
            {
                _ = int.TryParse(value, out var id);
                currentData.ID = id;
            }
        );

        config.Map("Fullname", (currentData, value) => currentData.Name = value);

        config.Map(
            "Available",
            (currentData, value) => currentData.Available = string.Equals(value, "true")
        );

        return config;
    }
}
