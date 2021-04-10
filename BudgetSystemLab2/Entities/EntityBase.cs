using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetSystemLab2
{
    public abstract class EntityBase
    {
        public bool IsValid
        {
            get
            {
                return Validate();
            }
        }

        public abstract bool Validate();
    }
}
