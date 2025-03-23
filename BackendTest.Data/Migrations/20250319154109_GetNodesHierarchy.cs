using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackendTest.Migrations
{
    /// <inheritdoc />
    public partial class GetNodesHierarchy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
			    CREATE FUNCTION [dbo].[GetNodesHierarchy] (@nodeId int)
				RETURNS @ResultNodes TABLE 
				(
					[NodeId] INT,
					[Name] NVARCHAR(100),
					[ParentNodeId] INT,
					[Lev] INT
				)
				AS
				BEGIN
					DECLARE @SelectedRootNode TABLE ([NodeId] INT, [Name] NVARCHAR(100), [ParentNodeId] INT, [Lev] INT);

					IF (@nodeId is not null)
					BEGIN
						INSERT INTO @SelectedRootNode
						SELECT [NodeId], [Name], [ParentNodeId], 1 as [Lev]
						FROM [Nodes]
						WHERE [NodeId] = @nodeId
					END
					ELSE
					BEGIN
						INSERT INTO @SelectedRootNode
						SELECT [NodeId], [Name], [ParentNodeId], 1 as [Lev]
						FROM [Nodes]
						WHERE [ParentNodeId] IS NULL
					END

					;WITH cte ([NodeId], [Name], [ParentNodeId], [Lev]) 
					AS (   
						  SELECT [NodeId], [Name], [ParentNodeId], [Lev]
						  FROM @SelectedRootNode
	  
						  UNION ALL

						  SELECT [Nodes].[NodeId], [Nodes].[Name], [Nodes].[ParentNodeId], [cte].[Lev] + 1
						  FROM cte 
						  INNER JOIN [Nodes]
						  ON [Nodes].[ParentNodeId] = [cte].[NodeId]
					)
					INSERT INTO @ResultNodes
					SELECT *
					FROM cte
					ORDER BY [NodeId]
				RETURN
				END
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
			    IF OBJECT_ID(N'[dbo].[GetNodesHierarchy]', 'IF') IS NOT NULL
	                DROP FUNCTION [dbo].[GetNodesHierarchy];
                GO;
            ");
        }
    }
}
