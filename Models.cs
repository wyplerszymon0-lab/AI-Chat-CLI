namespace AiChat;

public record ChatMessage(string Role, string Content);

public record OpenAiRequest(
    string Model,
    List<MessageDto> Messages,
    int MaxTokens,
    double Temperature
);

public record MessageDto(string Role, string Content);

public record OpenAiResponse(List<Choice> Choices, Usage Usage);

public record Choice(MessageDto Message);

public record Usage(int PromptTokens, int CompletionTokens, int TotalTokens);
