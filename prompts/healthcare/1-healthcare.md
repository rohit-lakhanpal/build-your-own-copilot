# Healthcare Example

GAI has significant potential to improve the accessibility and quality of 
healthcare delivery. It can enable more one on one patient care by 
reducing time spent on admin, improving personalisation by being 
embedded in wearable devices, and supporting the transition towards 
more proactive models of healthcare by allowing earlier diagnosis, at 
scale. Importantly, the key to realising these benefits will be robust 
protocols that ensure patient confidentiality and safety are maintained.

References: [Australia’s Generative AI opportunity Research paper | TechCouncil of Australia](https://techcouncil.com.au/wp-content/uploads/2023/07/230714-Australias-Gen-AI-Opportunity-Final-report-vF4.pdf).

| | |
| -: | :- |
| Industry | Healthcare |
| Usecase  | Faster and better quality medical note taking for staff, with audio to text capabilities automatically generating notes from consultations  |
| WHO | Help doctors & nurses |
| WHAT | GAI converts audio to a standardised electronic health record (EHR). The medical professional reviews before submission. |
| WOW | With less time spent on admin, more time can be spent attending to a patient’s complex needs or seeing more patients.|

## Stage 1
| | |
| -: | :- |
| Overview | Shorthand to Clinical Note |
| Techniques | Role Prompting |
| Model | GPT 35 Turbo (0613)  |
| Weights | TopP:0.95; Temp:0.7  |

**System Prompt**

```text
You are a Medical Assistant that ensures faster and better-quality medical note taking for staff.

Users will send you medical shorthand. 

You will respond by converting it to a Clinical Note, expanding on abbreviations. 
```

**User Prompt**
```text
John Brown, 73 M
Sx: CPOE and SOBOE 3/12 typical
PMHx: 
STEMI 2002. PCI LAD HREF.
CKD
CVA
AF on NOAC
MEDS bicor, aspirin, statin, rivaroxaban, entresto
OE
Well, BMI+ ECG NSR 80. BP 120/75
JVPNR. Nil pedal edema. HSDNM. Chest clear.
Imp
Stable. 
Plan
Lifestyle. Diet. GP f/u. PHT clinic because why not
```

**Output**

```text
Patient's name is John Brown, a 73-year-old male. He has been experiencing shortness of breath on exertion (SOBOE) and chest pain on exertion (CPOE) for the past 3 months, which is typical for him.   
  
His past medical history includes a ST-elevation myocardial infarction (STEMI) in 2002, for which he underwent percutaneous coronary intervention (PCI) of the left anterior descending artery (LAD) with a drug-eluting stent. He also has chronic kidney disease (CKD), a history of cerebrovascular accident (CVA), and atrial fibrillation (AF) for which he is on a non-vitamin K antagonist oral anticoagulant (NOAC).   
  
His current medications include Bicor (bisoprolol), aspirin, a statin, rivaroxaban, and Entresto (sacubitril/valsartan).   
  
On examination, his BMI is within normal limits, his electrocardiogram (ECG) shows normal sinus rhythm (NSR) with a rate of 80 beats per minute, and his blood pressure is 120/75 mmHg. His jugular venous pressure (JVP) is not raised, there is no pedal edema, and his heart sounds are normal. His lungs are clear on auscultation.   
  
The impression is that the patient is stable. The plan is to focus on lifestyle modifications, including diet, and for him to follow up with his general practitioner (GP). Additionally, he will be referred to the Pulmonary Hypertension Clinic for further evaluation.
```

## Stage 2

| | |
| -: | :- |
| Overview | Clinical Note to Structured Clinical Note |
| Techniques | Role Prompting, Prompt Chaining |
| Model | GPT 35 Turbo (0613)  |
| Weights | TopP:0.95; Temp:0.7  |


**System Prompt**

```text
You are a Medical Assistant that ensures faster and better-quality medical note taking for staff.

Users will send you a Clinical Note. 

You will respond by converting it to a Structured Clinical Note. 
```

**User Prompt**
```text
Patient's name is John Brown, a 73-year-old male. He has been experiencing shortness of breath on exertion (SOBOE) and chest pain on exertion (CPOE) for the past 3 months, which is typical for him.   
  
His past medical history includes a ST-elevation myocardial infarction (STEMI) in 2002, for which he underwent percutaneous coronary intervention (PCI) of the left anterior descending artery (LAD) with a drug-eluting stent. He also has chronic kidney disease (CKD), a history of cerebrovascular accident (CVA), and atrial fibrillation (AF) for which he is on a non-vitamin K antagonist oral anticoagulant (NOAC).   
  
His current medications include Bicor (bisoprolol), aspirin, a statin, rivaroxaban, and Entresto (sacubitril/valsartan).   
  
On examination, his BMI is within normal limits, his electrocardiogram (ECG) shows normal sinus rhythm (NSR) with a rate of 80 beats per minute, and his blood pressure is 120/75 mmHg. His jugular venous pressure (JVP) is not raised, there is no pedal edema, and his heart sounds are normal. His lungs are clear on auscultation.   
  
The impression is that the patient is stable. The plan is to focus on lifestyle modifications, including diet, and for him to follow up with his general practitioner (GP). Additionally, he will be referred to the Pulmonary Hypertension Clinic for further evaluation.
```

**Output**

```text
Structured Clinical Note:  
   
Patient Information:  
- Name: John Brown  
- Age: 73 years  
- Gender: Male  
   
Chief Complaints:  
- Shortness of breath on exertion (SOBOE) for the past 3 months  
- Chest pain on exertion (CPOE) for the past 3 months  
   
Medical History:  
- ST-elevation myocardial infarction (STEMI) in 2002, underwent percutaneous coronary intervention (PCI) of the left anterior descending artery (LAD) with a drug-eluting stent  
- Chronic kidney disease (CKD)  
- History of cerebrovascular accident (CVA)  
- Atrial fibrillation (AF), on a non-vitamin K antagonist oral anticoagulant (NOAC)  
   
Current Medications:  
- Bicor (bisoprolol)  
- Aspirin  
- Statin  
- Rivaroxaban  
- Entresto (sacubitril/valsartan)  
   
Examination Findings:  
- BMI: within normal limits  
- Electrocardiogram (ECG): normal sinus rhythm (NSR) with a rate of 80 beats per minute  
- Blood pressure: 120/75 mmHg  
- Jugular venous pressure (JVP): not raised  
- Pedal edema: absent  
- Heart sounds: normal  
- Lungs: clear on auscultation  
   
Impression:  
- Patient is stable  
   
Plan:  
- Focus on lifestyle modifications, including diet  
- Follow up with general practitioner (GP)  
- Referral to Pulmonary Hypertension Clinic for further evaluation.
```