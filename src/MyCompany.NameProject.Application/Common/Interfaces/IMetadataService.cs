using System.Collections.Generic;

namespace MyCompany.NameProject.Application.Common.Interfaces
{
    public interface IMetadataService
    {
        string Environment { get; }

        string WepApiVersion { get; }

        string WepProductName { get; }

        IEnumerable<string> Versions { get; }

        string VersionNumber { get; }

        string ConfigurationName { get; }
    }
}