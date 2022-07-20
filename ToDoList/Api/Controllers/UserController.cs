using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain;
using ToDoList.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using MediatR;
using ToDoList.Api.Features.Users;

namespace ToDoList.Api.Controllers
{
    //public class UserViewModel
    //{
    //    public Guid Id { get; set; }
    //    public string Name { get; set; }
    //    public int TodoItemsCount { get; set; }
    //}

    //public class TodoListItemViewModel
    //{

    //}

    [ApiController]
    [Route("users")]
    public class UserController : ControllerBase
    {
        private readonly DatabaseContext _databaseContext;
        private readonly IMediator _mediator;

        public UserController(DatabaseContext databaseContext, IMediator mediator)
        {
            _databaseContext = databaseContext;
            _mediator = mediator;
        }
        [HttpGet]
        public async Task<IReadOnlyCollection<GetUsersResponse>> GetUsers([FromQuery]GetUsersQuery query, CancellationToken token)
        {
            return await _mediator.Send(query, token);
        }

        [HttpPost]
        public async Task<Guid> CreateUser([FromBody]CreateUserCommand command, CancellationToken token)
        {
            return await _mediator.Send(command, token);
        }

    }
}
