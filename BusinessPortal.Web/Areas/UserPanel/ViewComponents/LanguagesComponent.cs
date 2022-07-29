using BusinessPortal.Application.Services.Implementation;
using BusinessPortal.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BusinessPortal.Web.Areas.UserPanel.ViewComponents
{
    public class LanguagesComponent : ViewComponent
    {
        #region Ctor

        private ILanguageService _language;
        public LanguagesComponent(ILanguageService language)
        {
            _language = language;
        }

        #endregion

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return await Task.FromResult((IViewComponentResult)View("LanguagesComponent", await _language.GetListOfLangauge()));
        }
    }
}
