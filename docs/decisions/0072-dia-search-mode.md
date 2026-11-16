---
title: "0072 A DIA search mode keyed on the DIA task's own tables"
parent: Decisions
nav_order: 72
status: accepted
date: 2026-11-16
decision-makers: the maintainer
---

# A DIA search mode keyed on the DIA task's own tables

## Context and Problem Statement

A DIA search task is coming to MetaMorpheus (#2968, then its calibration and task pull requests). Under
[0014](0014-new-engines-join-through-metamorpheus.md), it joins PXReprise as a task and a profile. Its output differs from
a DDA search's:
* It writes three tables: `AllDiaPrecursors.tsv` (one row per precursor per run), `AllDiaPeptides.tsv` and
  `AllDiaProteinGroups.tsv`. Their names follow the rule that a file name tells the quantification mode.
* It writes no `AllPSMs.psmtsv` and no `AllPeptides.psmtsv`.

The engine is built around the DDA tables:
* the search stage requires `AllPSMs.psmtsv`;
* the design check, the contamination count, the QC payload, pruning and the manifest all read `AllPSMs.psmtsv` or
  `AllQuantifiedProteinGroups.tsv`;
* the manifest writes `acquisition: DDA` for every entry.

So every DIA search would fail today. How should the engine take DIA output?

## Considered Options

* A DIA `AllPSMs.psmtsv`, written by the DIA task so that the DDA path runs unchanged
* A DIA mode in the engine, keyed on the DIA task's own tables

## Decision Outcome

Chosen option: "A DIA mode keyed on the DIA task's own tables", because a DIA precursor is not a peptide-spectrum
match. It is scored on co-eluting fragment traces across many scans. A table named `AllPSMs.psmtsv` would say its rows
are PSMs when they are not, and the data repository could not store them as PSM rows.

* **A DIA task kind and profile,** whose required files are `AllDiaPrecursors.tsv`, `AllDiaPeptides.tsv`,
  `AllDiaProteinGroups.tsv` and `results.txt`. Required files are per mode. No mode reads or writes another mode's file
  names.
* **Every reader of a DDA table gets a DIA branch:**
  * `AllDiaPrecursors.tsv` replaces `AllPSMs.psmtsv` (the contamination count, the QC payload and its provenance,
    pruning);
  * `AllDiaProteinGroups.tsv` replaces `AllQuantifiedProteinGroups.tsv` (the design check);
  * the search-task folder is the last `Task*DiaLibrarySearch*` folder;
  * the manifest writes the profile's acquisition.
* **The headline count is a metric of its own,** with its own definition in `DEFINITIONS.md`
  ([0011](0011-pipeline-number-definitions.md)). The headline is `All target precursors with q-value <= 0.01: N`. It is
  never the DDA precursor count and is never pooled with it.
* **The quantities keep their own identities:**
  * the tables' quantities cite DIA definitions of their own, never the label-free DDA ones;
  * statistics rows have a `dia` quantification style;
  * `dataset_overview.quant_scale` gets a DIA value. It never reads `label_free_apex`, and MS2 fragment areas never share
    a value with MS1 areas.
* **The data repository's ingest gets its own DIA branch.** It stores no DIA precursor as a PSM row.
* **A dry ingest comes before any code:** the DIA task's benchmark output is read through the engine's checks and the
  ingest's readers.
* **Nothing is built until** a MetaMorpheus release carries the DIA task and its benchmark is done. Until then a deposit
  the screen reads as DIA waits under its named status ([0018](0018-unsupported-data-waits-named.md)).

### Consequences

* Good, because a row's table always says what the row is, and DIA and DDA numbers can never pool by accident.
* Good, because the DDA path is unchanged.
* Bad, because every place that reads a DDA table needs a DIA branch, and each new reader must remember the mode.
* Bad, because the mode cannot be built or tested against released output until MetaMorpheus releases the task.
