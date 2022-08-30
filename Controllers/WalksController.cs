using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalksController : Controller
    {
        private readonly IWalkRepository _walkRepository;
        private readonly IWalkDifficultyRepository _difficultyRepository;
        private readonly IRegionRepository _regionRepository;
        private readonly IMapper _mapper;
        public WalksController(IWalkRepository walkRepository, 
            IMapper mapper, IWalkDifficultyRepository walkDifficultyRepository, IRegionRepository regionRepository)
        {
            _walkRepository = walkRepository;
            _difficultyRepository = walkDifficultyRepository;
            _mapper = mapper;
            _regionRepository = regionRepository;
        }

        [HttpGet]
        [Authorize(Roles = "reader")]
        public async Task<IActionResult> GetAllWalks()
        {
            var walks = await _walkRepository.GetAllAsync();
            var walksDTO = _mapper.Map<List<Models.DTO.Walk>>(walks);

            return Ok(walksDTO);
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetWalk")]
        [Authorize(Roles = "reader")]
        public async Task<IActionResult> GetWalk(Guid id)
        {
            var walk = await _walkRepository.GetAsync(id);
            

            if(walk == null)
            {
                return NotFound();
            }

            var walkDTO = _mapper.Map<Models.DTO.Walk>(walk);

            return Ok(walkDTO);
        }

        [HttpPost]
        [Authorize(Roles = "writer")]
        public async Task<IActionResult> AddWalk([FromBody] Models.DTO.AddWalkRequest addWalkRequest)
        {
            if (!(await ValidateWalk(addWalkRequest)))
            {
                return BadRequest(ModelState);
            }
            var walk = new Models.Domain.Walk
            {
                Name = addWalkRequest.Name,
                Length = addWalkRequest.Length,
                RegionId = addWalkRequest.RegionId,
                WalkDifficultyId = addWalkRequest.WalkDifficultyId
            };

            walk = await _walkRepository.AddAsync(walk);

            var walkDTO = _mapper.Map<Models.DTO.Walk>(walk);

            return CreatedAtAction(nameof(GetWalk), new { id = walkDTO.Id }, walkDTO);
        
        }

        [HttpPut]
        [Route("{id:guid}")]
        [Authorize(Roles = "Read Write")]
        public async Task<IActionResult> UpdateWalk([FromRoute] Guid id, [FromBody] Models.DTO.UpdateWalkRequest updateWalkRequest )
        {
            if (!(await ValidateWalk(updateWalkRequest)))
            {
                return BadRequest(ModelState);
            }

            var walk = new Models.Domain.Walk
            {
                Name = updateWalkRequest.Name,
                Length = updateWalkRequest.Length,
                RegionId = updateWalkRequest.RegionId,
                WalkDifficultyId = updateWalkRequest.WalkDifficultyId
            };

            walk = await _walkRepository.UpdateAsync(id, walk);

            if(walk == null)
            {
                return NotFound();
            }

            var walkDTO = _mapper.Map<Models.DTO.Walk>(walk);

            return Ok(walkDTO);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        [Authorize(Roles = "writer")]
        public async Task<IActionResult> DeleteWalkAsync(Guid id)
        {
            var walk = await _walkRepository.DeleteAsync(id);
            if(walk == null)
            {
                return NotFound();
            }

            var walkDto = _mapper.Map<Models.DTO.Walk>(walk);

            return Ok(walkDto);
        }

        private async Task<bool> ValidateWalk(dynamic addWalkRequest)
        {
            bool output = true;

            var difficulty = await _difficultyRepository.GetAsync(addWalkRequest.WalkDifficultyId);
            if(difficulty == null)
            {
                ModelState.AddModelError(nameof(addWalkRequest.WalkDifficultyId), $"{nameof(addWalkRequest.WalkDifficultyId)} is invalid.");
                output = false;
            }

            var region = await _regionRepository.GetAsync(addWalkRequest.RegionId);
            if(region == null)
            {
                ModelState.AddModelError(nameof(addWalkRequest.RegionId), $"{nameof(addWalkRequest.RegionId)} is invalid.");
                output = false;
            }

            return output;
        }
    }
}
