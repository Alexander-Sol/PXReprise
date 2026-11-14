---
title: "0043 An uncalibrated search's mass windows come from the probe's measured offset"
parent: Decisions
nav_order: 43
status: accepted
date: 2026-10-17
decision-makers: the project's AI agent, under the maintainer's authority
---

# An uncalibrated search's mass windows come from the probe's measured offset

## Context and Problem Statement

The low-resolution profile (`label-free-dda-lowres@2`) runs no calibration, yet kept MetaMorpheus's +/-5 ppm precursor
window and FlashLFQ's 5 ppm. One deposit (PXD045821) is 8.7 ppm off and identified 0.37% of its spectra. On one of its
runs: +/-5 ppm gave 292 PSMs, +/-14 ppm 19,120. The maintainer's rule stands: no calibration of low-resolution data until
MetaMorpheus's low-resolution calibration is proven.

## Considered Options

* Keep +/-5 ppm
* A fixed wide window, +/-20 ppm
* A window from each deposit's measured offset

## Decision Outcome

Chosen option: "A window from each deposit's measured offset", because a fixed +/-20 ppm cost a run that was 0.0 ppm
off 21% of its PSMs and 17% of its peptides.

* `[engine] precursor_window = "probe"` (needs `probe_search`; refused together with Calibration).
* The probe (+/-20 ppm) records each run's median offset (at least 20 PSMs at 1%).
* The search sets the precursor tolerance and FlashLFQ's ppm tolerance to ceil(max |offset|) + 5, at most 20, or keeps
  the defaults when no offset exceeds 2 ppm. Provenance records the window.
* First profile: `label-free-dda-lowres@3`.

### Consequences

* Good, because off-centre deposits are identified and quantified, and centred ones keep the narrow window.
* Bad, because three probe files stand for the whole deposit, and some deposits have only some files off.

## More Information

Entry 46 of the log (2026-10-18) tested MetaMorpheus's MS1 calibration in LowCID mode on ion-trap HCD files: calibration
fails ("Required 16, saw 0") and the search gives the uncalibrated numbers, so the measured window stays. It is untested
on true LowCID data (CID read in the ion trap); test it there before any low-resolution profile calibrates.
