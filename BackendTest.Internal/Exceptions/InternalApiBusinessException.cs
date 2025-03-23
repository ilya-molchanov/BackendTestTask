using BackendTest.Internal.Exceptions.Models;

namespace BackendTest.Internal.Exceptions
{
    public class InternalApiBusinessException : Exception
    {
        public InternalApiBusinessException(InternalApiErrorCodes businessLogicErrorCode, InternalBusinessData content, string? message = default)
            : base(message)
        {
            InternalApiBusinessErrorCode = businessLogicErrorCode;
            Content = content;
        }

        public InternalBusinessData Content { get; }

        public InternalApiErrorCodes InternalApiBusinessErrorCode { get; }
    }
}