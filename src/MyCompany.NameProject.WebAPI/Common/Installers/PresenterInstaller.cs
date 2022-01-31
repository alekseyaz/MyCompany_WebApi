using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using MyCompany.NameProject.Application.Common;

namespace MyCompany.NameProject.WebAPI.Common.Installers
{
    public class PresenterInstaller : IInstaller
    {
        public void Install(IServiceCollection services, IConfiguration configuration)
        {
            typeof(Program).Assembly.ExportedTypes
           .Where(x => !x.IsInterface && !x.IsAbstract && x.FullName.EndsWith(".Presenter"))
           .ToList()
           .ForEach(x => services.AddScoped(x));
        }
    }
}
