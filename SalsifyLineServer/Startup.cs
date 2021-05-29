using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SalsifyLineServer.Helpers;
using SalsifyLineServer.Services;
using SalsifyLineServer.Services.Interfaces;

namespace SalsifyLineClient
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            var tServices = services.BuildServiceProvider();
            var config = tServices.GetService<IConfiguration>();

            //Build the line index
            //This is done here to prevent the server from starting without the index being loaded
            //It could have been done inside the FileService somehow but that would slow down the initial requests
            var fileIndexer = new FileIndexer(config.GetValue<string>("fname"));
            var fileLineIndex = fileIndexer.GetIndex();

            //I've added the FileService instance as a singleton to keep the system simple
            services.AddSingleton<IFileService, FileService>(t =>
            {
                return new FileService(t.GetService<IConfiguration>(), fileLineIndex);
            });
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}