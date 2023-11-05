using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Reliability.Basic;
using Microsoft.SemanticKernel.Plugins.Core;
using Microsoft.SemanticKernel.Plugins.Web;
using Microsoft.SemanticKernel.Plugins.Web.Bing;

public static class KernelHelper
{
    public static IKernel LoadKernel()
    {
        var configuration = new ConfigurationHelper().GetConfiguration();

        var builder = new KernelBuilder();

        var defaultCompletion = true;

        // Loop through OpenAiConfigurations 
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


        builder.WithRetryBasic(new BasicRetryConfig
        {
            MaxRetryCount = 3,
            UseExponentialBackoff = true,
        });

        var kernel = builder.Build();

        // Add the Bing search engine if the API key is provided.
        if(configuration.BingSearchConfig != null && !string.IsNullOrEmpty(configuration.BingSearchConfig.ApiKey))
        {
            kernel.ImportFunctions(new WebSearchEnginePlugin(new BingConnector(configuration.BingSearchConfig.ApiKey)), "bing");
        }

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

        return kernel;
    }
}