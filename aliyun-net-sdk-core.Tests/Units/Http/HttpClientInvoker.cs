using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Aliyun.Acs.Core.Tests.Units.Http;
using Xunit;

namespace Aliyun.Acs.Core.Http
{
    public class HttpClientInvokerTest
    {
        private readonly string requestUrl = "https://www.aliyun.com/";

        [Fact]
        public async Task GetResponse()
        {
            var request = new HttpResponse(requestUrl);
            var content = Encoding.ASCII.GetBytes("someString");
            request.SetContent(content, "UTF-8", FormatType.FORM);
            request.Method = MethodType.GET;
            var response = await new HttpClientInvoker(_ => new HttpClientHandler()).GetResponseAsync(request);
            Assert.Equal("UTF-8", response.Encoding);
            Assert.Equal(MethodType.GET, response.Method);

            // When timeout!=0
            response = await new HttpClientInvoker(_ => new HttpClientHandler()).GetResponseAsync(request, 30000);

            // Done With No Exception
        }

        [Fact]
        public void GetHttpRequestMessage()
        {
            var request = HttpRequestTest.SetContent();
            var httpWebRequest = HttpClientInvoker.GetHttpRequestMessage(request);
            Assert.IsType<HttpRequestMessage>(httpWebRequest);
            Assert.Equal("application/octet-stream", httpWebRequest.Content.Headers.ContentType.ToString());

            request.Headers.Add("Accept", "accept");
            request.Headers.Add("Date", "Thu, 24 Jan 2019 05:16:46 GMT");

            request.Method = MethodType.POST;
            httpWebRequest = HttpClientInvoker.GetHttpRequestMessage(request);
            Assert.IsType<HttpRequestMessage>(httpWebRequest);
            Assert.Equal("application/octet-stream", httpWebRequest.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public Task GetResponseIgnoreCertificate()
        {
            var request = HttpRequestTest.SetContent();
            request.SetHttpsInsecure(true);

            return Assert.ThrowsAsync<NotSupportedException>(() => new HttpClientInvoker(_ => new HttpClientHandler()).GetResponseAsync(request));
        }

        [Fact]
        public Task GetResponseWithProxy()
        {
            var request = HttpRequestTest.SetContent();
            request.WebProxy = new WebProxy();

            return Assert.ThrowsAsync<NotSupportedException>(() => new HttpClientInvoker(_ => new HttpClientHandler()).GetResponseAsync(request));
        }
    }
}
