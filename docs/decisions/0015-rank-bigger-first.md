---
title: "0015 Rank new deposits bigger first, within limits (provisional)"
parent: Decisions
nav_order: 15
status: accepted
date: 2026-10-05
decision-makers: the maintainer
---

# Rank new deposits bigger first, within limits (provisional)

## Context and Problem Statement

A census can find hundreds of relevant deposits, more than can be searched soon. Depositors' file names and descriptions
rarely describe their experimental design well. In what order should `pxreprise rank` put them?

## Considered Options

* Rank on the design read from SDRFs or guessed from file names
* Bigger is better, within limits

## Decision Outcome

Chosen option: "Bigger is better, within limits", because the design is too often unreadable to rank on. This is a
provisional policy: a good guess, made fast on purpose.

* Tier 1: at most 60 raw files and 150 GB. Most raw files first, then the smaller download.
* Tier 2: larger than either limit, ranked last.
* A design read from a deposited SDRF or guessed from file names is written as information only, never ranked on.
* The preference behind it: sample groups with many replicates. Small deposits are not favoured (underpowered), and
  overly large ones are avoided.

### Consequences

* Good, because the rule is simple and needs no reading of the design.
* Bad, because a big deposit may be an enrichment, a cell-line study or a methods paper rather than a tissue study.

## More Information

Re-examine when: designs become readable (an SDRF for every deposit, [0065](0065-sdrf-for-every-deposit.md)); big
deposits turn out to be enrichments, cell lines or methods papers more often than whole-tissue studies; the 60-file or
150 GB limit changes.
