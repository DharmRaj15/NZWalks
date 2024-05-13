using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalksAPI.Data;
using NZWalksAPI.Models.Domain;
using NZWalksAPI.Models.DTO;

namespace NZWalksAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RegionController : ControllerBase
    {
        private readonly NZWalksDBContext dBContext;

        //called from DB
        public RegionController(NZWalksDBContext dBContext)
        {
            this.dBContext = dBContext;
        }

        //[HttpGet]
        //public IActionResult GetAllStatic()
        //{
        //    var region = new List<Region> {
        //        new Region
        //        {
        //            Id=new Guid(),
        //            Name = "Auckland Region",
        //            Code = "AKL",
        //            RegionImagerl = "https://media.istockphoto.com/id/1060826424/photo/2018-jan-3-auckland-new-zealand-panorama-view-beautiful-landcape-of-the-building-in-auckland.webp?s=1024x1024&w=is&k=20&c=waV6mp87PhQ5TTnECKkkpVWZRdV2p8VHHPJzFAfSzlg="
        //        },
        //        new Region
        //        {
        //            Id=new Guid(),
        //            Name="Wellington Region",
        //            Code="WLG",
        //            RegionImagerl="https://media.istockphoto.com/id/1173276130/photo/wellington-the-beehive-parliament-building-new-zealand.jpg?s=1024x1024&w=is&k=20&c=JHzWgJsoSQR7pntIqH_LAVT6Y4wFwIwN6jyau3Znytk="
        //        }
        //    };
        //    return Ok(region);  
        //}

        //added async heresssssssssssssss
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            //data retrived from the domain modal from db
            var regionsDomain = await dBContext.Regions.OrderByDescending(a => a.Code).ToListAsync();

            //return DTOs to as responce.
            var regionDto = new List<RegionDto>();
            foreach (var regionDomain in regionsDomain)
            {
                regionDto.Add(new RegionDto()
                {
                    Id = regionDomain.Id,
                    Name= regionDomain.Name,
                    Code = regionDomain.Code,
                    RegionImagerl=regionDomain.RegionImagerl
                });
            }
            return Ok(regionDto);
        }

        //get region by Id
        //GET: [pass id to route]
        [HttpGet]
        [Route("{Id:Guid}")]
        public IActionResult GetById([FromRoute]Guid Id)
        {
            //find method only used primary key field
            //var region = dBContext.Regions.Find(Id);

            //data retrived from the domain modal from db
            var regionDomain = dBContext.Regions.FirstOrDefault(x => x.Id == Id);
            if (regionDomain == null)
            {
                return NotFound();
            }

            //return DTOs to as responce.
            var regionDto = new List<RegionDto>();

            regionDto.Add(new RegionDto()
            {
                Id = regionDomain.Id,
                Name = regionDomain.Name,
                Code = regionDomain.Code,
                RegionImagerl = regionDomain.RegionImagerl
            });
            return Ok(regionDto);
        }

        //post verb to create new region
        //Post: is used to add data into the table
        [HttpPost]
        public IActionResult Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            //add validations
            if (ModelState.IsValid)
            {
                //Map Dto to domain modal
                var regionDomainModal = new Region
                {
                    Code = addRegionRequestDto.Code,
                    Name = addRegionRequestDto.Name,
                    RegionImagerl = addRegionRequestDto.RegionImagerl
                };

                //Use domain model to create region
                dBContext.Regions.Add(regionDomainModal);
                dBContext.SaveChanges();

                //map domain modal back to dto
                var regionDto = new RegionDto
                {
                    Id = regionDomainModal.Id,
                    Name = regionDomainModal.Name,
                    Code = regionDomainModal.Code,
                    RegionImagerl = regionDomainModal.RegionImagerl
                };
                return CreatedAtAction(nameof(GetById), new { Id = regionDto.Id }, regionDto);
                //return Ok(regionDomainModal);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        //Put: is used to update the existing region
        //PUT: verb is used to change the data
        [HttpPut]
        [Route("{Id:Guid}")]
        public IActionResult Update([FromRoute] Guid Id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            var regionDomainModal = dBContext.Regions.FirstOrDefault(x => x.Id == Id);
            if (regionDomainModal == null)
            {
                return NotFound();
            }
            //Map Dto to domain modal
            regionDomainModal.Code = updateRegionRequestDto.Code;
            regionDomainModal.Name = updateRegionRequestDto.Name;
            regionDomainModal.RegionImagerl = updateRegionRequestDto.RegionImagerl; 
            //update chnages
            dBContext.SaveChanges();

            //time to convert domain modal to dto
            var regionDto = new RegionDto
            {
                Id = regionDomainModal.Id,
                Code = regionDomainModal.Code,
                Name = regionDomainModal.Name,
                RegionImagerl = regionDomainModal.RegionImagerl
            };
            return Ok(regionDto);
        }

        //Delete is used to remove entry from the db
        //DELETE: verb is used
        [HttpDelete]
        [Route("{Id:Guid}")]
        public IActionResult Delete([FromRoute] Guid Id)
        {
            var regionDomainMmodel = dBContext.Regions.FirstOrDefault(x => x.Id == Id);
            //var regionDomainMmodels = dBContext.Regions.FromSqlRaw("");
            if (regionDomainMmodel == null)
            {
                return NotFound();
            }

            //delete 
            dBContext.Regions.Remove(regionDomainMmodel);
            dBContext.SaveChanges();

            //return deleted object back
            //map to the dto from the domainModel-+*
            var regionDto = new RegionDto
            {
                Id = regionDomainMmodel.Id,
                Code = regionDomainMmodel.Code,
                Name = regionDomainMmodel.Name,
                RegionImagerl = regionDomainMmodel.RegionImagerl
            };
            return Ok(regionDto);
        }
    }
}
