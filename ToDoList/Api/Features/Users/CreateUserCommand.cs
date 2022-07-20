using MediatR;
using ToDoList.Domain;
using ToDoList.Infrastructure.Database;

namespace ToDoList.Api.Features.Users
{
    public class CreateUserCommand : IRequest<Guid>
    {
        public string Name  { get; set; }
    }

    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Guid>
    {
        private readonly DatabaseContext _databaseContext;

        public CreateUserCommandHandler(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<Guid> Handle(CreateUserCommand request, CancellationToken token)
        {
            var user = new User(request.Name);
            _databaseContext.Add(user);
            await _databaseContext.SaveChangesAsync(token);

            return user.Id;
        }
    }
}
