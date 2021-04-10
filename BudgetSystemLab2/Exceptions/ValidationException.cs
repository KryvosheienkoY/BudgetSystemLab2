using System;
using System.Runtime.Serialization;

namespace BudgetSystemLab2
{
    [Serializable]
    internal class ValidationException : Exception
    {
        public ValidationException()
        {
        }

        public ValidationException(string message) : base(message)
        {
        }
    }
}