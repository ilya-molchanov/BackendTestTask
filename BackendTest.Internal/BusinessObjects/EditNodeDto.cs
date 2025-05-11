namespace BackendTest.Internal.BusinessObjects
{
    public class EditNodeDto
    {
        public EditNodeDto() { }

        public int NodeId { get; set; }

        public int? ParentNodeId { get; set; }

        public string? Name { get; set; }
    }
}