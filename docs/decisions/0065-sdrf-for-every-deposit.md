---
title: "0065 PXReprise makes every deposit's SDRF"
parent: Decisions
nav_order: 65
status: accepted
date: 2026-11-07
decision-makers: the maintainer
---

# PXReprise makes every deposit's SDRF

## Context and Problem Statement

An SDRF describes each sample and raw file of a deposit. Many deposits have none; deposited and community SDRFs carry
errors, omissions and inconsistencies (for instance a wrong instrument, which the raw file's header shows). The first
question had been curating SDRFs for its own deposits. Who should make them, and how?

## Considered Options

* Each question curates SDRFs for its own deposits
* Use the deposited or community SDRF where one exists
* PXReprise makes an SDRF for every deposit, independently, and compares it with every other SDRF

## Decision Outcome

Chosen option: "PXReprise makes an SDRF for every deposit", because the pipeline that searches a deposit needs its sample
record, and an independent SDRF is the only way to find the errors in the others.

1. Every deposit the pipeline searches gets an SDRF that PXReprise writes, whether or not PRIDE has one.
2. A question no longer curates SDRFs. Its earlier curated SDRFs become inputs. A question keeps its scientific choices
   (which deposits and files, its exceptions), not the sample record.
3. A split deposit ([0033](0033-split-mixed-deposits.md)) gets one SDRF per group, stored beside that group's raw files,
   so MetaMorpheus makes the group's quantitative design from it.
4. The SDRF tools (mzLib's SDRF reader, validator and drafter, and the evidence readers) are how the SDRFs are made;
   each SDRF and its upkeep are PXReprise's.
5. The paid evidence reading (a language model reading the paper and supplements) is a machine option, on where a key is
   configured and off otherwise. Every deposit gets an SDRF from the free sources either way, and the SDRF says which
   sources it had.
6. A sample fact the evidence cannot settle and the spectra cannot measure goes to the maintainer, with the question's
   owner copied (this amends [0034](0034-when-evidence-disagrees.md)).
7. Every value a deposited or community SDRF states that the raw files or the engine's measurements can check is
   checked: instrument model, dissociation and analyzer, label and kit, organism per file, protease and alkylation, file
   names. A disagreement is corrected in PXReprise's SDRF from the spectra and listed with both values and the evidence.
8. PXReprise's SDRF is made **independently**: no deposited or community SDRF is among its inputs. It is then compared
   with each of them, every column and row; each difference gets an effort to resolve it from the evidence (the paper,
   figures and tables included), and what stays unresolved is reported to a person with both values and each side's
   evidence. Depositors' contact details from PRIDE's record may be included to help that person, in private reports
   only: never in a bundle, a thread or anything published.

### Consequences

* Good, because every deposit gets a sample record, and errors in deposited SDRFs are found and reported.
* Bad, because making and comparing an SDRF for every deposit is work the pipeline did not do before, and the paid
  reading costs money.
