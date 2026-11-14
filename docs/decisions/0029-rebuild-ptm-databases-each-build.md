---
title: "0029 Rebuild the PTM-annotated databases at every pipeline build; never add enriched-for PTMs"
parent: Decisions
nav_order: 29
status: accepted
date: 2026-10-07
decision-makers: the maintainer
---

# Rebuild the PTM-annotated databases at every pipeline build; never add enriched-for PTMs

## Context and Problem Statement

The lab's profiles search proteomes annotated with PTM sites that the corpus itself has identified. The list of sites
grows as deposits are delivered. Who rebuilds these databases, when, and which sites may go in?

## Considered Options

* The question that uses the databases rebuilds them when it chooses
* PXReprise rebuilds them at every pipeline build

## Decision Outcome

Chosen option: "PXReprise rebuilds them at every pipeline build", because a pipeline build is when the search database
may change anyway.

* A pipeline build is every new search profile version: a MetaMorpheus change, a new proteome release, any database
  change.
* At each, `tools/ptm-db/BuildPtmDb.cs` rebuilds the PTM-annotated proteomes and the isoform files from a copy of the
  live catalog (never the live file, which a reader may hold), under a new tag; nothing is overwritten. The catalog id
  and every sha256 are recorded, consumers are warned before the search database changes, and the new files are wired
  into the new profile version.
* The selection rule is the consumer's. Each question brings its own as data.
* **Enriched-for PTMs are never added**, because they would flood the database with false positives. A site of a class is
  not counted from a deposit that enriches that class: declared (`data/ptm-db/enriched_ptm_exclusions_*.tsv`) or measured
  by the builder (any run with phospho at 5% or more, or GG at 2% or more, of its 1% target PSMs; whole proteomes run at
  0.1-0.3% phospho).

### Consequences

* Good, because every profile version names exactly which database build it searched.
* Bad, because each build is a database change, so each is a new profile version.

## More Information

Which UniProt sites stay in the database was narrowed later by [0061](0061-ptms-mimic-the-sample.md).
