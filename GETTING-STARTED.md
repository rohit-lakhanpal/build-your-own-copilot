<a id="readme-top"></a>

<!-- PROJECT SHIELDS -->
<!--
*** Using markdown "reference style" links for readability.
*** Reference links are enclosed in brackets [ ] instead of parentheses ( ).
*** See the bottom of this document for the declaration of the reference variables
*** for contributors-url, forks-url, etc. This is an optional, concise syntax you may use.
*** https://www.markdownguide.org/basic-syntax/#reference-style-links
-->

[![Contributors][contributors-shield]][contributors-url]
[![Forks][forks-shield]][forks-url]
[![Stargazers][stars-shield]][stars-url]
[![Issues][issues-shield]][issues-url]
[![MIT License][license-shield]][license-url]
[![LinkedIn][linkedin-shield]][linkedin-url]


<!-- PROJECT LOGO -->
<br />
<div align="center">
  <h3 align="center">
    Build your own Co-Pilot | Home
  </h3>

  <p align="center">
    powered by
    <br />
    <a href="https://github.com/rohit-lakhanpal/build-your-own-copilot">
    <img src="docs/img/logo.png" alt="Logo" height="80">
  </a>
    <br />    
    <a href="README.md">Introduction</a>
    →
    [ <a href="GETTING-STARTED.md"><u>Getting Started</u></a> ]
    →
    <a href="USE-CASES.md">Use Cases</a>
    →
    <a href="RESOURCES.md"><u>Resources</u></a> 
  </p>
</div>

## 1. Clone the repo

1. Clone the repo
    ```sh
    git clone https://github.com/rohit-lakhanpal/build-your-own-copilot.git
    ```

## 2. Deploy Cloud Resources

> ⚠️ **Note:** If you are running a hackathon & plan to use an ***Azure Pass*** for your team, please follow these [instructions](docs/deploy/AZURE_PASS.md) to deploy the resources.

### 2.a. Deploy Azure Cognitive Search

Click this button to deploy an `Azure Cognitive Search` service:

[![Deploy Azure Cognitive Search](https://aka.ms/deploytoazurebutton)](https://aka.ms/byoc-deploy-search)

#### Post-Deployment:
- After deploying, navigate to the **Azure Cognitive Search** resource within the Azure Portal.
- In the left pane, select **Keys** to access the admin and query keys.
- Copy the **Admin key** or **Query key** as required for your application.
- Setup Config:
  - To set up configuration values using `settings.json`:
    ```json
    "AIConfiguration": {
      "AzureSearch": {
        "Endpoint": "<YOUR_ENDPOINT>",
        "AdminKey": "<YOUR_ADMIN_KEY>"
      }
    }
    ```
    
  - Or using .NET user secrets
    ```cmd
    dotnet user-secrets set "AIConfiguration:AzureSearch:Endpoint" "<YOUR_ENDPOINT>"
    dotnet user-secrets set "AIConfiguration:AzureSearch:AdminKey" "<YOUR_ADMIN_KEY>"
    ```


#### Learn More:
- [Official Documentation](https://learn.microsoft.com/en-us/azure/search/)
- [How to Create an Azure Cognitive Search service](https://learn.microsoft.com/en-us/azure/search/search-create-service-portal)

<p align="right">(<a href="#readme-top">back to top</a>)</p>

---

### 2.b. Deploy Azure AI Speech

Click this button to deploy an `Azure AI Speech` service:

[![Deploy Azure AI Speech](https://aka.ms/deploytoazurebutton)](https://aka.ms/byoc-deploy-speech)

#### Post-Deployment:
- Go to the **Azure AI Speech** service in the Azure Portal.
- Select **Keys and Endpoint** in the left navigation pane.
- Here you can retrieve the primary and secondary keys for your service.

#### Learn More:
- [Official Documentation](https://learn.microsoft.com/en-us/azure/ai-services/speech-service/)

<p align="right">(<a href="#readme-top">back to top</a>)</p>

---

### 2.c. Deploy Azure AI Vision

Click this button to deploy an `Azure AI Vision` service:

[![Deploy Azure AI Vision](https://aka.ms/deploytoazurebutton)](https://aka.ms/byoc-deploy-vision)

#### Post-Deployment:
- Navigate to the **Azure AI Vision** resource in the Azure Portal.
- Click on **Keys and Endpoint** in the left sidebar.
- Retrieve the **Key1** or **Key2** as needed.

#### Learn More:
- [Official Documentation](https://learn.microsoft.com/en-us/azure/ai-services/computer-vision/)

<p align="right">(<a href="#readme-top">back to top</a>)</p>

---

### 2.d. Deploy Azure AI Language

Click this button to deploy an `Azure AI Language` service:

[![Deploy Azure AI Language](https://aka.ms/deploytoazurebutton)](https://aka.ms/byoc-deploy-language)

#### Post-Deployment:
- Once deployed, go to the **Azure AI Language** service inside the Azure Portal.
- Navigate to **Keys and Endpoint** from the left navigation pane.
- Copy the required **API key** for your application.

#### Learn More:
- [Official Documentation](https://learn.microsoft.com/en-us/azure/cognitive-services/language/)

<p align="right">(<a href="#readme-top">back to top</a>)</p>

---

### 2.e. Deploy Azure OpenAI Service

Click this button to deploy an `Azure OpenAI` service:

[![Deploy Azure OpenAI Service](https://aka.ms/deploytoazurebutton)](https://aka.ms/byoc-deploy-openai)

#### Post-Deployment:
- After deploying, open the **Azure OpenAI Service** resource within the Azure Portal.
- Click on **Keys and Endpoint** on the left sidebar.
- Retrieve the desired **API key** for integration with your application.
- Setup Config:
  - To set up configuration values using `settings.json`:
    ```json
    "AIConfiguration": {
      "GenerativeAI": {
        "DefaultProviders": {
          "CompletionService": "Azure",
          "ChatService": "Azure",
          "EmbeddingService": "Azure"
        },
        "Azure": {
          "Endpoint": "https://YOUR-SERVICE.azure.com/",
          "CompletionModelName": "YOUR gpt-3.5-turbo-instruct DEPLOYMENT NAME",
          "ChatModelName": "YOUR gpt-4 DEPLOYMENT NAME",
          "EmbeddingModelName": "YOUR text-embedding-ada-002 DEPLOYMENT NAME"
        }
      }
    }
    ```
    
  - Or using .NET user secrets
    ```cmd
      dotnet user-secrets set "AIConfiguration:GenerativeAI:DefaultProviders:CompletionService" "Azure"
      dotnet user-secrets set "AIConfiguration:GenerativeAI:DefaultProviders:ChatService" "Azure"
      dotnet user-secrets set "AIConfiguration:GenerativeAI:DefaultProviders:EmbeddingService" "Azure"
      dotnet user-secrets set "AIConfiguration:GenerativeAI:Azure:Endpoint" "https://YOUR-SERVICE.azure.com/"
      dotnet user-secrets set "AIConfiguration:GenerativeAI:Azure:CompletionModelName" "YOUR gpt-3.5-turbo-instruct DEPLOYMENT NAME"
      dotnet user-secrets set "AIConfiguration:GenerativeAI:Azure:ChatModelName" "YOUR gpt-4 DEPLOYMENT NAME"
      dotnet user-secrets set "AIConfiguration:GenerativeAI:Azure:EmbeddingModelName" "YOUR text-embedding-ada-002 DEPLOYMENT NAME"

    ```

#### Learn More:
- [Official Documentation](https://learn.microsoft.com/en-us/azure/cognitive-services/openai-service/)
- [Create and Deploy an Azure OpenAI Service Resource](https://learn.microsoft.com/en-us/azure/cognitive-services/openai-service/create-deploy-resource)

<p align="right">(<a href="#readme-top">back to top</a>)</p>

---

### 2.f. Deploy Azure AI Content Safety

Click this button to deploy an `Azure AI Content Safety` service:

[![Deploy Azure AI Content Safety](https://aka.ms/deploytoazurebutton)](https://aka.ms/byoc-deploy-content-safety)

#### Post-Deployment:
- Navigate to the **Azure AI Content Safety** service within the Azure Portal.
- Select **Keys and Endpoint** from the left pane.
- Copy the **Key1** or **Key2** as necessary for your project.

#### Learn More:
- [Official Documentation](https://learn.microsoft.com/en-us/azure/cognitive-services/content-safety/)
- [Content Safety Studio](https://contentsafety.cognitive.azure.com/)

<p align="right">(<a href="#readme-top">back to top</a>)</p>

---

### 2.g. Deploy Bing Search

Click this button to deploy a `Bing Search` service:

[![Deploy Bing Search](https://aka.ms/deploytoazurebutton)](byoc-deploy-bing-search)

#### Post-Deployment:
- After deployment, navigate to the **Bing Search** resource within the Azure Portal.
- In the left pane, select **Keys and Endpoint**.
- Here, you can access the primary and secondary keys for your Bing Search service. Copy either **Key1** or **Key2** as needed for your application.
- Setup Config:
  - To set up configuration values using `settings.json`:
    ```json
    "AIConfiguration": {
      "BingSearch": {
        "ApiKey": "<YOUR_BING_API_KEY>"
      }
    }
    ```
    
  - Or using .NET user secrets
    ```cmd
    dotnet user-secrets set "AIConfiguration:BingSearch:ApiKey" "<YOUR_BING_API_KEY>"
    ```

#### Learn More:
- [Official Documentation](https://learn.microsoft.com/en-us/azure/cognitive-services/bing-web-search/)
- [How to Create a Bing Search Resource](https://learn.microsoft.com/en-us/azure/cognitive-services/bing-web-search/create-bing-search-resource-portal)

<p align="right">(<a href="#readme-top">back to top</a>)</p>

## 3. Install Local Tools

Here are the documentation and resources for installing the local tools you requested:

#### 3.1. **VS Code Installation**:
   - **[Official Documentation](https://code.visualstudio.com/docs)**: Provides an overview, setup instructions, and introductory videos for getting started with VS Code.
   - **[Download Page](https://code.visualstudio.com/Download)**: You can download VS Code for Linux, macOS, or Windows from this page.

<p align="right">(<a href="#readme-top">back to top</a>)</p>

---

#### 3.2. **Extensions for Polyglot Notebooks**:
   - **[Polyglot Programming with Notebooks](https://marketplace.visualstudio.com/items?itemName=MS-dotnettools.dotnet-interactive-vscode)**: Describes the Polyglot Notebooks extension which allows multiple programming languages to be used natively in the same notebook in VS Code.
   - **[Install the Polyglot Notebooks extension](https://marketplace.visualstudio.com/items?itemName=MS-dotnettools.dotnet-interactive-vscode)**: Polyglot Notebooks for VS Code. Use multiple languages in one notebook with full language server support for each language and share variables between them.

These resources should provide a comprehensive guide to installing VS Code and setting up extensions for polyglot notebooks.

<p align="right">(<a href="#readme-top">back to top</a>)</p>

---

#### 3.3. **Chat-copilot**:
- **[Chat Copilot Sample Application](https://github.com/microsoft/chat-copilot)**: This repository contains a sample application that demonstrates how to use the Chat Copilot API to build a chatbot that can answer questions about products and services.

<p align="right">(<a href="#readme-top">back to top</a>)</p>

---

#### 3.4. **ChatGPT Plugin Quickstart using Python and FastAPI**:
- **[ChatGPT Plugin Quickstart using Python and FastAPI](https://github.com/Azure-Samples/openai-plugin-fastapi)**: This repository contains a sample ChatGPT Plugin using GitHub Codespaces, VS Code (locally), and Azure.

For this example, plese follow the [Run Locally](https://github.com/Azure-Samples/openai-plugin-fastapi#run-locally) instructions.

<p align="right">(<a href="#readme-top">back to top</a>)</p>

---
## 4. Azure ML Prompt Flow ##

### 4.1. General Prerequisites for Azure ML Prompt Flow

- [Get started with Prompt flow (preview)](https://learn.microsoft.com/en-us/azure/machine-learning/prompt-flow/get-started-prompt-flow?view=azureml-api-2#setup)

Please complete the following sections;

- [Prerequisites: Enable Prompt flow in your Azure Machine Learning workspace](https://learn.microsoft.com/en-us/azure/machine-learning/prompt-flow/get-started-prompt-flow?view=azureml-api-2#setup)
- [Setup - Connection](https://learn.microsoft.com/en-us/azure/machine-learning/prompt-flow/get-started-prompt-flow?view=azureml-api-2#connection)
- [Setup - Runtime](https://learn.microsoft.com/en-us/azure/machine-learning/prompt-flow/get-started-prompt-flow?view=azureml-api-2#runtime)

<p align="right">(<a href="#readme-top">back to top</a>)</p>

---


<!-- MARKDOWN LINKS & IMAGES -->
<!-- https://www.markdownguide.org/basic-syntax/#reference-style-links -->
[contributors-shield]: https://img.shields.io/github/contributors/rohit-lakhanpal/build-your-own-copilot.svg?style=for-the-badge
[contributors-url]: https://github.com/rohit-lakhanpal/build-your-own-copilot/graphs/contributors
[forks-shield]: https://img.shields.io/github/forks/rohit-lakhanpal/build-your-own-copilot.svg?style=for-the-badge
[forks-url]: https://github.com/rohit-lakhanpal/build-your-own-copilot/network/members
[stars-shield]: https://img.shields.io/github/stars/rohit-lakhanpal/build-your-own-copilot.svg?style=for-the-badge
[stars-url]: https://github.com/rohit-lakhanpal/build-your-own-copilot/stargazers
[issues-shield]: https://img.shields.io/github/issues/rohit-lakhanpal/build-your-own-copilot.svg?style=for-the-badge
[issues-url]: https://github.com/rohit-lakhanpal/build-your-own-copilot/issues
[license-shield]: https://img.shields.io/github/license/rohit-lakhanpal/build-your-own-copilot.svg?style=for-the-badge
[license-url]: https://github.com/rohit-lakhanpal/build-your-own-copilot/blob/master/LICENSE.txt
[linkedin-shield]: https://img.shields.io/badge/-LinkedIn-black.svg?style=for-the-badge&logo=linkedin&colorB=555
[linkedin-url]: https://www.linkedin.com/in/rohitlakhanpal

[openai.com]: https://img.shields.io/badge/OpenAI-5A5AFF?style=for-the-badge&logo=openai&logoColor=white
[openai-url]: https://openai.com/
[azure.com]: https://img.shields.io/badge/Microsoft_Azure-0078D4?style=for-the-badge&logo=microsoft-azure&logoColor=white
[azure-url]: https://azure.microsoft.com
[dotnet.microsoft.com]: https://img.shields.io/badge/.NET-512BD4?style=for-the-badge&logo=dotnet&logoColor=white
[dotnet-url]: https://dotnet.microsoft.com
[python.org]: https://img.shields.io/badge/Python-3776AB?style=for-the-badge&logo=python&logoColor=white
[python-url]: https://www.python.org
[learn-sk]: https://img.shields.io/badge/Semantic%20Kernel-5E5E5E?style=for-the-badge&logo=microsoft
[sk-url]: https://learn.microsoft.com/en-us/semantic-kernel/




