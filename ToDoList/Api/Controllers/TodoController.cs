using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoList.Infrastructure.Database;

namespace ToDoList.Api.Controllers
{
    [ApiController]
    [Route("todo")]
    public class TodoController : ControllerBase
    {
        private readonly DatabaseContext _databaseContext;

        public class TodoListItemViewModel
        {
            public Guid Id { get; set; }
            public string Title { get; set; }
            public string Content { get; set; }

        }

        public TodoController(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        [HttpGet]
        public async Task<IReadOnlyCollection<TodoListItemViewModel>> GetTodoListItems(Guid userId, CancellationToken token)
        {
            var user = await _databaseContext.Users.SingleAsync(user => user.Id == userId, token);

            return await _databaseContext.Entry(user).Collection(user => user.TodoListItems).Query()
                .Select(todoListItems => new TodoListItemViewModel
                {
                    Title = todoListItems.Title,
                    Content = todoListItems.Content,
                    Id = todoListItems.Id
                })
                .ToListAsync(token);

            //return await _databaseContext.Users.Where(user => user.Id == userId)
            //    .SelectMany(user => user.TodoListItems)
            //    .Select(todoListItems => new TodoListItemViewModel
            //    {
            //        Title = todoListItems.Title,
            //        Content = todoListItems.Content,
            //        Id = todoListItems.Id
            //    })
            //    .ToListAsync(token);
        }

        [HttpPost]
        public async Task<Guid> CreateTodoListItem(Guid userId, string title, string content, CancellationToken token)
        {
            var user = await _databaseContext.Users.SingleAsync(user => user.Id == userId, token);
            var todoListItem = user.AddTodoListItem(title);
            todoListItem.SetContent(content ?? string.Empty);

            await _databaseContext.SaveChangesAsync(token);

            return todoListItem.Id;
        }

        [HttpDelete]
        public async Task RemoveTodoListItem(Guid userId, Guid todoListItemId, CancellationToken token)
        {
            var user = await _databaseContext.Users.SingleAsync(user => user.Id == userId, token);
            var todoListItem = await _databaseContext.Entry(user).Collection(user => user.TodoListItems).Query()
                .SingleAsync(todoListItem=> todoListItem.Id==todoListItemId, token);
            user.RemoveTodoListItem(todoListItem);

            await _databaseContext.SaveChangesAsync(token);

            return;

        }
    }
}
