using BusinessPortal.Data.DbContext;
using BusinessPortal.Domain.Entities.Contact;
using BusinessPortal.Domain.Interfaces;
using BusinessPortal.Domain.ViewModels.Admin.Ticket;
using BusinessPortal.Domain.ViewModels.User.Ticket;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessPortal.Data.Repository
{
    public class TicektRepository : ITicketRepository
    {
        #region Ctor

        private readonly BusinessPortalDbContext _context;

        public TicektRepository(BusinessPortalDbContext context)
        {
            _context = context;
        }

        #endregion

        #region Admin Side

        public async Task<List<TicketMessage>> GetTicketMessages(ulong ticketId)
        {
            var messages = await _context.TicketMessages
                .Include(s => s.Ticket)
                .Include(s => s.Sender)
                .Where(s => s.TicketId == ticketId && !s.IsDelete)
                .OrderByDescending(s => s.CreateDate)
                .ToListAsync();

            return messages;
        }

        public async Task ReadTicketByAdmin(Ticket ticket)
        {
            _context.Update(ticket);
            await _context.SaveChangesAsync();
        }

        public async Task<Ticket?> GetTicketById(ulong ticketId)
        {
            return await _context.Tickets
                .Include(s => s.Owner)
                .FirstOrDefaultAsync(s => !s.IsDelete && s.Id == ticketId);
        }

        public async Task<AdminFilterTicketViewModel> FilterAdminTicketViewModel(AdminFilterTicketViewModel filter)
        {
            var query = _context.Tickets
                .Include(s => s.Owner)
                .Where(s => !s.IsDelete)
                .OrderByDescending(s => s.CreateDate)
                .AsQueryable();

            #region State

            switch (filter.AdminTicketFilterSeenByUserStatus)
            {
                case AdminTicketFilterSeenByUserStatus.All:
                    break;
                case AdminTicketFilterSeenByUserStatus.SeenByUser:
                    query = query.Where(s => s.IsReadByOwner);
                    break;
                case AdminTicketFilterSeenByUserStatus.NotSeenByUser:
                    query = query.Where(s => !s.IsReadByOwner);
                    break;
            }

            switch (filter.AdminTicketFilterSeenByAdminStatus)
            {
                case AdminTicketFilterSeenByAdminStatus.All:
                    break;
                case AdminTicketFilterSeenByAdminStatus.SeenByAdmin:
                    query = query.Where(s => s.IsReadByAdmin);
                    break;
                case AdminTicketFilterSeenByAdminStatus.NotSeenByAdmin:
                    query = query.Where(s => !s.IsReadByAdmin);
                    break;
            }

            switch (filter.AdminTicketFilterStatus)
            {
                case AdminTicketFilterStatus.All:
                    break;
                case AdminTicketFilterStatus.Answered:
                    query = query.Where(s => s.IsReadByOwner);
                    break;
                case AdminTicketFilterStatus.Closed:
                    query = query.Where(s => !s.IsReadByOwner);
                    break;
                case AdminTicketFilterStatus.Pending:
                    query = query.Where(s => !s.IsReadByOwner);
                    break;
            }

            switch (filter.AdminTicketFilterOnWorkingStatus)
            {
                case AdminTicketFilterOnWorkingStatus.All:
                    break;
                case AdminTicketFilterOnWorkingStatus.OnWorking:
                    query = query.Where(s => s.OnWorking);
                    break;
                case AdminTicketFilterOnWorkingStatus.NotWorking:
                    query = query.Where(s => !s.OnWorking);
                    break;
            }

            #endregion

            #region Filter

            if (!string.IsNullOrEmpty(filter.OwnerName))
            {
                query = query.Where(s => (s.Owner.Username).Contains(filter.OwnerName));
            }

            if (!string.IsNullOrEmpty(filter.TicketTitle))
            {
                query = query.Where(s => s.Title.Contains(filter.TicketTitle));
            }

            if (!string.IsNullOrEmpty(filter.UserEmail))
            {
                query = query.Where(s => s.Owner.Email.Contains(filter.UserEmail));
            }

            #endregion

            await filter.Paging(query);

            return filter;
        }

        public async Task AddTicket(Ticket ticket)
        {
            await _context.AddAsync(ticket);
            await _context.SaveChangesAsync();
        }

        public async Task AddMessage(TicketMessage message)
        {
            await _context.AddAsync(message);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTicket(Ticket ticket)
        {
            _context.Tickets.Update(ticket);
            await _context.SaveChangesAsync();
        }

        public async Task<TicketMessage?> GetTicketMessageById(ulong messageId)
        {
            return await _context.TicketMessages
                .FirstOrDefaultAsync(s => s.Id == messageId && !s.IsDelete);
        }

        public async Task UpdateTicketMessage(TicketMessage ticketMessage)
        {
            _context.TicketMessages.Update(ticketMessage);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Ticket>> GetLastestTicketForAdminDashboard()
        {
            return await _context.Tickets.Where(p => !p.IsDelete).Take(5).ToListAsync();
        }

        #endregion

        #region User Side

        public async Task<FilterSiteTicketViewModel> FilterSiteTicket(FilterSiteTicketViewModel filter, ulong userId)
        {
            var query = _context.Tickets
                .Include(s => s.Owner)
                .Where(s => s.OwnerId == userId && !s.IsDelete)
                .OrderByDescending(s => s.CreateDate)
                .AsQueryable();

            #region State

            switch (filter.UserTicketFilterStatus)
            {
                case UserTicketFilterStatus.All:
                    break;
                case UserTicketFilterStatus.Answered:
                    query = query.Where(s => s.IsReadByOwner);
                    break;
                case UserTicketFilterStatus.Closed:
                    query = query.Where(s => !s.IsReadByOwner);
                    break;
                case UserTicketFilterStatus.Pending:
                    query = query.Where(s => !s.IsReadByOwner);
                    break;
            }

            switch (filter.UserTicketFilterOnWorkingStatus)
            {
                case UserTicketFilterOnWorkingStatus.All:
                    break;
                case UserTicketFilterOnWorkingStatus.OnWorking:
                    query = query.Where(s => s.OnWorking);
                    break;
                case UserTicketFilterOnWorkingStatus.NotWorking:
                    query = query.Where(s => !s.OnWorking);
                    break;
            }

            #endregion

            #region Filter

            if (!string.IsNullOrEmpty(filter.TicketTitle))
            {
                query = query.Where(s => s.Title.Contains(filter.TicketTitle));
            }

            #endregion

            await filter.Paging(query);

            return filter;
        }

        #endregion
    }
}
