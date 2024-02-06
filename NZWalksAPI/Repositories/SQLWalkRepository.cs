using Microsoft.EntityFrameworkCore;
using NZWalksAPI.Data;
using NZWalksAPI.Models.Domain;

namespace NZWalksAPI.Repositories
{
    public class SQLWalkRepository : IWalkRepository
    {
        //inherit Iwalks Repo
        private readonly NZWalksDBContext dBContext;
        
        //inject dbContext
        public SQLWalkRepository(NZWalksDBContext dBContext)
        {
            this.dBContext = dBContext;
        }
        public async Task<Walk> CreateAsync(Walk walk)
        {
            await dBContext.Walks.AddAsync(walk);
            dBContext.SaveChanges();
            return walk;
        }

        public async Task<List<Walk>> GetAllWalksAsync()
        {
            return await dBContext.Walks.Include("Difficulty").Include("Region").ToListAsync();
        }

        public async Task<Walk?> GetByIdAsync(Guid id)
        {
            return await dBContext.Walks.Include("Difficulty").Include("Region").FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Walk?> UpdateAsync(Guid id, Walk walk)
        {
            var ExistingDomainValues =await dBContext.Walks.FirstOrDefaultAsync(x => x.Id == id);

            if (ExistingDomainValues == null)
            {
                return null;
            }

            ExistingDomainValues.Name = walk.Name;
            ExistingDomainValues.Description = walk.Description;
            ExistingDomainValues.LengthInKm = walk.LengthInKm;
            ExistingDomainValues.WalkImageUrl= walk.WalkImageUrl;
            ExistingDomainValues.RegionId= walk.RegionId;
            ExistingDomainValues.DifficultyId= walk.DifficultyId;

            await dBContext.SaveChangesAsync();

            return ExistingDomainValues;
        }


        public async Task<Walk?> DeleteAsync(Guid id)
        {
            var ExistingDomainModal = await dBContext.Walks.FirstOrDefaultAsync(x => x.Id == id);

            if(ExistingDomainModal == null)
            {
                return null;
            }

            dBContext.Remove(ExistingDomainModal);
            await dBContext.SaveChangesAsync();

            return ExistingDomainModal;
        }
    }
}
