using Common.Domain;
using Common.Repositories;
using Microsoft.AspNetCore.Mvc;
using Todos.Service;
using Todos.Service.Dto;

namespace Todos.Api.Controllers
{
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
        /// <returns>List all todos</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllToDoAsync( int? offset, string? labelFreeText, int? limit)
        {
            var todos =await _todosService.GetAllToDoAsync(offset, labelFreeText, limit);
            var countTodos = _todosService.Count(labelFreeText);
            HttpContext.Response.Headers.Append("X-Total-Count", countTodos.ToString());

            return Ok(todos);
        }
        [HttpGet("totalCount")]
        public IActionResult GetCountToDo( string? labelFreeText)
        {

            var todos = _todosService.Count(labelFreeText);

            return Ok(todos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetToDoByIdAsync(int id, CancellationToken cancellationToken)
        {
           
            var todo = await _todosService.GetToDoByIdAsync(id, cancellationToken);

            if (todo == null)
            {
                return NotFound($"Запись с ID = {id} отсутствует");
            }

            return Ok(todo);
        }

        [HttpGet("{id}/IsDone")]
        public IActionResult GetToDoByIdIsDone(int id)
        {
            var todo = _todosService.GetToDoById(id);

            if (todo == null)
            {
                return NotFound($"Запись с ID = {id} отсутствует");
            }

            return Ok(new { todo.Id, todo.IsDone });
        }
        //owner id is required
        [HttpPost]
        public async Task<IActionResult> AddToDoAsync(CreateTodoDto toDo)
        {
            var todo = await _todosService.CreateToDoAsync(toDo);
            
            return Created($"todos/{todo.Id}", todo);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateToDo(int id,UpdateToDoDto updateToDo)
        {
            updateToDo.Id = id;
            var todo = _todosService.UpdateToDo(updateToDo);

            if (todo == null)
            {
                return NotFound($"Запись с ID = {updateToDo.Id} отсутствует");
            }

            return Ok(todo);
        }

        [HttpPatch("{id}/IsDone")]
        public IActionResult UpdateToDoIsDone(int id, UpdateToDoDto updateToDo)
        {
            updateToDo.Id = id;
            var todo = _todosService.UpdateToDo(updateToDo);

            if (todo == null)
            {
                return NotFound($"Запись с ID = {id} отсутствует");
            }
           
            return Ok(new { todo.Id, todo.IsDone });
        }

        [HttpDelete]
        public IActionResult RemoveToDo([FromBody]int id)
        {
            var todo = _todosService.RemoveToDo(id);
            
            if (!todo)
            {
                return NotFound($"Запись с ID = {id} отсутствует");
            }

            return Ok($"Запись с ID = {id} удалена");
        }
       
    }
}
