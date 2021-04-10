using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetSystemLab2
{

    internal class NoAccessException : Exception
    {
        public NoAccessException()
        {
        }

        public NoAccessException(string message) : base(message)
        {
        }

    }
}
