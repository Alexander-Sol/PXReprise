---
title: "0035 Species, databases, spike-ins and the contaminant panel"
parent: Decisions
nav_order: 35
status: accepted
date: 2026-10-07
decision-makers: the maintainer
---

# Species, databases, spike-ins and the contaminant panel

## Context and Problem Statement

Deposits hold samples of one species, samples of several (xenografts, host and pathogen, co-cultures, spike-in
standards), and runs of different species side by side. The corpus is aggregated to answer biological questions, so
anything that inserts foreign peptides into it creates artifacts. How is each case searched and delivered?

## Considered Options

Not recorded as options; the maintainer ruled point by point.

## Decision Outcome

1. **A sample that truly holds several species** is searched once against all its species' databases together. A
   peptide identical in two species is reported as shared, never forced onto one.
2. **Runs from different species** in one deposit are split ([0033](0033-split-mixed-deposits.md)), one search per
   species, each against its own database. Combined databases only where a sample needs them.
3. **A species check in the probe step:** each probe file is searched briefly against every listed species. The result
   assigns runs and catches an unlisted or wrong species before the full download. Runs it cannot assign go to a person
   ([0034](0034-when-evidence-disagrees.md)).
4. **Mixed-species samples are written and reported honestly.** Nothing may silently write one species for a mixed
   sample. Such samples are held until the SDRF can say "this sample is human and mouse"; then each search reports, per
   species, the peptides unique to it and those shared.
5. **Databases by species:** human, mouse and rat use UniProt's canonical proteomes
   ([0038](0038-canonical-proteomes.md)) plus PTM-annotated builds ([0029](0029-rebuild-ptm-databases-each-build.md));
   any other species uses UniProt's reference proteome, named as such in provenance. **Any species beyond human, mouse and
   rat needs a person's approval** before analysis, recorded as question data, never as an engine default.
6. **Spike-ins, with great caution.** A declared spike-in (benchmark yeast or E. coli mixes, UPS1/UPS2, labelled
   references, iRT-type peptides) is searched as a species of its sample, so identification and FDR are right, and may be
   used for quantification checks. Its peptides and proteins are **never delivered** to the data repository: they are
   removed from the bundle, and the removal is counted in provenance. Every deposit with a declared spike-in is held for a
   person, who decides per deposit.
7. **Contaminant panel:** MetaMorpheus's `MetaMorpheusContaminants_NoUPS.xml` as it is. The UPS file only where a study
   declares a UPS spike-in. Between different accessions, protein parsimony decides which protein shared peptides go to.

### Consequences

* Good, because nothing in the corpus claims a species it was not measured in.
* Bad, because mixed-species samples, other species and spike-in deposits wait for people.
