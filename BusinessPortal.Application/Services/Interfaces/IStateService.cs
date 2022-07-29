using BusinessPortal.Domain.Entities.Location;
using BusinessPortal.Domain.ViewModels.Admin.State;
using BusinessPortal.Domain.ViewModels.Common;
using BusinessPortal.Domain.ViewModels.UserPanel;
using BusinessPortal.Domain.ViewModels.UserPanel.Location;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessPortal.Application.Services.Interfaces
{
    public interface IStateService
    {
        Task<Domain.Entities.Address.Address?> GetAddressByAddressId(ulong AddressId);

        Task<State> GetStateById(ulong stateId);

        Task<List<SelectListViewModel>> GetStateChildren(ulong stateId);

        Task<List<SelectListViewModel>> GetAllCountries();

        Task<bool> IsExistsStateById(ulong stateId);

        Task<CreateStateResult> CreateState(CreateStateViewModel stateViewModel , IFormFile? StateImage);

        Task<FilterStateViewModel> FilterState(FilterStateViewModel filter);

        Task<EditStateViewModel> FillEditStateViewModel(ulong stateId);

        Task<EditStateResult> EditState(EditStateViewModel stateViewModel, IFormFile? StateImage);

        Task<bool> DeleteState(ulong stateId);

        #region User Panel

        Task<FilterUserAddressViewModel> FilterAddresses(FilterUserAddressViewModel filter);

        List<SelectListItem> GetCountriesForDropdown();

        Task<CreateAddressFormUserPanelResult> CreateAddressByUser(CreateAddressFromUserPanel CreateAddressFromUserPanel);

        List<SelectListItem> GetSubLocationForDropDown(ulong locationId);

        Task<EditAddressViewModel> GetAddressForEditByUser(ulong addressId);

        Task<ulong> GetUserIdByAddressId(ulong addressId);

        Task<EditAddressResult> EditAddressByUser(EditAddressViewModel editAddressViewModel);

        Task<bool> SoftDeleteAddressByUser(ulong addressId);

        List<SelectListItem> GetUserAddressDrwopDown(ulong UserID);

        #endregion

    }
}
