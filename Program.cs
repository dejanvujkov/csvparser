string csvData =
    @"Identifier;Fullname;Available
3;Name1;true
4;Name2;false";

var parsedData = MapToRecords.MapCsvToRecords(
    csvData,
    ';',
    CsvDataMapper.GetCsvMappingConfiguration()
);

foreach (var data in parsedData)
{
    Console.WriteLine("==========================================");
    Console.WriteLine($"ID: {data.ID}, Name: {data.Name}, Available: {data.Available}");
}
