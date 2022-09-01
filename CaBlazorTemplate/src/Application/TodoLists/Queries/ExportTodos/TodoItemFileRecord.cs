using CaBlazorTemplate.Application.Common.Mappings;
using CaBlazorTemplate.Domain.Entities;

namespace CaBlazorTemplate.Application.TodoLists.Queries.ExportTodos;

public class TodoItemRecord : IMapFrom<TodoItem>
{
    public string? Title { get; set; }

    public bool Done { get; set; }
}
