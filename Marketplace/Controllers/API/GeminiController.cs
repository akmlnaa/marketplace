using Microsoft.AspNetCore.Mvc;
using Marketplace.Models.Gemini;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Marketplace.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeminiController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly string _apiKey;

        public GeminiController(IConfiguration config)
        {
            _config = config;
            _apiKey = _config["Gemini:ApiKey"];
        }

        [HttpPost("Ajukan")]
        public async Task<IActionResult> Ajukan([FromBody] GeminiRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Question))
                return BadRequest("Pertanyaan tidak boleh kosong");

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _apiKey);

            var payload = new
            {
                contents = new[]
                {
                    new {
                        parts = new[] {
                            new { text = request.Question }
                        }
                    }
                }
            };

            var jsonPayload = JsonSerializer.Serialize(payload);
            var response = await httpClient.PostAsync(
                "https://generativelanguage.googleapis.com/v1beta/models/gemini-pro:generateContent?key=" + _apiKey,
                new StringContent(jsonPayload, Encoding.UTF8, "application/json"));

            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, "Gagal menghubungi Gemini");

            var jsonString = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(jsonString);

            var answer = doc.RootElement
                .GetProperty("candidates")[0]
                .GetProperty("content")
                .GetProperty("parts")[0]
                .GetProperty("text")
                .GetString();

            return Ok(answer);
        }
    }
}
