---
title: "0039 Catch up out-of-date searches, one search in four"
parent: Decisions
nav_order: 39
status: accepted
date: 2026-10-07
decision-makers: the maintainer
---

# Catch up out-of-date searches, one search in four

## Context and Problem Statement

When a new profile version replaces an old one ([0007](0007-profile-versions-never-edited.md)), every deposit searched
under the old one is out of date. Re-searching them all at once would stop new deposits for weeks; never re-searching
them leaves old results behind. The maintainer did not want to spend all the machine's time on old datasets.

## Considered Options

* Re-search only by exception
* Re-search everything before any new deposit
* Interleave: a share of searches are re-searches until none is left

## Decision Outcome

Chosen option: "Interleave", because new deposits keep flowing while the old ones catch up.

* The batch searches new deposits and, every `catch_up_every`-th start (4 at the time), re-searches one deposit
  searched under a profile the question no longer names. With no new deposit waiting, re-searches run back to back.
* A re-search moves the old folder aside, fetches again, delivers again and replaces the manifest entry.
* Order by gain: rat deposits first (their database changed most), then deposits the question now has a curated design
  for, then the oldest.
* A re-search also brings the deposit up to the current design, chemistry, settings and provenance rules, not only the
  new database.

### Consequences

* Good, because old deposits contribute more after their re-search.
* Bad, because a quarter of the machine's searches go to deposits already delivered.
