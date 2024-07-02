using Asp.Versioning;
using Asp.Versioning.Builder;
using Asp.Versioning.Conventions;

namespace Microservice.Core.EndPoints;

public static class ApiVersionsConfig
{
    public static readonly ApiVersionSet? VersionSet;
    private static readonly List<ApiVersion?> Versions;
    private  static readonly ApiVersion DefaultVersion = new ApiVersion(1, 0);

    static ApiVersionsConfig()
    {
        Versions = LoadVersionsFromConfig(); // Load versions from configuration
        
        if (Versions.Count != 0)
        {
            VersionSet = new ApiVersionSetBuilder("API Versions")
                .HasApiVersions(Versions!)
                .Build();
        }    
    }
    
    private static List<ApiVersion?> LoadVersionsFromConfig()
    {
        return
        [
            new ApiVersion(1, 0),
            new ApiVersion(2, 0)
        ];
    }
    public static ApiVersion GetVersion(int majorVersion, int minorVersion)
    {
        return Versions.FirstOrDefault(v =>
            v?.MajorVersion == majorVersion && v.MinorVersion == minorVersion) ?? DefaultVersion;
    }
}