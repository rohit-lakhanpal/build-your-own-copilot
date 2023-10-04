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
    Build your own Co-Pilot | Use Cases
  </h3>

  <p align="center">
    powered by
    <br />
    <a href="https://github.com/rohit-lakhanpal/build-your-own-copilot">
    <img src="docs/img/logo.png" alt="Logo" height="80">
  </a>
    <br />    
    <a href="/README.md">Introduction</a>
    ·
    <a href="/GETTING-STARTED.md">Getting Started</a>
    ·
    <a href="/USE-CASES.md"><strong>Use Cases</strong></a>
  </p>
</div>

## Samples

| Id    | Industry       | Overview                                                         | Samples                                      |
|-------|----------------|------------------------------------------------------------------|----------------------------------------------|
| 1     | Healthcare     | "Add wow" | Hack Prompt <br /> POC Notebook <br /> MLP Code |

## Common use-cases for LLMs:
| Use Case                                   | Description                                                                                                                                                             |
|--------------------------------------------|-------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| **Summarisation**                          | Large language models can condense lengthy texts into shorter summaries, retaining key information, which is helpful in digesting large volumes of information quickly. |
| **Writing Aid**                            | They can assist in drafting, editing, or enhancing written content by suggesting better word choices, correcting grammar, or even helping with style and tone.          |
| **Image Generation**                       | Though primarily designed for text, some models can be fine-tuned to generate images from textual descriptions, bridging the gap between language and visual representation. |
| **Code Generation or Transformation**      | By understanding programming languages, these models can aid in writing code, debugging, or even translating code between different programming languages.              |
| **Chat and Conversation**                  | Large models can simulate human-like conversations in chatbots, providing natural responses and maintaining contextual understanding over a dialogue.                     |
| **Question-answering**                    | They excel at providing precise answers to user queries by searching through large datasets or pre-trained knowledge, making them useful in customer service or education.   |
| **Reason over Structured or Unstructured Data** | They can analyse and draw insights from data, whether structured (like databases) or unstructured (like text documents), aiding in decision-making processes.              |
| **Search**                                 | By understanding the semantic meaning behind queries, large language models can enhance search engine capabilities, delivering more relevant results.                     |


### Extrapolate these into your own scenarios:

| Category  | Example                                                                                                                                                  |
|-----------|----------------------------------------------------------------------------------------------------------------------------------------------------------|
| **Analyse**    | Support customers to understand and reconcile costs and invoices across dimensions.                                                                 |
| **Insights**   | Actionable insights into cloud usage and costs using their own and similar customer's data.                                                        |
| **Optimise**   | Provide tools and recommendations for customers to optimise on their cloud costs.                                                                  |
| **Simulate**   | Enable customers to simulate scenarios which would change their future cloud costs.                                                                |


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


## Prompt Engineering Guidelines
1. Give clearer instructions
1. Split complex tasks into simpler sub-tasks
1. Structure the instructions to keep the model on task
1. Prompt the model to explain before answering
1. Aks for justifications of many possible answer, and then synthesize
1. Generate may outputs and then use the mdoel to pick the best one.

## Prompt Engineering Techniques
1. Role prompting or Personas
1. Prompt Chaining
1. Prompt Chunking
1. Zero, One, or Few Shot Reasoning
1. Chain-of-Thought Reasoning
1. Selection-inference

## Other Conecepts
1. Fine Tuning
1. Hallucination
1. Retrieval Augmented Generation

## Tools
1. OpenAI Portal
1. Machine Learning Portal
1. Speech Studio
1. Vision Studio
1. Language Studio
1. ?Document Intelligence
1. ?Content Moderation Studio
1. Azure AI Content Safety

## Talk Track
1. Baseline [5 minutes] 
    1. Our journey of democratised ai
    1. GenAI (Completion & Embeddings) & common use-cases
    1. Empower your people
        1. Hackers, Hipsters and Hustlers
    1. Use a methodology to bring your MLP to life (Hack, POC, Prod and Beyond!)
        1. Explore & understand capibilities via Hack
        1. Define, Evaluate and Build your MLP
        1. GTM with Microsoft
        1. Iterate & Expand
1. Concept of your own co-pilot & co-pilots beyond chat [5 minutes] - Joint
    1. In this session, let's explore the tools you can use to build your co-pilot
1. Exploring the tools while `Hacking` [5 minutes] 
    1. Speech Studio
    1. Vision Studio
    1. Language Studio
    1. OpenAI Portal
    1. Notebooks
1. Explore Use-Cases for Hacking [10 minutes] 
    1. Example 1
    1. Example 2
1. Exploring the tools for `POC` [5 minutes]
    1. Machine Learning Portal (Prompt Flow)
    1. Ready to go Code 
1. Exploring the tools for `Production` [10 minutes]
    1. Semantic Kernel
1. Sample use-cases using Semantic Kernel [10 minutes]
1. Close [5 mins]


Analyse | Insights | Optimise | Simulate

In app | Conversational Interface 



