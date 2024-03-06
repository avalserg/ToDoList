using Common.Domain;
using Common.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Todos.Service;
using Todos.Service.Dto;

namespace Todos.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TodosController : ControllerBase
    {
        private readonly ITodosService _todosService;
        private readonly IBaseRepository<User> _userRepository;
     

        public TodosController(ITodosService todosService,IBaseRepository<User> userRepository)
        {
            _todosService = todosService;
            _userRepository = userRepository;
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="labelFreeText">search by label field</param>
        /// <param name="limit">max count return values</param>
        /// <param name="offset">count missed values</param>
        /// <returns>List all todos</returns>
        [HttpGet]
        public IActionResult GetAllToDo( int? offset, string? labelFreeText, int? limit)
        {
            var todos = _todosService.GetAllToDo(offset, labelFreeText, limit);
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
        public IActionResult GetToDoById(int id)
        {
           
            var todo = _todosService.GetToDoById(id);

            if (todo == null)
            {
                return NotFound($"������ � ID = {id} �����������");
            }

            return Ok(todo);
        }

        [HttpGet("{id}/IsDone")]
        public IActionResult GetToDoByIdIsDone(int id)
        {
            var todo = _todosService.GetToDoById(id);

            if (todo == null)
            {
                return NotFound($"������ � ID = {id} �����������");
            }

            return Ok(new { todo.Id, todo.IsDone });
        }
        //owner id is required
        [HttpPost]
        public IActionResult AddToDo(CreateTodoDto toDo)
        {
            var todo = _todosService.CreateToDo(toDo);
            
            return Created($"todos/{todo.Id}", todo);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateToDo(int id,UpdateToDoDto updateToDo)
        {
            updateToDo.Id = id;
            var todo = _todosService.UpdateToDo(updateToDo);

            if (todo == null)
            {
                return NotFound($"������ � ID = {updateToDo.Id} �����������");
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
                return NotFound($"������ � ID = {id} �����������");
            }
           
            return Ok(new { todo.Id, todo.IsDone });
        }

        [HttpDelete]
        public IActionResult RemoveToDo([FromBody]int id)
        {
            var todo = _todosService.RemoveToDo(id);
            
            if (!todo)
            {
                return NotFound($"������ � ID = {id} �����������");
            }

            return Ok($"������ � ID = {id} �������");
        }
       
    }
}
