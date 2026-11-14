---
title: "0031 Quantify inside MetaMorpheus, with match-between-runs and a design, on every search"
parent: Decisions
nav_order: 31
status: accepted
date: 2026-10-07
decision-makers: the maintainer
---

# Quantify inside MetaMorpheus, with match-between-runs and a design, on every search

## Context and Problem Statement

Earlier searches ran without quantification inside MetaMorpheus, and without an experimental design for most deposits.
Who specifies quantification, and is it on by default?

## Considered Options

* Quantify later, in a separate step, only where a design exists
* Quantify inside MetaMorpheus on every search, with match-between-runs and a design

## Decision Outcome

Chosen option: "Quantify inside MetaMorpheus on every search", because PXReprise runs the pipeline, so it specifies
quantification, and every deposit can then contribute quantities.

* PXReprise specifies quantification per sample type (label-free, TMT, DIA, ...).
* Label-free: FlashLFQ inside MetaMorpheus, with match-between-runs on and an experimental design.
* The design comes from the question's curated SDRF, else the deposit's SDRF, else a default SDRF PXReprise writes (one
  condition "all", each file its own biological replicate, fraction 1, technical replicate 1; source
  `pxreprise_default`).
* The quantification project still owns the rules for designs and the statistics after the search.

### Consequences

* Good, because quantities exist for every delivered deposit.
* Bad, because a fractionated deposit is wrong under the default design.

## More Information

The default design is retired by [0066](0066-sdrf-makes-the-design.md): PXReprise's own SDRF for every deposit
([0065](0065-sdrf-for-every-deposit.md)) makes the design, and a deposit whose SDRF gives no usable design waits for a
person. This record supersedes an earlier rule that FlashLFQ was never switched on inside MetaMorpheus.
