using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BusinessPortal.Domain.ViewModels.Common
{
    public class BasePaging<T>
    {
        public BasePaging()
        {
            Page = 1;
            TakeEntity = 12;
            HowManyShowPageAfterAndBefore = 5;
            Entities = new List<T>();
        }

        public int Page { get; set; }

        public int PageCount { get; set; }

        public int AllEntitiesCount { get; set; }

        public int StartPage { get; set; }

        public int EndPage { get; set; }

        public int TakeEntity { get; set; }

        public int SkipEntity { get; set; }

        public int HowManyShowPageAfterAndBefore { get; set; }

        public List<T> Entities { get; set; }

        public PagingViewModel GetCurrentPaging()
        {
            return new PagingViewModel
            {
                EndPage = this.EndPage,
                Page = this.Page,
                StartPage = this.StartPage
            };
        }

        public string GetShownEntitiesPagesTitle()
        {
            if (AllEntitiesCount != 0)
            {
                var startItem = 1;
                var endItem = AllEntitiesCount;

                if (EndPage > 1)
                {
                    startItem = (Page - 1) * TakeEntity + 1;
                    endItem = Page * TakeEntity > AllEntitiesCount ? AllEntitiesCount : Page * TakeEntity;
                }

                if (CultureInfo.CurrentCulture.Name == "en-US")
                {
                    return $"Show {startItem} Until {endItem} From {AllEntitiesCount}";
                }
                if (CultureInfo.CurrentCulture.Name == "ru-RU")
                {
                    return $"Шоу {startItem} До того как {endItem} От {AllEntitiesCount}";
                } 
                if (CultureInfo.CurrentCulture.Name == "ar-SA")
                {
                    return $"يعرض {startItem} حتى{endItem} من عند {AllEntitiesCount}";
                } 
                if (CultureInfo.CurrentCulture.Name == "tr-TR")
                {
                    return $"Göstermek {startItem} Değin {endItem} İtibaren {AllEntitiesCount}";
                }
                if (CultureInfo.CurrentCulture.Name == "pt-PT")
                {
                    return $"exposição {startItem} Até {endItem} A partir de {AllEntitiesCount}";
                }

                return $"نمایش {startItem} تا {endItem} از {AllEntitiesCount}";
            }

            if (CultureInfo.CurrentCulture.Name == "en-IR")
            {
                return $"0 Item";
            }

            return $"0 آیتم";
        }

        public async Task<BasePaging<T>> Paging(IQueryable<T> queryable)
        {
            TakeEntity = TakeEntity;

            var allEntitiesCount = await queryable.CountAsync();

            var pageCount = Convert.ToInt32(Math.Ceiling(allEntitiesCount / (double)TakeEntity));

            Page = Page > pageCount ? pageCount : Page;
            if (Page <= 0) Page = 1;
            AllEntitiesCount = allEntitiesCount;
            HowManyShowPageAfterAndBefore = HowManyShowPageAfterAndBefore;
            SkipEntity = (Page - 1) * TakeEntity;
            StartPage = Page - HowManyShowPageAfterAndBefore <= 0 ? 1 : Page - HowManyShowPageAfterAndBefore;
            EndPage = Page + HowManyShowPageAfterAndBefore > pageCount ? pageCount : Page + HowManyShowPageAfterAndBefore;
            PageCount = pageCount;
            Entities = await queryable.Skip(SkipEntity).Take(TakeEntity).ToListAsync();

            return this;
        }
    }

    public class PagingViewModel
    {
        public int Page { get; set; }

        public int StartPage { get; set; }

        public int EndPage { get; set; }
    }
}
