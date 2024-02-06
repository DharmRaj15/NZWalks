using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalksAPI.Data;
using NZWalksAPI.Models.Domain;
using NZWalksAPI.Models.DTO;
using NZWalksAPI.Repositories;

namespace NZWalksAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewRegionController : Controller
    {
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        public NewRegionController(IRegionRepository regionRepository,IMapper mapper)
        {
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            //directly call go to respoditory insted of db context
            var regionsDomain = await regionRepository.GetAllAsyc();

            var regionDTO = new List<RegionDto>();

            foreach (var region in regionsDomain)
            {
                regionDTO.Add(new RegionDto
                {
                    Id = region.Id,
                    Code = region.Code,
                    Name = region.Name,
                    RegionImagerl = region.RegionImagerl
                });
            }

            return Ok(regionDTO);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var regionDomain = await regionRepository.GetByID(id);

            if (regionDomain == null)
            {
                return NotFound();
            }

            var regionDTO = new RegionDto
            {
                Id = regionDomain.Id,
                Code = regionDomain.Code,
                Name = regionDomain.Name,
                RegionImagerl = regionDomain.RegionImagerl
            };

            return Ok(regionDTO);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            var regionDomainModal = new Region
            {
                Code = addRegionRequestDto.Code,
                Name = addRegionRequestDto.Name,
                RegionImagerl = addRegionRequestDto.RegionImagerl
            };

            regionDomainModal = await regionRepository.Create(regionDomainModal);

            var regionDTO = new RegionDto
            {
                Id = regionDomainModal.Id,
                Code = regionDomainModal.Code,
                Name = regionDomainModal.Name,
                RegionImagerl = regionDomainModal.RegionImagerl
            };

            return CreatedAtAction(nameof(GetById), new { id = regionDTO.Id }, regionDTO);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            var RegionDomainModal = new Region
            {
                Code = updateRegionRequestDto.Code,
                Name = updateRegionRequestDto.Name,
                RegionImagerl = updateRegionRequestDto.RegionImagerl
            };

            RegionDomainModal = await regionRepository.Update(id, RegionDomainModal);

            if (RegionDomainModal == null)
            {
                return NotFound();
            }

            var regionDTO = new RegionDto
            {
                Code = RegionDomainModal.Code,
                Name = RegionDomainModal.Name,
                RegionImagerl = RegionDomainModal.RegionImagerl,
            };

            return Ok(regionDTO);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var domainRegionModal = await regionRepository.Delete(id);

            if (domainRegionModal == null)
            {
                return NotFound();
            }

            var regionDTO = new RegionDto
            {
                Id = domainRegionModal.Id,
                Code = domainRegionModal.Code,
                Name = domainRegionModal.Name,
                RegionImagerl = domainRegionModal.RegionImagerl
            };

            return Ok(regionDTO);
        }
    }
}
