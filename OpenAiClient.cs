using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AiChat;

public interface IOpenAiClient
{
    Task<string> CompleteAsync(List<MessageDto> messages);
}

public class OpenAiClient : IOpenAiClient
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private const string Model = "gpt-4o-mini";
    private const string Endpoint = "https://api.openai.com/v1/chat/completions";

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    };

    public OpenAiClient(HttpClient httpClient, string apiKey)
    {
        _httpClient = httpClient;
        _apiKey = apiKey;
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
    }

    public async Task<string> CompleteAsync(List<MessageDto> messages)
    {
        var request = new OpenAiRequest(
            Model: Model,
            Messages: messages,
            MaxTokens: 1000,
            Temperature: 0.7
        );

        var response = await _httpClient.PostAsJsonAsync(Endpoint, request, JsonOptions);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<OpenAiResponse>(JsonOptions);

        return result?.Choices.FirstOrDefault()?.Message.Content
            ?? throw new Exception("Empty response from API");
    }
}
