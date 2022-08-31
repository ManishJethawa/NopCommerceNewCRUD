using System.Collections.Generic;
using Nop.Web.Framework.Models;

namespace XcellenceIT.Plugin.Misc.NewCRUD.Models
{
    public partial record NewCRUDNavigationModel : BaseNopModel
    {
        public NewCRUDNavigationModel()
        {
            NewCRUDNavigationItems = new List<NewCRUDNavigationItemModel>();
        }
        public IList<NewCRUDNavigationItemModel> NewCRUDNavigationItems { get; set; }

        public int SelectedTab { get; set; }
    }
    public record NewCRUDNavigationItemModel : BaseNopModel
    {
        public string RouteName { get; set; }
        public string Title { get; set; }
        public int Tab { get; set; }
        public string ItemClass { get; set; }
    }
    public enum NewCRUDNavigationEnum
    {
        MYCATAGORY = 160
    }
}
