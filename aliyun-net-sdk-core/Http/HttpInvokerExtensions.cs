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

using System.Threading;
using System.Threading.Tasks;

namespace Aliyun.Acs.Core.Http
{
    public static class HttpInvokerExtensions
    {
        public static HttpResponse GetResponse(this IHttpInvoker invoker, HttpRequest request)
        {
            return invoker.GetResponse(request, null);
        }

        public static Task<HttpResponse> GetResponseAsync(this IHttpInvoker invoker, HttpRequest request)
        {
            return invoker.GetResponseAsync(request, CancellationToken.None, null);
        }

        public static Task<HttpResponse> GetResponseAsync(this IHttpInvoker invoker, HttpRequest request, CancellationToken cancellationToken)
        {
            return invoker.GetResponseAsync(request, cancellationToken, null);
        }

        public static Task<HttpResponse> GetResponseAsync(this IHttpInvoker invoker, HttpRequest request, int? timeout)
        {
            return invoker.GetResponseAsync(request, CancellationToken.None, timeout);
        }
    }
}
