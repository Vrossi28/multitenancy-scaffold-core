using Microsoft.EntityFrameworkCore;
using Vrossi.ScaffoldCore.Common.Models;
using Vrossi.ScaffoldCore.Infrastructure.MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vrossi.ScaffoldCore.Infrastructure.EntityFrameworkCore.Pagination
{
    public static class DataGridPaginationExtensions
    {
        public static async Task<PaginationResult<T>> GetPagination<T>(this IQueryable<T> query, IPaginationRequest request) where T : class
        {
            query = query.AsNoTracking();

            var result = new PaginationResult<T>
            {
                CurrentPage = request.Page,
                PageSize = request.PageSize,
                RowCount = await query.CountAsync()
            };

            query = query.OrderBy(request.SortColumns);

            if (request.AllPages)
                result.CurrentPage = 0;
            else
                query = query.Skip(request.Page).Take(request.PageSize);

            result.Items = await query.ToListAsync();

            return result;
        }
    }
}
