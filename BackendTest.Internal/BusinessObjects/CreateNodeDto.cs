namespace BackendTest.Internal.BusinessObjects
{
    public class CreateNodeDto
    {
        public CreateNodeDto() { }

        public int? ParentNodeId { get; set; }

        public string? Name { get; set; }
    }
}