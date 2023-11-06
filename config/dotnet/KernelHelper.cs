using static Microsoft.DotNet.Interactive.Formatting.PocketViewTags;
using System;
using System.IO;
using System.Text.Json;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Text;
using Microsoft.SemanticKernel.Reliability.Basic;
using Microsoft.SemanticKernel.Plugins.Core;
using Microsoft.SemanticKernel.Plugins.Web;
using Microsoft.SemanticKernel.Plugins.Web.Bing;
using Microsoft.SemanticKernel.Connectors.AI.OpenAI;
using Microsoft.SemanticKernel.Connectors.Memory.AzureCognitiveSearch;
using Microsoft.SemanticKernel.Memory;
using Microsoft.SemanticKernel.Plugins.Memory;
using Microsoft.SemanticKernel.Orchestration;
public static class KernelHelper
{
    public static IKernel LoadKernel()
    {
        var builder = new KernelBuilder();
        var configuration = new ConfigurationHelper().GetConfiguration();
        ConfigureKernel(builder, configuration);
        var kernel = builder.Build();
        LoadPlugins(kernel, configuration);
        return kernel;
    }

    private static void ConfigureKernel(KernelBuilder builder, CopilotAppConfig configuration)
    {
        var defaultCompletion = true;

        foreach (var openAiConfig in configuration.OpenAiConfigs)
        {
            if (openAiConfig.ApiType == "azure")
            {
                if (openAiConfig.ModelType == "completions")
                {
                    builder.WithAzureChatCompletionService(
                        openAiConfig.ModelId,       // Azure OpenAI deployment name.
                        openAiConfig.BaseUrl,       // Azure OpenAI deployment URL.
                        openAiConfig.ApiKey,        // Azure OpenAI API key.
                        true,                       // Whether to use the service also for text completion, if supported.
                        openAiConfig.ServiceId,     // A local identifier for the given AI service.                    
                        defaultCompletion           // Whether the service should be the default for its type.
                    );
                    if (defaultCompletion) { defaultCompletion = false; }
                }
                if (openAiConfig.ModelType == "embeddings")
                {
                    builder.WithAzureTextEmbeddingGenerationService(
                        openAiConfig.ModelId,       // Azure OpenAI deployment name.
                        openAiConfig.BaseUrl,       // Azure OpenAI deployment URL.
                        openAiConfig.ApiKey,        // Azure OpenAI API key.                        
                        openAiConfig.ServiceId,     // A local identifier for the given AI service.                    
                        true                        // Whether the service should be the default for its type.
                    );
                }
            }

            // Check if the ApiType is OpenAi
            if (openAiConfig.ApiType == "openai")
            {
                if (openAiConfig.ModelType == "completions")
                {
                    builder.WithOpenAIChatCompletionService(
                        openAiConfig.ModelId,       // OpenAI model name.
                        openAiConfig.ApiKey,        // OpenAI API key.
                        openAiConfig.OrgId,         // OpenAI organization id.              
                        openAiConfig.ServiceId,     // A local identifier for the given AI service.                    
                        true                        // Whether to use the service also for text completion, if supported.
                    );
                }
                if (openAiConfig.ModelType == "embeddings")
                {
                    builder.WithOpenAITextEmbeddingGenerationService(
                        openAiConfig.ModelId,       // OpenAI model name.
                        openAiConfig.ApiKey,        // OpenAI API key.
                        openAiConfig.OrgId,         // OpenAI organization id.              
                        openAiConfig.ServiceId      // A local identifier for the given AI service.                                          
                    );
                }
            }
        }

        builder.WithMemoryStorage(new AzureCognitiveSearchMemoryStore(
            configuration.AzureSearchConfig.Endpoint,
            configuration.AzureSearchConfig.AdminKey));


        builder.WithRetryBasic(new BasicRetryConfig
        {
            MaxRetryCount = 2,
            UseExponentialBackoff = true,
        });
    }

    private static void LoadPlugins(IKernel kernel, CopilotAppConfig configuration)
    {
        // Add the Bing search engine if the API key is provided.
        if(configuration.BingSearchConfig != null && !string.IsNullOrEmpty(configuration.BingSearchConfig.ApiKey))
        {
            kernel.ImportFunctions(new WebSearchEnginePlugin(new BingConnector(configuration.BingSearchConfig.ApiKey)), "bing");
        }

        kernel.ImportFunctions(new WebFileDownloadPlugin(), "web");
        kernel.ImportFunctions(new SearchUrlPlugin(), "url");

        // Adding Text Plugin. See: https://github.com/microsoft/semantic-kernel/blob/main/dotnet/src/Plugins/Plugins.Core/TextPlugin.cs
        // SKContext.Variables["input"] = "  hello world  "
        // {{text.trim $input}} => "hello world"
        // {{text.trimStart $input} => "hello world  "
        // {{text.trimEnd $input} => "  hello world"
        // SKContext.Variables["input"] = "hello world"
        // {{text.uppercase $input}} => "HELLO WORLD"
        // SKContext.Variables["input"] = "HELLO WORLD"
        // {{text.lowercase $input}} => "hello world"
        kernel.ImportFunctions(new TextPlugin(), "text");

        // Adding Conversation Plugin. See: https://github.com/microsoft/semantic-kernel/blob/main/dotnet/src/Plugins/Plugins.Core/ConversationSummaryPlugin.cs
        kernel.ImportFunctions(new ConversationSummaryPlugin(kernel), "conversation");

        // Adding Time Plugin. See: https://github.com/microsoft/semantic-kernel/blob/main/dotnet/src/Plugins/Plugins.Core/TimePlugin.cs
        // Examples:
        // {{time.date}}            => Sunday, 12 January, 2031
        // {{time.today}}           => Sunday, 12 January, 2031
        // {{time.now}}             => Sunday, January 12, 2031 9:15 PM
        // {{time.utcNow}}          => Sunday, January 13, 2031 5:15 AM
        // {{time.time}}            => 09:15:07 PM
        // {{time.year}}            => 2031
        // {{time.month}}           => January
        // {{time.monthNumber}}     => 01
        // {{time.day}}             => 12
        // {{time.dayOfMonth}}      => 12
        // {{time.dayOfWeek}}       => Sunday
        // {{time.hour}}            => 9 PM
        // {{time.hourNumber}}      => 21
        // {{time.daysAgo $days}}   => Sunday, January 12, 2025 9:15 PM
        // {{time.lastMatchingDay $dayName}} => Sunday, 7 May, 2023
        // {{time.minute}}          => 15
        // {{time.minutes}}         => 15
        // {{time.second}}          => 7
        // {{time.seconds}}         => 7
        // {{time.timeZoneOffset}}  => -08:00
        // {{time.timeZoneName}}    => PST
        kernel.ImportFunctions(new TimePlugin(), "time"); 

        // Adding Wait Plugin. See: https://github.com/microsoft/semantic-kernel/blob/main/dotnet/src/Plugins/Plugins.Core/WaitPlugin.cs
        // Examples:
        // {{wait.seconds 10}} => Wait 10 seconds
        kernel.ImportFunctions(new WaitPlugin(), "wait");

        // Adding Math Plugin. See: https://github.com/microsoft/semantic-kernel/blob/main/dotnet/src/Plugins/Plugins.Core/MathPlugin.cs
        // Examples:
        // {{math.Add}} => FirstNumber + SecondNumber  (provided in the SKContext)
        // {{math.Subtract}} => FirstNumber - SecondNumber (provided in the SKContext)
        kernel.ImportFunctions(new MathPlugin(), "math");

        // Adding Http Plugin. See: https://github.com/microsoft/semantic-kernel/blob/main/dotnet/src/Plugins/Plugins.Core/HttpPlugin.cs
        // Examples:
        // {{http.getAsync $url}}
        // {{http.postAsync $url}}
        // {{http.putAsync $url}}
        // {{http.deleteAsync $url}}
        kernel.ImportFunctions(new HttpPlugin(), "http");

        // Adding FileIO Plugin. See: https://github.com/microsoft/semantic-kernel/blob/main/dotnet/src/Plugins/Plugins.Core/FileIOPlugin.cs
        // Examples:
        // {{file.readAsync $path }} => "hello world"
        // {{file.writeAsync}}        
        kernel.ImportFunctions(new FileIOPlugin(), "file");
    }

    public static async Task<List<string>> GetChunksAsync(string filePath)
    {
        const int maxTokensPerParagraph = 160;
        const int maxTokensPerLine = 60;

        // Check if file exists
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"File not found: {filePath}");
        }
        
        // Read file from local file system        
        var streamReader = new StreamReader(filePath);
        var text = await streamReader.ReadToEndAsync();

        var lines = TextChunker.SplitPlainTextLines(text, maxTokensPerLine);
        return TextChunker.SplitPlainTextParagraphs(lines, maxTokensPerParagraph);
    }

    public static async Task<List<string>> SaveFileAsync(IKernel kernel, string path, string db)
    {
        var chunks = await GetChunksAsync(path);

        var ids = new List<string>();
        
        // Save chunks into memory
        for (var i = 0; i < chunks.Count(); i++)
        {
            string chunk = chunks[i]; 
            string chunkId = $"{db}-{i}";
            ids.Add(await SaveChunkAsync(kernel, chunk, db, chunkId));   
            Console.WriteLine($"- Saved chunk {i}.");         
        }

        return ids;
    }

    public static async Task<string> SaveChunkAsync(IKernel kernel, string text, string db, string id = "", string description = "", string metadata = "")
    {
        if(string.IsNullOrEmpty(id))
        {
            id = Guid.NewGuid().ToString();
        }

        if(string.IsNullOrEmpty(description))
        {
            description = $"d:{db} c:{id}";
        }

        if(string.IsNullOrEmpty(metadata))
        {
            metadata = id;
        }

        await kernel.Memory.SaveInformationAsync(
            collection: db,                // Collection where to save the information.
            text: text,                    // Information to save.
            id: id,                        // Unique identifier.
            description: description,      // Optional description.
            additionalMetadata: metadata   // Optional string for saving custom metadata.
        );

        return id;
    }

    public static async Task<List<dynamic>> SearchDatabaseAsync(IKernel kernel, string db, string q, int limit = 1, double minRelevanceScore = 0.5)
    {
        var list = new List<dynamic>();

        await foreach (var item in kernel.Memory.SearchAsync(
            collection: db,                         // Collection to search in.
            query: q,                               // Query to search for.
            limit: limit,                           // Maximum number of results to return.
            minRelevanceScore: minRelevanceScore    // Minimum relevance score to return.
            ))
        {
            list.Add(item.Metadata);            
        }

        return list;
    }

    public static async Task<List<string>> DeleteChunkAsync(IKernel kernel, string db, List<string> ids)
    {
        var list = new List<string>();

        foreach(var i in ids)
        {
            list.Add(await DeleteChunkAsync(kernel, db, i));
        }

        return list;
    }

    public static async Task<string> DeleteChunkAsync(IKernel kernel, string db, string id)
    {
        await kernel.Memory.RemoveAsync(
            db,     // Collection where to remove the information.
            id      // Unique identifier.
        );

        return id;        
    }

    public static void Print(KernelResult r, PrintElement element = PrintElement.Paragraph)
    {
        Print(r.GetValue<string>(), element);
    }

    public static void Print(FunctionResult r, PrintElement element = PrintElement.Paragraph)
    {
        Print(r.GetValue<string>(), element);
    }

    public static void Print(string text, PrintElement element)
    {
        // Important to escape any HTML tags in the text
        text = text.Replace("<", "&lt;").Replace(">", "&gt;");

        var baseStyle = "color:#a0d2eb;background-color:#8458B3;";      
        var style = $"{baseStyle}padding:10px;border-radius:5px;";
        switch(element)
        {
            case PrintElement.Header:                      
                display(p[style:$"{style};font-weight:bolder;text-decoration:underline;"](text));
                break;
            case PrintElement.Paragraph:                
                display(p[style:style](text));
                break;
            case PrintElement.Pre:                
                display(div[style:$"{style}font-family: monospace;"](span[style:$"white-space:pre;color:black!important;background-color:#a0d2eb!important"](text)));
                break;
            case PrintElement.Muted:
                display(pre(text));
                break;
        }
    }

    public static void Print(string text)
    {
        Print(text, PrintElement.Paragraph);
    }
    public static void Info(string text)
    {
        Print(text, PrintElement.Muted);
    }
}

public enum PrintElement 
{
    Header,
    Paragraph,
    Pre,

    Muted
}