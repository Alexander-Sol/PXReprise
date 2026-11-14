---
title: "0036 One identification filter, used everywhere"
parent: Decisions
nav_order: 36
status: accepted
date: 2026-10-07
decision-makers: the maintainer
---

# One identification filter, used everywhere

## Context and Problem Statement

A search applies a confidence cut-off at several steps: reporting identifications, choosing the peptides for protein
inference, and choosing the peptides that are quantified. These steps did not all use the same cut-off.

## Considered Options

* Each step keeps its own cut-off
* One cut-off for every step, with a stated fallback

## Decision Outcome

Chosen option: "One cut-off for every step", because a peptide should not be identified by one rule and quantified by
another.

* PEP q-value below 0.01 is the cut-off for reporting a peptide as identified, for the peptides used in protein
  inference, and for the peptides that are quantified.
* Where PEP cannot be computed for a dataset: q-value notch below 0.01, and lastly q-value below 0.01.
* Which one was used is recorded.

### Consequences

* Good, because every count and every quantity rests on the same identifications.
* Bad, because it needs changes in mzLib and MetaMorpheus. A profile turns it on only once a MetaMorpheus release
  carries them, so check at every MetaMorpheus version change.
