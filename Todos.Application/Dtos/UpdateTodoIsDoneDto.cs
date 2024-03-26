using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todos.Application.Dtos
{
    public class UpdateTodoIsDoneDto
    {
        public int Id { get; set; }
        public bool IsDone { get; set; }
        public int OwnerId { get; set; }

    }
}
