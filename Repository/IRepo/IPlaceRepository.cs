using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepo
{
    public interface IPlaceRepository
    {
        List<Place> GetAllPlace(Guid Id);
        Task<List<Place>> GetAllPlaceByHostID(string Id);
        Task<Place> GetAllPlaceByHostIDAndPlaceID(string HostId, Guid placeId);
        Task<Place> GetPlaceByPlaceID(Guid placeId);
        Task AddNew(Place place);
        Task Update(Place place);
        Task Remove(Place place);
    }
}
