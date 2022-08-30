using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nop.Core;
using Nop.Data;
using Nop.Services.Localization;
using XcellenceIT.Plugin.Misc.NewCRUD.Domain;

namespace XcellenceIT.Plugin.Misc.NewCRUD.Services
{
    public partial class NewCRUDModelServices : INewCRUDModelServices
    {
        private readonly IRepository<NewCRUDDomain> _newCRUDRepository;
        private readonly ILocalizationService _localizationService;

        public NewCRUDModelServices(IRepository<NewCRUDDomain> newCRUDRepository, ILocalizationService localizationService)
        {
            _newCRUDRepository = newCRUDRepository;
            _localizationService = localizationService;
        }

        public virtual async Task<IPagedList<NewCRUDDomain>> GetAllUsersAsync(
            string search_FirstName = null,
            string search_LastName = null,
            int serch_Age = 0,
            string search_City = null,
            int pageIndex = 0,
            int pageSize = int.MaxValue
            )
        {
            return await _newCRUDRepository.GetAllPagedAsync(query =>
            {
                if (!string.IsNullOrEmpty(search_FirstName))
                    query = query.Where(query => query.FirstName == search_FirstName);

                if (!string.IsNullOrEmpty(search_LastName))
                    query = query.Where(query => query.LastName == search_LastName);

                if (serch_Age != 0)
                    query = query.Where(query => query.Age == serch_Age);

                if (!string.IsNullOrEmpty(search_City))
                    query = query.Where(query => query.City == search_City);

                return query;
            }, pageIndex, pageSize);
        }

        public virtual async Task InsertUser(NewCRUDDomain newCRUDDomain)
        {
            await _newCRUDRepository.InsertAsync(newCRUDDomain);
        }

        public virtual async Task UpdateUsersAsync(NewCRUDDomain newCRUDDomain)
        {
            await _newCRUDRepository.UpdateAsync(newCRUDDomain);

        }
        public virtual async Task DeleteUsersAsync(NewCRUDDomain newCRUDDomain)
        {
            await _newCRUDRepository.DeleteAsync(newCRUDDomain);
        }

        public virtual async Task<IList<NewCRUDDomain>> GetUsersByIdsAsync(int[] userIds)
        {
            return await _newCRUDRepository.GetByIdsAsync(userIds, cache => default);
        }

        public virtual async Task<NewCRUDDomain> GetUserByIdAsync(int userId)
        {
            return await _newCRUDRepository.GetByIdAsync(userId, cache => default);
        }

        public virtual async Task<IList<NewCRUDDomain>> GetAllUserAsync()
        {
            var usrs = await GetAllUsersAsync(string.Empty);

            return usrs;
        }
    }
}
