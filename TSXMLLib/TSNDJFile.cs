using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

using static TSXMLLib.Viewmodel;

namespace TSXMLLib
{
    public class TSNDJFile : TSFile, ITSFileFactory<TSNDJFile>
    {
        private TSNDJFile(FileInfo file, ReadOnlyCollection<Serialization> objects) : base(file)
        {
            Objects = objects;
        }

        public ReadOnlyCollection<Serialization> Objects { get; private set; }
        public Uri? AssociatedXMLUri { get; private set; } // TODO: implement reading xmluri

        static string ITSFileFactory<TSNDJFile>.Extension => ".tsndj";

        internal class TSNDJConverter : JsonConverter<Serialization>
        {
            public override Serialization Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                using JsonDocument doc = JsonDocument.ParseValue(ref reader);
                var root = doc.RootElement;

                string? Ref = root.GetProperty("Ref").GetString();
                JsonElement valueElement = root.GetProperty("Value");

                string? typeName = valueElement.GetProperty("$type").GetString();
                Type? type = typeName is not null ? Type.GetType(typeName) : null;

                if (type is not null && Ref is not null)
                {
                    dynamic? value = JsonSerializer.Deserialize(valueElement.GetProperty("$value").GetRawText(), type, options);

                    if (value is not null) return new(Ref, value);
                }

                return new(string.Empty, string.Empty);
            }

            public override void Write(Utf8JsonWriter writer, Serialization value, JsonSerializerOptions options)
            {
                Type type = value.Value.GetType();

                writer.WriteStartObject();
                writer.WriteString("Ref", value.Ref);

                writer.WritePropertyName("Value");
                writer.WriteStartObject();
                writer.WriteString("$type", type.AssemblyQualifiedName);
                writer.WritePropertyName("$value");
                JsonSerializer.Serialize(writer, value.Value, type, options);
                writer.WriteEndObject();

                writer.WriteEndObject();
            }
        }

        internal static readonly JsonSerializerOptions JsonOptions = new()
        {
            Converters = { new TSNDJConverter() }
        };

        private static async Task<ReadOnlyCollection<Serialization>> ParseAsync(FileInfo file)
        {
            Collection<Serialization> objects = [];

            using StreamReader reader = new(file.FullName);

            while (await reader.ReadLineAsync() is string line)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                if (TryDeserialize(line, out Serialization ser)) objects.Add(ser);
            }

            return objects.AsReadOnly();

            static bool TryDeserialize<T>(string jsonLine, out T? result)
            {
                result = default;

                try
                {
                    result = JsonSerializer.Deserialize<T>(jsonLine, JsonOptions);
                    return result is not null;
                }

                catch { return false; }
            }
        }

        public static async Task<TSNDJFile?> CreateFromLocalFileAsyncCore(FileInfo file) => new(file, await ParseAsync(file));

        public async Task<bool> UpdateAsync(Viewmodel model)
        {
            if (File is not null)
            {
                ReadOnlyCollection<Serialization> originalObjects = Objects;

                try
                {
                    string temp = Path.GetTempFileName();
                    await System.IO.File.WriteAllTextAsync(temp, model.Serialize());

                    File.Replace(temp, File.FullName);

                    Objects = await ParseAsync(File);

                    return true;
                }

                catch
                {
                    Objects = originalObjects;
                    return false;
                }
            }

            else return false;
        }
    }
}
