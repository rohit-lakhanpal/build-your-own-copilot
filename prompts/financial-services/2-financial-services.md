# Professional Financial Services Example

The professional services industry is a highly educated workforce with relatively low levels of productivity. This makes the industry a prime candidate for GAI transformation. As GAI automates routine and well defined tasks, the highly skilled workforce can spend more time on higher value thinking. With GAI, Australia could build on its reputation for high calibre knowledge workers, particularly in financial and legal 
sectors.

![Sector deep-dive: Professional and Financial Services](/docs/img/professional-and-financial-services.png "Sector deep-dive: Professional and Financial Services ")


References: 
- [Australia’s Generative AI opportunity Research paper | TechCouncil of Australia](https://techcouncil.com.au/wp-content/uploads/2023/07/230714-Australias-Gen-AI-Opportunity-Final-report-vF4.pdf).
- [Combat financial crime with AI and advanced technology from Microsoft](https://www.microsoft.com/en-us/industry/blog/financial-services/2023/06/05/combat-financial-crime-with-ai-and-advanced-technology-from-microsoft/)

| | |
| -: | :- |
| Industry | Financial Services |
| Usecase  | Combat financial crime with AI by detecting fraud |
| WHAT | GAI to analyze financial transactions & predict possibility & reason for fraud. |
| WOW | Safeguards your finances while ensuring privacy and security. <br /> <br /> <small>Worldwide, the estimated total of laundered money in a year is at least two percent of global gross domestic product, or USD 800 billion. [1](https://www.businessdit.com/money-laundering-statistics/)  <br /> For financial services organizations, the cost of financial crime compliance reached USD213.9 billion in 2021—USD56.7 billion in Canada and the United States alone in 2022, a 13.6 percent increase from 2021. [2](https://www.finextra.com/pressarticle/88026/global-spend-on-financial-crime-compliance-at-financial-institutions-reaches-2139-billion), [3](https://risk.lexisnexis.com/insights-resources/research/true-cost-of-financial-crime-compliance-study-for-the-united-states-and-canada) </small>|


## Scenario 1 (using gpt-4)

| | |
| -: | :- |
| Overview | Fraud detection. |
| Techniques | Role prompting |
| Model | GPT 4 Turbo (0613)  |
| Weights | TopP:0.5; Temp:0.3  |

**System Prompt**

```text
You are a Financial AI assistant analysing potential fraudulent bank transactions. Users will provide a list of transactions and recent bank statements for validation. Determine if the transactions are likely to be fraudulent.
 
You must provide the following information for each transaction:
1. Date of the transaction (key: "date", format: "YYYY-MM-DD", example: "2023-10-04")   
2. Transaction description (key: "description"; format: String; example: "Grocery Shopping at Walmart")   
3. Transaction amount in AUD (key: "amount"; format: Numeric, example: -23.45)   
4. The % possibility of Fraud expressed as a percentage (key: "fraud_chance"; format: Decimal, example: 0.05)
5. Reason why the transaction was marked fraudulent (key: "fraud_reason", format: String, example: "Unusual spending pattern detected")

Please answer in JSON machine-readable format, using the keys from above. Format the output as a JSON object called "results". Pretty print the JSON and make sure that it is properly closed at the end.
```

**User Prompt**
```text
### Transactions
02/07/2023 Rent -$1,200
03/07/2023 Rent -$1,200
04/07/2023 R3nt -$1,200
03/07/2023 Groceries -$490

### Recent bank statement
Date Description Amount
01/05/2023 Salary +$5,000
02/05/2023 Rent -$1,200
03/05/2023 Groceries -$150
04/05/2023 Gas -$50
05/05/2023 Netflix -$15
06/05/2023 Coffee -$5
07/05/2023 Transfer to savings -$500
08/05/2023 ATM withdrawal -$100
09/05/2023 Restaurant -$80
10/05/2023 Phone bill -$40
11/05/2023 Gym membership -$30
12/05/2023 Online shopping -$120
13/05/2023 Interest earned +$5
02/06/2023 Rent -$1,200
05/05/2023 Groceries -$180
```

**Output**

```json
{
  "results": [
    {
      "date": "2023-07-02",
      "description": "Rent",
      "amount": -1200,
      "fraud_chance": 0.00,
      "fraud_reason": "Regular monthly transaction"
    },
    {
      "date": "2023-07-03",
      "description": "Rent",
      "amount": -1200,
      "fraud_chance": 0.95,
      "fraud_reason": "Duplicate rent payment in a short time span"
    },
    {
      "date": "2023-07-04",
      "description": "R3nt",
      "amount": -1200,
      "fraud_chance": 0.99,
      "fraud_reason": "Unusual spelling of a regular transaction"
    },
    {
      "date": "2023-07-03",
      "description": "Groceries",
      "amount": -490,
      "fraud_chance": 0.75,
      "fraud_reason": "Significantly higher than usual grocery spending"
    }
  ]
}
```



## Scenario 2 (using gpt-3-5-turbo)
| | |
| -: | :- |
| Overview | Fraud detection. |
| Techniques | Role prompting |
| Model | GPT 3.5 Turbo-16k-0613  |
| Weights | TopP:0.5; Temp:0.3  |

**System Prompt**

```text
Same as Scenario 1
```

**User Prompt**
```text
### Transactions
Same as Scenario 1
```

**Output**

As you can see below, the results are not as good as Stage 1.

```json
{
  "results": [
    {
      "date": "2023-07-02",
      "description": "Rent",
      "amount": -1200,
      "fraud_chance": 0.05,
      "fraud_reason": "Unusual spending pattern detected"
    },
    {
      "date": "2023-07-03",
      "description": "Rent",
      "amount": -1200,
      "fraud_chance": 0.05,
      "fraud_reason": "Unusual spending pattern detected"
    },
    {
      "date": "2023-07-04",
      "description": "R3nt",
      "amount": -1200,
      "fraud_chance": 0.95,
      "fraud_reason": "Unusual transaction description"
    },
    {
      "date": "2023-07-03",
      "description": "Groceries",
      "amount": -490,
      "fraud_chance": 0.05,
      "fraud_reason": "Unusual spending pattern detected"
    }
  ]
}
```

## Scenario 3 (using gpt-3-5-turbo & Chain-Of-Thought)

| | |
| -: | :- |
| Overview | Fraud detection. |
| Techniques | Role prompting & Chain-of-Thought Reasoning |
| Model | GPT 3.5 Turbo-16k-0613  |
| Weights | TopP:0.5; Temp:0.3  |

To fix the issue with Scenario 2, here we will use Chain-Of-Thought Reasoning.

**System Prompt**

```text
You are a Financial AI assistant analysing potential fraudulent bank transactions. Users will provide a list of transactions and recent bank statements for validation. Determine if the transactions are likely to be fraudulent.

Think like a financial crimes investigator and think step-by-step.
```

**User**
```text
### Transactions
02/07/2023 Rent -$1,200
03/07/2023 Rent -$1,200
04/07/2023 R3nt -$1,200
03/07/2023 Groceries -$490

### Recent bank statement
Date Description Amount
01/05/2023 Salary +$5,000
02/05/2023 Rent -$1,200
03/05/2023 Groceries -$150
04/05/2023 Gas -$50
05/05/2023 Netflix -$15
06/05/2023 Coffee -$5
07/05/2023 Transfer to savings -$500
08/05/2023 ATM withdrawal -$100
09/05/2023 Restaurant -$80
10/05/2023 Phone bill -$40
11/05/2023 Gym membership -$30
12/05/2023 Online shopping -$120
13/05/2023 Interest earned +$5
02/06/2023 Rent -$1,200
05/05/2023 Groceries -$180
```

**Assistant**
```text
To determine if the transactions are likely to be fraudulent, let's analyze the provided information step-by-step:

1. Compare the transactions with the recent bank statement:
   - The transaction on 02/07/2023 for Rent (-$1,200) matches the bank statement entry on 02/06/2023 for Rent (-$1,200). This transaction is valid.
   - The transaction on 03/07/2023 for Rent (-$1,200) does not have a matching entry in the bank statement. This transaction is potentially fraudulent.
   - The transaction on 04/07/2023 for R3nt (-$1,200) does not have a matching entry in the bank statement. Additionally, the description is misspelled, which raises suspicion. This transaction is likely fraudulent.
   - The transaction on 03/07/2023 for Groceries (-$490) does not have a matching entry in the bank statement. This transaction is potentially fraudulent.

2. Analyze the patterns and inconsistencies:
   - The consistent pattern of Rent payments on the 2nd of each month suggests that the transaction on 03/07/2023 for Rent (-$1,200) is likely fraudulent.
   - The misspelled description in the transaction on 04/07/2023 for R3nt (-$1,200) raises suspicion and indicates a potential fraudulent transaction.
   - The transaction on 03/07/2023 for Groceries (-$490) does not have a matching entry in the bank statement, which is suspicious.

Based on the analysis, the transactions on 03/07/2023 for Rent (-$1,200), 04/07/2023 for R3nt (-$1,200), and 03/07/2023 for Groceries (-$490) are likely to be fraudulent. It is recommended to further investigate these transactions and report them to the bank for verification.
```

**Optional**
```
Extract macro trends across all transactions and look for inconsistencies.
```

**User**
```text
Provide this following information for each transaction:
1. Date of the transaction (key: "date", format: "YYYY-MM-DD", example: "2023-10-04")   
2. Transaction description (key: "description"; format: String; example: "Grocery Shopping at Walmart")   
3. Transaction amount in AUD (key: "amount"; format: Numeric, example: -23.45)   
4. The % possibility of Fraud expressed as a percentage (key: "fraud_chance"; format: Decimal, example: 0.05)
5. Reason why the transaction was marked fraudulent (key: "fraud_reason", format: String, example: "Unusual spending pattern detected")

Please answer in JSON machine-readable format, using the keys from above. Format the output as a JSON object called "results". Pretty print the JSON and make sure that it is properly closed at the end.
```

**Assistant**
```text
{
  "results": [
    {
      "date": "2023-07-03",
      "description": "Rent",
      "amount": -1200,
      "fraud_chance": 0.9,
      "fraud_reason": "Unusual spending pattern detected"
    },
    {
      "date": "2023-07-04",
      "description": "R3nt",
      "amount": -1200,
      "fraud_chance": 0.95,
      "fraud_reason": "Unusual transaction description"
    },
    {
      "date": "2023-07-03",
      "description": "Groceries",
      "amount": -490,
      "fraud_chance": 0.8,
      "fraud_reason": "Inconsistent amount with previous groceries transaction"
    }
  ]
}
```

