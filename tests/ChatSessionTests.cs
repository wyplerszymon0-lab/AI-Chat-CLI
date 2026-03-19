using AiChat;

namespace AiChat.Tests;

public class FakeOpenAiClient : IOpenAiClient
{
    public List<List<MessageDto>> ReceivedMessages = [];
    public string ReplyWith = "Hello from AI";

    public Task<string> CompleteAsync(List<MessageDto> messages)
    {
        ReceivedMessages.Add(messages);
        return Task.FromResult(ReplyWith);
    }
}

public class ChatSessionTests
{
    [Fact]
    public async Task SendAsync_AddsUserAndAssistantToHistory()
    {
        var fake = new FakeOpenAiClient();
        var session = new ChatSession(fake);

        await session.SendAsync("Hello");

        Assert.Equal(2, session.History.Count);
        Assert.Equal("user", session.History[0].Role);
        Assert.Equal("Hello", session.History[0].Content);
        Assert.Equal("assistant", session.History[1].Role);
        Assert.Equal("Hello from AI", session.History[1].Content);
    }

    [Fact]
    public async Task SendAsync_IncludesSystemPromptInRequest()
    {
        var fake = new FakeOpenAiClient();
        var session = new ChatSession(fake);

        await session.SendAsync("Hi");

        var sentMessages = fake.ReceivedMessages.First();
        Assert.Equal("system", sentMessages.First().Role);
    }

    [Fact]
    public async Task SendAsync_IncludesFullHistoryInEachRequest()
    {
        var fake = new FakeOpenAiClient();
        var session = new ChatSession(fake);

        await session.SendAsync("First");
        await session.SendAsync("Second");

        var lastRequest = fake.ReceivedMessages.Last();
        var roles = lastRequest.Select(m => m.Role).ToList();

        Assert.Contains("system", roles);
        Assert.Equal(2, roles.Count(r => r == "user"));
        Assert.Equal(1, roles.Count(r => r == "assistant"));
    }

    [Fact]
    public async Task Clear_RemovesAllHistory()
    {
        var fake = new FakeOpenAiClient();
        var session = new ChatSession(fake);

        await session.SendAsync("Hello");
        session.Clear();

        Assert.Empty(session.History);
    }

    [Fact]
    public async Task SendAsync_ReturnsAssistantReply()
    {
        var fake = new FakeOpenAiClient { ReplyWith = "Specific reply" };
        var session = new ChatSession(fake);

        var result = await session.SendAsync("Test");

        Assert.Equal("Specific reply", result);
    }
}
