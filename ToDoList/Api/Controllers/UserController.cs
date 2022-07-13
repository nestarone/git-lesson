using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain;
using ToDoList.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace ToDoList.Api.Controllers
{
    public class UserViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int TodoItemsCount { get; set; }
    }

    public class TodoListItemViewModel
    {

    }

    [ApiController]
    [Route("users")]
    public class UserController : ControllerBase
    {
        private readonly DatabaseContext _databaseContext;

        public UserController(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }
        [HttpGet]
        public async Task<IReadOnlyCollection<UserViewModel>> GetUsers(string? keywords, CancellationToken token)
        {
            var query = _databaseContext.Users.AsQueryable();

            if (!string.IsNullOrEmpty(keywords))
            {
                query = query.Where(user => user.Name.ToLower().Contains(keywords.ToLower()));
            }

            return await query.Select(user => new UserViewModel
            {
                Id = user.Id,
                Name = user.Name,
                TodoItemsCount = user.TodoListItems.Count()
            })
                .ToListAsync(token);
        }

        [HttpPost]
        public async Task<Guid> CreateUser(string name, CancellationToken token)
        {
            var user = new User(name);
            _databaseContext.Add(user);
            await _databaseContext.SaveChangesAsync(token);

            return user.Id;

        }

    }
}
