using MediatR;
using Microsoft.EntityFrameworkCore;
using ToDoList.Infrastructure.Database;

namespace ToDoList.Api.Features.Todos
{
    public class DeleteTodoListCommand:IRequest<Unit>
    {
        public Guid userId { get; set; }
        public Guid todoListItemId { get; set; }
    }

    public class DeleteTodoListCommandHandler : IRequestHandler<DeleteTodoListCommand, Unit> 
    { 
        private DatabaseContext _databaseContext;

        public DeleteTodoListCommandHandler(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<Unit> Handle(DeleteTodoListCommand request, CancellationToken token)
        {
            var user = await _databaseContext.Users.SingleAsync(user => user.Id == request.userId, token);
            var todoListItem = await _databaseContext.Entry(user).Collection(user => user.TodoListItems).Query()
                .SingleAsync(todoListItem => todoListItem.Id == request.todoListItemId, token);
            user.RemoveTodoListItem(todoListItem);

            await _databaseContext.SaveChangesAsync(token);

            return Unit.Value;
        }
    }
}
