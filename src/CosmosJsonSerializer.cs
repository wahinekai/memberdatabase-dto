// -----------------------------------------------------------------------
// <copyright file="CosmosJsonSerializer.cs" company="Wahine Kai">
// Copyright (c) Wahine Kai. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace WahineKai.MemberDatabase.Dto
{
    using System.IO;
    using System.Text;
    using Microsoft.Azure.Cosmos;
    using Newtonsoft.Json;

    /// <summary>
    /// Custom JSON Serializer for use with Cosmos DB
    /// </summary>
    public class CosmosJsonSerializer : CosmosSerializer
    {
        private static readonly Encoding DefaultEncoding = new UTF8Encoding(false, true);
        private readonly JsonSerializer serializer;
        private readonly JsonSerializerSettings serializerSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="CosmosJsonSerializer"/> class.
        /// </summary>
        public CosmosJsonSerializer()
            : this(new JsonSerializerSettings())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CosmosJsonSerializer"/> class.
        /// </summary>
        /// <param name="serializerSettings">Serializer settings to use with this Cosmos Serializer</param>
        public CosmosJsonSerializer(JsonSerializerSettings serializerSettings)
        {
            this.serializerSettings = serializerSettings;
            this.serializer = JsonSerializer.Create(this.serializerSettings);
        }

        /// <summary>
        /// Converts from a stream to a T type
        /// </summary>
        /// <typeparam name="T">The type to convert to</typeparam>
        /// <param name="stream">The stream to convert to the type</param>
        /// <returns>The type</returns>
        public override T FromStream<T>(Stream stream)
        {
            using (stream)
            {
                if (typeof(Stream).IsAssignableFrom(typeof(T)))
                {
                    return (T)(object)stream;
                }

                using var sr = new StreamReader(stream);
                using var jsonTextReader = new JsonTextReader(sr);

                #pragma warning disable CS8603 // Using outside code and trusting it
                return this.serializer.Deserialize<T>(jsonTextReader);
                #pragma warning restore CS8603
            }
        }

        /// <summary>
        /// Converts from T to a stream
        /// </summary>
        /// <typeparam name="T">The type to convert from</typeparam>
        /// <param name="input">The object to convert from</param>
        /// <returns>A stream to convert to</returns>
        public override Stream ToStream<T>(T input)
        {
            var streamPayload = new MemoryStream();
            using var streamWriter = new StreamWriter(streamPayload, encoding: DefaultEncoding, bufferSize: 1024, leaveOpen: true);
            using var writer = new JsonTextWriter(streamWriter) { Formatting = Formatting.None };

            this.serializer.Serialize(writer, input);
            writer.Flush();
            streamWriter.Flush();

            streamPayload.Position = 0;
            return streamPayload;
        }
    }
}
