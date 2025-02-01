using Hangfire.Console;
using Hangfire.Server;
using Vrossi.ScaffoldCore.Common.Infrastructure.Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Vrossi.ScaffoldCore.Infrastructure.Hangfire
{
    public static class HangfireConsoleExtensions
    {
        public static void WriteError(this PerformContext context, string errorMessage) => context.WriteLineWithTextColor(errorMessage, ConsoleTextColor.Red);
        public static void WriteSucess(this PerformContext context, string errorMessage) => context.WriteLineWithTextColor(errorMessage, ConsoleTextColor.Green);
        private static void WriteLineWithTextColor(this PerformContext context, string errorMessage, ConsoleTextColor textColor)
        {
            if (errorMessage == null)
                throw new ArgumentNullException(nameof(errorMessage));

            context.SetTextColor(textColor);
            context.WriteLine(errorMessage);
            context.ResetTextColor();
        }

        public static void WriteMessage(this PerformContext context, IHangfireResponse response)
        {
            if (response == null)
                throw new ArgumentNullException(nameof(response));

            var text = response.ToString();

            if (response.StatusCode == HttpStatusCode.OK)
                context.WriteSucess(text);
            else
                context.WriteError(text);
        }
    }
}
