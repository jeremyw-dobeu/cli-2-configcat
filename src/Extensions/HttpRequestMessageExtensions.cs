﻿using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace System.Net.Http
{
    static class HttpRequestMessageExtensions
    {
        public static async Task<HttpRequestMessage> CloneAsync(this HttpRequestMessage request)
        {
            var clone = new HttpRequestMessage(request.Method, request.RequestUri)
            {
                Content = await request.Content.CloneAsync(),
                Version = request.Version
            };

            foreach (var prop in request.Properties)
                clone.Properties.Add(prop);
            foreach (var header in request.Headers)
                clone.Headers.Add(header.Key, header.Value);

            return clone;
        }

        public static async Task<HttpContent> CloneAsync(this HttpContent content)
        {
            if (content == null) return null;

            var stream = new MemoryStream();
            await content.CopyToAsync(stream);
            stream.Position = 0;
            var clone = new StreamContent(stream);

            foreach (var header in content.Headers)
                clone.Headers.Add(header.Key, header.Value);

            return clone;
        }
    }
}
