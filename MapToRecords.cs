public static class MapToRecords
{
    /// <summary>
    /// Every row is exactly one object in return list
    /// </summary>
    /// <param name="csvData">csv string to parse</param>
    /// <param name="delimeter">field delimeter in csv string</param>
    /// <param name="mappingConfig">configuration for csv file</param>
    /// <typeparam name="T">object to map data to</typeparam>
    /// <returns>IEnumerable of desired object</returns>
    public static IEnumerable<T> MapCsvToRecords<T>(
        string csvData,
        char delimeter,
        CsvMappingConfiguration<T> mappingConfig
    )
        where T : class, new()
    {
        var lines = csvData.Replace("\r", "").Split('\n');
        var returnList = new List<T>();
        if (lines.Length < 2)
        {
            return returnList;
        }

        var headers = lines[0].Split(delimeter);
        for (int i = 1; i < lines.Length; i++)
        {
            var values = lines[i].Split(delimeter);

            var currentRecord = new T();

            for (int j = 0; j < headers.Length; j++)
            {
                if (mappingConfig.Mapping.TryGetValue(headers[j], out var mappingAction))
                {
                    if (values.Length <= j)
                    {
                        break;
                    }

                    if (values[j] is not null)
                    {
                        mappingAction(currentRecord, values[j]);
                    }
                }
            }
            returnList.Add(currentRecord);
        }

        return returnList;
    }

    /// <summary>
    /// Map whole csv to 1 object
    /// </summary>
    /// <param name="csvData">csv string to parse</param>
    /// <param name="delimeter">field delimeter in csv string</param>
    /// <param name="mappingConfig">configuration for csv file</param>
    /// <typeparam name="T">object to map data to</typeparam>
    /// <returns>desired object</returns>
    public static T MapCsvToRecord<T>(
        string csvData,
        char delimeter,
        CsvMappingConfiguration<T> mappingConfig
    )
        where T : class, new()
    {
        var lines = csvData.Replace("\r", "").Split('\n');

        if (lines.Length < 2)
            return new T(); // Not enough data

        var headers = lines[0].Split(delimeter);
        var currentRecord = new T();
        for (int i = 1; i < lines.Length; i++)
        {
            var values = lines[i].Split(delimeter);

            for (int j = 0; j < headers.Length; j++)
            {
                if (mappingConfig.Mapping.TryGetValue(headers[j], out var mappingAction))
                {
                    if (values.Length <= j)
                    {
                        break;
                    }

                    if (values[j] is not null)
                    {
                        mappingAction(currentRecord, values[j]);
                    }
                }
            }
        }

        return currentRecord;
    }
}

public class CsvMappingConfiguration<T>
    where T : class, new()
{
    public Dictionary<string, Action<T, string>> Mapping { get; } =
        new Dictionary<string, Action<T, string>>();

    public void Map(string columnHeader, Action<T, string> mapAction)
    {
        Mapping[columnHeader] = mapAction;
    }
}
