using AutoMapper;
using BackendTest.Data.Entities;
using BackendTest.Data.Repositories.Interfaces;
using BackendTest.Internal.BusinessObjects;
using BackendTest.Internal.Exceptions;
using BackendTest.Internal.Exceptions.Models;
using BackendTest.WebApi.Filters.Exception;
using Microsoft.AspNetCore.Mvc;
using System.Web.Http.Description;

namespace BackendTestWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [TypeFilter(typeof(CustomExceptionFilter))]
    public class NodeController : ControllerBase
    {
        private readonly ILogger<NodeController> _logger;
        private readonly INodeRepository _nodeRepository;
        private readonly IMapper _mapper;

        public NodeController(ILogger<NodeController> logger, INodeRepository nodeRepository, IMapper mapper)
        {
            _logger = logger;
            _nodeRepository = nodeRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Get a node's tree by id
        /// </summary>
        /// <param name="id">Id of the given node</param>
        /// <returns>
        /// The <see cref="Task"/> of <seealso cref="NodeDto" />.
        /// </returns>
        [HttpGet(Name = "GetNodeTree")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ResponseType(typeof(NodeDto))]
        public async Task<NodeDto> Get(int id, CancellationToken cancellationToken = default)
        {
            _logger.LogDebug("NodeController.GetNodeTree was called...");
            if (id < 1)
            {
                _logger.LogError("The node Id parameter is invalid (less than 1).");
                throw new InternalApiBusinessException(InternalApiErrorCodes.BadRequest,
                    new InternalBusinessData("The node Id parameter is invalid (less than 1)."));
            }

            List<Node> linkedNodes = await _nodeRepository.GetNodeWithLinkedNodesAsync(id, cancellationToken);

            if (!linkedNodes.Any())
            {
                _logger.LogError($"The node with id {id} does not exist or has been deleted.");
                throw new InternalApiBusinessException(InternalApiErrorCodes.ItemNotFound,
                    new InternalBusinessData($"The node with id {id} does not exist or has been deleted."));
            }

            //catch (Exception ex)
            //{
            // _logger.LogError("Testing custom exception filter.");
            // throw new InternalApiBusinessException(123, "Testing custom exception filter.", new InternalBusinessData("Testing custom exception filter"));

            return TreeExtensions.ToTree(linkedNodes, linkedNodes.First(x => x.NodeId == id).ParentNodeId);
        }

        /// <summary>
        /// Deletes a node
        /// </summary>
        /// <param name="id">Id of the given node</param>
        /// <returns>Status code</returns>
        [HttpDelete(Name = "DeleteNode")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ResponseType(typeof(IActionResult))]
        public async Task<IActionResult> DeleteNodeAsync(int id, CancellationToken cancellationToken = default)
        {
            _logger.LogDebug("NodeController.DeleteNodeAsync was called...");
            if (id < 1)
            {
                _logger.LogError("The node Id parameter is invalid (less than 1).");
                throw new InternalApiBusinessException(InternalApiErrorCodes.BadRequest,
                    new InternalBusinessData("The node Id parameter is invalid (less than 1)."));
            }

            List<Node> linkedNodes = await _nodeRepository.GetNodeWithLinkedNodesAsync(id);

            if (!linkedNodes.Any())
            {
                _logger.LogError($"The node with id {id} does not exist or has been deleted.");
                throw new InternalApiBusinessException(InternalApiErrorCodes.ItemNotFound,
                    new InternalBusinessData($"The node with id {id} does not exist or has been deleted."));
            }

            if (linkedNodes.Count > 1)
            {
                _logger.LogError($"The node with id {id} cannot be deleted because contains children. Please delete the children nodes and try again.");
                throw new InternalApiBusinessException(InternalApiErrorCodes.CannotDelete,
                    new InternalBusinessData($"The node with id {id} cannot be deleted because contains children. Please delete the children nodes and try again."));
            }

            _logger.LogDebug($"Delete the given node with an '{id}'");

            await _nodeRepository.DeleteAsync(linkedNodes.First(x => x.NodeId == id));
            await _nodeRepository.CommitAsync();

            _logger.LogDebug($"Delete operation has been finished");

            return Ok();
        }

        /// <summary>
        /// Creates new node
        /// </summary>
        /// <param name="newNode">New node entity</param>
        /// <returns>Id of the created node</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ResponseType(typeof(ActionResult<int>))]
        public async Task<ActionResult<int>> CreateNodeAsync([FromBody] CreateNodeDto newNode)
        {
            if (newNode == null)
            {
                _logger.LogError("The node model is null.");
                throw new InternalApiBusinessException(InternalApiErrorCodes.BadRequest,
                    new InternalBusinessData("The node model is null."));
            }

            var node = _mapper.Map<Node>(newNode);

            if (newNode != null &&
                newNode.ParentNodeId != null)
            {
                var parentNode = await _nodeRepository.GetNodeAsync(newNode.ParentNodeId.Value);
                if (parentNode == null)
                {
                    _logger.LogError($"The node with given parent id {newNode.ParentNodeId.Value} does not exist or has been deleted.");
                    throw new InternalApiBusinessException(InternalApiErrorCodes.ParentItemNotFound,
                        new InternalBusinessData($"The node with given parent id {newNode.ParentNodeId.Value} does not exist or has been deleted."));
                }
            }

            _logger.LogDebug($"Insert operation has been started");

            await _nodeRepository.InsertAsync(node);
            await _nodeRepository.CommitAsync();

            _logger.LogDebug($"New node entity has been inserted with an id '{node.NodeId}'");

            return Created(string.Empty, node.NodeId);
        }

        /// <summary>
        /// Edit a given node
        /// </summary>
        /// <param name="node">A given node entity</param>
        /// <returns>Updated node</returns>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ResponseType(typeof(ActionResult))]
        public async Task<ActionResult> EditNodeAsync([FromBody] EditNodeDto node)
        {
            if (node == null)
            {
                _logger.LogError("The node model is null.");
                throw new InternalApiBusinessException(InternalApiErrorCodes.BadRequest,
                    new InternalBusinessData("The node model is null."));
            }

            var dbNode = _mapper.Map<Node>(node);

            if (dbNode == null)
            {
                _logger.LogError($"The node with given id {node.NodeId} does not exist or has been deleted.");
                throw new InternalApiBusinessException(InternalApiErrorCodes.ItemNotFound,
                    new InternalBusinessData($"The node with given id {node.NodeId} does not exist or has been deleted."));
            }

            if (dbNode != null &&
                dbNode.ParentNodeId != null)
            {
                var parentNode = await _nodeRepository.GetNodeAsync(dbNode.ParentNodeId.Value);
                if (parentNode == null)
                {
                    _logger.LogError($"The node with given parent id {(node.ParentNodeId.HasValue ? node.ParentNodeId.Value : null)} does not exist or has been deleted.");
                    throw new InternalApiBusinessException(InternalApiErrorCodes.ParentItemNotFound,
                        new InternalBusinessData($"The node with given parent id {(node.ParentNodeId.HasValue ? node.ParentNodeId.Value : null)} does not exist or has been deleted."));
                }
            }

            _logger.LogDebug($"Edit operation has been started");

            await _nodeRepository.UpdateAsync(dbNode);
            await _nodeRepository.CommitAsync();

            _logger.LogDebug($"A given node entity with an id '{dbNode.NodeId}' has been updated");

            return Ok();
        }
    }
}
