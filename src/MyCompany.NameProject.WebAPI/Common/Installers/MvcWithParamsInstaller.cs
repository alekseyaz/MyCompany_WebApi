using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyCompany.NameProject.Application.Common;
using MyCompany.NameProject.Application.Common.Interfaces;
using MyCompany.NameProject.WebAPI.Common.Filters;

namespace MyCompany.NameProject.WebAPI.Common.Installers
{
    public class MvcWithParamsInstaller : IInstaller
    {
        public void Install(IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions().Configure<ApiFilterSettings>(configuration.GetSection(ApiFilterSettings.SectionName));
            var metaDataSrvr = services.BuildServiceProvider().GetRequiredService<IMetadataService>();
            services.AddMvc(options => ConfigureMvcOptions(options, configuration, metaDataSrvr));
        }

        private MvcOptions ConfigureMvcOptions(MvcOptions options, IConfiguration configuration, IMetadataService metadataService)
        {
            //var apiFilterSetting = configuration.GetSection(ApiFilterSettings.SectionName).Get<ApiFilterSettings>();

            //if (apiFilterSetting != null && apiFilterSetting.UseCustomResponseHeaders)
            //    options.Filters.Add(new AddHeaderAttribute(metadataService));

            options.Filters.Add(new ApiExceptionFilter(configuration));

            return options;
        }

    }
}
