using CaBlazorTemplate.Application.TodoLists.Queries.ExportTodos;

namespace CaBlazorTemplate.Application.Common.Interfaces;

public interface ICsvFileBuilder
{
    byte[] BuildTodoItemsFile(IEnumerable<TodoItemRecord> records);
}
