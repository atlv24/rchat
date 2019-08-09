using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nop.Core.Infrastructure;

namespace Nop.Plugin.Widgets.RChat
{
	public class RChatStartup : INopStartup
	{
		int INopStartup.Order => 999;

		void INopStartup.ConfigureServices(IServiceCollection services, IConfiguration configuration)
		{
			services.AddSignalR();
		}

		void INopStartup.Configure(IApplicationBuilder application)
		{
			application.UseSignalR(routes =>
			{
				routes.MapHub<RChatHub>("/rchat");
			});
		}
	}
}