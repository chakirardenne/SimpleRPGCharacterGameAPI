using System.Text.Json.Serialization;

namespace Tutorial_DotNet.Models;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum RpgClass {
    Knight = 1, Mage = 2, Cleric = 3
}