using System;
using System.Linq;
using System.Threading.Tasks;
using Nop.Web.Framework.Models.Extensions;
using XcellenceIT.Plugin.Misc.NewCRUD.Domain;
using XcellenceIT.Plugin.Misc.NewCRUD.Models;
using XcellenceIT.Plugin.Misc.NewCRUD.Services;

namespace XcellenceIT.Plugin.Misc.NewCRUD.Factories
{
    public class NewCRUDModelFactories : INewCRUDModelFactories
    {
        private readonly INewCRUDModelServices _newCRUDModelServices;

        public NewCRUDModelFactories(INewCRUDModelServices newCRUDModelServices)
        {
            _newCRUDModelServices = newCRUDModelServices;
        }

        public virtual Task<NewCRUDSearchModel> PreapareNewCRUDSearchModelAsync(NewCRUDSearchModel newCRUDsearchModel)
        {
            if (newCRUDsearchModel == null)
                throw new ArgumentException(nameof(newCRUDsearchModel));

            newCRUDsearchModel.SetGridPageSize();

            return Task.FromResult(newCRUDsearchModel);

        }

        public virtual Task<NewCRUDModel> PrepareNewCRUDModelAsync(NewCRUDModel newCRUDModel, NewCRUDDomain newCRUDDomain)
        {
            if (newCRUDDomain != null)
            {
                newCRUDModel = newCRUDModel ?? new NewCRUDModel();
                newCRUDModel.Id = newCRUDDomain.Id;
                newCRUDModel.FirstName = newCRUDDomain.FirstName;
                newCRUDModel.LastName = newCRUDDomain.LastName;
                newCRUDModel.Age = newCRUDDomain.Age;
                newCRUDModel.City = newCRUDDomain.City;

            }
            return Task.FromResult(newCRUDModel);
        }

        public virtual async Task<NewCRUDListModel> PreapareNewCRUDListModelAsync(NewCRUDSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            var users = await _newCRUDModelServices.GetAllUsersAsync(
                search_FirstName: searchModel.SearchByFirstName,
                search_LastName: searchModel.SearchByLastName,
                serch_Age: searchModel.SearchByAge,
                search_City: searchModel.SearchByCity,
                pageIndex: searchModel.Page - 1,
                pageSize: searchModel.PageSize
                );

            var model = await new NewCRUDListModel().PrepareToGridAsync(searchModel, users, () =>
            {
                return users.SelectAwait(user =>
                {
                    var usr = new NewCRUDModel()
                    {
                        Id = user.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Age = user.Age,
                        City = user.City
                    };
                    return new ValueTask<NewCRUDModel>(usr);
                });
            });

            return model;
        }
    }
}
