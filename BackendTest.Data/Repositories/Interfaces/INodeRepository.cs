using BackendTest.Data.Entities;

namespace BackendTest.Data.Repositories.Interfaces
{
    public interface INodeRepository
    {
        Task<List<Node>> GetNodeWithLinkedNodesAsync(int id, CancellationToken cancellationToken = default);

        Task<Node?> GetNodeAsync(int nodeId);

        Task DeleteAsync(Node record, CancellationToken cancellationToken = default);

        Task CommitAsync(CancellationToken cancellationToken = default);

        Task InsertAsync(Node record, CancellationToken cancelToken = default);

        Task UpdateAsync(Node record, CancellationToken cancelToken = default);
    }
}
