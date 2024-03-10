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

        public async Task<List<Walk>> GetAllWalksAsync(string? filterOn = null, string? filterQuery = null, 
            string? sortBy = null, bool isAccending = true, int pageNumber = 1, int pageSize = 1000)
        { 
            var walks = dBContext.Walks.Include("Difficulty").Include("Region").AsQueryable();

            //pagination it will be after order by
            //walks = walks.Skip((1 - 1) * 1).Take(10);

            //filterring 
            if(string.IsNullOrWhiteSpace(filterOn)==false && string.IsNullOrWhiteSpace(filterQuery) == false)
            {
                if (filterOn.Equals("Name", StringComparison.CurrentCultureIgnoreCase))
                {
                    walks = walks.Where(x => x.Name.Contains(filterQuery));
                }
                if (filterOn.Equals("Description", StringComparison.CurrentCultureIgnoreCase))
                {
                    walks = walks.Where(x => x.Description.Contains(filterQuery));
                }
                if (filterOn.Equals("LengthInKm", StringComparison.CurrentCultureIgnoreCase))
                {
                    walks = walks.Where(x => x.LengthInKm == Convert.ToDouble(filterQuery));
                }
            }

            //Sorting fucntionality
            if (string.IsNullOrWhiteSpace(sortBy) == false)
            {
                if (sortBy.Equals("Name", StringComparison.CurrentCultureIgnoreCase))
                {
                    walks = isAccending ? walks.OrderBy(x => x.Name) : walks.OrderByDescending(x => x.Name);
                }
                else if (sortBy.Equals("Length", StringComparison.CurrentCultureIgnoreCase))
                {
                    walks = isAccending ? walks.OrderBy(x => x.LengthInKm) : walks.OrderByDescending(x => x.LengthInKm);
                }
            }

            //Pagination
            var skipResult = (pageNumber - 1) * pageSize;
            return await walks.ToListAsync();
            //giving error 
            //return await walks.Skip(skipResult).Take(pageSize).ToListAsync();
            //return await dBContext.Walks.Include("Difficulty").Include("Region").ToListAsync();
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
