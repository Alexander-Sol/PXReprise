---
title: "0030 PXReprise determines what experiment each deposit is"
parent: Decisions
nav_order: 30
status: accepted
date: 2026-10-07
decision-makers: the maintainer
---

# PXReprise determines what experiment each deposit is

## Context and Problem Statement

Whether a deposit is a whole lysate or a phospho enrichment, which protease and alkylant it used, and how it is labelled
all change how it must be searched and what its results mean. No project owned that determination.

## Considered Options

* Each question decides for its own deposits
* The SDRF tools decide
* PXReprise decides, where it already applies the consequence

## Decision Outcome

Chosen option: "PXReprise decides", because PXReprise already reads the protocol (its screen and chemistry detector) and
routes each deposit to a profile, so the determination lives where its consequence is applied.

* Scope, per deposit and per run where kinds mix: the sample (whole lysate, organelle or fraction, secreted or
  extracellular vesicles); enrichment and its target (a PTM, a bait protein, a protein class, a compartment, a peptide
  class, or none); chemistry (protease, alkylant; [0021](0021-pxreprise-decides-search-chemistry.md)); labelling
  (label-free, TMT, SILAC, 18O, dimethyl); immunopeptidome class (MHC-I or MHC-II).
* It is done at download, before the search, and written to the manifest and as SDRF cells.
* The lines: the SDRF tools (in mzLib) own the SDRF format, the drafter and its evidence readers; the quantification
  project owns how each type is quantified; a question only reports what it notices.

### Consequences

* Good, because one place answers "what is this deposit?" for every question.
* Bad, because PXReprise is responsible when the answer is wrong.
