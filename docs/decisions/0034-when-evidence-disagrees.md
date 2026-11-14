---
title: "0034 When evidence disagrees: spectra first, and tell a person"
parent: Decisions
nav_order: 34
status: accepted
date: 2026-10-07
decision-makers: the maintainer
---

# When evidence disagrees: spectra first, and tell a person

## Context and Problem Statement

A deposit's text, its SDRF and its spectra can disagree. Some disagreements the engine can settle; some need a person.
A deposit that silently waits with a status nobody reads is a deposit lost.

## Considered Options

* Trust the text
* Trust the spectra where they can measure the fact, and the source order elsewhere; announce every wait that needs a
  person

## Decision Outcome

Chosen option: the second, because the spectra are the measurement, and a wait nobody sees is never resolved.

1. A fact the spectra can measure (isobaric reporter ions, fragmentation, analyzer, enrichment level from the probe
   search): the spectra win. The text's claim and the disagreement go in `experiment_notes.md`.
2. A fact the spectra cannot measure (alkylant, protease, species in a sample): the source order of
   [0021](0021-pxreprise-decides-search-chemistry.md) decides.
3. A curated SDRF that the spectra contradict: the deposit waits. Nothing is overruled.
4. **Every wait that needs a person's decision is announced,** never left as a status. It is written to the run's
   `decisions_needed` list (the deposit, the conflicting evidence with sources, the choices and what each would do),
   sent to the question's owner naming who must decide, and shown until it is answered. A wait only for a capability (a
   profile not built yet) is reported in the census.

### Consequences

* Good, because every open decision has an owner and a place where it is seen.
* Bad, because people must answer, and a deposit waits until they do.

## More Information

[0065](0065-sdrf-for-every-deposit.md) amends point 4 for sample facts the evidence cannot settle: they go to the
maintainer, with the question's owner copied.
