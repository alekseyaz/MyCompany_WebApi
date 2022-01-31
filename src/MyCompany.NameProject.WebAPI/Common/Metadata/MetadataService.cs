using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using MyCompany.NameProject.Application.Common.Exceptions;
using MyCompany.NameProject.Application.Common.Interfaces;

namespace MyCompany.NameProject.WebAPI.Common.Metadata
{
    public class MetadataService : IMetadataService
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly List<string> _versions;

        public MetadataService(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
            _versions = new List<string>();
            SetEnvironment();
            SetVersion();
            SetWebApiData();
        }

        public string Environment { get; private set; }

        public IEnumerable<string> Versions => _versions;

        public string WepApiVersion { get; private set; }

        public string WepProductName { get; private set; }

        public string VersionNumber => WepApiVersion;

        public string ConfigurationName { get; private set; }

        private void SetEnvironment()
        {
            ConfigurationName = _webHostEnvironment.EnvironmentName;
            Environment= _configuration.GetSection("Environment").Get<string>();
        }

        private void SetVersion()
        {
            foreach (var assembly in new[] { typeof(Program).GetTypeInfo().Assembly,
                                            typeof(BadRequestException).GetTypeInfo().Assembly })
            {
                _versions.Add($"{assembly.GetCustomAttribute<AssemblyProductAttribute>().Product}: { assembly.GetCustomAttribute<AssemblyFileVersionAttribute>().Version}");
            }
        }

        private void SetWebApiData()
        {
            var assembly = typeof(Program).GetTypeInfo().Assembly;
            WepApiVersion = assembly.GetCustomAttribute<AssemblyFileVersionAttribute>().Version;
            WepProductName = assembly.GetCustomAttribute<AssemblyProductAttribute>().Product;
        }
    }
}
