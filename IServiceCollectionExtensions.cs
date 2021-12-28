using System.Diagnostics;
using NSwag;
using NSwag.Generation.AspNetCore;
using NSwag.Generation.Processors.Security;

namespace TestBackTickNSwag;

public static class IServiceCollectionExtensions
{
    internal static void ConfigureSwagger(this IServiceCollection services)
    {
        StackFrame[] frames = new StackTrace().GetFrames();
        string initialAssembly = (from f in frames
                select f.GetMethod().ReflectedType.AssemblyQualifiedName
            ).Distinct().Last();

        services.AddOpenApiDocument(document =>
        {
            AddParameter(document, "Language", "Type in NL/FR", "X-language");
            AddParameter(document, "Company", "Type the companyId", "X-companyId");
            AddParameter(document, "User", "Type in userId", "X-userId");

            document.PostProcess = d =>
            {
                d.Info.Version = "v1";
                d.Info.Title = initialAssembly;
                d.Info.Description = "";
                d.Info.TermsOfService = "None";
            };

        });
    }

    private static void AddParameter(AspNetCoreOpenApiDocumentGeneratorSettings document, string name,
        string description, string headerName)
    {
        document.AddSecurity(name, Enumerable.Empty<string>(), new OpenApiSecurityScheme
        {
            Type = OpenApiSecuritySchemeType.ApiKey,
            Name = headerName,
            In = OpenApiSecurityApiKeyLocation.Header,
            Description = description
        });

        document.OperationProcessors.Add(
            new AspNetCoreOperationSecurityScopeProcessor(name));
    }

    public static IMvcBuilder ConfigureServicesX(this IServiceCollection services)
    {
        var result = services.AddControllers()
            .AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );

        services.ConfigureSwagger();

        return result;
    }
}