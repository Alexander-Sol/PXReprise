---
title: "0059 Delete raw files as soon as their calibration succeeds"
parent: Decisions
nav_order: 59
status: accepted
date: 2026-11-02
decision-makers: the maintainer
---

# Delete raw files as soon as their calibration succeeds

## Context and Problem Statement

A large deposit's raw files can fill the disk while it is searched. Calibration is per file and changes nothing in the
eventual search: GPTMD and the search read only the calibrated mzML. Do the raw files need to stay?

## Considered Options

* Keep raw files until the search is delivered
* Delete each raw file as soon as its calibration succeeds

## Decision Outcome

Chosen option: "Delete each raw file as soon as its calibration succeeds", because nothing after calibration reads it.

* Machine option `delete_raw_after_calibration` (default true).
* While MetaMorpheus runs, the search stage checks each minute, and once after: a raw file is deleted when its
  non-empty `<name>-calib.mzML` and its `<name>-calib.toml` both exist (`Search/CalibratedRaws.cs`).
* Only PXReprise's own folders are touched.
* Measured on one deposit (PXD048877): 30 calibrated mzML files took 37.1 GB, against 52.7 GB of raw files.

### Consequences

* Good, because a deposit needs far less disk while it is searched.
* Bad, because a search problem found later (for example, unalkylated cysteines) means downloading the raw files again.
  The probe step should catch such problems before the full download.
