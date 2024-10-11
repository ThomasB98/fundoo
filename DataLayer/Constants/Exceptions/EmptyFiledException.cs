using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Constants.Exceptions
{
    /// <summary>
    /// Exception that is thrown when a required field is found to be empty.
    /// </summary>
    public class EmptyFieldException : Exception
    {
        /// <summary>
        /// Gets the name of the field that is empty.
        /// </summary>
        public string FieldName { get; }

        /// <summary>
        /// Gets the type of the field that is empty (optional).
        /// </summary>
        public Type FieldType { get; }

        /// <summary>
        /// Initializes a new instance of the EmptyFieldException class with the specified field name.
        /// </summary>
        /// <param name="fieldName">The name of the field that is empty.</param>
        public EmptyFieldException(string fieldName)
            : base($"The field '{fieldName}' cannot be empty.")
        {
            FieldName = fieldName;
        }

        /// <summary>
        /// Initializes a new instance of the EmptyFieldException class with the specified field name and field type.
        /// </summary>
        /// <param name="fieldName">The name of the field that is empty.</param>
        /// <param name="fieldType">The type of the field that is empty.</param>
        public EmptyFieldException(string fieldName, Type fieldType)
            : base($"The field '{fieldName}' of type '{fieldType.Name}' cannot be empty.")
        {
            FieldName = fieldName;
            FieldType = fieldType;
        }

        
    }

}
