using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

public class ConfigurationHelper
{
    private readonly string _configuration;
    private readonly CopilotAppConfig appConfig;

    private string pathToConfigFile = "../../../config/settings.json";

    public ConfigurationHelper(string path):this()
    {
        pathToConfigFile = path;
    }
    public ConfigurationHelper()
    {        
        if (!System.IO.File.Exists(pathToConfigFile))
        {
            throw new Exception($"The configuration file {pathToConfigFile} does not exist.");
        }
        else
        {
            _configuration = System.IO.File.ReadAllText(pathToConfigFile);            
            appConfig = JsonSerializer.Deserialize<CopilotAppConfig>(_configuration, new JsonSerializerOptions
            {
                ReadCommentHandling = JsonCommentHandling.Skip,
                AllowTrailingCommas = true,
            });
            ValidateConfigurations();
        }
    }

    public CopilotAppConfig GetConfiguration()
    {
        return appConfig;
    }


    public void ValidateConfigurations()
    {
        ValidateOpenAiConfigurations(appConfig.OpenAiConfigs);
        ValidateAzureSearchConfig(appConfig.AzureSearchConfig);
        ValidateBingSearchConfig(appConfig.BingSearchConfig);
    }

    private void ValidateAzureSearchConfig(AzureSearchConfig azureSearchConfig)
    {
        if (azureSearchConfig == null || string.IsNullOrEmpty(azureSearchConfig.Endpoint) || string.IsNullOrEmpty(azureSearchConfig.AdminKey))
            throw new Exception("AzureSearchConfig section is missing or invalid. Please ensure the section exists and Endpoint and AdminKey are properly configured.");
    }

    private void ValidateBingSearchConfig(BingSearchConfig bingSearchConfig)
    {
        if (bingSearchConfig == null || string.IsNullOrEmpty(bingSearchConfig.ApiKey))
            throw new Exception("BingSearchConfig section is missing or invalid. Please ensure the section exists and ApiKey is properly configured.");
    }

    private void ValidateOpenAiConfigurations(List<OpenAiConfiguration> openAiConfigs)
{
    if (openAiConfigs == null || !openAiConfigs.Any())
        throw new Exception("OpenAiConfigurations section is missing or invalid. Please ensure the section exists and contains valid configurations.");

    // Check for duplicate ServiceIds
    var duplicateServiceIds = openAiConfigs.GroupBy(config => config.ServiceId)
                                           .Where(group => group.Count() > 1)
                                           .Select(group => group.Key)
                                           .ToList();

    if (duplicateServiceIds.Any())
        throw new Exception($"Duplicate ServiceId(s) found: {string.Join(", ", duplicateServiceIds)}.");

    foreach (var config in openAiConfigs)
    {
        if (string.IsNullOrEmpty(config.ApiType) || (config.ApiType != "openai" && config.ApiType != "azure"))
            throw new Exception($"Invalid ApiType '{config.ApiType}' in OpenAiConfigurations. Allowed values are 'openai' and 'azure'.");

        if (string.IsNullOrEmpty(config.ApiKey))
            throw new Exception($"ApiKey is missing for configuration index {openAiConfigs.IndexOf(config)} in OpenAiConfigurations.");

        if (config.ApiType == "openai" && string.IsNullOrEmpty(config.OrgId))
            throw new Exception($"Invalid OpenAi specific configurations for configuration index {openAiConfigs.IndexOf(config)} in OpenAiConfigurations. Please ensure OrgId is properly configured.");

        if (config.ApiType == "azure" && string.IsNullOrEmpty(config.BaseUrl))
            throw new Exception($"Invalid Azure specific configurations for configuration index {openAiConfigs.IndexOf(config)} in OpenAiConfigurations. Please ensure BaseUrl is properly configured.");

        if (config.ApiType == "azure" && string.IsNullOrEmpty(config.ModelId))
            throw new Exception($"Invalid Azure specific configurations for configuration index {openAiConfigs.IndexOf(config)} in OpenAiConfigurations. Please ensure ModelId (a.k.a Deployment Id) is properly configured.");

        if (config.ApiType == "azure" && (string.IsNullOrEmpty(config.ModelType) || (config.ModelType != "embeddings" && config.ModelType != "completions")))
            throw new Exception($"Invalid Azure specific configurations for configuration index {openAiConfigs.IndexOf(config)} in OpenAiConfigurations. Please ensure ModelType is properly configured. Allowed values are 'completions' and 'embeddings'.");
    }
}

}
