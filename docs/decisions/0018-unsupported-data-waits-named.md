---
title: "0018 Data no profile searches correctly waits under a named status (provisional)"
parent: Decisions
nav_order: 18
status: accepted
date: 2026-10-05
decision-makers: the maintainer
---

# Data no profile searches correctly waits under a named status (provisional)

## Context and Problem Statement

Some deposits need a search no profile offers yet: non-tryptic immunopeptidomes and peptidomes, intact proteins
(top-down), and DiLeu-labelled samples. Searching them with a tryptic, bottom-up or TMT profile gives wrong results. Are
they out of scope?

## Considered Options

* Treat them as out of scope and drop them
* Search them with the closest profile
* Keep them, waiting under a named status, until a profile for them exists

## Decision Outcome

Chosen option: "Keep them, waiting under a named status", because they are in scope; they need profiles of their own.
This record also covers entry 19 of the log. Both are provisional.

* Non-tryptic deposits (MHC/HLA immunopeptidomes, ligandomes, endogenous peptidomes, "unspecified enzyme") wait as
  `waiting_nonspecific_cleavage`. Their profile will use no protease, a non-specific search over a peptide length range
  (about 8-14 residues for MHC class I, 12-25 for class II), and usually no fixed alkylation.
* Top-down deposits and DiLeu or iDiLeu-labelled deposits wait under their own statuses. DiLeu is never routed as
  isobaric: that profile searches TMT masses.
* Top-down is read from the title, description and protocols only. PRIDE's "Top-down proteomics" keyword sat on 25
  bottom-up deposits among 795 checked.
* A top-down profile must pass MetaMorpheus's whole top-down parameter set, not just the `top-down` protease
  ([0027](0027-metamorpheus-facts-the-search-relies-on.md)).

### Consequences

* Good, because these deposits are kept and counted, not lost.
* Bad, because the screen's precision was checked (795 real records: 4 top-down, 1 DiLeu, all plausible) but its recall
  was not measured.

## More Information

Re-examine when: a non-specific, top-down or DiLeu profile exists, or a searched deposit turns out to be top-down or
labelled anyway.
