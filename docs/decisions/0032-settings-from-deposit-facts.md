---
title: "0032 Decide search settings at download, from the deposit's facts"
parent: Decisions
nav_order: 32
status: accepted
date: 2026-10-07
decision-makers: the maintainer
---

# Decide search settings at download, from the deposit's facts

## Context and Problem Statement

A deposit's settings (modifications, fragmentation, databases) used to come from the profile, with problems found only
after the search, when the results looked wrong. The maintainer decided that settings must be decided before the search,
not afterwards. How?

## Considered Options

* Profile defaults, corrected after a bad result
* A settings plan made at download from a written record of the deposit's facts

## Decision Outcome

Chosen option: "A settings plan made at download", structured as follows.

1. **An experiment record** ([0030](0030-pxreprise-determines-the-experiment.md)), per deposit and per file, each fact
   with its source and evidence, written before any search.
2. **A pure rule** from facts to a complete settings plan (task TOMLs and per-file TOMLs) within the profile's method
   family. A combination the rule does not cover waits with a named reason, never a silent default.
3. **The SDRF, the design and a readable `experiment_notes.md`**, all from that record.
4. **Tests for every contingency:** rule tests (a scenario and its expected difference from MetaMorpheus's defaults),
   every written TOML read back through MetaMorpheus's parser, and small live runs for facts the code cannot prove.

Rulings made with it:

* An **enriched PTM** is a variable modification on its target residues, not fixed, with at most 3 modifications per
  peptide for an enriched deposit; every other deposit keeps MetaMorpheus's 2.
* **Fragmentation per file, from the headers.** Every MS2 header is read at QC (mzLib's dissociation type and analyzer)
  and each file's type goes in its `<raw>.toml`: HCD, CID, ETD, EThcD, and LowCID for CID read in an ion trap.
  Autodetect only for a file that mixes types when every type is one MetaMorpheus supports and every MS2 is Orbitrap.
  Anything else (an unsupported or missing header, mixed analyzers) waits with a named reason. The depositor's statement
  is compared with the headers; a disagreement is noted, never acted on.
* **Species is a fact of the record too,** per file or sample, with its source; the databases follow from it
  ([0035](0035-species-and-databases.md)).

### Consequences

* Good, because each choice is visible, sourced and tested before hours of search are spent.
* Bad, because a deposit the rules do not cover waits instead of being searched.
