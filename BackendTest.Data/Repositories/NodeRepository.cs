using BackendTest.Data.Entities;
using BackendTest.Data.Repositories.Interfaces;

namespace BackendTest.Data.Repositories
{
    public class NodeRepository : INodeRepository
    {
        private readonly NodesDbContext _context;

        public NodeRepository(NodesDbContext context) {
            _context = context;
        }

        public async Task<List<Node>> GetNodeWithLinkedNodesAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.GetNodeWithLinkedNodesAsync(id, cancellationToken);
        }

        public async Task DeleteAsync(Node record, CancellationToken cancellationToken = default)
        {
            _context.Nodes.Remove(record);
            await Task.CompletedTask;
        }

        public async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task InsertAsync(Node record, CancellationToken cancelToken = default)
        {
            await _context.Nodes.AddAsync(record, cancelToken).ConfigureAwait(false);
        }

        public virtual async Task UpdateAsync(Node record, CancellationToken cancelToken = default)
        {
            _context.Nodes.Update(record);
            await Task.CompletedTask;
        }

        public async Task<Node?> GetNodeAsync(int nodeId)
        {
            return await _context.GetNodeAsync(nodeId);
        }
    }
}
