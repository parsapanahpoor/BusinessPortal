﻿using BusinessPortal.Domain.Entities.Contact;
using BusinessPortal.Domain.ViewModels.Admin.Ticket;
using BusinessPortal.Domain.ViewModels.User.Ticket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessPortal.Application.Services.Interfaces
{
    public interface ITicketService
    {
        #region Admin 

        Task<AdminFilterTicketViewModel> FilterAdminTicketViewModel(AdminFilterTicketViewModel filter);

        Task<bool> AddTicketFromAdminPanel(AddTicketViewModel addTicket, ulong adminId);

        Task<Ticket?> GetTicketById(ulong ticketId);

        Task ReadTicketByAdmin(Ticket ticket);

        Task<List<TicketMessage>> GetTicketMessages(ulong ticketId);

        Task<bool> CreateAnswerTicketAdmin(AnswerTicketAdminViewModel answer, ulong userId);

        Task<string> ChangeTicketStatus(int state, ulong ticketId);

        Task<bool> ChangeOnWorkingTicketStatus(ulong ticketId);

        Task<bool> DeleteTicketMessage(ulong messageId);

        Task<List<Ticket>> GetLastestTicketForAdminDashboard();

        #endregion

        #region User Side

        Task<FilterSiteTicketViewModel> FilterSiteTicket(FilterSiteTicketViewModel filter, ulong userId);

        Task<ulong> CreateTicket(CreateTicketViewModel create, ulong userId);

        Task<UserPanelTicketDetailViewModel> GetUserPanelTicketDetailViewModel(ulong ticketId, ulong userId);

        Task<bool> AnswerTicketByUser(AnswerTicketViewModel answer, ulong userId);

        #endregion
    }
}
