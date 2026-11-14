---
title: "0011 PXReprise owns the definitions of its pipeline numbers"
parent: Decisions
nav_order: 11
status: accepted
date: 2026-09-29
decision-makers: the maintainer
---

# PXReprise owns the definitions of its pipeline numbers

## Context and Problem Statement

Numbers like "PSMs at 1% FDR" or "protein groups" are counted differently by different tools. These definitions were
first written by the first question (aging), when its scripts ran the pipeline. Once PXReprise ran the pipeline, who
should own them?

## Considered Options

* Leave them with the first question
* PXReprise owns the pipeline's own counts, and each stage declares which definitions it uses

## Decision Outcome

Chosen option: "PXReprise owns the pipeline's own counts", because the engine is what counts them.

* PXReprise's definitions are in `DEFINITIONS.md`, each with an ID `pxreprise:<ID> v<n>`.
* A stage's `provenance.json` declares `"definitions": "pxreprise"`. A record that declares nothing is read with the
  older `aging:` IDs, which give the same numbers.
* Quantitative definitions (intensities, match-between-runs) belong to the quantification project, under its own IDs.
  PXReprise never redefines another project's number.

### Consequences

* Good, because every number in a bundle can be traced to one written definition.
* Bad, because bundles made before the switch keep the old IDs, so readers must accept both.
