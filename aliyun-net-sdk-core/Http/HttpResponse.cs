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
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Aliyun.Acs.Core.Http
{
    public class HttpResponse : HttpRequest
    {
        private static readonly HttpWebRequestInvoker s_httpWebRequestInvoker = new HttpWebRequestInvoker();
        private static HttpClientInvoker s_httpClientInvoker;

        private static IHttpInvoker HttpInvoker
        {
            get
            {
                return s_httpClientInvoker ?? (IHttpInvoker)s_httpWebRequestInvoker;
            }
        }

        public HttpResponse(string strUrl) : base(strUrl)
        {
        }

        public HttpResponse()
        {
        }

        public int Status { get; set; }

        public string HttpVersion { get; set; }

        public new void SetContent(byte[] content, string encoding, FormatType? format)
        {
            Content = content;
            Encoding = encoding;
            ContentType = format;
        }

        public static byte[] ReadContent(HttpResponse response, HttpWebResponse rsp)
        {
            return HttpWebRequestInvoker.ReadContent(response, rsp);
        }

        public static HttpResponse GetResponse(HttpRequest request, int? timeout = null)
        {
            return HttpInvoker.GetResponse(request, timeout);
        }

        public static HttpWebRequest GetWebRequest(HttpRequest request)
        {
            return HttpWebRequestInvoker.GetWebRequest(request);
        }

        public bool isSuccess()
        {
            return 200 <= Status && 300 > Status;
        }

        public static void UseHttpClient(Func<string, System.Net.Http.HttpMessageHandler> httpMessageHandlerFactory)
        {
            if (httpMessageHandlerFactory == null)
            {
                throw new ArgumentNullException(nameof(httpMessageHandlerFactory));
            }

            s_httpClientInvoker = new HttpClientInvoker(httpMessageHandlerFactory);
        }

        public static void UseHttpWebRequest()
        {
            s_httpClientInvoker = null;
        }

        public static Task<HttpResponse> GetResponseAsync(HttpRequest request)
        {
            return HttpInvoker.GetResponseAsync(request, default(CancellationToken), null);
        }

        public static Task<HttpResponse> GetResponseAsync(HttpRequest request, CancellationToken cancellationToken)
        {
            return HttpInvoker.GetResponseAsync(request, cancellationToken, null);
        }

        public static Task<HttpResponse> GetResponseAsync(HttpRequest request, CancellationToken cancellationToken, int? timeout)
        {
            return HttpInvoker.GetResponseAsync(request, cancellationToken, timeout);
        }
    }
}
