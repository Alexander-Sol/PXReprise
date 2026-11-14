---
title: "0016 Disease keywords need a stated reference group; watch keywords never queue (provisional)"
parent: Decisions
nav_order: 16
status: accepted
date: 2026-10-05
decision-makers: the maintainer
---

# Disease keywords need a stated reference group; watch keywords never queue (provisional)

## Context and Problem Statement

A question can widen its census with disease terms (`disease_keywords`). A disease deposit is useful to a question only
if it also holds a reference group to compare against. A question may also want to follow deposits it does not yet
search. How should the census treat both?

## Considered Options

* Queue every disease deposit
* Queue a disease deposit only when its text names a reference group
* Leave reference groups to a person, deposit by deposit

## Decision Outcome

Chosen option: "Queue a disease deposit only when its text names a reference group". This is a provisional policy.

* A deposit found only by `disease_keywords` is queued only if its PRIDE text names a reference group: control,
  healthy, untreated, vehicle, sham, wild type, placebo, age-matched, cognitively normal, young, and similar words.
  Otherwise it goes on the census's watch list as `no_reference_group_found`.
* `watch_keywords` (entry 17 of the log): deposits they find are discovered, screened and written to the census's watch
  list, and never queued.
* A keyword may appear in only one of `keywords`, `disease_keywords` and `watch_keywords`.

### Consequences

* Good, because disease deposits with nothing to compare against stay out of the queue.
* Bad, because it is a guess from text: cell-line and knockdown controls count as reference groups, and they are not
  healthy tissue.

## More Information

Re-examine when: a question curates these deposits, or results show cell-line comparisons dominating.
