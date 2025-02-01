using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Vrossi.ScaffoldCore.Infrastructure.Hangfire
{
    internal class HangfireEmailManager : IHangfireEmailManager
    {
        public async Task<HangfireEmailResponse> Send<TPayload>(string url, TPayload payload, Dictionary<string, string> headers = null) where TPayload : class
        {
            try
            {

                return new HangfireEmailResponse()
                {
                    Success = true,
                    Message = "Success"
                };
            }
            catch (Exception ex)
            {
                return new HangfireEmailResponse()
                {
                    Success = false,
                    Message = $"Error while sending email: {ex.Message}"
                };
            }
        }
    }

    internal interface IHangfireEmailManager
    {
        Task<HangfireEmailResponse> Send<TPayload>(string url, TPayload payload, Dictionary<string, string> headers = null) where TPayload : class;
    }
}
