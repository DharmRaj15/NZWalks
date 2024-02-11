using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalksAPI.CustomActionFilters;
using NZWalksAPI.Models.Domain;
using NZWalksAPI.Models.DTO;
using NZWalksAPI.Repositories;

namespace NZWalksAPI.Controllers
{
    //api/walks
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : Controller
    {
        private readonly IMapper mapper;
        private readonly IWalkRepository walkRepository;


        //create walks add data
        public WalksController(IMapper mapper, IWalkRepository walkRepository)
        {
            this.mapper = mapper;
            this.walkRepository = walkRepository;
        }

        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddWalksRequestDto addWalksRequestDto)
        {
            //map dto to domain modal
            var walkDomainModal = mapper.Map<Walk>(addWalksRequestDto);

            //call sql repo methos
            await walkRepository.CreateAsync(walkDomainModal);

            //map domain class to dto
            return Ok(mapper.Map<WalksDto>(walkDomainModal));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWalks()
        {
            var WalksDomain = await walkRepository.GetAllWalksAsync();
            if (WalksDomain == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<List<WalksDto>>(WalksDomain));
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetWalkById([FromRoute] Guid id)
        {
            var WalkDomain = await walkRepository.GetByIdAsync(id);
            if (WalkDomain == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<WalksDto>(WalkDomain));
        }

        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        public async Task<IActionResult> Update(Guid id,UpdateWalkRequestDto updateWalkRequestDto)
        {
            //map DTO to domain modal
            var walkDomainModal = mapper.Map<Walk>(updateWalkRequestDto);

            walkDomainModal = await walkRepository.UpdateAsync(id, walkDomainModal);

            if(walkDomainModal == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<WalksDto>(walkDomainModal));

        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var WalkDomainModal = await walkRepository.DeleteAsync(id);

            if (WalkDomainModal == null)
            {
                return NotFound();
            }

            //map domain o DTO
            return Ok(mapper.Map<WalksDto>(WalkDomainModal));
        }
    }
}
