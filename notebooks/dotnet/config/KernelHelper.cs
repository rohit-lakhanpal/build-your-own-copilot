using System.IO;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.Memory.AzureCognitiveSearch;
using Microsoft.SemanticKernel.Memory;
using Microsoft.SemanticKernel.Text;
using Microsoft.SemanticKernel.Reliability;
using Microsoft.SemanticKernel.Reliability.Basic;
using Microsoft.SemanticKernel.SemanticFunctions;
using static Microsoft.SemanticKernel.SemanticFunctions.PromptTemplateConfig;
using Microsoft.SemanticKernel.AI.ChatCompletion;
using Microsoft.SemanticKernel.TemplateEngine.Prompt;
using Microsoft.SemanticKernel.Orchestration;

public static class KernelHelper
{
    public static async Task<ChatHistory> ProcessAsync(
        IKernel kernel, 
        string systemMessage, 
        string userMessage,
        SKContext context,
        ChatRequestSettings chatRequestSettings = null)

    {
        var chatCompletionService = kernel.GetService<IChatCompletion>();        
        var promptRenderer = new PromptTemplateEngine();        
        
        if(chatRequestSettings == null)
        {
            chatRequestSettings = new ChatRequestSettings
            {
                MaxTokens = 2000,
                Temperature = 0.7,
                TopP = 0.9,                
            };
        }

        var chatHistory = chatCompletionService.CreateNewChat(await promptRenderer.RenderAsync(systemMessage, context));
        chatHistory.AddUserMessage(await promptRenderer.RenderAsync(userMessage, context));
               
        var result = await chatCompletionService.GenerateMessageAsync(chatHistory, chatRequestSettings);
        chatHistory.AddAssistantMessage(result);

        return chatHistory;
    }

    public static async Task PrepareDatabaseAsync(IKernel kernel, string path, string db)
    {
        var chunks = await Utils.GetChunksAsync(path);
        Console.WriteLine($"Chunks: {chunks.Count()}");


        // Save chunks into memory
        for (var i = 0; i < chunks.Count(); i++)
        {
            string chunk = chunks[i]; 

            await kernel.Memory.SaveInformationAsync(
                db,                         // collection name
                chunk,                      // text
                $"{db}-{i}",                // id
                $"Dataset: {db} Chunk: {i}",// title or description
                i.ToString()               // metadata
            );

            Console.WriteLine($"Chunk {i} of {chunks.Count} saved to memory collection {db}");
        }
    }

    public static async Task<List<dynamic>> SearchDatabaseAsync(IKernel kernel, string db, string q, int limit = 1)
    {
        var list = new List<dynamic>();

        await foreach (var item in kernel.Memory.SearchAsync(
            collection: db, query: q, limit: limit))
        {
            list.Add(item.Metadata);
            //Utils.Print($"{item.Metadata.Description} - {item.Metadata.Text}");
        }

        return list;
    }
}