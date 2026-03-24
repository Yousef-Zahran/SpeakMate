using SpeakMate.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using SpeakMate.Core.DTOs.Translation;

namespace SpeakMate.Infrastructure.Services
{
    public class TranslationService : ITranslationService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public TranslationService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<string> TranslateAsync(string text, string fromLanguage, string toLanguage)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;

            var client = _httpClientFactory.CreateClient();

            var requestBody = new
            {
                model = "llama-3.1-8b-instant",
                messages = new[]
                {
                    new { role = "system", content = "You are a translator. Return ONLY the translated text, nothing else. No explanations, no quotes." },
                    new { role = "user", content = $"Translate from {fromLanguage} to {toLanguage}:\n\n{text}" }
                }
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.groq.com/openai/v1/chat/completions");
            request.Headers.Add("Authorization", $"Bearer {_configuration["Groq:ApiKey"]}");
            request.Content = new StringContent(
                JsonSerializer.Serialize(requestBody),
                Encoding.UTF8,
                "application/json"
            );

            var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var errorBody = await response.Content.ReadAsStringAsync();
                throw new Exception($"Groq Translation failed. Status: {response.StatusCode}. Body: {errorBody}");
            }

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<GroqResponseDto>(json);

            return result?.Choices?.FirstOrDefault()?.Message?.Content?.Trim() ?? string.Empty;
        }
    }
   
}