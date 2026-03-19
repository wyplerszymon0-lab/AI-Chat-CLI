namespace AiChat;

public class ChatSession
{
    private readonly IOpenAiClient _client;
    private readonly List<ChatMessage> _history = [];
    private const string SystemPrompt = "You are a helpful, concise assistant. Answer clearly and directly.";

    public ChatSession(IOpenAiClient client)
    {
        _client = client;
    }

    public async Task<string> SendAsync(string userMessage)
    {
        _history.Add(new ChatMessage("user", userMessage));

        var messages = BuildMessages();
        var reply = await _client.CompleteAsync(messages);

        _history.Add(new ChatMessage("assistant", reply));
        return reply;
    }

    public void Clear()
    {
        _history.Clear();
    }

    public void PrintHistory()
    {
        if (_history.Count == 0)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("No messages yet.");
            Console.ResetColor();
            return;
        }

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"--- History ({_history.Count} messages) ---");
        Console.ResetColor();

        foreach (var msg in _history)
        {
            var color = msg.Role == "user" ? ConsoleColor.Green : ConsoleColor.Magenta;
            Console.ForegroundColor = color;
            Console.Write($"{msg.Role.ToUpper()}: ");
            Console.ResetColor();
            Console.WriteLine(msg.Content);
        }

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("-----------------------------------");
        Console.ResetColor();
        Console.WriteLine();
    }

    public IReadOnlyList<ChatMessage> History => _history.AsReadOnly();

    private List<MessageDto> BuildMessages()
    {
        var messages = new List<MessageDto>
        {
            new("system", SystemPrompt)
        };

        messages.AddRange(_history.Select(m => new MessageDto(m.Role, m.Content)));
        return messages;
    }
}
