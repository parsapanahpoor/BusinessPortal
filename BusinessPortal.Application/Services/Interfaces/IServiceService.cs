using BusinessPortal.Domain.Entities.Product;
using BusinessPortal.Domain.Entities.Services;
using BusinessPortal.Domain.ViewModels.Admin.Service;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessPortal.Application.Services.Interfaces
{
    public interface IServiceService
    {
        #region Admin 

        #region Service Category

        //Get Main Categories For Show In Site Header
        Task<List<ServicesCategoryInfo>> GetMaiCategoriesForShowInSiteHeader();

        Task<FilterServiceCategoryViewModel> FilterServiceCategory(FilterServiceCategoryViewModel filter);

        //Get Service Category By Service Category Id 
        Task<ServicesCategory?> GetServiceCategoryById(ulong serviceCategoryId);

        //Create Service Category
        Task<CreateServicecCategoryResult> CreateServiceCategory(CreateServiceCategoryViewModel serviceCategory, IFormFile? serviceCategoryImage);

        //Fill Edit Service Category Info
        Task<EditServiceCategoryViewModel?> FillServiceArticleCategoryViewModel(ulong serviceCategoryId);

        //Edit Service Group
        Task<EditServiceCategoryResult> EditService(EditServiceCategoryViewModel serviceCategory, IFormFile? serviceCategoryImage);

        //Delete Service Category
        Task<bool> DeleteServiceCategory(ulong serviceCategoryId);

        #endregion

        #endregion
    }
}
