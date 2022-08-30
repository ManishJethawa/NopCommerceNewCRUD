using System.Collections.Generic;
using System.Threading.Tasks;
using Nop.Core;
using XcellenceIT.Plugin.Misc.NewCRUD.Domain;

namespace XcellenceIT.Plugin.Misc.NewCRUD.Services
{
    public partial interface INewCRUDModelServices
    {
        Task<IPagedList<NewCRUDDomain>> GetAllUsersAsync(

            string search_FirstName = null,
            string search_LastName = null,
            int serch_Age = 0,
            string search_City = null,
            int pageIndex = 0,
            int pageSize = int.MaxValue
            );
        Task InsertUser(NewCRUDDomain newCRUDDomain);

        Task<IList<NewCRUDDomain>> GetAllUserAsync();

        Task<IList<NewCRUDDomain>> GetUsersByIdsAsync(int[] userIds);

        Task<NewCRUDDomain> GetUserByIdAsync(int userId);

        Task UpdateUsersAsync(NewCRUDDomain newCRUDDomain);

        Task DeleteUsersAsync(NewCRUDDomain newCRUDDomain);

    }
}
