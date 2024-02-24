using Microsoft.AspNetCore.Mvc;
namespace ToDoList.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ToDoListController : ControllerBase
    {
        
        private static  List<Model.ToDoList> Todos = new()
        {
            new Model.ToDoList()
            {
                Id = 1, IsDone = true, Label = "Проверить почту", CreatedDate = DateTime.Now, UpdateDate = default
            },
            new Model.ToDoList()
            {
                Id = 2, IsDone = false, Label = "Сходить в магазин", CreatedDate = DateTime.Now, UpdateDate = default
            }, 
            new Model.ToDoList()
            {
                Id = 3, IsDone = false, Label = "Выгулять собаку", CreatedDate = DateTime.Now, UpdateDate = default
            }, 
            new Model.ToDoList()
            {
                Id = 4, IsDone = false, Label = "Сделать зарядку", CreatedDate = DateTime.Now, UpdateDate = default
            },
        };
        private readonly ILogger<ToDoListController> _logger;

        public ToDoListController(ILogger<ToDoListController> logger)
        {
            _logger = logger;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="limit">max count return values</param>
        /// <param name="offset">count missed values</param>
        /// <returns>List all todos</returns>
        [HttpGet]
        public IActionResult GetAllToDo( int offset, int limit = 5)
        {
           
            var todos = Todos.OrderBy(t=>t.Id).Skip(offset).Take(limit).ToList();
            if (todos.Count == 0)
            {
                return NotFound("Список дел отсутствует");
            }
            return Ok(todos);
        } 
        [HttpGet("{id}")]
        public IActionResult GetToDoById(int id)
        {
            var todo = Todos.FirstOrDefault(t=>t.Id==id);
            if (todo == null)
            {
                return NotFound("Запись с таким ID отсутствует");
            }
            return Ok(todo);
        } 
        [HttpGet("{id}/IsDone")]
        public IActionResult GetToDoByIdIsDone(int id)
        {
            var todo = Todos.FirstOrDefault(t=>t.Id==id);
            if (todo == null)
            {
                return NotFound("Запись с таким ID отсутствует");
            }

            return Ok( new{ todo.Id,todo.IsDone} );
        }  
        [HttpPost]
        public IActionResult AddToDo(Model.ToDoList toDo)
        {
            toDo.Id = Todos.Max(todo => todo.Id) + 1;
            toDo.CreatedDate= DateTime.UtcNow;
            
            Todos.Add(toDo);
            return Created($"todos/{toDo.Id}",toDo);
        }
        [HttpPut("{id}")]
        public IActionResult UpdateToDo(int id,Model.ToDoList newToDo)
        {
            var todo = Todos.SingleOrDefault(t => t.Id == id);
            if (todo == null)
            {
                return NotFound("Такой записи не существует");
            }
            todo.IsDone = newToDo.IsDone;
            todo.Label = newToDo.Label;
            todo.UpdateDate= DateTime.UtcNow;


            return Ok(todo);
        } 
        [HttpPatch("{id}/IsDone")]
        public IActionResult UpdateToDoIsDone(int id,bool isDone)
        {
            var todo = Todos.SingleOrDefault(t => t.Id == id);
            if (todo == null)
            {
                return NotFound("Такой записи не существует");
            }
            todo.IsDone = isDone;
           
            return Ok(new { todo.Id, todo.IsDone });
        } 
        [HttpDelete("{id}")]
        public IActionResult RemoveToDo(int id)
        {
            var todo = Todos.SingleOrDefault(t => t.Id == id);
            if (todo == null)
            {
                return NotFound("Такой записи не существует");
            }
            Todos.Remove(todo);
            return Ok(todo);
        }

    }
}
