---
title: "0021 PXReprise decides each deposit's search chemistry"
parent: Decisions
nav_order: 21
status: accepted
date: 2026-10-05
decision-makers: the maintainer
---

# PXReprise decides each deposit's search chemistry

## Context and Problem Statement

Deposits differ in protease, cysteine alkylation, labels and organism. The facts sit in the SDRF, the PRIDE record and
the protocol text, and these sources are often missing or disagree. Who decides the chemistry a deposit is searched
with, from which source, and what is the default when nothing is stated? This record also covers entries 22, 23 and 24
of the log, which settled the source order and the defaults the same day.

## Considered Options

For the sources (entry 22):

* SDRF and PRIDE fields only
* Two sources must agree
* Act on any clear mention, recording its source

## Decision Outcome

**The decision is PXReprise's.** A profile states its protease and modifications explicitly in the task files. When a
deposit's chemistry does not fit, PXReprise routes it to another profile or makes it wait; it never searches with
chemistry it knows is wrong. A protease other than trypsin that PRIDE names is searched with that protease.

**Sources, in order** (entry 22; chosen option "Act on any clear mention", because a good guess beats no answer): the
question's curated SDRF; the deposit's own or community SDRF (`comment[cleavage agent details]`,
`comment[modification parameters]`, `comment[label]`); PRIDE's identified PTMs and quantification methods; the protocol
text; else the default. One clear mention is enough. Every choice is recorded in provenance with its source, and a
text-only guess is flagged in the manifest.

**Alkylation** (entry 23, provisional): with nothing usable stated, fixed carbamidomethyl on C. Iodoacetamide or
chloroacetamide: the same. Another stated alkylant (NEM, d5-NEM, acrylamide, MMTS, iodoacetic acid, vinylpyridine): that
modification instead. A light/heavy pair (d0/d5-NEM): both variable. NEM named as the only alkylant: variable on C
(entry 28). No alkylation stated: no fixed modification on C.

**Protease** (entry 24, provisional): default trypsin; trypsin with Lys-C: trypsin; one other protease (Lys-C, Glu-C,
Asp-N, chymotrypsin, Arg-C, Lys-N, ...): that protease. Several proteases in separate runs: each file is assigned from
the SDRF or a protease word in its name (`_GluC_`, `_AspN_`); if any file cannot be assigned, the deposit waits as
`waiting_multi_protease`.

### Consequences

* Good, because every search's chemistry and the reason for it are in its provenance.
* Bad, because a guess from text can be wrong; it is flagged so that it can be found.

## More Information

* The SDRF consulted here is, since [0065](0065-sdrf-for-every-deposit.md), the one PXReprise makes for every deposit.
* What happens when the spectra disagree with the text: [0034](0034-when-evidence-disagrees.md).
* Re-examine the defaults when: text guesses prove wrong in searched deposits; a deposit of unknown alkylation shows a
  cysteine-peptide deficit like the NEM deposits (2-3% of peptidoforms against 11-22%); many deposits wait on proteases.
