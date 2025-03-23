namespace BackendTest.Internal.Exceptions.Models
{
    public class ErrorModel
    {
        public string Type { get; set; }
        public string Id { get; set; }
        public InternalBusinessData Data { get; set; }

        public ErrorModel(string type, string id, InternalBusinessData data)
        {
            Type = type;
            Id = id;
            Data = data;
        }
    }

    public record InternalBusinessData
    {
        public string Message { get; set; }
        public InternalBusinessData(string message) => Message = message;
    }
}
