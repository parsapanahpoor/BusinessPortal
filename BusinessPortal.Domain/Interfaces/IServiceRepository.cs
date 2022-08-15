using BusinessPortal.Domain.Entities.Product;
using BusinessPortal.Domain.Entities.Services;
using BusinessPortal.Domain.ViewModels.Admin.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessPortal.Domain.Interfaces
{
    public interface IServiceRepository
    {
        #region Admin 

        #region Service Category

        //Get Main Categories For Show In Site Header
        Task<List<ServicesCategoryInfo>> GetMaiCategoriesForShowInSiteHeader();

        Task<FilterServiceCategoryViewModel> FilterServiceCategory(FilterServiceCategoryViewModel filter);

        //Get Service Category By Service Category Id 
        Task<ServicesCategory?> GetServiceCategoryById(ulong serviceCategoryId);

        //Is Exist Service Category By Unique Name
        Task<bool> IsExistServiceCategoryByUniqueName(string uniqueName);

        //Is Exist Any Service Category By Id 
        Task<bool> IsExistServiceCategoryById(ulong serviceCategoryId);

        //Add Service Categories
        Task<ulong> AddServiceCategory(ServicesCategory serviceCategory);

        //Add Service Category Info
        Task AddServiceCategoryInfo(List<ServicesCategoryInfo> serviceCategoryInfos);

        //Fill Edit Service Category Info
        Task<EditServiceCategoryViewModel?> FillServiceArticleCategoryViewModel(ulong serviceCategoryId);

        //Update Service Category
         void UpdateServiceCategory(ServicesCategory serviceCategory);

        //Update Service Category Info
        void UpdateServiceCategoryInfo(ServicesCategoryInfo serviceCategoryInfo);

        //Delete Service Category Info
        Task DeleteServiceCategoryInfo(ulong serviceCategoryId);

        //Save Changes
        Task Savechanges();

        //Delete Service Category And Service Category Info
        Task DeleteServiceCategory(ServicesCategory serviceCategory);

        #endregion

        #endregion
    }
}
