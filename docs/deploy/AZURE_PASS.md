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
    Setup Cloud Resources using Azure Pass
  </h3>

  <p align="center">
    powered by
    <br />
    <a href="https://github.com/rohit-lakhanpal/build-your-own-copilot">
      <img src="../../docs/img/logo.png" alt="Logo" height="80">
    </a>
  </p>
</div>

# Overview

To streamline the process of setting up users and resources for a hackathon, this script automates the creation of Azure users, resource groups, and multiple cognitive services.

## Some things to note when using an Azure Pass

- DO NOT redeem promo code with an email account that is attached to an EA, the pass will not work.
- Promo code needs to be redeemed within 90-days of being received.
- Customer Live ID/Org ID will be limited to one concurrent Azure Pass Sponsorship at a time.
- Monetary credit can't be used toward third party services, premier support or Azure MarketPlace and cannot be added to existing subscriptions.
- If you add a payment instrument to the subscription and the subscription is active at the conclusion of the offer it will be converted to Pay-As-You-Go.
- Subscriptions are activated within minutes of the promo code being redeemed.

<p align="right">(<a href="#readme-top">back to top</a>)</p>

---

## Pre-requisites

1. Make sure you have the Azure CLI and `jq` installed in your environment.
2. You need Azure admin privileges to create users and resources.

<p align="right">(<a href="#readme-top">back to top</a>)</p>

---
## Setup Instructions

1. **Identify Your Azure Domain**:
    - Navigate to [Azure Active Directory Overview](https://portal.azure.com/#view/Microsoft_AAD_IAM/ActiveDirectoryMenuBlade/~/Overview).
    - Note down the Primary domain. It would typically look like `your-domain.onmicrosoft.com`.
    - Replace `your-domain.onmicrosoft.com` in the script with the Primary domain you just identified.

2. **Pre-create Language & Vision Services**:
    - Due to Microsoft's policy, you need to manually create Language (Text Analytics) and Vision (Computer Vision) services at least once. This is to ensure that you've accepted the terms of use for these services.
    - After you have created them once, the script can automate the process for subsequent creations.

3. **Run the Automation Script**:
    - Open the Azure shell at [Azure Cloud Shell](https://shell.azure.com/).
    - Copy the script and paste it into the Azure Cloud Shell to run.

<p align="right">(<a href="#readme-top">back to top</a>)</p>

### Script Overview

- The script begins by defining essential variables like `domain`, `password`, and `location`.
- It then generates a random 6-letter prefix that will be prefixed to each cognitive service's name. This ensures uniqueness of service names.
- A loop runs for 5 iterations (indicating 5 users or teams). For each iteration:
    - A user is created with the display name `Hackathon Team X` and a user principal name `team-X@your-domain.onmicrosoft.com`.
    - A resource group is created for the user.
    - The user is assigned a Contributor role to their respective resource group.
    - Checks are performed for the existence of each cognitive service kind. If the kind exists, the service is created. If not, a message is recorded indicating the inability to create that particular service.
- After the loop completes, a summary of all operations (success and failure messages) is displayed.

<p align="right">(<a href="#readme-top">back to top</a>)</p>

### Notes

- Ensure you have sufficient permissions and quotas in your Azure subscription to create multiple instances of the services.
- Always validate the users and resources after the script completes to ensure everything is set up as expected.

<p align="right">(<a href="#readme-top">back to top</a>)</p>

### Script

Replace the values of `domain` and `password` with your own values and run the script as described in the [Setup Instructions](#setup-instructions) section. In addition, you can change the number of iterations in the `for` loop to create more or less teams. The script will create 5 teams by default.

```sh
domain="your-domain.onmicrosoft.com"
password="YOUR-COMPLEX-PASSWORD"
location="eastus"  # Azure location for the resource groups

# Generate a random 6-letter prefix
prefix=$(head /dev/urandom | tr -dc 'a-z' | head -c6)

# Initialize an array to store messages
declare -a messages

# Create 5 teams (please change this number depending on the number of teams you want to create)
for i in {1..5}
do
    # Construct unique password for each user
    ipassword="$password$i"
    
    # Create a user with a display name and principal name
    az ad user create \
        --display-name "Hackathon Team $i" \
        --user-principal-name "team-$i@$domain" \
        --password "$ipassword"
    messages+=("User team-$i@$domain has been created.")

    # Create a resource group corresponding to each user
    az group create \
        --name "resource-group-$i" \
        --location "$location"
    messages+=("Resource group resource-group-$i has been created.")

    # Assign the user the Contributor role for their respective resource group
    az role assignment create \
        --role Contributor \
        --assignee "team-$i@$domain" \
        --resource-group "resource-group-$i"
    messages+=("Assigned Contributor role to team-$i@$domain for resource-group-$i.")

    # Check and create the services based on their existence

    # Speech service
    speech_exists=$(az cognitiveservices account list-kinds | jq 'contains(["SpeechServices"])')
    if [ "$speech_exists" == "true" ]; then
        az cognitiveservices account create \
            --name "${prefix}-speech-svc-$i" \
            --resource-group "resource-group-$i" \
            --kind SpeechServices \
            --sku S0 \
            --location "$location" \
            --yes
        messages+=("${prefix}-speech-svc-$i has been created.")
    else
        messages+=("Unable to create ${prefix}-speech-svc-$i as SpeechServices resource type does not exist.")
    fi

    # Language authoring (Text Analytics)
    language_exists=$(az cognitiveservices account list-kinds | jq 'contains(["TextAnalytics"])')
    if [ "$language_exists" == "true" ]; then
        az cognitiveservices account create \
            --name "${prefix}-language-svc-$i" \
            --resource-group "resource-group-$i" \
            --kind TextAnalytics \
            --sku S \
            --location "$location" \
            --yes
        messages+=("${prefix}-language-svc-$i has been created.")
    else
        messages+=("Unable to create ${prefix}-language-svc-$i as TextAnalytics resource type does not exist.")
    fi

    # Content safety (Content Moderator)
    content_exists=$(az cognitiveservices account list-kinds | jq 'contains(["ContentSafety"])')
    if [ "$content_exists" == "true" ]; then
        az cognitiveservices account create \
            --name "${prefix}-content-safety-svc-$i" \
            --resource-group "resource-group-$i" \
            --kind ContentSafety \
            --sku S0 \
            --location "$location" \
            --yes
        messages+=("${prefix}-content-safety-svc-$i has been created.")
    else
        messages+=("Unable to create ${prefix}-content-safety-svc-$i as ContentSafety resource type does not exist.")
    fi

    # Computer vision
    vision_exists=$(az cognitiveservices account list-kinds | jq 'contains(["ComputerVision"])')
    if [ "$vision_exists" == "true" ]; then
        az cognitiveservices account create \
            --name "${prefix}-vision-svc-$i" \
            --resource-group "resource-group-$i" \
            --kind ComputerVision \
            --sku S1 \
            --location "$location" \
            --yes
        messages+=("${prefix}-vision-svc-$i has been created.")
    else
        messages+=("Unable to create ${prefix}-vision-svc-$i as ComputerVision resource type does not exist.")
    fi

    # OpenAI
    openai_exists=$(az cognitiveservices account list-kinds | jq 'contains(["OpenAI"])')
    if [ "$openai_exists" == "true" ]; then
        az cognitiveservices account create \
            --name "${prefix}-openai-svc-$i" \
            --resource-group "resource-group-$i" \
            --kind OpenAI \
            --sku S0 \
            --location "$location" \
            --yes
        messages+=("${prefix}-openai-svc-$i has been created.")
    else
        messages+=("Unable to create ${prefix}-openai-svc-$i as OpenAI resource type does not exist.")
    fi
    messages+=("--------------")
done

# Display the stored messages
echo -e "\nSummary of operations:"
echo -e "======================\n\n"
for msg in "${messages[@]}"; do
    echo "$msg"
done
```

Once the script completes, you should see a summary of all operations performed. It might look something like this:

```text
Summary of operations:
======================

User team-1@contoso-hack.onmicrosoft.com has been created.
Resource group resource-group-1 has been created.
Assigned Contributor role to team-1@contoso-hack.onmicrosoft.com for resource-group-1.
wichsx-speech-svc-1 has been created.
wichsx-language-svc-1 has been created.
wichsx-content-safety-svc-1 has been created.
wichsx-vision-svc-1 has been created.
Unable to create wichsx-openai-svc-1 as OpenAI resource type does not exist.
--------------
User team-2@contoso-hack.onmicrosoft.com has been created.
Resource group resource-group-2 has been created.
Assigned Contributor role to team-2@contoso-hack.onmicrosoft.com for resource-group-2.
wichsx-speech-svc-2 has been created.
wichsx-language-svc-2 has been created.
wichsx-content-safety-svc-2 has been created.
wichsx-vision-svc-2 has been created.
Unable to create wichsx-openai-svc-2 as OpenAI resource type does not exist.
--------------
```
#### Azure OpenAI resource creation only (optional)
In most cases you'll find that you cannot provision Azure Open AI resources when Azure pass was granted, this section highlights the creation of these resources only. 
```sh
location="canadaeast"  # Azure location for the OpenAI resources where GPT-4 is enabled

# Generate a random 6-letter prefix
prefix=$(head /dev/urandom | tr -dc 'a-z' | head -c6)

# Initialize an array to store messages
declare -a messages
for i in {1..5}
  do
  # OpenAI
      openai_exists=$(az cognitiveservices account list-kinds | jq 'contains(["OpenAI"])')
      if [ "$openai_exists" == "true" ]; then
          az cognitiveservices account create \
              --name "${prefix}-openai-svc-$i" \
              --resource-group "resource-group-$i" \
              --kind OpenAI \
              --sku S0 \
              --location "$location" \
              --yes
          ## deploy the models
          az cognitiveservices account keys list \
        --name "${prefix}-openai-svc-$i" \
        --resource-group "resource-group-$i" \ | jq -r .key1
    # OpenAI deploy the models text-embedding-ada-002, gpt-4-32k, gpt-35-turbo-16k
    az cognitiveservices account deployment create \
        --name "${prefix}-openai-svc-$i" \
        --resource-group "resource-group-$i" \
        --deployment-name text-embedding-ada-002 \
        --model-name text-embedding-ada-002 \
        --model-format OpenAI \
        --model-version "2"

    az cognitiveservices account deployment create \
        --name "${prefix}-openai-svc-$i" \
        --resource-group "resource-group-$i" \
        --deployment-name gpt-4-32k \
        --model-name gpt-4-32k \
        --model-format OpenAI \
        --model-version "0613"

    az cognitiveservices account deployment create \
        --name "${prefix}-openai-svc-$i" \
        --resource-group "resource-group-$i" \
        --deployment-name gpt-35-turbo-16k \
        --model-name gpt-35-turbo-16k \
        --model-format OpenAI \
        --model-version "0613"
          messages+=("${prefix}-openai-svc-$i has been created.")
      else
          messages+=("Unable to create ${prefix}-openai-svc-$i as OpenAI resource type does not exist.")
      fi
      messages+=("--------------")
  done
```
Once the script completes, you should see a summary of all operations performed. It might look something like this:

```text
Summary of operations:
======================

wichsx-openai-svc-1 has been created.
--------------
wichsx-openai-svc-2 has been created.
--------------
```

<p align="right">(<a href="#readme-top">back to top</a>)</p>

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



