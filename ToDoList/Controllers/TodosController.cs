using Common.Repositories;
using Microsoft.AspNetCore.Mvc;
using Todos.Service;

namespace Todos.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TodosController : ControllerBase
    {
        private readonly ITodosService _todosService;
        private readonly IUserRepository _userRepository;

        public TodosController(ITodosService todosService,IUserRepository userRepository)
        {
            _todosService = todosService;
            _userRepository = userRepository;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="limit">max count return values</param>
        /// <param name="offset">count missed values</param>
        /// <returns>List all todos</returns>
        [HttpGet]
        public IActionResult GetAllToDo( int? offset, int? ownerId, string? labelFreeText, int? limit)
        {
           
            var todos = _todosService.GetAllToDo(offset,ownerId, labelFreeText, limit);
            
            return Ok(todos);
        } 

        [HttpGet("{id}")]
        public IActionResult GetToDoById(int id)
        {
           
            var todo = _todosService.GetToDoById(id);

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
        public IActionResult AddToDo(Domain.Todos toDo)
        {
            var todo = _todosService.AddToDo(toDo);

            return Created($"todos/{todo.Id}", todo);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateToDo(int id, Domain.Todos newToDo)
        {
            var todo = _todosService.UpdateToDo(id, newToDo);

            if (todo == null)
            {
                return NotFound($"Запись с ID = {id} отсутствует");
            }

            return Ok(todo);
        }

        [HttpPatch("{id}/IsDone")]
        public IActionResult UpdateToDoIsDone(int id, Domain.Todos newToDo)
        {
            var todo = _todosService.UpdateToDo(id, newToDo);

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
