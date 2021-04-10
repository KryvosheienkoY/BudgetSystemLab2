using System;

namespace BudgetSystemLab2.Navigation
{
    public interface INavigatable<TObject> where TObject: Enum
    {
        public TObject Type { get; }

        public void ClearSensitiveData();
    }
}
