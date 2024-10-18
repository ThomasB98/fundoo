using System.Runtime.Serialization;

namespace DataLayer.Constants.Exceptions
{
    [Serializable]
    internal class CollaborationNotExistsException : Exception
    {
        public CollaborationNotExistsException()
        {
        }

        public CollaborationNotExistsException(string? message) : base(message)
        {
        }

        public CollaborationNotExistsException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected CollaborationNotExistsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}