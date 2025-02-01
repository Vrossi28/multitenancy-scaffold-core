using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Vrossi.ScaffoldCore.Infrastructure.MediatR
{
    public abstract class Request<TDto> : IMetadataRequest, IRequest<Response<TDto>>
    where TDto : class
    {
        [JsonIgnore]
        public Guid UserId { get; set; }
        [JsonIgnore]
        public Guid? XTenantId { get; set; }
        [JsonIgnore]
        public Guid? TenantId { get; set; }
        [JsonIgnore]
        public bool IsAdmin { get; set; } = false;
        [JsonIgnore]
        public IEnumerable<Guid> AllowedTenantIds { get; set; } = Array.Empty<Guid>();
    }
    public abstract class Request : IMetadataRequest, IRequest<Response>
    {
        [JsonIgnore]
        public Guid UserId { get; set; }
        [JsonIgnore]
        public Guid? XTenantId { get; set; }
        [JsonIgnore]
        public Guid? TenantId { get; set; }
        [JsonIgnore]
        public bool IsAdmin { get; set; } = false;
        [JsonIgnore]
        public IEnumerable<Guid> AllowedTenantIds { get; set; } = Array.Empty<Guid>();
    }
    public interface IMetadataRequest
    {
        Guid UserId { get; set; }
        Guid? TenantId { get; set; }
        Guid? XTenantId { get; set; }
        bool IsAdmin { get; set; }
        IEnumerable<Guid> AllowedTenantIds { get; set; }
    }
    public class PaginationRequest<TDto> : Request<TDto>, IPaginationRequest
        where TDto : class
    {
        public int Page { get; init; }
        public int PageSize { get; init; } = 10;
        public string[] SortColumns { get; init; } = Array.Empty<string>();
        public bool AllPages { get; init; } = false;
    }
    public interface IPaginationRequest : IMetadataRequest
    {
        int Page { get; init; }
        int PageSize { get; init; }
        string[] SortColumns { get; init; }
        bool AllPages { get; init; }
    }
}
