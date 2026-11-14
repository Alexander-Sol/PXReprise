---
title: "0037 Count identifications from the whole search, not per-file FDR"
parent: Decisions
nav_order: 37
status: accepted
date: 2026-10-07
decision-makers: the maintainer
---

# Count identifications from the whole search, not per-file FDR

## Context and Problem Statement

MetaMorpheus can recompute FDR for each file separately (its "Individual File Results"). The number of peptide
identifications per file varies widely, so per-file q-value cut-offs move up and down: two peptides with the same score
can pass in one sample and fail in another. The whole-search tables (AllPSMs, AllPeptides) hold everything to one
standard. Which should the engine count from?

## Considered Options

* MetaMorpheus's per-file results
* The whole-search set, restricted to each run

## Decision Outcome

Chosen option: "The whole-search set, restricted to each run", because it holds every file to one consistent standard.

* Every identification count, per run included, is the whole-search set restricted to that run.
* Per-file results may be used only for a valid reason, carefully, and the reason is stated where they are used.
* **No individual-file tables** (entry 63 of the log, 2026-11-05): profiles from `label-free-dda@14`,
  `label-free-dda-lowres@7` and `tmt-dda@3` on set `WriteIndividualFiles = false`. The QC payload takes a file's MS2
  scan count from the spectra QC when `results.txt` has no per-file line. Per-run precursor counts have no other source
  than MetaMorpheus, so they wait for a MetaMorpheus release that writes them without the tables.

### Consequences

* Good, because per-run numbers can be compared across runs and deposits.
* Good, because the per-file tables were large and mostly unused.
* Bad, because deposits searched under the new profiles lack per-run precursor counts until that MetaMorpheus release.
