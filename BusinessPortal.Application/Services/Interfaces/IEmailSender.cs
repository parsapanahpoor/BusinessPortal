using BusinessPortal.Domain.Entities.SiteSetting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessPortal.Application.Services.Interfaces
{
    public interface IEmailSender
    {
        Task<bool> SendEmail(string to, string subject, string body);
        Task<EmailSetting> GetDefaultEmailSetting();
    }
}
