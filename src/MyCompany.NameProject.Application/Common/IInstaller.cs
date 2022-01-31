using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MyCompany.NameProject.Application.Common
{
    /// <summary>
    /// Интерфейс для реализации функционала по регистрации типов
    /// </summary>
    public interface IInstaller
    {
        /// <summary>
        /// Регистрация зависимостей в контейнере
        /// </summary>
        void Install(IServiceCollection services, IConfiguration configuration);
    }
}
