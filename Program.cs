using AiChat;

var apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY") ?? "";

if (string.IsNullOrEmpty(apiKey))
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("Error: OPENAI_API_KEY environment variable is not set.");
    Console.ResetColor();
    return;
}

var httpClient = new HttpClient();
var openAiClient = new OpenAiClient(httpClient, apiKey);
var chat = new ChatSession(openAiClient);

Console.ForegroundColor = ConsoleColor.Cyan;
Console.WriteLine("=================================");
Console.WriteLine("       AI CHAT CLI — C#          ");
Console.WriteLine("=================================");
Console.ResetColor();
Console.WriteLine("Commands: /history  /clear  /exit");
Console.WriteLine();

while (true)
{
    Console.ForegroundColor = ConsoleColor.Green;
    Console.Write("You: ");
    Console.ResetColor();

    var input = Console.ReadLine()?.Trim();

    if (string.IsNullOrEmpty(input)) continue;

    if (input == "/exit")
    {
        Console.WriteLine("Goodbye!");
        break;
    }

    if (input == "/clear")
    {
        chat.Clear();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Conversation cleared.");
        Console.ResetColor();
        continue;
    }

    if (input == "/history")
    {
        chat.PrintHistory();
        continue;
    }

    try
    {
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.Write("AI:  ");
        Console.ResetColor();

        var response = await chat.SendAsync(input);
        Console.WriteLine(response);
        Console.WriteLine();
    }
    catch (Exception ex)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Error: {ex.Message}");
        Console.ResetColor();
    }
}
