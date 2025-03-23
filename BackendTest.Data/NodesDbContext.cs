using BackendTest.Data.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace BackendTest.Data
{
    public class NodesDbContext : DbContext
    {
        public NodesDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Node> Nodes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Node>(entity =>
            {
                entity.HasKey(t => t.NodeId);

                entity.Property(t => t.Name)
                .HasMaxLength(100);

                entity.ToTable("Nodes");

                // entity.HasOne(e => e.Parent)
                //    .WithMany(e => e.Childs)
                //    .HasForeignKey(e => e.ParentNodeId)
                //    .IsRequired(false)
                //    .OnDelete(DeleteBehavior.Restrict);
            });

            //Seed to Nodes
            string nodesJson = System.IO.File.ReadAllText("nodes.json");
            List<Node> nodes = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Node>>(nodesJson);

            foreach (Node node in nodes)
                modelBuilder.Entity<Node>().HasData(node);
        }

        public async Task<Node?> GetNodeAsync(int nodeId)
        {
            return await Nodes.FirstOrDefaultAsync(x => x.NodeId == nodeId);
        }

        public async Task<List<Node>> GetNodeWithLinkedNodesAsync(int id, CancellationToken cancellationToken = default)
        {
            SqlParameter param = new SqlParameter("@nodeId", id);
            return await Nodes
                .FromSqlRaw("SELECT * FROM dbo.GetNodesHierarchy(@nodeId)", param)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
