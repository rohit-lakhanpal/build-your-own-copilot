# Manafacturing example

GAI may be a game-changer for Australian manufacturing, by enabling 
a suite of advanced manufacturing capabilities that play to Australia’s 
existing manufacturing strengths of producing high quality and highly 
technical products. Importantly, shifting to advanced manufacturing is 
a strategic priority for both State and Federal Governments. 

References: [Australia’s Generative AI opportunity Research paper | TechCouncil of Australia](https://techcouncil.com.au/wp-content/uploads/2023/07/230714-Australias-Gen-AI-Opportunity-Final-report-vF4.pdf).

| | |
| -: | :- |
| Industry | Manufacturing |
| Usecase  |  Assist upskilling of apprentices and technicians, through conversational on-the-job training of simple concepts  |
| WHO | Technicians and field workers. |
| WHAT |  Technicians can use natural language to get the right procedure to repair a physical asset. |
| WOW | Significantly reduce the time for technicians to be able to repair physical assets. |

## Stage 1
| | |
| -: | :- |
| Overview | Use Azure ML Prompt Flow to search PDF repair manuals for procedure on how to repair a xbox controller. |
| Techniques | Retrieval Augmented Generation (RAG) |
| Model | GPT 35 Turbo (0613)  |
| Weights | TopP:0.95; Temp:0.7  |

Based on the Bring Your Own Data QnA sample in Azure ML Studio.

**Prompt Flow GUI**

![Prompt Flow GUI](./manufacturing.png "Prompt Flow GUI")

**System Prompt**

```text
You are an AI assistant that helps users answer questions given a specific context. You will be given a context and asked a question based on that context. Your answer should be as precise as possible and should only come from the context.
Please add citation after each sentence when possible in a form "(Source: citation)". 
```

**User Prompt**
```text
how to do replace the bumper in a xbox controller
```

**Output**

```text
To replace the bumper in an Xbox controller, you need to first remove the top case and mid-frame as detailed in the Procedure – Removal (Top Case) and Procedure – Removal (Mid-Frame) respectively. Then, gently lift the bumper off the controller and replace it by gently placing it on the controller, making sure that the inner posts slot into the controller, not over the mid-frame. Finally, using a plastic pry tool, hook the left and right edge clips of the bumper to the mid-frame. (Source: Elite Series 2 instructions.pdf, page 26-28)
```

**Prompt Flow Raw File**

```yaml
id: bring_your_own_data_qna
name: Bring Your Own Data QnA
inputs:
  question:
    type: string
    default: what is the process to remove the bumper
    is_chat_input: false
outputs:
  output:
    type: string
    reference: ${answer_the_question_with_context.output}
nodes:
- name: embed_the_question
  type: python
  source:
    type: package
    tool: promptflow.tools.embedding.embedding
  inputs:
    connection: mf-pf-aoai
    deployment_name: ada-002
    input: ${inputs.question}
  use_variants: false
- name: search_question_from_indexed_docs
  type: python
  source:
    type: package
    tool: promptflow_vectordb.tool.vector_index_lookup.VectorIndexLookup.search
  inputs:
    path: azureml://subscriptions/aaaa-bbbb-cccc-dddd/resourcegroups/mf-prompt-flow-rg/providers/Microsoft.MachineLearningServices/workspaces/mf-aml-workspace/data/xbox-vindex/versions/1
    query: ${embed_the_question.output}
    top_k: 2
  use_variants: false
- name: generate_prompt_context
  type: python
  source:
    type: code
    path: generate_prompt_context.py
  inputs:
    search_result: ${search_question_from_indexed_docs.output}
  use_variants: false
- name: Prompt_variants
  use_variants: true
- name: answer_the_question_with_context
  type: llm
  source:
    type: code
    path: answer_the_question_with_context.jinja2
  inputs:
    deployment_name: gpt-35-turbo
    temperature: 0
    top_p: 1
    stop: ""
    max_tokens: 1000
    presence_penalty: 0
    frequency_penalty: 0
    logit_bias: ""
    prompt_text: ${Prompt_variants.output}
  provider: AzureOpenAI
  connection: mf-pf-aoai
  api: chat
  module: promptflow.tools.aoai
  use_variants: false
node_variants:
  Prompt_variants:
    default_variant_id: variant_0
    variants:
      variant_0:
        node:
          name: Prompt_variants
          type: prompt
          source:
            type: code
            path: Prompt_variants.jinja2
          inputs:
            contexts: ${generate_prompt_context.output}
            question: ${inputs.question}
      variant_1:
        node:
          name: Prompt_variants
          type: prompt
          source:
            type: code
            path: Prompt_variants__variant_1.jinja2
          inputs:
            contexts: ${generate_prompt_context.output}
            question: ${inputs.question}
      variant_2:
        node:
          name: Prompt_variants
          type: prompt
          source:
            type: code
            path: Prompt_variants__variant_2.jinja2
          inputs:
            contexts: ${generate_prompt_context.output}
            question: ${inputs.question}
environment:
  python_requirements_txt: requirements.txt
```