using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Service
{
    public interface ICurrentUserSerice
    {
        public string CurrentUserId { get; }
        public string[] CurrentUserRoles { get; }
    }
}
