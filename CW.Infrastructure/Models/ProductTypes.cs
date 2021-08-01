using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CW.Infrastructure.Models
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ProductTypes
    {
        Books,
        Electronics,
        Food,
        Furniture,
        Toys
    }
}
