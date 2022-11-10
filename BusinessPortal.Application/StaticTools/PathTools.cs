using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessPortal.Application.StaticTools
{
    public static class PathTools
    {

        #region Default Images

        public static readonly string DefaultStateLogo = "/UploadedImages/DefaultImages/Default.png";

        #endregion

        #region State

        public static string Stateimage = "/UploadedImages/StateImage/";
        public static string StateimageServerOrigin = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UploadedImages/StateImage/");

        #endregion

        #region Site

        public static string SiteFarsiName = "BussinesPortasl";
        public static string SiteAddress = "https://localhost:7286";

        public static readonly string SiteLogo = "/content/images/site/logo/main/";
        public static readonly string SiteLogoServer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/content/images/site/logo/main/");

        public static readonly string DefaultSiteLogo = "/content/images/site/logo/default/logo.png";
        public static readonly string SiteLogoThumb = "/content/images/site/logo/thumb/";
        public static readonly string SiteLogoThumbServer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/content/images/site/logo/thumb/");

        public static readonly string EmailBanner = "/content/images/site/emailBanner/main/";
        public static readonly string EmailBannerServer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/content/images/site/emailBanner/main/");

        public static readonly string EmailBannerThumb = "/content/images/site/emailBanner/thumb/";
        public static readonly string EmailBannerThumbServer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/content/images/site/emailBanner/thumb/");

        #endregion

        #region UserAvatar

        public static readonly string UserAvatarPath = "/content/images/user/main/";
        public static readonly string UserAvatarPathServer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/content/images/user/main/");

        public static readonly string UserAvatarPathThumb = "/content/images/user/thumb/";
        public static readonly string UserAvatarPathThumbServer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/content/images/user/thumb/");

        public static readonly string DefaultUserAvatar = "/content/images/user/DefaultAvatar.png";

        #endregion

        #region Ckeditor

        public static readonly string UploadCkEditorImagePath = "/content/upload/ckeditor/images/";
        public static readonly string UploadCkEditorImagePathServer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/content/upload/ckeditor/images/");

        #endregion

        #region Advertisement Image

        public static string DefaultAdvertisementimage = "/UploadedImages/DefaultImages/Default.png";

        public static string AdvertisementOriginimage = "/UploadedImages/AdvertisementImages/Origin/";
        public static string AdvertisementThumbimage = "/UploadedImages/AdvertisementImages/Thumb/";
        public static string AdvertisementimageServerOrigin = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UploadedImages/AdvertisementImages/Origin/");
        public static string AdvertisementImageServerThumb = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UploadedImages/AdvertisementImages/Thumb/");
        public static string AdvertisementimageServerOriginThumb = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UploadedImages/AdvertisementImages/Thumb/");

        #endregion

        #region Product Category Image

        public static string DefaultProductCategoryimage = "/UploadedImages/DefaultImages/Default.png";

        public static string ProductCategoryOriginimage = "/UploadedImages/ProductCategories/Origin/";
        public static string ProductCategoryThumbimage = "/UploadedImages/ProductCategories/Thumb/";
        public static string ProductCategoryimageServerOrigin = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UploadedImages/ProductCategories/Origin/");
        public static string ProductCategoryImageServerThumb = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UploadedImages/ProductCategories/Thumb/");
        public static string ProductCategoryimageServerOriginThumb = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UploadedImages/ProductCategories/Thumb/");

        #endregion

        #region Product Category Image

        public static string DefaultServiceCategoryimage = "/UploadedImages/DefaultImages/Default.png";

        public static string ServiceCategoryOriginimage = "/UploadedImages/ServiceCategory/Origin/";
        public static string ServiceCategoryThumbimage = "/UploadedImages/ServiceCategory/Thumb/";
        public static string ServiceCategoryimageServerOrigin = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UploadedImages/ServiceCategory/Origin/");
        public static string ServiceCategoryImageServerThumb = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UploadedImages/ServiceCategory/Thumb/");
        public static string ServiceCategoryimageServerOriginThumb = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UploadedImages/ServiceCategory/Thumb/");

        #endregion

        #region Countries Image

        public static string DefaultCountryimage = "/UploadedImages/DefaultImages/Default.png";

        public static string CountryOriginimage = "/UploadedImages/Country/Origin/";
        public static string CountryThumbimage = "/UploadedImages/Country/Thumb/";
        public static string CountryimageServerOrigin = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UploadedImages/Country/Origin/");
        public static string CountryImageServerThumb = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UploadedImages/Country/Thumb/");
        public static string CountryimageServerOriginThumb = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UploadedImages/Country/Thumb/");

        #endregion

        #region Ads 

        public static string DefaultAdsimage = "/UploadedImages/DefaultImages/Default.png";

        public static string AdsOriginimage = "/UploadedImages/AdsImages/Origin/";
        public static string AdsThumbimage = "/UploadedImages/AdsImages/Thumb/";
        public static string AdsimageServerOrigin = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UploadedImages/AdsImages/Origin/");
        public static string AdsImageServerThumb = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UploadedImages/AdsImages/Thumb/");
        public static string AdsimageServerOriginThumb = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UploadedImages/AdsImages/Thumb/");

        #endregion
    }
}
