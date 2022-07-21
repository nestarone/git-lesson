using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ToDoList.Infrastructure.Database;

namespace ToDoList.Api.Features.Todos
{
    public class UpdateTodoListCommand:IRequest<Guid>
    {
        public Guid userId { get; set; }
        public Guid todoListItemId { get; set; }
        public string title { get; set; }
        public string content { get; set; }
    }

    public class UpdateTodoListCommandValidator : AbstractValidator<UpdateTodoListCommand>
    {
        public UpdateTodoListCommandValidator()
        {
            RuleFor(c => c.title).NotEmpty();
            //RuleFor(c => c.userId).NotNull();
        }
    }

    public class UpdateTodoListCommandHandler : IRequestHandler<UpdateTodoListCommand, Guid>
    {
        private DatabaseContext _databaseContext;

        public UpdateTodoListCommandHandler(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<Guid> Handle(UpdateTodoListCommand request, CancellationToken token)
        {
            var user = await _databaseContext.Users.SingleAsync(user => user.Id == request.userId, token);
            var todoListItem = await _databaseContext.Entry(user).Collection(user => user.TodoListItems).Query()
                .SingleAsync(todoListItem => todoListItem.Id == request.todoListItemId, token);
            user.UpdateTodoListItem(todoListItem, request.title, request.content);

            await _databaseContext.SaveChangesAsync(token);
            return todoListItem.Id;
        }
    }
}
