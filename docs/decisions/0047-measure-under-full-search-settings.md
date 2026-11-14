---
title: "0047 Measure a profile change under the full search's settings before re-searching"
parent: Decisions
nav_order: 47
status: accepted
date: 2026-10-21
decision-makers: the maintainer
---

# Measure a profile change under the full search's settings before re-searching

## Context and Problem Statement

`label-free-dda-lowres@4` searched ion-trap HCD data as LowCID. It was justified by one-file tests run under the probe's
settings (a +/-20 ppm precursor window, three species' databases, phospho, GG and acetyl as variable modifications). In
full searches it lost 1-16% of PSMs on all three deposits searched both ways (77 of 84 files lower). Ten deposits had
already been re-searched under it.

## Considered Options

* Keep `@4` and its ten re-searches
* Revert, and from now on measure under the full search's own settings first

## Decision Outcome

Chosen option: "Revert, and measure under the full search's own settings first", because a test under different settings
measured a different search.

* The ten re-searches were undone and their earlier bundles brought back.
* Any profile change is measured under the full search's own settings, against the delivered search, before deposits
  are called under-identified or re-searched.
* The next version, `label-free-dda-lowres@5`, uses LowCID in the probe only and searches HCD at +/-0.5 Da. Under one
  deposit's own full-search settings, HCD and LowCID came out about even (8,627 and 8,420 PSMs over three files).

### Consequences

* Good, because a re-search is only started on evidence from the search it will repeat.
* Bad, because each profile change needs a full-settings test first, which takes longer than a probe test.
