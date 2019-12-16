using SimpleWebApi.Models;
using SimpleWebApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace ReceipesMApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ReceipesMController : ControllerBase
    {
        private readonly  ReceipeService _receipeService;

        public ReceipesMController(ReceipeService receipeService)
        {
            _receipeService = receipeService;
        }


        //get the lookup values for the unit select
        [HttpGet("{action}")]
        public UnitLookup[] GetUnits()
        {
            return _receipeService.GetUnitLookups().ToArray();
        }

        [HttpGet]
        [Route("search/{term}")]
        public ActionResult<IEnumerable<ReceipeMSearchResult>> Search(string term)
        {
            return this._receipeService.Search(term);
            
        }

        [HttpGet]
        public ActionResult<List<ReceipeM>> Get() =>
            _receipeService.Get();

        [HttpGet("{id:length(24)}", Name = "GetReceipeM")]
        public ActionResult<ReceipeM> Get(string id)
        {
            var receipe = _receipeService.Get(id);

            if (receipe == null)
            {
                return NotFound();
            }

            return receipe;
        }

        [HttpPost]
        public ActionResult<ReceipeM> Create(ReceipeM receipe)
        {
            _receipeService.Create(receipe);

            return CreatedAtRoute("GetReceipeM", new { id = receipe.Id.ToString() }, receipe);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, ReceipeM existingReceipe)
        {
            var receipe = _receipeService.Get(id);

            if (receipe == null)
            {
                return NotFound();
            }

            _receipeService.Update(id, existingReceipe);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var receipe = _receipeService.Get(id);

            if (receipe == null)
            {
                return NotFound();
            }

            _receipeService.Remove(receipe.Id);

            return NoContent();
        }
    }
}
