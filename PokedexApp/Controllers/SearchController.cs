using Microsoft.AspNetCore.Mvc;
using Pokedex.Common;
using Pokedex.Logging.Interfaces;
using PokedexApp.Interfaces;
using PokedexApp.Models;
using System;
using System.Threading.Tasks;

namespace PokedexApp.Controllers
{
    public class SearchController : Controller
    {
        private ILoggerAdapter<SearchController> _logger;
        private IPokedexAppLogic _pokedexAppLogic;
        public SearchController(ILoggerAdapter<SearchController> logger, IPokedexAppLogic pokedexAppLogic)
        {
            _logger = logger;
            _pokedexAppLogic = pokedexAppLogic;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                SearchViewModel searchViewModel = await _pokedexAppLogic.GetSearchForm();

                return View(searchViewModel);
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Index(SearchViewModel searchViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    SearchViewModel results = await _pokedexAppLogic.Search(searchViewModel);

                    return View(results);
                }
                else
                {
                    _logger.LogInformation(Constants.InvalidRequest);

                    return await Index();
                }
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            return View(Constants.Error, new ErrorViewModel() { Message = ex.Message });
        }
    }
}