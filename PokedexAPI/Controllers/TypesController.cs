using cloudscribe.Pagination.Models;
using Microsoft.AspNetCore.Mvc;
using Pokedex.Common;
using Pokedex.Common.Interfaces;
using Pokedex.Logging.Interfaces;
using PokedexAPI.Interfaces;
using PokedexAPI.Models.Output;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PokedexAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TypesController : ControllerBase
    {
        private IPokedexAPILogic _pokedexAPILogic;
        private IPaginationHelper _paginationHelper;
        private ILoggerAdapter<TypesController> _logger;
        public TypesController(IPokedexAPILogic pokedexAPILogic, IPaginationHelper paginationHelper,
            ILoggerAdapter<TypesController> logger)
        {
            _pokedexAPILogic = pokedexAPILogic;
            _paginationHelper = paginationHelper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetTypes([FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = Constants.PageSize)
        {
            List<GenericLookupResult> types = await _pokedexAPILogic.GetAllTypes();

            PagedResult<GenericLookupResult> pagedTypes =
                _paginationHelper.GetPagedResults(types, pageNumber, pageSize);

            return Ok(pagedTypes.Data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTypeById(int id)
        {
            GenericLookupResult pokeball = await _pokedexAPILogic.GetTypeById(id);

            if (pokeball == null)
            {
                _logger.LogInformation(Constants.InvalidRequest + " for " + Constants.Type + Constants.WithId + id);

                return BadRequest();
            }

            return Ok(pokeball);
        }
    }
}