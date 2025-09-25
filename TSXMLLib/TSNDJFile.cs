using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design.Serialization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

using static TSXMLLib.Viewmodel;

namespace TSXMLLib // TODO: (GENERAL) go through all "if x is y z" and see if it could be simplified with "if x is not null" (new local not actually required)
{
    public class TSNDJFile : TSFile, ITSFileFactory<TSNDJFile>
    {
        private TSNDJFile(Uri source, FileInfo localFile, ReadOnlyCollection<Serialization> objects, Uri? associatedXMLUri) : base(source, localFile)
        {
            AssociatedXMLUri = associatedXMLUri;
            Objects = objects;
        }

        public ReadOnlyCollection<Serialization> Objects { get; private set; }
        public Uri? AssociatedXMLUri { get; private set; }

        static string ITSFileFactory<TSNDJFile>.Extension => ".tsndj";

        internal class TSNDJConverter : JsonConverter<Serialization>
        {
            private static readonly HashSet<Type> PrimitiveTypes =
            [
                typeof(string),
                typeof(bool),
                typeof(int),
                typeof(double),
                typeof(DateTime)
            ];

            private static readonly ReadOnlyDictionary<string, Type> KnownTypes = new Dictionary<string, Type>()
            {
                ["System.Uri"] = typeof(Uri)
            }.AsReadOnly();

            public override Serialization Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                using JsonDocument doc = JsonDocument.ParseValue(ref reader);
                var root = doc.RootElement;

                if (root.ValueKind != JsonValueKind.Array || root.GetArrayLength() < 2) throw new NotImplementedException(); // TODO: error on invalid json

                string? Ref = root[0].GetString();
                JsonElement typeAndValueElement = root[1];

                if (typeAndValueElement.ValueKind == JsonValueKind.Object && typeAndValueElement.TryGetProperty("t", out var typeProperty))
                {
                    string? typeName = typeProperty.GetString();
                    if (typeName is not null)
                    {
                        Type? type = KnownTypes.TryGetValue(typeName, out Type? knownType) ? knownType : Type.GetType(typeName);

                        if (type is not null && Ref is not null && typeAndValueElement.TryGetProperty("v", out var valueProperty))
                        {
                            dynamic? value = JsonSerializer.Deserialize(valueProperty.GetRawText(), type, options);

                            return new(Ref, value);
                        }
                    }
                }

                else
                {
                    dynamic? primitive = typeAndValueElement.ValueKind switch
                    {
                        JsonValueKind.String => typeAndValueElement.GetString(),
                        JsonValueKind.True => true,
                        JsonValueKind.False => false,
                        JsonValueKind.Number => typeAndValueElement.TryGetInt32(out var i) ? i : typeAndValueElement.TryGetDouble(out var d) ? d : 0, // TODO: not sure if this should actually default to 0
                        _ => typeAndValueElement.GetRawText() // TODO: not sure if this is what we actually want to do
                    };

                    if (primitive is not null) return new(Ref, primitive);
                }

                // TODO: error on invalid json
                throw new NotImplementedException();
            }

            public override void Write(Utf8JsonWriter writer, Serialization value, JsonSerializerOptions options)
            {
                Type? type = value.Value?.GetType();

                writer.WriteStartArray();
                writer.WriteStringValue(value.Ref);

                if (type is not null)
                {
                    if (PrimitiveTypes.Contains(type))
                    {
                        JsonSerializer.Serialize(writer, value.Value, type, options);
                    }

                    else
                    {
                        if (type.FullName is string typeSerialization)
                        {
                            if (!KnownTypes.Values.Contains(type)
                                && type.Assembly.GetName().Name is string shortAssembly
                                && shortAssembly != "System.Private.CoreLib"
                                && shortAssembly is not null) typeSerialization += $", {shortAssembly}";

                            writer.WriteStartObject();
                            writer.WriteString("t", typeSerialization);
                            writer.WritePropertyName("v");
                            JsonSerializer.Serialize(writer, value.Value, type, options);
                            writer.WriteEndObject();
                        }

                        else
                        {
                            throw new NotImplementedException(); // TODO: handle
                        }
                    }

                    writer.WriteEndArray();
                }
            }
        }

        internal static readonly JsonSerializerOptions JsonOptions = new()
        {
            Converters = { new TSNDJConverter() }
        };

        private static async Task<(ReadOnlyCollection<Serialization>, Uri?)> ParseAsync(FileInfo file, CancellationToken cancellationToken)
        {
            Collection<Serialization> objects = [];
            Uri? associatedXMLUri = null;

            using StreamReader reader = new(file.FullName);

            while (await reader.ReadLineAsync(cancellationToken) is string line)
            {
                if (!string.IsNullOrWhiteSpace(line) && TryDeserialize(line, out Serialization ser))
                {
                    if (ser.Ref == "\u0006" && ser.Value is Uri uri) associatedXMLUri = uri; // TODO: magic string
                    objects.Add(ser);
                }
            }

            return (objects.AsReadOnly(), associatedXMLUri);

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

        public static async Task<TSNDJFile?> CreateCoreAsync(Uri source, FileInfo localFile, CancellationToken cancellationToken)
        {
            (ReadOnlyCollection<Serialization> serializations, Uri? uri) = await ParseAsync(localFile, cancellationToken);
            return new(source, localFile, serializations, uri);
        }
    }
}
