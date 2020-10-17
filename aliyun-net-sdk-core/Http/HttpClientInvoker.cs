/*
 * Licensed to the Apache Software Foundation (ASF) under one
 * or more contributor license agreements.  See the NOTICE file
 * distributed with this work for additional information
 * regarding copyright ownership.  The ASF licenses this file
 * to you under the Apache License, Version 2.0 (the
 * "License"); you may not use this file except in compliance
 * with the License.  You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing,
 * software distributed under the License is distributed on an
 * "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
 * KIND, either express or implied.  See the License for the
 * specific language governing permissions and limitations
 * under the License.
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Aliyun.Acs.Core.Exceptions;
using Aliyun.Acs.Core.Utils;

namespace Aliyun.Acs.Core.Http
{
    internal class HttpClientInvoker : IHttpInvoker
    {
        // Default read timeout 10s
        private const int DEFAULT_TIMEOUT_IN_MilliSeconds = 10000;

        private static readonly int bufferLength = 1024;

        private Func<string, HttpMessageHandler> _httpMessageHandlerFactory;

        public HttpClientInvoker(Func<string, HttpMessageHandler> httpMessageHandlerFactory)
        {
            _httpMessageHandlerFactory = httpMessageHandlerFactory;
        }

        public HttpResponse GetResponse(HttpRequest request, int? timeout)
        {
            return GetResponseAsync(request, CancellationToken.None, timeout).GetAwaiter().GetResult();
        }

        private static async Task ParseHttpResponseAsync(HttpResponse httpResponse, HttpResponseMessage httpWebResponse, CancellationToken cancellationToken)
        {
            httpResponse.Content = await ReadContentAsync(httpResponse, httpWebResponse, cancellationToken).ConfigureAwait(false);
            httpResponse.Status = (int)httpWebResponse.StatusCode;
            httpResponse.Headers = new Dictionary<string, string>();
            httpResponse.Method = ParameterHelper.StringToMethodType(httpWebResponse.RequestMessage.Method.Method);
            httpResponse.HttpVersion = httpWebResponse.Version.ToString();

            foreach (var kv in httpWebResponse.Headers.Union(httpWebResponse.Content.Headers))
            {
                httpResponse.Headers.Add(kv.Key, kv.Value.First());
            }

            var contentType = DictionaryUtil.Get(httpResponse.Headers, "Content-Type");

            if (null != contentType)
            {
                httpResponse.Encoding = "UTF-8";
                var split = contentType.Split(';');
                httpResponse.ContentType = ParameterHelper.StingToFormatType(split[0].Trim());
                if (split.Length > 1 && split[1].Contains("="))
                {
                    var codings = split[1].Split('=');
                    httpResponse.Encoding = codings[1].Trim().ToUpper();
                }
            }
        }

        public static async Task<byte[]> ReadContentAsync(HttpResponse response, HttpResponseMessage rsp, CancellationToken cancellationToken)
        {
            using (var ms = new MemoryStream())
            using (var stream = await rsp.Content.ReadAsStreamAsync().ConfigureAwait(false))
            {
                if (stream == null)
                {
                    return new byte[0];
                }

                await stream.CopyToAsync(ms, bufferLength, cancellationToken).ConfigureAwait(false);

                return ms.ToArray();
            }
        }

        public async Task<HttpResponse> GetResponseAsync(HttpRequest request, CancellationToken cancellationToken, int? timeout)
        {
            var httpWebRequest = GetHttpRequestMessage(request);

            var name = "aliyun-" + HttpUtility.ParseQueryString(httpWebRequest.RequestUri.Query)["regionId"];

            var client = new HttpClient(_httpMessageHandlerFactory(name)) { Timeout = TimeSpan.FromMilliseconds(timeout ?? DEFAULT_TIMEOUT_IN_MilliSeconds) };

            if (request.WebProxy != null || request.IgnoreCertificate)
            {
                throw new NotSupportedException($"Please configure A and B of named '{name}' in httpMessageHandlerFactory");
            }

            HttpResponseMessage httpWebResponse;
            var httpResponse = new HttpResponse(httpWebRequest.RequestUri.AbsoluteUri);

            try
            {
                using (httpWebResponse = await client.SendAsync(httpWebRequest, cancellationToken).ConfigureAwait(false))
                {
                    await ParseHttpResponseAsync(httpResponse, httpWebResponse, cancellationToken).ConfigureAwait(false);
                    return httpResponse;
                }
            }
            catch (OperationCanceledException ex)
            {
                throw new ClientException("SDK.WebException",
                    string.Format("HttpWebRequest timeout, the request url is {0} {1}",
                        httpWebRequest.RequestUri == null ? "empty" : httpWebRequest.RequestUri.Host, ex));
            }
            catch (HttpRequestException ex)
            {
                throw new ClientException("SDK.ServerUnreachable:",
                    string.Format("Server unreachable: connection to url: {0} failed. {1}",
                        httpWebRequest.RequestUri == null ? "empty" : httpWebRequest.RequestUri.Host,
                        ex));
            }
            catch (Exception ex)
            {
                throw new ClientException("SDK.Exception",
                    string.Format("The request url is {0} {1}",
                        httpWebRequest.RequestUri == null ? "empty" : httpWebRequest.RequestUri.Host, ex));
            }
        }

        public static HttpRequestMessage GetHttpRequestMessage(HttpRequest request)
        {
            var httpWebRequest = new HttpRequestMessage(new HttpMethod(request.Method.ToString()), request.Url);

            foreach (var header in request.Headers)
            {
                httpWebRequest.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            if ((request.Method == MethodType.POST || request.Method == MethodType.PUT) && request.Content != null)
            {
                httpWebRequest.Content = new ByteArrayContent(request.Content);

                foreach (var header in request.Headers)
                {
                    httpWebRequest.Content.Headers.TryAddWithoutValidation(header.Key, header.Value);
                }
            }

            return httpWebRequest;
        }
    }
}
