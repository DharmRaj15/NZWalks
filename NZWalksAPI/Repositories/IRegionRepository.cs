using NZWalksAPI.Models.Domain;

namespace NZWalksAPI.Repositories
{
    public interface IRegionRepository
    {
        //get all data from the DB
        Task<List<Region>> GetAllAsyc();
        
        //get data by guid
        Task<Region?> GetByID(Guid id);
        
        //insert data into the db
        Task<Region> Create(Region region);
        
        //update data into db
        Task<Region?> Update(Guid Id, Region region);

        Task<Region?> Delete(Guid Id);
    }
}
