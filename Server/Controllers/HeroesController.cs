using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessLayer.Repositories;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    public class HeroesController : Controller
    {
        private readonly HeroesRepository _heroesRepository;

        public HeroesController(HeroesRepository heroesRepository)
        {
            _heroesRepository = heroesRepository;
        }

        // GET api/heroes
        [HttpGet]
        public async Task<IEnumerable<Hero>> Get()
        {
            return await _heroesRepository.GetAll();
        }

        // GET api/heroes/5
        [HttpGet("{id}", Name = "GetHero")]
        public async Task<IActionResult> Get(int id)
        {
            var model = await _heroesRepository.GetById(id);
            if(model == null)
            {
                return NotFound();
            }
            return new ObjectResult(model);
        }

        // POST api/heroes
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Hero model)
        {
            await _heroesRepository.Create(model);
            return CreatedAtRoute("GetHero", new { id = model.Id }, model);
        }

        // PUT api/heroes/5
        [HttpPut]
        public async Task<IActionResult> Put([FromBody]Hero model)
        {
            await _heroesRepository.Update(model);
            return new NoContentResult();
        }

        // DELETE api/heroes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _heroesRepository.Delete(id);
            return new NoContentResult();
        }
    }
}
