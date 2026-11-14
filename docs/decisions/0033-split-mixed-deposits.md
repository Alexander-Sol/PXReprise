---
title: "0033 Split mixed deposits into separate searches"
parent: Decisions
nav_order: 33
status: accepted
date: 2026-10-07
decision-makers: the maintainer
---

# Split mixed deposits into separate searches

## Context and Problem Statement

MetaMorpheus sets modifications and databases per search only ([0027](0027-metamorpheus-facts-the-search-relies-on.md)).
Some deposits mix files that differ in label, species, enrichment or chemistry. Searched together, some files get the
wrong settings.

## Considered Options

* Search the whole deposit with one set of settings
* Make the deposit wait
* Split it into groups, each its own search

## Decision Outcome

Chosen option: "Split it into groups", because every file then gets the settings that fit it.

* Files that differ in anything a per-file TOML cannot carry are grouped, and each group is its own search under the same
  accession: its own folder, probe, provenance and delivery, keyed `<accession>-<group>`.
* The provenance names the group and the reason, and the deposit's `experiment_notes.md` explains the split.
* A split is declared as question data, `[splits.<accession>]` (see `docs/questions.md`).

### Consequences

* Good, because a mixed deposit is searched correctly instead of waiting.
* Bad, because the data repository and the catalog must carry several searches per accession.

## More Information

The strata a quantitative analysis must keep apart (tissue, cell type, enriched against whole) are kept apart here, at
search time, rather than by splitting them later in statistics.
