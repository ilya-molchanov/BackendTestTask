namespace BackendTest.Internal.Exceptions
{
    public enum InternalApiErrorCodes
    {
        ItemNotFound = 101,

        CannotDelete = 102,

        ParentItemNotFound = 103,

        BadRequest = 400,

        Unauthorized = 401,

        NotFound = 404,

        Conflict = 409,

        UnhandledException = 500
    }
}
