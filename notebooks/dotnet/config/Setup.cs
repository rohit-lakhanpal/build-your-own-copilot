using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.Memory.AzureCognitiveSearch;
using Microsoft.SemanticKernel.Memory;
using Microsoft.SemanticKernel.Text;
using Microsoft.SemanticKernel.Reliability;
using Microsoft.SemanticKernel.Reliability.Basic;
using Microsoft.SemanticKernel.SemanticFunctions;
using Microsoft.SemanticKernel.Skills.Core;
using Microsoft.SemanticKernel.Skills.Web;
using Microsoft.SemanticKernel.Skills.Web.Bing;
using Microsoft.Extensions.Logging;

public static class Setup
{
    public static AIConfiguration Configuration { get; private set; }   

    static Setup()
    {
        // string settingsFilePath = "../config/settings.json";

        // if (!File.Exists(settingsFilePath))
        //     throw new FileNotFoundException("Settings file not found", settingsFilePath);

        // var settingsJson = File.ReadAllText(settingsFilePath);

        // var jObject = JObject.Parse(settingsJson);
        // var aiConfiguration = jObject.SelectToken("AIConfiguration");
        

        // if (aiConfiguration == null)
        //     throw new InvalidOperationException("AIConfiguration not found in settings.json");

        // Configuration = aiConfiguration.ToObject<AIConfiguration>();

        
        Configuration = GetConfiguration();
        ValidateConfiguration(Configuration);        
    }

    public static IKernel LoadKernel()
    {
        var kernel = Kernel.Builder

            // Use Azure Cognitive Search for the kernel Memory
            .WithMemoryStorage(new AzureCognitiveSearchMemoryStore(
                Configuration.AzureSearch.Endpoint,
                Configuration.AzureSearch.AdminKey)
            )

            // Use Azure OpenAI for Embeddings (model: text-embedding-ada-002)
            .WithAzureTextEmbeddingGenerationService(
                deploymentName: Configuration.GenerativeAI.Azure.EmbeddingDeploymentName,   
                endpoint: Configuration.GenerativeAI.Azure.Endpoint,
                apiKey: Configuration.GenerativeAI.Azure.ApiKey,
                setAsDefault: Configuration.GenerativeAI.DefaultProviders.EmbeddingService.Equals("Azure", StringComparison.OrdinalIgnoreCase)
            )
            
            // Use OpenAI for Embeddings (model: text-embedding-ada-002)
            // .WithOpenAITextEmbeddingGenerationService(
            //     modelId: Configuration.GenerativeAI.OpenAI.EmbeddingModelName,
            //     apiKey: Configuration.GenerativeAI.OpenAI.ApiKey,
            //     orgId: Configuration.GenerativeAI.OpenAI.OrganizationId,
            //     setAsDefault: Configuration.GenerativeAI.DefaultProviders.EmbeddingService.Equals("OpenAI", StringComparison.OrdinalIgnoreCase)    
            // )

            // Use Azure OpenAI for Semantic Functions (model = gpt-35-turbo-16k)
            .WithAzureChatCompletionService(
                //serviceId: "",
                deploymentName: Configuration.GenerativeAI.Azure.ChatDeploymentName,
                endpoint: Configuration.GenerativeAI.Azure.Endpoint,
                apiKey: Configuration.GenerativeAI.Azure.ApiKey,
                setAsDefault: Configuration.GenerativeAI.DefaultProviders.ChatService.Equals("Azure", StringComparison.OrdinalIgnoreCase)
            )

            // Use OpenAI for Semantic Functions (model = gpt-4-0613)
            // .WithOpenAIChatCompletionService(
            //     modelId: Configuration.GenerativeAI.OpenAI.ChatModelName,
            //     apiKey: Configuration.GenerativeAI.OpenAI.ApiKey,
            //     orgId: Configuration.GenerativeAI.OpenAI.OrganizationId,
            //     setAsDefault: Configuration.GenerativeAI.DefaultProviders.ChatService.Equals("OpenAI", StringComparison.OrdinalIgnoreCase)
            // )

            // Use Azure OpenAI for Semantic Functions (model = gpt-35-turbo-instruct)
            // .WithAzureTextCompletionService(
            //     deploymentName: Configuration.GenerativeAI.Azure.CompletionDeploymentName,
            //     endpoint: Configuration.GenerativeAI.Azure.Endpoint,
            //     apiKey: Configuration.GenerativeAI.Azure.ApiKey,
            //     setAsDefault: Configuration.GenerativeAI.DefaultProviders.CompletionService.Equals("Azure", StringComparison.OrdinalIgnoreCase)
            // )

            // Use OpenAI for Semantic Functions (model = gpt-3.5-turbo-instruct-0914)
            // .WithOpenAITextCompletionService(
            //     modelId: Configuration.GenerativeAI.OpenAI.CompletionModelName,
            //     apiKey: Configuration.GenerativeAI.OpenAI.ApiKey,
            //     orgId: Configuration.GenerativeAI.OpenAI.OrganizationId,
            //     setAsDefault: Configuration.GenerativeAI.DefaultProviders.CompletionService.Equals("OpenAI", StringComparison.OrdinalIgnoreCase)
            // )
            
            .WithRetryBasic(new BasicRetryConfig{
                UseExponentialBackoff = true,
                MaxRetryCount = 3
            })  

        .Build();

        // Note: In future releases, skills will be renamed to plugins
        // Import Bing Plugin/Skill         
        kernel.ImportSkill(new TextSkill(), "text");  
        kernel.ImportSkill(new TimeSkill(), "time");
        kernel.ImportSkill(new MathSkill(), "math");
        // kernel.ImportSkill(new WaitSkill());        
        // kernel.ImportSkill(new HttpSkill());

        kernel.ImportSkill(new ConversationSummarySkill(kernel), "conversation");
        kernel.ImportSkill(new WebFileDownloadSkill(), "web");        
        kernel.ImportSkill(new WebSearchEngineSkill(new BingConnector(Configuration.BingSearch.ApiKey)), "bing");
        
        return kernel;
    }


    private static AIConfiguration GetConfiguration()
    {
        var cb1 = new Microsoft.Extensions.Configuration.ConfigurationBuilder();
        var cb2 =  Microsoft.Extensions.Configuration.FileConfigurationExtensions.SetBasePath(cb1, Path.Combine(Directory.GetCurrentDirectory(), "../config"));
        var cb3 = Microsoft.Extensions.Configuration.JsonConfigurationExtensions.AddJsonFile(cb2, "settings.json", true);
        var cb4 = Microsoft.Extensions.Configuration.EnvironmentVariablesExtensions.AddEnvironmentVariables(cb3);
        var configuration = cb4.Build();
        var aiConfiguration = new AIConfiguration();
        var section = configuration.GetSection("AIConfiguration");        
        Microsoft.Extensions.Configuration.ConfigurationBinder.Bind(section, aiConfiguration);
        return aiConfiguration;
    }    

    private static void ValidateConfiguration(AIConfiguration configuration)
    {
        // Validate Bing Search API Key
        if (string.IsNullOrWhiteSpace(configuration.BingSearch.ApiKey))
            throw new InvalidOperationException("Bing Search API Key is not set in settings.json");

        // Validate Azure Search Endpoint and Admin Key
        if (string.IsNullOrWhiteSpace(configuration.AzureSearch.Endpoint))
            throw new InvalidOperationException("Azure Search Endpoint is not set in settings.json");

        if (!Uri.IsWellFormedUriString(configuration.AzureSearch.Endpoint, UriKind.Absolute))
            throw new InvalidOperationException("Azure Search Endpoint is not a valid URI in settings.json");

        if (string.IsNullOrWhiteSpace(configuration.AzureSearch.AdminKey))
            throw new InvalidOperationException("Azure Search Admin Key is not set in settings.json");

        // Validate DefaultProviders
        //ValidateDefaultProvider(configuration, "CompletionService");
        ValidateDefaultProvider(configuration, "ChatService");
        ValidateDefaultProvider(configuration, "EmbeddingService");        
    }

    private static void ValidateDefaultProvider(AIConfiguration configuration, string service)
    {
        var provider = typeof(DefaultProviders)
            .GetProperty(service)
            ?.GetValue(configuration.GenerativeAI.DefaultProviders) as string;

        if (provider == null)
            throw new InvalidOperationException($"Invalid service name {service} in DefaultProviders");

        if (provider.Equals("Azure", StringComparison.OrdinalIgnoreCase))
        {
            var azureConfig = configuration.GenerativeAI.Azure;
            ValidateAzureProvider(azureConfig, service);
        }
        else if (provider.Equals("OpenAI", StringComparison.OrdinalIgnoreCase))
        {
            var openAIConfig = configuration.GenerativeAI.OpenAI;
            ValidateOpenAIProvider(openAIConfig, service);
        }
        else
            throw new InvalidOperationException($"Invalid provider {provider} for {service} in DefaultProviders");
    }

    private static void ValidateAzureProvider(Azure azureConfig, string service)
    {
        if (string.IsNullOrWhiteSpace(azureConfig.Endpoint))
            throw new InvalidOperationException($"Azure Endpoint for {service} is not set in settings.json");

        if (!Uri.IsWellFormedUriString(azureConfig.Endpoint, UriKind.Absolute))
            throw new InvalidOperationException($"Azure Endpoint for {service} is not a valid URI in settings.json");

        if (string.IsNullOrWhiteSpace(azureConfig.ApiKey))
            throw new InvalidOperationException($"Azure API Key for {service} is not set in settings.json");

        var deploymentNameProperty = typeof(Azure)
            .GetProperty($"{service.Replace("Service", string.Empty)}DeploymentName");

        var deploymentName = deploymentNameProperty?.GetValue(azureConfig) as string;

        if (string.IsNullOrWhiteSpace(deploymentName))
            throw new InvalidOperationException($"Azure Deployment Name for {service} is not set in settings.json");
    }

    private static void ValidateOpenAIProvider(OpenAI openAIConfig, string service)
    {
        if (string.IsNullOrWhiteSpace(openAIConfig.OrganizationId))
            throw new InvalidOperationException("OpenAI Organization ID is not set in settings.json");

        if (string.IsNullOrWhiteSpace(openAIConfig.ApiKey))
            throw new InvalidOperationException("OpenAI API Key is not set in settings.json");

        var modelNameProperty = typeof(OpenAI)
            .GetProperty($"{service.Replace("Service", string.Empty)}ModelName");

        var modelName = modelNameProperty?.GetValue(openAIConfig) as string;

        if (string.IsNullOrWhiteSpace(modelName))
            throw new InvalidOperationException($"OpenAI Model Name for {service} is not set in settings.json");
    }

}


public class AIConfiguration
{
    [JsonProperty("GenerativeAI")]
    public GenerativeAI GenerativeAI { get; set; }

    [JsonProperty("AzureSearch")]
    public AzureSearch AzureSearch { get; set; }

    [JsonProperty("BingSearch")]
    public BingSearch BingSearch { get; set; }
}

public class GenerativeAI
{
    [JsonProperty("DefaultProviders")]
    public DefaultProviders DefaultProviders { get; set; }

    [JsonProperty("Azure")]
    public Azure Azure { get; set; }

    [JsonProperty("OpenAI")]
    public OpenAI OpenAI { get; set; }
}

public class DefaultProviders
{
    [JsonProperty("CompletionService")]
    public string CompletionService { get; set; }

    [JsonProperty("ChatService")]
    public string ChatService { get; set; }

    [JsonProperty("EmbeddingService")]
    public string EmbeddingService { get; set; }
}

public class Azure
{
    [JsonProperty("Endpoint")]
    public string Endpoint { get; set; }

    [JsonProperty("ApiKey")]
    public string ApiKey { get; set; }

    [JsonProperty("CompletionDeploymentName")]
    public string CompletionDeploymentName { get; set; }

    [JsonProperty("ChatDeploymentName")]
    public string ChatDeploymentName { get; set; }

    [JsonProperty("EmbeddingDeploymentName")]
    public string EmbeddingDeploymentName { get; set; }
}

public class OpenAI
{
    [JsonProperty("OrganizationId")]
    public string OrganizationId { get; set; }

    [JsonProperty("ApiKey")]
    public string ApiKey { get; set; }

    [JsonProperty("CompletionModelName")]
    public string CompletionModelName { get; set; }

    [JsonProperty("ChatModelName")]
    public string ChatModelName { get; set; }

    [JsonProperty("EmbeddingModelName")]
    public string EmbeddingModelName { get; set; }
}

public class AzureSearch
{
    [JsonProperty("Endpoint")]
    public string Endpoint { get; set; }

    [JsonProperty("AdminKey")]
    public string AdminKey { get; set; }
}

public class BingSearch
{
    [JsonProperty("ApiKey")]
    public string ApiKey { get; set; }
}
