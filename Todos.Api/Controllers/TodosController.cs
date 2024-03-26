using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Todos.Application.Command.CreateTodo;
using Todos.Application.Command.RemoveTodo;
using Todos.Application.Command.UpdateTodo;
using Todos.Application.Command.UpdateTodoIsDone;
using Todos.Application.Query.GetCountTodos;
using Todos.Application.Query.GetListTodos;
using Todos.Application.Query.GetTodoById;

namespace Todos.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class TodosController : ControllerBase
    {
        private readonly IMediator _mediator;


        public TodosController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="labelFreeText">search by label field</param>
        /// <param name="limit">max count return values</param>
        /// <param name="offset">count missed values</param>
        /// <param name="descending">sort asc desc</param>
        /// <param name="cancellationToken"></param>
        /// <returns>List all todos</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllToDoAsync([FromQuery] GetListTodosQuery getListTodosQuery, CancellationToken cancellationToken)
        {

            var todos = await _mediator.Send(getListTodosQuery, cancellationToken);
            var countTodos =await _mediator.Send(new GetCountTodosQuery() { NameFreeText = getListTodosQuery.NameFreeText }, cancellationToken);
            HttpContext.Response.Headers.Append("X-Total-Count", countTodos.ToString());
            return Ok(todos);

        }
        
        [HttpGet("totalCount")]
        public async Task<IActionResult> GetCountToDoAsync(string? labelFreeText, CancellationToken cancellationToken)
        {

            var todos = await _mediator.Send(new GetCountTodosQuery() { NameFreeText = labelFreeText },
                cancellationToken);

            return Ok(todos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetToDoByIdAsync(int id, CancellationToken cancellationToken)
        {
            var getTodo = await _mediator.Send(new GetTodoByIdQuery(){Id = id}, cancellationToken);
          
            return Ok(getTodo);

        }

        [HttpGet("{id}/IsDone")]
        public async Task<IActionResult> GetToDoByIdIsDoneAsync(int id, CancellationToken cancellationToken)
        {
            var getTodo = await _mediator.Send(new GetTodoByIdQuery() { Id = id }, cancellationToken);
            
            return Ok(new { getTodo.Id, getTodo.IsDone });

        }

        [HttpPost]
        public async Task<IActionResult> AddToDoAsync(CreateTodoCommand createTodoCommand, CancellationToken cancellationToken)
        {
            var todo = await _mediator.Send(createTodoCommand, cancellationToken);

            return Created($"todos/{todo.Id}", todo);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateToDoAsync(int id, UpdateTodoCommand updateToDoCommand, CancellationToken cancellationToken)
        {
            updateToDoCommand.Id = id;
            var todo = await _mediator.Send(updateToDoCommand, cancellationToken);
            return Ok(todo);

        }

        [HttpPatch("{id}/IsDone")]
        public async Task<IActionResult> UpdateToDoIsDoneAsync(int id, UpdateTodoIsDoneCommand updateToDoIsDoneCommand, CancellationToken cancellationToken)
        {
            updateToDoIsDoneCommand.Id = id;
            var todo = await _mediator.Send(updateToDoIsDoneCommand, cancellationToken);
            return Ok(new { todo.Id, todo.IsDone });
           
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveToDoAsync([FromBody] int id, CancellationToken cancellationToken)
        {
            await _mediator.Send(new RemoveTodoCommand() { Id = id }, cancellationToken);
           
            return Ok($"Запись с ID = {id} удалена");

        }

    }
}
