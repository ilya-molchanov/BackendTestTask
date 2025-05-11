using BackendTest.Data.Entities;

namespace BackendTest.Internal.BusinessObjects
{
    public class NodeDto
    {
        public NodeDto()
        {
            Children = new List<NodeDto>();
        }

        public int NodeId { get; set; }

        public int? ParentNodeId { get; set; }

        public string? Name { get; set; }

        public ICollection<NodeDto> Children { get; set; }        
    }

    public static class TreeExtensions
    {
        public static NodeDto ToTree(this List<Node> items, int? parentNodeId)
        {
            var lookup = items.ToLookup(foo => foo.ParentNodeId);
            Func<Node, NodeDto>? selector = null;
            selector = item => new NodeDto()
            {
                NodeId = item.NodeId,
                ParentNodeId = item.ParentNodeId,
                Name = item.Name,
                Children = lookup[item.NodeId].Select(selector).ToList()
            };
            return lookup[parentNodeId].Select(selector).FirstOrDefault();
        }

        public static NodeDto? FindNode(NodeDto parent, int id)
        {
            if (parent != null)
            {
                if (parent.NodeId == id)
                {
                    return parent;
                }

                if (parent.Children != null)
                {
                    foreach (var child in parent.Children)
                    {
                        if (child.NodeId == id)
                            return child;
                        NodeDto node = FindNode(child, id);

                        if (node != null)
                            return node;
                    }
                }
            }
            return null;
        }
    }
}