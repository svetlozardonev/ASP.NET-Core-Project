using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(PickMovie.Areas.Identity.IdentityHostingStartup))]
namespace PickMovie.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}