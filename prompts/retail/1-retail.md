# Retail Example

As a result of having to invest in omnichannel capabilities during the 
pandemic, the retail industry should be better placed to integrate 
GAI into existing digital platforms. Such integration could be 
particularly important as companies look for new ways to 
differentiate and personalise products, while remaining cost-competitiv

References: [Australia’s Generative AI opportunity Research paper | TechCouncil of Australia](https://techcouncil.com.au/wp-content/uploads/2023/07/230714-Australias-Gen-AI-Opportunity-Final-report-vF4.pdf).


| | |
| -: | :- |
| Industry | Retail |
| Usecase  | Key feature summarisation and product descriptions to present most relevant content in concise form |
| WHO | Retail product mangers |
| WHAT | AOAI takes a very long product description and summarizes it into one sentence. |
| WOW | Significantly reduces the time for product owners and marketers to produce content. |

## Stage 1
| | |
| -: | :- |
| Overview | Summarise product description |
| Techniques | Prompt Engineering - zero shot example |
| Model | GPT 35 Turbo (0613)  |
| Weights | TopP:0.95; Temp:0.7  |

**System Prompt**

```text
You are an AI assistant that summarises product descriptions. The summary should be limited to one sentence. 
```

**User Prompt**
```text
For one person    
Use on up to 5 devices simultaneously    
Works on PC, Mac, iPhone, iPad, and Android phones and tablets    
1 TB (1,000 GB) of secure cloud storage    
Word, Excel, PowerPoint, and OneNote with offline access and premium features    
Microsoft Defender advanced security for your personal data and devices    
OneDrive ransomware protection for your files and photos    
Outlook ad-free secure email    
Microsoft Editor advanced spelling, grammar, and writing assistance    
Clipchamp video editor with premium filters and effects    
Premium, customizable templates    
Expanded content library of high-quality photos, icons, fonts, videos, and audio    
Contact support via chat or email to get help when you need it
```

**Output**

```text
Get the ultimate productivity package with Microsoft 365 Personal, including 1TB cloud storage, advanced security features, and premium software for all your devices!
```

## Stage 2

| | |
| -: | :- |
| Overview | Summarise product description in the voice of the customer |
| Techniques | Prompt Engineering - few shot example |
| Model | GPT 35 Turbo (0613)  |
| Weights | TopP:0.95; Temp:0.7  |


**System Prompt**

```text
You are an AI assistant that summarises product descriptions. The summary should be limited to one sentence. 
You should provide a greeting, the product summary and a cool sign off phrase.
You have the personality of a hip and cool young person
```

**User Prompt**
```text
For one person  
Use on up to 5 devices simultaneously  
Works on PC, Mac, iPhone, iPad, and Android phones and tablets  
1 TB (1,000 GB) of secure cloud storage  
Word, Excel, PowerPoint, and OneNote with offline access and premium features  
Microsoft Defender advanced security for your personal data and devices  
OneDrive ransomware protection for your files and photos  
Outlook ad-free secure email  
Microsoft Editor advanced spelling, grammar, and writing assistance  
Clipchamp video editor with premium filters and effects  
Premium, customizable templates  
Expanded content library of high-quality photos, icons, fonts, videos, and audio  
Contact support via chat or email to get help when you need it
```

**Output**

```text
Hey there! Get Microsoft 365 Personal with 1TB cloud storage, access to Word, Excel, PowerPoint, OneNote, Outlook, Microsoft Defender, OneDrive ransomware protection, Microsoft Editor, Clipchamp video editor, customizable templates, and an expanded content library of photos, icons, fonts, videos, and audio with support via chat or email. Stay safe, secure and productive!
```