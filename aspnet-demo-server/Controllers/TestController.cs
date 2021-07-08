using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace aspnet_demo_server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public TestController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet]
        public async Task<ActionResult<string>> Test()
        {
            HttpRequestMessage request = new(HttpMethod.Get, new Uri("http://google.com"));
            HttpResponseMessage res = await _httpClient.SendAsync(request);
            res.EnsureSuccessStatusCode();

            return Ok(await res.Content.ReadAsStringAsync());
        }
    }
}
