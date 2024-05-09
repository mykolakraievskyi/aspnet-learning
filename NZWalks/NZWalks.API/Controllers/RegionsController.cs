using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;
        private readonly IRegionRepository regionRepository;

        public RegionsController(NZWalksDbContext dbContext, IRegionRepository regionRepository)
        {
            this.dbContext = dbContext;
            this.regionRepository = regionRepository;
        }
        // GET ALL REGIONS
        // GET: https://localhost:port/api/regions
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            // Get data from database - domain models
            var regionsDomain = await regionRepository.GetAllAsync();

            // Map domain models to DTO
            var regionsDto = new List<RegionDto>();
            foreach (var regionDomain in regionsDomain)
            {
                regionsDto.Add(new RegionDto()
                {
                    Id = regionDomain.Id, 
                    Name = regionDomain.Name,
                    Code = regionDomain.Code,
                    RegionImageUrl = regionDomain.RegionImageUrl
                });
            }

            // Return DTO
            return Ok(regionsDto);
        }

        // GET SINGLE REGION (Get region by id)
        // GET: https://localhost:port/api/regions/{id}
        [HttpGet]
        [Route("{id:Guid}")] // make sure that the name here matches the name in the method parameters
        public async Task<IActionResult> GetById([FromRoute] Guid id) 
        {
            //var region = _dbContext.Regions.Find(id); // only takes primary key

            // Get region domain model from DB
            var regionDomain = await regionRepository.GetByIdAsync(id);

            if (regionDomain == null)
            {
                return NotFound();
            }

            // Map/Convert Region Domain Model to DTO
            var regionDto = new RegionDto()
            {
                Id = regionDomain.Id,
                Name = regionDomain.Name,
                Code = regionDomain.Code,
                RegionImageUrl = regionDomain.RegionImageUrl
            };

            // Return the DTO back to client
            return Ok(regionDto);
        }

        // POST To create new region
        // POST: https://localhost:port/api/regions
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            // Map or Convert DTO to domain model
            var regionDomainModel = new Region()
            {
                Code = addRegionRequestDto.Code,
                Name = addRegionRequestDto.Name,
                RegionImageUrl = addRegionRequestDto.RegionImageUrl
            };

            // Use Domain model to create region
            regionDomainModel = await regionRepository.CreateAsync(regionDomainModel);

            // Map domain model to DTO
            var regionDto = new RegionDto()
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };

            // 201 to give a location of a resource
            return CreatedAtAction(nameof(GetById), new { id = regionDto.Id}, regionDto);
        }

        // Update region
        // PUT: https://localhost:port/api/regions/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            var regionDomainModel = new Region()
            {
                Code = updateRegionRequestDto.Code,
                Name = updateRegionRequestDto.Name,
                RegionImageUrl = updateRegionRequestDto.RegionImageUrl
            };
            // Check whether the region exists
            regionDomainModel =  await regionRepository.UpdateAsync(id, regionDomainModel);
            if (regionDomainModel == null)
            {
                return NotFound();
            }

            //Convert domain model to DTO
            var regionDto = new RegionDto() 
            { 
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };

            return Ok(regionDto);
        }

        // Delete Region
        // DELETE: https://localhost:port/api/regions/{id}
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var regionDomainModel = await regionRepository.DeleteAsync(id);
            if ( regionDomainModel == null)
            {
                return NotFound();
            }

            // Delete region

            // (optional) return the deleted region
            // map Domain model to dto
            var regionDto = new RegionDto()
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };

            return Ok(regionDto);
        }
    }
}
