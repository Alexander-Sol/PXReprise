---
title: "0064 A new question builds its own databases, profiles and libraries"
parent: Decisions
nav_order: 64
status: accepted
date: 2026-11-06
decision-makers: the maintainer
---

# A new question builds its own databases, profiles and libraries

## Context and Problem Statement

The second question, type 2 diabetes, began on a contributor's own computer. The lab's PTM-annotated databases and
spectral libraries are built from the first question's corpus and are not published. What does a new question start
from?

## Considered Options

* Copy the lab's databases and libraries to the new question
* The new question mimics the lab's method and builds its own

## Decision Outcome

Chosen option: "Mimic the method, build its own", because the lab's databases stay unpublished and each library chain
must come from its own project's data.

* The new question runs on the same settings as the lab's profiles of the time (`label-free-dda@14`,
  `label-free-dda-lowres@7`, `tmt-dda@3`), with its own repository, store, manifest, site, databases, profiles and
  spectral library chains. Nothing is copied from the lab's machines.
* A new user's install uses a profile whose databases download by themselves.
* This sets aside, for now, the single shared corpus of [0041](0041-public-data-architecture.md) for this question.

### Consequences

* Good, because each question's databases and libraries are traceable to its own data.
* Bad, because two stores now exist, which the public data architecture means to avoid in the long run.
