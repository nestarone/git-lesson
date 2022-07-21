using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ToDoList.Infrastructure.Database;

namespace ToDoList.Api.Features.Todos
{
    public class AddTodoListCommand:IRequest<Guid>
    {
        public Guid userId { get; set; }
        public string title { get; set; }
        public string content { get; set; }
    }

    public class AddTodoListCommandValidator : AbstractValidator<AddTodoListCommand>
    {
        public AddTodoListCommandValidator()
        {
            RuleFor(c => c.title).NotEmpty();
            //RuleFor(c => c.userId).NotNull();
        }
    }

    public class AddTodoItemHandler : IRequestHandler<AddTodoListCommand, Guid> { 
        private readonly DatabaseContext _databaseContext;

        public AddTodoItemHandler(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<Guid> Handle(AddTodoListCommand request, CancellationToken token)
        {
            var user = await _databaseContext.Users.SingleAsync(user => user.Id == request.userId, token);
            var todoListItem = user.AddTodoListItem(request.title);
            todoListItem.SetContent(request.content ?? string.Empty);

            await _databaseContext.SaveChangesAsync(token);

            return todoListItem.Id;
        }
    }
}
