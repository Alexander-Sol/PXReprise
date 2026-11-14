---
title: "0007 A profile version is never edited"
parent: Decisions
nav_order: 7
status: accepted
date: 2026-09-29
decision-makers: the maintainer
---

# A profile version is never edited

## Context and Problem Statement

The first profile, `label-free-dda@1`, searched with the first question's own PTM-annotated databases, which exist only
where that question built them. A generic profile was needed for everyone else. More broadly: how can a search method
change without changing what the results already delivered under it mean?

## Considered Options

* Edit the profile in place and re-search everything
* Edit the profile in place and leave earlier results as they are
* Make every change a new profile version

## Decision Outcome

Chosen option: "Make every change a new profile version", because a profile version then always names one exact method.

* Any change to how deposits are searched is a new version: a new MetaMorpheus release, a new database, a changed
  setting. An existing version is never edited.
* `label-free-dda@2` was the first generic version: `@1`'s search with UniProt databases named by proteome ID
  (`uniprot = "UP..."`), downloaded once by the engine through mzLib and recorded, with date and sha256, in each
  search's provenance.
* Deposits searched under an earlier version keep their results and their meaning. When to re-search them is decided
  separately ([0039](0039-catch-up-re-search.md)).

### Consequences

* Good, because every delivered result names the exact method behind it.
* Bad, because versions accumulate, and a question must name the versions it accepts.
* `AGENTS.md` tells agents never to edit files under `profiles/`.

## More Information

Which UniProt set a profile uses changed later, from reviewed entries only to canonical proteomes
([0038](0038-canonical-proteomes.md)). That change shipped as new versions, as this record requires.
