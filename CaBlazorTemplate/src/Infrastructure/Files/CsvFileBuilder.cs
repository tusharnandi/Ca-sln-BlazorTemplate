using System.Globalization;
using CaBlazorTemplate.Application.Common.Interfaces;
using CaBlazorTemplate.Application.TodoLists.Queries.ExportTodos;
using CaBlazorTemplate.Infrastructure.Files.Maps;
using CsvHelper;

namespace CaBlazorTemplate.Infrastructure.Files;

public class CsvFileBuilder : ICsvFileBuilder
{
    public byte[] BuildTodoItemsFile(IEnumerable<TodoItemRecord> records)
    {
        using var memoryStream = new MemoryStream();
        using (var streamWriter = new StreamWriter(memoryStream))
        {
            using var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture);

            csvWriter.Configuration.RegisterClassMap<TodoItemRecordMap>();
            csvWriter.WriteRecords(records);
        }

        return memoryStream.ToArray();
    }
}
