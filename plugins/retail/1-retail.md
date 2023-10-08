# Retail Example

As a result of having to invest in omnichannel capabilities during the 
pandemic, the retail industry should be better placed to integrate 
GAI into existing digital platforms. Such integration could be 
particularly important as companies look for new ways to 
differentiate and personalise products, while remaining cost-competitive

![Sector deep-dive: Retail and Customer Services](https://raw.githubusercontent.com/rohit-lakhanpal/build-your-own-copilot/main/docs/img/retail-and-customer-services.png)

References: [Australia’s Generative AI opportunity Research paper | TechCouncil of Australia](https://techcouncil.com.au/wp-content/uploads/2023/07/230714-Australias-Gen-AI-Opportunity-Final-report-vF4.pdf).


| | |
| -: | :- |
| Industry | Retail |
| Use case  | Provide personalised customer support and automate customer onboarding with sophisticated conversational assistants. |
| WHO | Customers |
| WHAT | Create a plugin that can be used in ChatGPT, Bing Chat and Windows Copilot |
| WOW | Gives developers and product managers a chat environment to develop and test AI based plugins. |

This example is based on 2 existing samples:

- Chat Copilot Sample Application - https://github.com/microsoft/chat-copilot
- ChatGPT Plugin Quickstart using Python and FastAPI - https://github.com/Azure-Samples/openai-plugin-fastapi


## Stage 1
| | |
| -: | :- |
| Overview | Chat Copilot Sample Application |
| Techniques | No custom plugin installed |

![Chat Copilot](../docs/img/retail1.png "Chat Copilot")

**User Prompt**

```text
I am going for a hike. Can you recommend some products?
```

**Output**

```text
Certainly, I can help with that! What kind of products are you looking for specifically? Hiking gear, food, or something else?
```

**User Prompt**

```text
Hiking boots
```

**Output**

```text
Great! There are many hiking boot options available on the market, but here are a few popular and highly rated choices:

Salomon Quest 4D 3 GTX - These boots are designed with comfort and durability in mind. They feature Gore-Tex waterproofing and a supportive design to keep your feet comfortable on long hikes.

Merrell Moab 2 Waterproof - The Merrell Moab 2 is another popular choice for hiking boots. They're lightweight, waterproof, and provide excellent traction on a variety of terrains.

Keen Targhee III Waterproof - These boots offer a comfortable fit right out of the box and are designed to provide excellent support on rough terrain while keeping your feet dry.

Vasque Breeze III GTX - These boots are well-suited for backpacking trips or long hikes thanks to their comfortable fit, ankle support, and durable construction.

I hope this helps! Let me know if you have any other questions.
```

## Stage 2
| | |
| -: | :- |
| Overview | Chat Copilot Sample Application |
| Techniques | Custom plugin installed |

![Chat Copilot](../docs/img/retail2.png "Chat Copilot")

**User Prompt**

```text
I am going for a hike. Can you recommend some products?
```

**Output**

```text
Based on your query, here are some products that might be useful for your hike:

Contoso Hiking Boots: Durable hiking boots for all-day comfort on the trails
Contoso Hiking Backpack: Lightweight backpack with ample storage for day hikes
Contoso Hiking Poles: Collapsible hiking poles for added stability on the trails
Contoso Hiking Jacket: Waterproof and breathable jacket for rainy hikes
Contoso Hiking Pants: Quick-drying and stretchy pants for hiking and backpacking
Contoso Hiking Water Bottle: Stainless steel water bottle with a carabiner clip for easy carrying
Please note that these are just recommendations and you should choose products based on your specific needs and preferences.
```

**Custom plugin AI manifest**

```json
{
    "schema_version": "v1",
    "name_for_model": "contosoproducts",
    "name_for_human": "Product Search",
    "description_for_model": "Plugin for searching through Contoso's outdoor and recreational products. Use it whenever a user asks about products or activities related to camping, hiking, climbing or camping.",
    "description_for_human": "Search through Contosos wide range of outdoor and recreational products",
    "auth": {
        "type": "none"
    },
    "api": {
        "type": "openapi",
        "url": "http://localhost:8000/.well-known/openapi.yaml"
    },
    "logo_url": "http://localhost:8000/.well-known/logo.png",
    "contact_email": "support@example.com",
    "legal_info_url": "https://www.example.com/legal/"
}
```