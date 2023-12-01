using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace GameStore.Shared.Extensions;

[ExcludeFromCodeCoverage]
public static class ObjectExtensions
{
    public static T DeepClone<T>(this T source)
    {
        string json = JsonConvert.SerializeObject(source, settings: new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
        });
        return JsonConvert.DeserializeObject<T>(json);
    }
}