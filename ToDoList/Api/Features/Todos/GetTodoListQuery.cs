using MediatR;
using Microsoft.EntityFrameworkCore;
using ToDoList.Infrastructure.Database;

namespace ToDoList.Api.Features.Todos
{

    

    public class TodoListResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

    }

    public class GetTodoListQuery:IRequest<IReadOnlyCollection<TodoListResponse>>
    {
        public Guid userId { get; set; }
    }

    public class GetTodoListHandler : IRequestHandler<GetTodoListQuery, IReadOnlyCollection<TodoListResponse>>
    {
        private readonly DatabaseContext _databaseContext;

        public GetTodoListHandler(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<IReadOnlyCollection<TodoListResponse>> Handle(GetTodoListQuery request, CancellationToken token)
        {
            var user = await _databaseContext.Users.SingleAsync(user => user.Id == request.userId, token);

            return await _databaseContext.Entry(user).Collection(user => user.TodoListItems).Query()
                .Select(todoListItems => new TodoListResponse
                {
                    Title = todoListItems.Title,
                    Content = todoListItems.Content,
                    Id = todoListItems.Id
                })
                .ToListAsync(token);
        }
    }
}
