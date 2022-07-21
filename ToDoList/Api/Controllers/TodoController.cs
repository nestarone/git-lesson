using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoList.Api.Features.Todos;
using ToDoList.Infrastructure.Database;

namespace ToDoList.Api.Controllers
{
    [ApiController]
    [Route("todo")]
    public class TodoController : ControllerBase
    {
        private readonly DatabaseContext _databaseContext;
        private readonly IMediator _mediator;

        public class TodoListItemViewModel
        {
            public Guid Id { get; set; }
            public string Title { get; set; }
            public string Content { get; set; }

        }

        public TodoController(DatabaseContext databaseContext, IMediator mediator)
        {
            _databaseContext = databaseContext;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IReadOnlyCollection<TodoListResponse>> GetTodoListItems([FromQuery] GetTodoListQuery query, CancellationToken token)
        {
            return await _mediator.Send(query, token);
        }

        [HttpPost]
        public async Task<Guid> CreateTodoListItem([FromBody] AddTodoListCommand command, CancellationToken token)
        {
            return await _mediator.Send(command, token);
        }

        [HttpPut]
        public async Task<Guid> UpdateTodoListItem([FromBody] UpdateTodoListCommand command, CancellationToken token)
        {
            return await _mediator.Send(command, token);
        }

        [HttpDelete]
        public async Task RemoveTodoListItem([FromBody] DeleteTodoListCommand command, CancellationToken token)
        {
            await _mediator.Send(command, token);
            return;
        }


    }
}
