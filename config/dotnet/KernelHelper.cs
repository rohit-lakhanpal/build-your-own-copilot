using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Reliability.Basic;

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

        return builder.Build();
    }
}