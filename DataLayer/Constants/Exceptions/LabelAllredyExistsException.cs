using ModelLayer.Model.Entity;
using System.Runtime.Serialization;

namespace DataLayer.Constants.Exceptions
{
    [Serializable]
    internal class LabelAllredyExistsException : Exception
    {
        private Label labelName;

        public LabelAllredyExistsException()
        {
        }

        public LabelAllredyExistsException(Label labelName)
        {
            this.labelName = labelName;
        }

        public LabelAllredyExistsException(string? message) : base(message)
        {
        }

        public LabelAllredyExistsException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected LabelAllredyExistsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}