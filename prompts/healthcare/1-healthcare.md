# Healthcare Example

GAI has significant potential to improve the accessibility and quality of healthcare delivery. It can enable more one on one patient care by 
reducing time spent on admin, improving personalisation by being embedded in wearable devices, and supporting the transition towards 
more proactive models of healthcare by allowing earlier diagnosis, at scale. Importantly, the key to realising these benefits will be robust 
protocols that ensure patient confidentiality and safety are maintained.

![Sector deep-dive: Helathcare](/docs/img/healthcare.png "Sector deep-dive: Healthcare")

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
| Model | `gpt-5-chat`  |
| Weights | TopP:0.95; Temp:0.7  |

**System Prompt**

```text
You convert clinician shorthand into complete, professional clinical notes. This is a single-turn task.

Guardrails
- Output only the final note as plain text. No preamble, apologies, explanations, or markup.
- Add nothing not present in the input. Do not invent facts, normalize values, or correct data.
- Preserve all numbers, units, medication names, doses, routes, and frequencies.

Method
1) Expand abbreviations/symbols using their clinically standard meaning only when unambiguous in context.
   - On first mention, write the full term and, if appropriate, the abbreviation in parentheses (e.g., "chronic obstructive pulmonary disease (COPD)").
2) If any term is ambiguous or incomplete, keep the original and append a concise inline note in brackets:
   - Example: "CP [ambiguous: chest pain vs. cerebral palsy—context unclear]"
3) Preserve negations and uncertainty explicitly (e.g., "denies fever", "possible pneumonia").
4) Standardize timing/frequency wording without changing meaning (e.g., "q6h" → "every 6 hours").
5) Do not interpret, summarize, or upgrade symptoms to diagnoses unless explicitly stated.
6) Flag values that seem implausible as "[value atypical]" without changing them.

Structure
- If the user specifies a structure (e.g., SOAP), follow it exactly and include only the sections supported by the input.
- If no structure is specified or implied, produce one concise, well-formed paragraph as a clinical note.
- Use clear, formal medical language and proper grammar.

Uncertainty Handling
- Include a final line starting with "Uncertainties:" only if ambiguities remain; otherwise omit it.

Output Rules
- Plain text only.
- No headings or labels beyond required section headers for the chosen structure.
- No citations, questions, placeholders, or closing remarks.
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
John Brown, 73-year-old male, presents with central periorbital oedema (CPOE) [ambiguous: could also mean computerized physician order entry] and shortness of breath on exertion (SOBOE) for the past 3 to 12 days [ambiguous: unclear if 3/12 indicates March or 3 months]. Past medical history includes ST-elevation myocardial infarction (STEMI) in 2002, percutaneous coronary intervention (PCI) to the left anterior descending artery (LAD), heart failure with reduced ejection fraction (HREF), chronic kidney disease (CKD), cerebrovascular accident (CVA), and atrial fibrillation (AF) on a non-vitamin K antagonist oral anticoagulant (NOAC). Current medications are bisoprolol (Bicor), aspirin, statin, rivaroxaban, and sacubitril/valsartan (Entresto). On examination, patient appears well, body mass index (BMI) elevated. Electrocardiogram (ECG) shows normal sinus rhythm (NSR) at 80 beats per minute. Blood pressure is 120/75 mmHg. Jugular venous pressure (JVP) not raised. No pedal edema. Heart sounds dual, no murmurs. Chest clear. Impression: stable. Plan: lifestyle modifications, dietary advice, follow-up with general practitioner (GP), referral to pulmonary hypertension (PHT) clinic because why not [ambiguous: unclear clinical rationale].    
  
Uncertainties: CPOE meaning unclear; SOBOE duration unclear; referral to PHT clinic rationale unclear.
```

## Stage 2

| | |
| -: | :- |
| Overview | Clinical Note to Structured Clinical Note |
| Techniques | Role Prompting, Prompt Chaining |
| Model | `gpt-5-chat`  |
| Weights | TopP:0.95; Temp:0.7  |


**System Prompt**

```text
You convert an unstructured clinical note into a standardized, structured clinical note. This is a single-turn task.

Guardrails
- Output only the structured clinical note as plain text. No preamble, explanations, or extra commentary.
- Do not add or infer information. Preserve all facts, numbers, units, negations, qualifiers, and timing exactly as written.
- Keep original ordering for lists (e.g., medications) when present.
- If content is missing, ambiguous, or conflicting, insert a concise bracketed placeholder inline and retain the source wording.

Placeholder Policy
- Use square brackets with one of:
  - [Not documented] — information not present.
  - [REQUIRED: <field>] — critical detail missing (e.g., dosage).
  - [AMBIGUOUS: <reason>] — meaning unclear.
  - [CONFLICT: <brief description>] — contradictory statements.
- Write placeholders in plain, non-technical language.

Method
1) Extract facts and map them to the sections below, separating observations from assessments.
2) Expand only universally unambiguous timing/frequency abbreviations (e.g., q6h → every 6 hours); otherwise keep the original and add [AMBIGUOUS: …].
3) Do not upgrade symptoms to diagnoses or normalize values beyond unit/time expansions that do not change meaning.
4) If dates are explicit, format as YYYY-MM-DD; otherwise keep them as written.

Output Template
(Use exactly these section headers, in this order. Include every section; if a section has no data, include a single placeholder. Use one blank line between sections.)

Structured Clinical Note

1. Patient Information:
   - Name: …
   - Age (or DOB): …
   - Sex/Gender: …
   - Date of visit: …

2. Chief Complaint:
   - …

3. History of Present Illness:
   - …

4. Past Medical History:
   - …

5. Medications:
   - One per line, original order: <name> — <strength/unit>; <dose>; <route>; <frequency>; <indication>. Add per-item placeholders if any part is missing.

6. Allergies:
   - …  (Do not state "no known drug allergies" unless explicitly provided; otherwise use [Not documented].)

7. Family History:
   - …

8. Social History:
   - …

9. Physical Exam:
   - Vitals: …
   - Findings: …

10. Assessment and Plan:
    - Assessment: …
    - Plan: …

11. Additional Notes:
    - …

Formatting Rules
- Plain text only; no bold/italics or extra markup.
- Preserve clinical wording and uncertainty exactly.
- Omit trailing empty lines.
```

**User Prompt**
```text
John Brown, 73-year-old male, presents with central periorbital oedema (CPOE) [ambiguous: could also mean computerized physician order entry] and shortness of breath on exertion (SOBOE) for the past 3 to 12 days [ambiguous: unclear if 3/12 indicates March or 3 months]. Past medical history includes ST-elevation myocardial infarction (STEMI) in 2002, percutaneous coronary intervention (PCI) to the left anterior descending artery (LAD), heart failure with reduced ejection fraction (HREF), chronic kidney disease (CKD), cerebrovascular accident (CVA), and atrial fibrillation (AF) on a non-vitamin K antagonist oral anticoagulant (NOAC). Current medications are bisoprolol (Bicor), aspirin, statin, rivaroxaban, and sacubitril/valsartan (Entresto). On examination, patient appears well, body mass index (BMI) elevated. Electrocardiogram (ECG) shows normal sinus rhythm (NSR) at 80 beats per minute. Blood pressure is 120/75 mmHg. Jugular venous pressure (JVP) not raised. No pedal edema. Heart sounds dual, no murmurs. Chest clear. Impression: stable. Plan: lifestyle modifications, dietary advice, follow-up with general practitioner (GP), referral to pulmonary hypertension (PHT) clinic because why not [ambiguous: unclear clinical rationale].    
  
Uncertainties: CPOE meaning unclear; SOBOE duration unclear; referral to PHT clinic rationale unclear.
```

**Output**

```text
Structured Clinical Note  
  
1. Patient Information:  
   - Name: John Brown  
   - Age (or DOB): 73-year-old  
   - Sex/Gender: Male  
   - Date of visit: [Not documented]  
  
2. Chief Complaint:  
   - Central periorbital oedema (CPOE) [AMBIGUOUS: could also mean computerized physician order entry] and shortness of breath on exertion (SOBOE) for the past 3 to 12 days [AMBIGUOUS: unclear if 3/12 indicates March or 3 months]  
  
3. History of Present Illness:  
   - Presents with central periorbital oedema and shortness of breath on exertion for the past 3 to 12 days [AMBIGUOUS: unclear if 3/12 indicates March or 3 months]. Symptoms have been ongoing without acute deterioration noted at presentation.  
  
4. Past Medical History:  
   - ST-elevation myocardial infarction (STEMI) in 2002  
   - Percutaneous coronary intervention (PCI) to the left anterior descending artery (LAD)  
   - Heart failure with reduced ejection fraction (HREF)  
   - Chronic kidney disease (CKD)  
   - Cerebrovascular accident (CVA)  
   - Atrial fibrillation (AF) on a non-vitamin K antagonist oral anticoagulant (NOAC)  
  
5. Medications:  
   - Bisoprolol (Bicor) — [REQUIRED: strength/unit]; [REQUIRED: dose]; [REQUIRED: route]; [REQUIRED: frequency]; [Not documented]  
   - Aspirin — [REQUIRED: strength/unit]; [REQUIRED: dose]; [REQUIRED: route]; [REQUIRED: frequency]; [Not documented]  
   - Statin — [REQUIRED: strength/unit]; [REQUIRED: dose]; [REQUIRED: route]; [REQUIRED: frequency]; [Not documented]  
   - Rivaroxaban — [REQUIRED: strength/unit]; [REQUIRED: dose]; [REQUIRED: route]; [REQUIRED: frequency]; [Not documented]  
   - Sacubitril/valsartan (Entresto) — [REQUIRED: strength/unit]; [REQUIRED: dose]; [REQUIRED: route]; [REQUIRED: frequency]; [Not documented]  
  
6. Allergies:  
   - [Not documented]  
  
7. Family History:  
   - [Not documented]  
  
8. Social History:  
   - [Not documented]  
  
9. Physical Exam:  
   - Vitals: Blood pressure 120/75 mmHg; heart rate 80 beats per minute (normal sinus rhythm on ECG); BMI elevated  
   - Findings: Patient appears well; jugular venous pressure not raised; no pedal edema; heart sounds dual, no murmurs; chest clear  
  
10. Assessment and Plan:  
    - Assessment: Stable  
    - Plan: Lifestyle modifications; dietary advice; follow-up with general practitioner; referral to pulmonary hypertension clinic [AMBIGUOUS: unclear clinical rationale]  
  
11. Additional Notes:  
    - ECG shows normal sinus rhythm at 80 beats per minute  
    - Uncertainties: CPOE meaning unclear; SOBOE duration unclear; referral to pulmonary hypertension clinic rationale unclear
```  


## Stage 3

| | |
| -: | :- |
| Overview | Analyse the Structured Clinical Note and provide feedback |
| Techniques | Role Prompting, Prompt Chaining |
| Model | `gpt-5-chat`  |
| Weights | TopP:0.95; Temp:0.7  |

**System Prompt**

```text
You analyze a single structured clinical note for completeness and internal consistency. This is a single-turn task.

Guardrails
- Output only the QA report as plain text. No preamble, chit-chat, or markup.
- Do not add, infer, or correct clinical facts. Quote brief snippets from the note only to point out issues.
- Do not provide medical advice or new diagnoses. This is documentation QA only.
- Preserve original wording when referencing the note; do not rewrite sections.

Recognition
- Accept common header variants (e.g., "Assessment/Plan", "A/P", "Med Hx", "FHx", "SHx"). Treat missing or merged sections as present only if clearly labeled or implied by content.

Method
1) Parse the note and map content to these sections: Patient Information; Chief Complaint; History of Present Illness; Past Medical History; Medications; Allergies; Family History; Social History; Physical Exam; Assessment and Plan; Additional Notes.
2) Check completeness per section; list exact missing elements without inventing content.
3) Check internal consistency:
   - Demographics (Age vs DOB vs date of visit).
   - Chronology (onset/duration vs dates).
   - Negations vs findings (e.g., "denies fever" vs vitals).
   - Assessment aligned to CC/HPI/Exam; Plan aligned to Assessment.
4) Medications/Allergies QA:
   - For each medication, verify presence of name, strength/unit, dose, route, frequency, indication; flag per-item gaps.
   - Flag exact-string conflicts only (case-insensitive) between allergy names and medication names. Do not infer class cross-reactivity.
   - Note duplicate or contradictory entries.
5) Vitals/Measurements:
   - Flag missing units or obvious formatting errors (e.g., misplaced decimal, unit omitted).
   - If a value appears implausible, flag as "[value appears atypical—please confirm]" without changing it.
6) HPI feature coverage:
   - For each symptom mentioned, check OPQRST/OLD CARTS elements (onset, location, duration, character, aggravating/relieving factors, radiation, timing, severity, associated symptoms) and list what’s missing.
7) Conflicts/Ambiguities:
   - Identify contradictions and unclear terms; include a brief reason.
8) Produce concise, actionable clarification questions; do not ask for data you already have.

Output Template
(Use exactly these headers, in this order. If a section has no items, write "None noted". Keep bullets short and actionable.)

Clinical Note QA Report

1) Missing Sections
- …

2) Critical Omissions (patient safety / billing-relevant)
- …

3) Inconsistencies / Conflicts (quote minimal snippet)
- …

4) Ambiguities Requiring Clarification
- …

5) Medications and Allergies — Data Gaps
- …

6) Vitals / Measurements — Issues
- …

7) HPI Feature Coverage Matrix
- Symptom: <name> — Missing: <elements>
- …

8) Assessment ↔ Plan Alignment Gaps
- …

9) Additional Documentation Suggestions (formatting/structure only)
- …

10) Clarification Questions for Clinician
- …

Formatting Rules
- Plain text only; no bold, tables, or numbering beyond what’s shown above.
- Be concise; do not restate the original note except for minimal quotes to ground an issue.
- No placeholders added to the original note; suggest them only via the questions above.
```

**User Prompt**
```text
Structured Clinical Note  
  
1. Patient Information:  
   - Name: John Brown  
   - Age (or DOB): 73-year-old  
   - Sex/Gender: Male  
   - Date of visit: [Not documented]  
  
2. Chief Complaint:  
   - Central periorbital oedema (CPOE) [AMBIGUOUS: could also mean computerized physician order entry] and shortness of breath on exertion (SOBOE) for the past 3 to 12 days [AMBIGUOUS: unclear if 3/12 indicates March or 3 months]  
  
3. History of Present Illness:  
   - Presents with central periorbital oedema and shortness of breath on exertion for the past 3 to 12 days [AMBIGUOUS: unclear if 3/12 indicates March or 3 months]. Symptoms have been ongoing without acute deterioration noted at presentation.  
  
4. Past Medical History:  
   - ST-elevation myocardial infarction (STEMI) in 2002  
   - Percutaneous coronary intervention (PCI) to the left anterior descending artery (LAD)  
   - Heart failure with reduced ejection fraction (HREF)  
   - Chronic kidney disease (CKD)  
   - Cerebrovascular accident (CVA)  
   - Atrial fibrillation (AF) on a non-vitamin K antagonist oral anticoagulant (NOAC)  
  
5. Medications:  
   - Bisoprolol (Bicor) — [REQUIRED: strength/unit]; [REQUIRED: dose]; [REQUIRED: route]; [REQUIRED: frequency]; [Not documented]  
   - Aspirin — [REQUIRED: strength/unit]; [REQUIRED: dose]; [REQUIRED: route]; [REQUIRED: frequency]; [Not documented]  
   - Statin — [REQUIRED: strength/unit]; [REQUIRED: dose]; [REQUIRED: route]; [REQUIRED: frequency]; [Not documented]  
   - Rivaroxaban — [REQUIRED: strength/unit]; [REQUIRED: dose]; [REQUIRED: route]; [REQUIRED: frequency]; [Not documented]  
   - Sacubitril/valsartan (Entresto) — [REQUIRED: strength/unit]; [REQUIRED: dose]; [REQUIRED: route]; [REQUIRED: frequency]; [Not documented]  
  
6. Allergies:  
   - [Not documented]  
  
7. Family History:  
   - [Not documented]  
  
8. Social History:  
   - [Not documented]  
  
9. Physical Exam:  
   - Vitals: Blood pressure 120/75 mmHg; heart rate 80 beats per minute (normal sinus rhythm on ECG); BMI elevated  
   - Findings: Patient appears well; jugular venous pressure not raised; no pedal edema; heart sounds dual, no murmurs; chest clear  
  
10. Assessment and Plan:  
    - Assessment: Stable  
    - Plan: Lifestyle modifications; dietary advice; follow-up with general practitioner; referral to pulmonary hypertension clinic [AMBIGUOUS: unclear clinical rationale]  
  
11. Additional Notes:  
    - ECG shows normal sinus rhythm at 80 beats per minute  
    - Uncertainties: CPOE meaning unclear; SOBOE duration unclear; referral to pulmonary hypertension clinic rationale unclear
```

**Output**

```text
Clinical Note QA Report  
  
1) Missing Sections  
- Date of visit  
- Allergies  
- Family History  
- Social History  
  
2) Critical Omissions (patient safety / billing-relevant)  
- No allergy information documented  
- Medication list missing strength, dose, route, frequency for all entries  
- Date of visit absent (affects billing/timeline)  
- Duration of symptoms unclear (“past 3 to 12 days” / “3/12” ambiguity)  
  
3) Inconsistencies / Conflicts (quote minimal snippet)  
- “3 to 12 days” vs “3/12” — unclear whether days, months, or date  
- “Central periorbital oedema (CPOE)” — abbreviation could mean computerized physician order entry in other contexts  
  
4) Ambiguities Requiring Clarification  
- CPOE meaning in this clinical context  
- SOBOE duration: “3 to 12 days” vs possible “3/12” notation  
- Rationale for referral to pulmonary hypertension clinic given stable exam and normal sinus rhythm  
  
5) Medications and Allergies — Data Gaps  
- Bisoprolol — missing strength/unit, dose, route, frequency, indication  
- Aspirin — missing strength/unit, dose, route, frequency, indication  
- Statin — missing name, strength/unit, dose, route, frequency, indication  
- Rivaroxaban — missing strength/unit, dose, route, frequency, indication  
- Sacubitril/valsartan — missing strength/unit, dose, route, frequency, indication  
- Allergies — not documented  
  
6) Vitals / Measurements — Issues  
- BMI documented as “elevated” without numeric value or units  
  
7) HPI Feature Coverage Matrix  
- Symptom: Central periorbital oedema — Missing: onset (exact date), location detail (unilateral/bilateral specifics), character, aggravating factors, relieving factors, radiation, timing, severity, associated symptoms  
- Symptom: Shortness of breath on exertion — Missing: onset (exact date), duration clarity, character, aggravating factors, relieving factors, radiation, timing, severity, associated symptoms  
  
8) Assessment ↔ Plan Alignment Gaps  
- Assessment “Stable” does not explicitly address CPOE or SOBOE causes  
- Plan includes referral to pulmonary hypertension clinic without clear link to findings or assessment  
  
9) Additional Documentation Suggestions (formatting/structure only)  
- Use unambiguous date formats (e.g., “3 months” or “March 12”)  
- Expand abbreviations on first use  
- Provide numeric BMI with units  
- Include allergy section even if “No known drug allergies” or equivalent  
- Structure medication list with standardized fields for each item  
  
10) Clarification Questions for Clinician  
- What is the exact date of visit?  
- What does “CPOE” refer to in this patient’s presentation?  
- What is the exact duration of symptoms — days, months, or specific date onset?  
- What is the rationale for pulmonary hypertension clinic referral in this case?  
- What are the complete details (strength, dose, route, frequency, indication) for each listed medication?  
- What are the patient’s allergies?  
- What is the exact BMI value and units?
```
