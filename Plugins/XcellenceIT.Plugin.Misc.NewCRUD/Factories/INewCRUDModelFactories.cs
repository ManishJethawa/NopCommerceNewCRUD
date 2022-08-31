using System.Threading.Tasks;
using XcellenceIT.Plugin.Misc.NewCRUD.Domain;
using XcellenceIT.Plugin.Misc.NewCRUD.Models;

namespace XcellenceIT.Plugin.Misc.NewCRUD.Factories
{
    public interface INewCRUDModelFactories
    {
        Task<NewCRUDSearchModel> PreapareNewCRUDSearchModelAsync(NewCRUDSearchModel searchModel);

        Task<NewCRUDListModel> PreapareNewCRUDListModelAsync(NewCRUDSearchModel searchModel);

        Task<NewCRUDModel> PrepareNewCRUDModelAsync(NewCRUDModel newCRUDModel, NewCRUDDomain newCRUDDomain);
       
        Task<NewCRUDNavigationModel> PrepareCustomerNavigationModelAsync(int selectedTabId = 0);
    }
}
