using net8_aot_api.Entity;
using System.Text.Json.Serialization;

[JsonSerializable(typeof(List<Todo>))]
internal partial class AppJsonSerializerContext : JsonSerializerContext { }
