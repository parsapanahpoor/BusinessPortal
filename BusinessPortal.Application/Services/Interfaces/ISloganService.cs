using BusinessPortal.Domain.ViewModels.Admin.Slogan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessPortal.Application.Services.Interfaces
{
    public interface ISloganService
    {
        #region Admin Side 

        //Fill Add Or Edit Slogan Admin Side 
        Task<CreateOrEditSloganViewModel> FillCreateOrEditSloganViewModel();

        //Create Or Edit Slogan 
        Task<bool> CreateOrEditSlogan(CreateOrEditSloganViewModel model);

        #endregion

        #region Site Side 

        //Get Slogan 
        Task<Domain.ViewModels.Site.Slogan.Slogan?> GetSlogan();

        #endregion
    }
}
