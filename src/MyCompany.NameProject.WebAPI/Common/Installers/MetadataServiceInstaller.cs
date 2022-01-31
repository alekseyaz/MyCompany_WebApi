using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyCompany.NameProject.Application.Common;
using MyCompany.NameProject.Application.Common.Interfaces;
using MyCompany.NameProject.WebAPI.Common.Metadata;

namespace MyCompany.NameProject.WebAPI.Common.Installers
{
    public class MetadataServiceInstaller : IInstaller
    {
        public void Install(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IMetadataService, MetadataService>();
        }
    }
}
