using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Users.Service.Dto
{
    public class AuthUserDto
    {
        public string Password { get; set; } = default!;
        public string Login { get; set; } = default!;
    }
}
