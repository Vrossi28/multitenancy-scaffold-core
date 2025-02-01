using Microsoft.EntityFrameworkCore;
using Vrossi.ScaffoldCore.Infrastructure.MediatR;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vrossi.ScaffoldCore.Infrastructure.Extensions
{
    public static class ExceptionExtensions
    {
        public static Response<TDto> MapException<TDto>(this Exception exception) where TDto : class
        {
            if (exception is DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException is PostgresException pgException)
                {
                    if (pgException.SqlState == "23505")
                    {
                        return Response<TDto>.Error("A unique constraint violation occurred.");
                    }
                }
            }

            return Response<TDto>.Error(exception.Message);
        }
    }
}
