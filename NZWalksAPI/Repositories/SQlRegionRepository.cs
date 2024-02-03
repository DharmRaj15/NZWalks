using Microsoft.EntityFrameworkCore;
using NZWalksAPI.Data;
using NZWalksAPI.Models.Domain;

namespace NZWalksAPI.Repositories
{
    public class SQlRegionRepository : IRegionRepository
    {
        private NZWalksDBContext dbContext { get; }
        public SQlRegionRepository(NZWalksDBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<Region>> GetAllAsyc()
        {
            return await dbContext.Regions.ToListAsync();
        }

        public async Task<Region?> GetByID(Guid id)
        {
            return await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Region> Create(Region region)
        {
            await dbContext.Regions.AddAsync(region);
            await dbContext.SaveChangesAsync();
            return region;
        }

        public async Task<Region?> Update(Guid Id,Region region)
        {
            var existingRegion = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == Id);

            if(existingRegion == null)
            {
                return null;
            }
            existingRegion.Id = Id;
            existingRegion.Code = region.Code;
            existingRegion.Name = region.Name;
            existingRegion.RegionImagerl = region.RegionImagerl;

            await dbContext.SaveChangesAsync();
            return existingRegion;
        }

        public async Task<Region?> Delete(Guid Id)
        {
            var deleteRegion = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == Id);

            if (deleteRegion == null)
            {
                return null;
            }

            dbContext.Regions.Remove(deleteRegion);
            await dbContext.SaveChangesAsync();

            return deleteRegion;
        }
    }
}
