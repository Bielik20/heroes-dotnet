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
    public class QuestsController : Controller
    {
        private readonly QuestsRepository _questsRepository;

        public QuestsController(QuestsRepository questsRepository)
        {
            _questsRepository = questsRepository;
        }

        // GET api/quests
        [HttpGet]
        public async Task<IEnumerable<Quest>> Get()
        {
            return await _questsRepository.GetAll();
        }

        // GET api/quests/ownedbyhero/1
        [HttpGet("OwnedByHero/{heroId}")]
        public async Task<IEnumerable<Quest>> GetOwnedByHero(int heroId)
        {
            return await _questsRepository.GetOwnedByHero(heroId);
        }

        // GET api/quests/5
        [HttpGet("{id}", Name = "GetQuest")]
        public async Task<IActionResult> Get(int id)
        {
            var model = await _questsRepository.GetById(id);
            if(model == null)
            {
                return NotFound();
            }
            return new ObjectResult(model);
        }

        // POST api/quests
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Quest model)
        {
            await _questsRepository.Create(model);
            return CreatedAtRoute("GetQuest", new { id = model.Id }, model);
        }

        // PUT api/quests/5
        [HttpPut]
        public async Task<IActionResult> Put([FromBody]Quest model)
        {
            await _questsRepository.Update(model);
            return new NoContentResult();
        }

        // DELETE api/quests/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _questsRepository.Delete(id);
            return new NoContentResult();
        }
    }
}
