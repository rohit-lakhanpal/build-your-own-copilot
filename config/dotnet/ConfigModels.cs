using System.Collections.Generic;

public class OpenAiConfiguration
{
    public string ApiType { get; set; }
    public string ApiKey { get; set; }
    public string OrgId { get; set; }
    public string BaseUrl { get; set; }
    public string ModelId { get; set; }
    public string ModelType { get; set; }
    public string ServiceId { get; set; }
}

public class AzureSearchConfig
{
    public string Endpoint { get; set; }
    public string AdminKey { get; set; }
}

public class BingSearchConfig
{
    public string ApiKey { get; set; }
}

public class CopilotAppConfig
{
    public List<OpenAiConfiguration> OpenAiConfigs { get; set; }
    public AzureSearchConfig AzureSearchConfig { get; set; }
    public BingSearchConfig BingSearchConfig { get; set; }    
}