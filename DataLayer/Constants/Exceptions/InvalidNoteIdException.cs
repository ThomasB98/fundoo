using System.Runtime.Serialization;

namespace DataLayer.Constants.Exceptions
{
    [Serializable]
    internal class InvalidNoteIdException : Exception
    {
        public InvalidNoteIdException()
        {
        }

        public InvalidNoteIdException(string? message) : base(message)
        {
        }

        public InvalidNoteIdException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected InvalidNoteIdException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}