using AutoMapper;
using Vrossi.ScaffoldCore.Infrastructure.MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vrossi.ScaffoldCore.Application.Infrastructure.Extensions
{
    public static class MediatRExtensions
    {
        public static Response<TDto> MapToResponse<TDto>(this IMapper mapper, object source)
            where TDto : class
        {
            if (source == null)
                return Response<TDto>.NotFound();

            var dto = mapper.Map<TDto>(source);

            return Response<TDto>.Success(dto);
        }

        public static Response<List<TDto>> MapToResponseList<TDto>(this IMapper mapper, IEnumerable<object> source)
        where TDto : class
        {
            if (source == null)
                return Response<List<TDto>>.NotFound();

            var dtos = new List<TDto>();

            foreach (var item in source)
            {
                var dto = mapper.Map<TDto>(item);
                dtos.Add(dto);
            }

            return Response<List<TDto>>.Success(dtos);
        }
    }
}
