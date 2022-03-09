using BusinessPortal.Domain.Entities.Location;
using BusinessPortal.Domain.ViewModels.Admin.State;
using BusinessPortal.Domain.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessPortal.Application.Services.Interfaces
{
    public interface IStateService
    {
        Task<State> GetStateById(ulong stateId);

        Task<List<SelectListViewModel>> GetStateChildren(ulong stateId);

        Task<List<SelectListViewModel>> GetAllCountries();

        Task<bool> IsExistsStateById(ulong stateId);

        Task<CreateStateResult> CreateState(CreateStateViewModel stateViewModel);

        Task<FilterStateViewModel> FilterState(FilterStateViewModel filter);

        Task<EditStateViewModel> FillEditStateViewModel(ulong stateId);

        Task<EditStateResult> EditState(EditStateViewModel stateViewModel);

        Task<bool> DeleteState(ulong stateId);
    }
}
