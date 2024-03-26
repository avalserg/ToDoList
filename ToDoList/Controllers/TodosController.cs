using Common.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Todos.Service;
using Todos.Service.Dto;

namespace Todos.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class TodosController : ControllerBase
    {
        private readonly ITodosService _todosService;
        
        public TodosController(ITodosService todosService)
        {
            _todosService = todosService;
           
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
        public async Task<IActionResult> GetAllToDoAsync( int? offset, string? labelFreeText, int? limit,bool? descending,CancellationToken cancellationToken)
        {
          
            var todos =await _todosService.GetAllToDoAsync(offset, labelFreeText, limit,descending,cancellationToken);
            var countTodos = await _todosService.CountAsync(labelFreeText,cancellationToken);
            HttpContext.Response.Headers.Append("X-Total-Count", countTodos.ToString());
            return Ok(todos);
           
        }
        [HttpGet("totalCount")]
        public async Task<IActionResult> GetCountToDoAsync( string? labelFreeText,CancellationToken cancellationToken)
        {

            var todos = await _todosService.CountAsync(labelFreeText,cancellationToken);

            return Ok(todos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetToDoByIdAsync(int id, CancellationToken cancellationToken)
        {
            var getTodo = await _todosService.GetToDoByIdAsync(id, cancellationToken);
            //if (getTodo == null)
            //{
            //    return NotFound($"Запись с ID = {id} отсутствует");
            //}
           
            return Ok(getTodo);
            
        }

        [HttpGet("{id}/IsDone")]
        public async Task<IActionResult> GetToDoByIdIsDoneAsync(int id, CancellationToken cancellationToken)
        {
            var getTodo = await _todosService.GetToDoByIdAsync(id, cancellationToken);
            //if (getTodo == null)
            //{
            //    return NotFound($"Запись с ID = {id} отсутствует");
            //}
         
            return Ok(new { getTodo?.Id, getTodo?.IsDone });
            
        }
        
        [HttpPost]
        public async Task<IActionResult> AddToDoAsync(CreateTodoDto toDo, CancellationToken cancellationToken)
        {
            var todo = await _todosService.CreateToDoAsync(toDo, cancellationToken);
            
            return Created($"todos/{todo?.Id}", todo);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateToDoAsync(int id,UpdateToDoDto updateToDo, CancellationToken cancellationToken)
        {
            updateToDo.Id = id;
            //var updateTodo = await _todosService.GetToDoByIdAsync(id, cancellationToken);
            //if (updateTodo == null)
            //{
            //    return NotFound($"Запись с ID = {id} отсутствует");
            //}

            var todo = await _todosService.UpdateToDoAsync(updateToDo, cancellationToken);
            return Ok(todo);

        }

        [HttpPatch("{id}/IsDone")]
        public async Task<IActionResult> UpdateToDoIsDoneAsync(int id, UpdateToDoDto updateToDo, CancellationToken cancellationToken)
        {
            updateToDo.Id = id;
            //var getTodo = await _todosService.GetToDoByIdAsync(id, cancellationToken);
            //if (getTodo == null)
            //{
            //    return NotFound($"Запись с ID = {id} отсутствует");
            //}

            var todo = await _todosService.UpdateToDoAsync(updateToDo, cancellationToken);
            return Ok(new { todo?.Id, todo?.IsDone });

        }

        [HttpDelete]
        public async Task<IActionResult> RemoveToDoAsync([FromBody]int id, CancellationToken cancellationToken)
        {
            //var getTodo = await _todosService.GetToDoByIdAsync(id, cancellationToken);
            //if (getTodo == null)
            //{
            //    return NotFound($"Запись с ID = {id} отсутствует");
            //}

            await _todosService.RemoveToDoAsync(id, cancellationToken);
            return Ok($"Запись с ID = {id} удалена");

        }
       
    }
}
