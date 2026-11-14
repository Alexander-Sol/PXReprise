---
title: "0051 Clean the spectral libraries at every registration"
parent: Decisions
nav_order: 51
status: accepted
date: 2026-10-27
decision-makers: the maintainer
---

# Clean the spectral libraries at every registration

## Context and Problem Statement

Each search adds its spectra to the organism's spectral library. Some of those spectra are false: peptides whose
C-terminal modification would have blocked trypsin from cutting there, and GPTMD's hydroxylation on P and K, which a
GPTMD study put among the false classes. Should the libraries be cleaned, or started again from nothing?

## Considered Options

* Start again from an empty library
* Clean the current libraries now, and at every registration

## Decision Outcome

Chosen option: "Clean now, and at every registration", because the maintainer preferred clean libraries to starting
from no library.

* `Search/LibraryCleaning.cs` drops a spectrum whose C-terminal modification blocks trypsin at every occurrence of its
  base sequence (kept at a protein C-terminus, or when the peptide occurs nowhere in the database), and GPTMD's
  hydroxylation on P and K.
* It runs at every library registration, and on demand with `pxreprise library repair <organism> --databases ...`.

### Consequences

* Good, because the libraries keep their earlier evidence without the known false spectra.
* Bad, because the engine carries a rule that belongs in the tools that write libraries.

## More Information

Superseded once mzLib and MetaMorpheus write clean libraries themselves.
