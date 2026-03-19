# ai-chat-cli

A command-line AI chat application built with C# and .NET 8. Talks to OpenAI GPT-4o-mini, maintains conversation history and supports CLI commands.

## What you learn from this project

- Classes, records and interfaces in C#
- async/await in practice
- HttpClient and JSON serialization
- Dependency injection via interface (IOpenAiClient)
- Unit testing with xUnit and fake objects

## Run
```bash
export OPENAI_API_KEY=your_key_here
dotnet run
```

## Test
```bash
cd Tests
dotnet test
```

## Commands

| Command | Description |
|---|---|
| `/history` | Print conversation history |
| `/clear` | Clear conversation history |
| `/exit` | Exit the program |

## Project Structure
```
ai-chat-cli/
├── Program.cs          # Entry point, CLI loop
├── Models.cs           # Records — ChatMessage, OpenAiRequest, Response
├── OpenAiClient.cs     # HTTP client + IOpenAiClient interface
├── ChatSession.cs      # Session logic, history, request builder
├── AiChat.csproj
└── Tests/
    ├── ChatSessionTests.cs
    └── AiChat.Tests.csproj
```

## Tech Stack

- .NET 8
- C# 12
- OpenAI API — gpt-4o-mini
- xUnit — unit testing

## Author

**Szymon Wypler**

## License

MIT
```

---

Structure:
```
ai-chat-cli/
├── Program.cs
├── Models.cs
├── OpenAiClient.cs
├── ChatSession.cs
├── AiChat.csproj
└── Tests/
    ├── ChatSessionTests.cs
    └── AiChat.Tests.csproj
