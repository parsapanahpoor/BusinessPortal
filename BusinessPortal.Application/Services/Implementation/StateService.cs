using BusinessPortal.Application.Extensions;
using BusinessPortal.Application.Security;
using BusinessPortal.Application.Services.Interfaces;
using BusinessPortal.Application.StaticTools;
using BusinessPortal.Data.DbContext;
using BusinessPortal.Domain.Entities.Location;
using BusinessPortal.Domain.ViewModels.Admin.State;
using BusinessPortal.Domain.ViewModels.Common;
using BusinessPortal.Domain.ViewModels.UserPanel;
using BusinessPortal.Domain.ViewModels.UserPanel.Location;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessPortal.Application.Services.Implementation
{
    public class StateService : IStateService
    {
        #region Ctor

        public BusinessPortalDbContext _context { get; set; }

        public StateService(BusinessPortalDbContext context)
        {
            _context = context;
        }

        #endregion

        public async Task<Domain.Entities.Address.Address?> GetAddressByAddressId(ulong AddressId)
        {
            return await _context.Addresses.FirstOrDefaultAsync(p => p.Id == AddressId);
        }

        public Task<State?> GetStateById(ulong stateId)
        {
            return _context.States.FirstOrDefaultAsync(s => !s.IsDelete && s.Id == stateId);
        }

        public async Task<List<SelectListViewModel>> GetStateChildren(ulong stateId)
        {
            return await _context.States.Where(s => s.ParentId.HasValue && s.ParentId.Value == stateId && !s.IsDelete)
                .Select(s => new SelectListViewModel
                {
                    Id = s.Id,
                    Title = s.Title
                }).ToListAsync();
        }

        public async Task<List<SelectListViewModel>> GetAllCountries()
        {
            return await _context.States.Where(s => s.ParentId == null && !s.IsDelete)
                .Select(s => new SelectListViewModel
                {
                    Id = s.Id,
                    Title = s.Title
                }).ToListAsync();
        }

        public async Task<bool> IsExistsStateById(ulong stateId)
        {
            return await _context.States.AnyAsync(s => s.Id == stateId && !s.IsDelete);
        }

        public async Task<CreateStateResult> CreateState(CreateStateViewModel stateViewModel, IFormFile? StateImage)
        {
            if (await _context.States.AnyAsync(a => a.UniqueName == stateViewModel.UniqueName && !a.IsDelete))
            {
                return CreateStateResult.UniqNameIsExist;
            }

            if (stateViewModel.ParentId.HasValue && stateViewModel.ParentId.Value != 0)
            {
                var parent = await _context.States.FirstOrDefaultAsync(p => !p.IsDelete && p.Id == stateViewModel.ParentId);

                if (parent == null) return CreateStateResult.Fail;
            }

            if (!stateViewModel.ParentId.HasValue && StateImage == null)
            {
                return CreateStateResult.ImageNotFound;
            }

            var state = new State()
            {
                Title = stateViewModel.Title,
                UniqueName = stateViewModel.UniqueName,
            };

            if (stateViewModel.ParentId != null && stateViewModel.ParentId != 0)
            {
                state.ParentId = stateViewModel.ParentId;
            }

            #region State Image

            if (StateImage != null)
            {
                var imageName = Guid.NewGuid() + Path.GetExtension(StateImage.FileName);
                StateImage.AddImageToServer(imageName, PathTools.StateimageServerOrigin, 25, 25);
                state.IconeName = imageName;
            }

            #endregion

            await _context.States.AddAsync(state);
            await _context.SaveChangesAsync();


            return CreateStateResult.Success;
        }

        public async Task<FilterStateViewModel> FilterState(FilterStateViewModel filter)
        {
            var query = _context.States
                .Include(a => a.Parent)
                .Where(a => !a.IsDelete)
                .OrderByDescending(s => s.CreateDate)
                .AsQueryable();

            #region State

            switch (filter.StateStatus)
            {
                case StateStatus.All:
                    break;
                case StateStatus.NotDeleted:
                    query = query.Where(a => !a.IsDelete);
                    break;
                case StateStatus.Deleted:
                    query = query.Where(a => a.IsDelete);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            #endregion

            #region Filter

            if (!string.IsNullOrEmpty(filter.Title))
            {
                query = query.Where(s => EF.Functions.Like(s.Title, $"%{filter.Title}%"));
            }

            if (!string.IsNullOrEmpty(filter.UniqeName))
            {
                query = query.Where(s => EF.Functions.Like(s.UniqueName, $"%{filter.UniqeName}%"));
            }

            if (filter.ParentId != null)
            {
                query = query.Where(a => a.ParentId == filter.ParentId);
                filter.ParentState = await _context.States.FirstOrDefaultAsync(a => a.Id == filter.ParentId);
            }
            else
            {
                query = query.Where(a => a.ParentId == null);
            }

            #endregion

            await filter.Paging(query);

            return filter;
        }

        public async Task<EditStateViewModel> FillEditStateViewModel(ulong stateId)
        {
            var state = await _context.States.SingleOrDefaultAsync(a => a.Id == stateId);

            if (state == null) return null;

            var result = new EditStateViewModel()
            {
                Title = state.Title,
                UniqueName = state.UniqueName,
                StateId = state.Id,
                ParentId = state.ParentId,
                IconeName = state.IconeName,
            };

            return result;
        }

        public async Task<EditStateResult> EditState(EditStateViewModel stateViewModel , IFormFile? StateImage)
        {
            var state = await _context.States.SingleOrDefaultAsync(a => a.Id == stateViewModel.StateId);

            if (state == null)
            {
                return EditStateResult.Fail;
            }

            if (stateViewModel.ParentId.HasValue && stateViewModel.ParentId.Value != 0)
            {
                var parent = await _context.States.FirstOrDefaultAsync(p => !p.IsDelete && p.Id == stateViewModel.ParentId);

                if (parent == null) return EditStateResult.Fail;
            }

            if (await _context.States.AnyAsync(a => a.UniqueName == stateViewModel.UniqueName && a.Id != state.Id && !a.IsDelete ))
            {
                return EditStateResult.UniqNameIsExist;
            }

            if (!stateViewModel.ParentId.HasValue && state.IconeName == null && StateImage == null)
            {
                return EditStateResult.ImageNotfound;
            }

            #region State Image

            if (StateImage != null)
            {
                    var imageName = Guid.NewGuid() + Path.GetExtension(StateImage.FileName);
                StateImage.AddImageToServer(imageName, PathTools.StateimageServerOrigin, 25 , 25);

                    if (!string.IsNullOrEmpty(state.IconeName))
                    {
                    state.IconeName.DeleteImage(PathTools.StateimageServerOrigin , null);
                    }

                    state.IconeName = imageName;
            }

            #endregion


            state.Title = stateViewModel.Title;
            state.UniqueName = stateViewModel.UniqueName;

            _context.States.Update(state);
            await _context.SaveChangesAsync();

            return EditStateResult.Success;
        }

        public async Task<bool> DeleteState(ulong stateId)
        {
            var state = await _context.States.SingleOrDefaultAsync(a => a.Id == stateId);

            if (state == null)
            {
                return false;
            }

            state.IsDelete = true;

            _context.States.Update(state);
            await _context.SaveChangesAsync();

            return true;
        }

        #region User Panel

        public async Task<FilterUserAddressViewModel> FilterAddresses(FilterUserAddressViewModel filter)
        {
            var query = _context.Addresses
                .Include(p => p.User)
                .Include(s => s.LocationState)
                .Include(s => s.LocationCountry)
                .Include(s => s.LocationCity)
                .Where(p => !p.IsDelete && p.UserId == filter.UserId)
                .AsQueryable();

            #region State

            switch (filter.FilterAddressState)
            {
                case FilterAddressState.All:
                    break;
                case FilterAddressState.Deleted:
                    query = query.Where(s => s.IsDelete);
                    break;
                case FilterAddressState.NotDeleted:
                    query = query.Where(s => !s.IsDelete);
                    break;
            }
            switch (filter.OrderBy)
            {
                case FilterAddressOrder.CreateDate_Dec:
                    query = query.OrderByDescending(s => s.CreateDate);
                    break;
                case FilterAddressOrder.CreateDate_Asc:
                    query = query.OrderBy(s => s.CreateDate);
                    break;
            }

            #endregion

            #region Filter

            if (!string.IsNullOrEmpty(filter.AddresssTitle))
                query = query.Where(s => EF.Functions.Like(s.AddressTitle, $"%{filter.AddresssTitle.Trim()}%"));

            if (!string.IsNullOrEmpty(filter.Mobile))
                query = query.Where(s => EF.Functions.Like(s.Mobile, $"%{filter.Mobile.Trim()}%"));

            if (!string.IsNullOrEmpty(filter.Email))
                query = query.Where(s => EF.Functions.Like(s.Email, $"%{filter.Email.ToLower().Trim()}%"));

            #endregion


            await filter.Paging(query);

            return filter;
        }

        public List<SelectListItem> GetCountriesForDropdown()
        {
            return _context.States
                      .Where(s => s.ParentId == null && !s.IsDelete)
                       .Select(g => new SelectListItem()
                       {
                           Text = g.Title,
                           Value = g.Id.ToString()
                       }).ToList();
        }

        public async Task<CreateAddressFormUserPanelResult> CreateAddressByUser(CreateAddressFromUserPanel CreateAddressFromUserPanel)
        {
            var newAddress = new Domain.Entities.Address.Address()
            {
                UserId = CreateAddressFromUserPanel.UserId,
                CountryId = CreateAddressFromUserPanel.CountryId,
                StateId = CreateAddressFromUserPanel.StateId,
                CityId = CreateAddressFromUserPanel.CityId,
                AddressTitle = CreateAddressFromUserPanel.AddressTitle.SanitizeText(),
                CreateDate = System.DateTime.Now,
                Email = CreateAddressFromUserPanel.Email.SanitizeText(),
                Mobile = CreateAddressFromUserPanel.Mobile.SanitizeText(),
                UserAddress = CreateAddressFromUserPanel.UserAddress,
                IsDelete = false,
            };

            await _context.Addresses.AddAsync(newAddress);
            await _context.SaveChangesAsync();

            return CreateAddressFormUserPanelResult.Success;
        }

        public List<SelectListItem> GetSubLocationForDropDown(ulong locationId)
        {
            return _context.States
                         .Where(s => s.ParentId == locationId && !s.IsDelete)
                          .Select(g => new SelectListItem()
                          {
                              Text = g.Title,
                              Value = g.Id.ToString()
                          }).ToList();
        }

        public async Task<EditAddressViewModel> GetAddressForEditByUser(ulong addressId)
        {
            var address = await _context.Addresses.FindAsync(addressId);

            if (address == null) return null;

            return new EditAddressViewModel()
            {
                AddressId = address.Id,
                AddressTitle = address.AddressTitle,
                CountryId = address.CountryId,
                StateId = address.StateId,
                CityId = address.CityId,
                Email = address.Email,
                Mobile = address.Mobile,
                UserAddress = address.UserAddress,
                CreateDate = address.CreateDate
            };
        }

        public async Task<ulong> GetUserIdByAddressId(ulong addressId)
        {
            return await _context.Addresses
                            .Where(s => s.Id == addressId)
                            .Select(s => s.UserId).FirstOrDefaultAsync();
        }

        public async Task<EditAddressResult> EditAddressByUser(EditAddressViewModel editAddressViewModel)
        {
            var address = await _context.Addresses.FirstOrDefaultAsync(p => !p.IsDelete && p.Id == editAddressViewModel.AddressId);

            if (address == null) return EditAddressResult.NotFound;

            #region Fill Moddel

            address.AddressTitle = editAddressViewModel.AddressTitle.SanitizeText();
            address.UserAddress = editAddressViewModel.UserAddress.SanitizeText();
            address.Mobile = editAddressViewModel.Mobile.SanitizeText();
            address.Email = editAddressViewModel.Email.SanitizeText();
            address.CityId = editAddressViewModel.CityId;
            address.StateId = editAddressViewModel.StateId;
            address.CountryId = editAddressViewModel.CountryId;
            address.CreateDate = editAddressViewModel.CreateDate;
            address.IsDelete = false;

            #endregion


            _context.Addresses.Update(address);
            await _context.SaveChangesAsync();

            return EditAddressResult.Success;
        }

        public async Task<bool> SoftDeleteAddressByUser(ulong addressId)
        {
            var address = await _context.Addresses.FirstOrDefaultAsync(p => !p.IsDelete && p.Id == addressId);

            if (address == null) return false;

            address.IsDelete = true;

            _context.Addresses.Update(address);
            await _context.SaveChangesAsync();

            return true;
        }

        public List<SelectListItem> GetUserAddressDrwopDown(ulong UserID)
        {
            #region English Language

            if (CultureInfo.CurrentCulture.Name == "en-US")
            {
                return _context.Addresses.Where(p => p.UserId == UserID && p.IsDelete == false)
                       .Select(p => new SelectListItem
                       {
                           Text = $"Address Title : {p.AddressTitle} , Mobile : {p.Mobile} ",
                           Value = p.Id.ToString(),
                       }).ToList();
            }

            #endregion

            #region Turkish Language

            if (CultureInfo.CurrentCulture.Name == "tr-TR")
            {
                return _context.Addresses.Where(p => p.UserId == UserID && p.IsDelete == false)
                       .Select(p => new SelectListItem
                       {
                           Text = $"Adres başlığı : {p.AddressTitle} , mobil : {p.Mobile} ",
                           Value = p.Id.ToString(),
                       }).ToList();
            }

            #endregion

            #region Portuguese Language

            if (CultureInfo.CurrentCulture.Name == "pt-PT")
            {
                return _context.Addresses.Where(p => p.UserId == UserID && p.IsDelete == false)
                       .Select(p => new SelectListItem
                       {
                           Text = $"título do endereço : {p.AddressTitle} , Móvel : {p.Mobile} ",
                           Value = p.Id.ToString(),
                       }).ToList();
            }

            #endregion

            #region Arabic Language

            if (CultureInfo.CurrentCulture.Name == "ar-SA")
            {
                return _context.Addresses.Where(p => p.UserId == UserID && p.IsDelete == false)
                       .Select(p => new SelectListItem
                       {
                           Text = $"عنوان عنوان: {p.AddressTitle} , التليفون المحمول : {p.Mobile} ",
                           Value = p.Id.ToString(),
                       }).ToList();
            }

            #endregion

            #region Russian Language

            if (CultureInfo.CurrentCulture.Name == "ru-RU")
            {
                return _context.Addresses.Where(p => p.UserId == UserID && p.IsDelete == false)
                       .Select(p => new SelectListItem
                       {
                           Text = $"Название адреса : {p.AddressTitle} , Мобильный : {p.Mobile} ",
                           Value = p.Id.ToString(),
                       }).ToList();
            }

            #endregion

            #region Persian Language

            return _context.Addresses.Where(p => p.UserId == UserID && p.IsDelete == false)
                        .Select(p => new SelectListItem
                        {
                            Text = $"عنوان آدرس : {p.AddressTitle} و موبایل : {p.Mobile} ",
                            Value = p.Id.ToString(),
                        }).ToList();

            #endregion

        }

        #endregion

    }
}
