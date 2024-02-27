using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;

namespace GameStore.API.OptionsSetup;

[ExcludeFromCodeCoverage]
public class RequestLocalizationOptionsSetup : IConfigureOptions<RequestLocalizationOptions>
{
    public void Configure(RequestLocalizationOptions options)
    {
        var supportedCultures = new List<CultureInfo>
        {
            new("en"),
            new("ru-RU"),
            new("uk-UA"),
        };

        options.DefaultRequestCulture = new RequestCulture("en");
        options.SupportedCultures = supportedCultures;
    }
}