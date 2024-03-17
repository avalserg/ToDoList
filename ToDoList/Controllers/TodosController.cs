using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Todos.Service;
using Todos.Service.Dto;
using Users.Service;

namespace Todos.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class TodosController : ControllerBase
    {
        private readonly ITodosService _todosService;
        private readonly IUserService _userService;

        public TodosController(ITodosService todosService, IUserService userService)
        {
            _todosService = todosService;
            _userService = userService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="labelFreeText">search by label field</param>
        /// <param name="limit">max count return values</param>
        /// <param name="offset">count missed values</param>
        /// <returns>List all todos</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllToDoAsync( int? offset, string? labelFreeText, int? limit,bool? descending,CancellationToken cancellationToken)
        {
            var currentLoggedInUserId = User.FindFirst(ClaimTypes.NameIdentifier);
            var currentLoggedInUser = await _userService.GetUserByIdAsync(int.Parse(currentLoggedInUserId!.Value), cancellationToken);

            if (currentLoggedInUser!.Roles.Any(r => r.ApplicationUserRole.Name == "Admin"))
            {
                 var todos =await _todosService.GetAllToDoAsync(offset, labelFreeText, limit,descending,cancellationToken);
                 var countTodos = await _todosService.CountAsync(labelFreeText,cancellationToken);
                 HttpContext.Response.Headers.Append("X-Total-Count", countTodos.ToString());
                 return Ok(todos);
            }
            else
            {
                var todos = await _todosService.GetAllToDoAsync(offset, labelFreeText, limit, descending, cancellationToken);
                var resTodo = todos.Where(t => t.OwnerId == currentLoggedInUser.Id);
                return Ok(resTodo);
            }
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
            if (getTodo == null)
            {
                return NotFound($"Запись с ID = {id} отсутствует");
            }
            var currentLoggedInUserId = User.FindFirst(ClaimTypes.NameIdentifier);
            var currentLoggedInUser = (await _userService.GetUserByIdAsync(int.Parse(currentLoggedInUserId!.Value), cancellationToken))!;
            
            if (currentLoggedInUser.Id == getTodo.OwnerId ||
                currentLoggedInUser.Roles.Any(r => r.ApplicationUserRole.Name == "Admin"))
            {
                return Ok(getTodo);
            }
            return BadRequest($"Запись с ID = {id} не может быть просмотрена пользователем {currentLoggedInUser.Login}");
            
        }

        [HttpGet("{id}/IsDone")]
        public async Task<IActionResult> GetToDoByIdIsDoneAsync(int id, CancellationToken cancellationToken)
        {
            var getTodo = await _todosService.GetToDoByIdAsync(id, cancellationToken);
            if (getTodo == null)
            {
                return NotFound($"Запись с ID = {id} отсутствует");
            }
            var currentLoggedInUserId = User.FindFirst(ClaimTypes.NameIdentifier);
            var currentLoggedInUser = (await _userService.GetUserByIdAsync(int.Parse(currentLoggedInUserId!.Value), cancellationToken))!;

            if (currentLoggedInUser.Id == getTodo.OwnerId ||
                currentLoggedInUser.Roles.Any(r => r.ApplicationUserRole.Name == "Admin"))
            {
                return Ok(new { getTodo.Id, getTodo.IsDone });
            }
            return BadRequest($"Запись с ID = {id} не может быть просмотрена пользователем {currentLoggedInUser.Login}");
           
        }
        
        [HttpPost]
        public async Task<IActionResult> AddToDoAsync(CreateTodoDto toDo, CancellationToken cancellationToken)
        {
            var todo = await _todosService.CreateToDoAsync(toDo, cancellationToken);
            
            return Created($"todos/{todo.Id}", todo);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateToDoAsync(int id,UpdateToDoDto updateToDo, CancellationToken cancellationToken)
        {
            updateToDo.Id = id;
            var updateTodo = await _todosService.GetToDoByIdAsync(id, cancellationToken);
            if (updateTodo==null)
            {
                return NotFound($"Запись с ID = {id} отсутствует");
            }
            var currentLoggedInUserId = User.FindFirst(ClaimTypes.NameIdentifier);
            var currentLoggedInUser = (await _userService.GetUserByIdAsync(int.Parse(currentLoggedInUserId!.Value), cancellationToken))!;
            if (currentLoggedInUser.Id == updateToDo.OwnerId ||
                currentLoggedInUser.Roles.Any(r => r.ApplicationUserRole.Name == "Admin"))
            {
                var todo = await _todosService.UpdateToDoAsync(updateToDo, cancellationToken);
                return Ok(todo);
            }
           
            return BadRequest($"Todo with {id} не может быть изменен юзером {currentLoggedInUser.Login}");
            
        }

        [HttpPatch("{id}/IsDone")]
        public async Task<IActionResult> UpdateToDoIsDoneAsync(int id, UpdateToDoDto updateToDo, CancellationToken cancellationToken)
        {
            updateToDo.Id = id;
            var getTodo = await _todosService.GetToDoByIdAsync(id, cancellationToken);
            if (getTodo == null)
            {
                return NotFound($"Запись с ID = {id} отсутствует");
            }
            var currentLoggedInUserId = User.FindFirst(ClaimTypes.NameIdentifier);
            var currentLoggedInUser = (await _userService.GetUserByIdAsync(int.Parse(currentLoggedInUserId!.Value), cancellationToken))!;
            if (currentLoggedInUser.Id == getTodo.OwnerId ||
                currentLoggedInUser.Roles.Any(r => r.ApplicationUserRole.Name == "Admin"))
            {
                var todo = await _todosService.UpdateToDoAsync(updateToDo, cancellationToken);
                return Ok(new { todo.Id, todo.IsDone });
            }

            return BadRequest($"Todo with {id} не может быть изменен юзером {currentLoggedInUser.Login}");
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveToDoAsync([FromBody]int id, CancellationToken cancellationToken)
        {
            var getTodo = await _todosService.GetToDoByIdAsync(id, cancellationToken);
            if (getTodo == null)
            {
                return NotFound($"Запись с ID = {id} отсутствует");
            }
            var currentLoggedInUserId = User.FindFirst(ClaimTypes.NameIdentifier);
            var currentLoggedInUser = (await _userService.GetUserByIdAsync(int.Parse(currentLoggedInUserId!.Value), cancellationToken))!;

            if (currentLoggedInUser.Id == getTodo.OwnerId ||
                currentLoggedInUser.Roles.Any(r => r.ApplicationUserRole.Name == "Admin"))
            { 
                await _todosService.RemoveToDoAsync(id, cancellationToken);
                return Ok($"Запись с ID = {id} удалена");
            }
            return BadRequest($"Запись с ID = {id} не может быть удалена пользователем {currentLoggedInUser.Login}");
          
        }
       
    }
}
