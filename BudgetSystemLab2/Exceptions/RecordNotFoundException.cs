using System;
using System.Runtime.Serialization;

namespace BudgetSystemLab2
{
    [Serializable]
    internal class RecordNotFoundException : Exception
    {
        public RecordNotFoundException()
        {
        }

        public RecordNotFoundException(string message) : base(message)
        {
        }
    }
}