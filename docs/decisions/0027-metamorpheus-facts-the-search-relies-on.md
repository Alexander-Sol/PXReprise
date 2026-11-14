---
title: "0027 How the engine maps chemistry onto MetaMorpheus"
parent: Decisions
nav_order: 27
status: accepted
date: 2026-10-05
decision-makers: the maintainer
---

# How the engine maps chemistry onto MetaMorpheus

## Context and Problem Statement

[0021](0021-pxreprise-decides-search-chemistry.md) says what chemistry a deposit is searched with. MetaMorpheus decides
what can be expressed, and some of its behaviour fails silently. These facts were checked against the MetaMorpheus
1.1.11 binaries. How must the search stage use them?

## Considered Options

Not recorded as options: each point below follows from what MetaMorpheus can and cannot do.

## Decision Outcome

1. **Per-file protease.** A `<raw>.toml` beside a raw file, with `DigestionAgent = "Glu-C"`, sets that file's protease;
   calibration carries it into the calibrated file's toml, so GPTMD and Search keep it. One protease goes in the task
   toml; several go in per-file tomls. A deposit whose files are all assigned does not wait.
2. **Modifications are per task only.** MetaMorpheus's file-specific parameters have no modification keys, so cysteine
   chemistry is per deposit. Files that need different modifications are split
   ([0033](0033-split-mixed-deposits.md)).
3. **Two silent fallbacks become failures.** "Unrecognized mod" (an unknown modification string is skipped) and "Problem
   parsing the file-specific toml" (a bad per-file toml falls back to the task's protease) fail the search.
4. **Non-specific search** (`SearchType = "NonSpecific"`) only in the Search task. Calibration and GPTMD refuse it and
   stay specific.
5. **Top-down** needs the parameter set MetaMorpheus's GUI applies and its command line does not: precursor
   deconvolution on, provided precursor info off, precursor max charge 60, product max charge 20, internal ions on
   (minimum length 10), no quantification, `PlusOrMinusThreeMM`, no variable modifications, no MS/MS peak trimming,
   `MaxModificationIsoforms` 4096, orphan peaks not assumed z = 1.
6. **Nothing in MetaMorpheus reads protease or modifications from an SDRF,** or infers the protease from the data.
   PXReprise writes them into the task files itself.

### Consequences

* Good, because a search never runs with chemistry it silently dropped.
* Bad, because these facts belong to one MetaMorpheus version: check them again at every MetaMorpheus version change.
