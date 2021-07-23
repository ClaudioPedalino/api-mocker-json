using Microsoft.AspNetCore.Mvc;
using mock_json.Interfaces;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;

namespace mock_json.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MockController : ControllerBase
    {
        private readonly IMockService _mockService;

        public MockController(IMockService mockService)
        {
            _mockService = mockService;
        }

        [HttpGet("test")]
        [ApiExplorerSettings(IgnoreApi = true)]
        [Produces("application/json")]
        public ActionResult Get([FromQuery] string file)
            => Ok(_mockService.GetMockData(file));

        [HttpGet("keys")]
        [ProducesResponseType(typeof(List<string>), (int)HttpStatusCode.OK)]
        [Produces("application/json")]
        [SwaggerOperation(
            Summary = "Get all keys",
            Description = "public method rey",
            OperationId = "GetAllKeys",
            Tags = new[] { "mock", "keys", "pepe" }
        )]
        public ActionResult GetAllKeys([FromQuery] int paginationSize = 50, int pageNumber = 1)
            => Ok(_mockService.GetAllKeys(paginationSize, pageNumber));

        [HttpGet("{key}")]
        public ActionResult GetByKey([FromRoute] string key)
        {
            var response = _mockService.GetByKey(key);
            return Ok(response);
        }

        [HttpPost("{key}")]
        [Produces("application/json")]
        //[ApiExplorerSettings(GroupName = "XYZ - A collection of XYZ APIs")]
        public ActionResult Create([FromRoute] string key, [FromBody] JsonElement payload)
            => Ok(_mockService.Create(key, payload));
    }
}