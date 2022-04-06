using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(FoodWaste.Areas.Identity.IdentityHostingStartup))]
namespace FoodWaste.Areas.Identity
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