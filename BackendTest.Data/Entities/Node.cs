namespace BackendTest.Data.Entities
{
    public class Node
    {
        public Node()
        {
        }

        public int NodeId { get; set; }

        public int? ParentNodeId { get; set; }

        public string? Name { get; set; }
    }
}
