using MediatR;
using Microsoft.EntityFrameworkCore;
using ToDoList.Infrastructure.Database;

namespace ToDoList.Api.Features.Users
{
    public class GetUsersResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int TodoItemsCount { get; set; }
    }

    public class GetUsersQuery : IRequest<IReadOnlyCollection<GetUsersResponse>>
    {
        public string? Keywords { get; set; }
    }

    public class GetUserQueryHandler : IRequestHandler<GetUsersQuery, IReadOnlyCollection<GetUsersResponse>>
    {
        private readonly DatabaseContext _databaseContext;

        public GetUserQueryHandler(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<IReadOnlyCollection<GetUsersResponse>> Handle(GetUsersQuery request, CancellationToken token)
        {
            var query = _databaseContext.Users.AsQueryable();

            if (!string.IsNullOrEmpty(request.Keywords))
            {
                query = query.Where(user => user.Name.ToLower().Contains(request.Keywords.ToLower()));
            }

            return await query.Select(user => new GetUsersResponse
            {
                Id = user.Id,
                Name = user.Name,
                TodoItemsCount = user.TodoListItems.Count()
            })
                .ToListAsync(token);
        }
    }
}
