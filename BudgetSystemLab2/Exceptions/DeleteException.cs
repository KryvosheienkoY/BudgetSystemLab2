using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetSystemLab2
{
    class DeleteException:Exception
    {
        public DeleteException() { }
        public DeleteException(string message) : base(message) { }
    }
}
