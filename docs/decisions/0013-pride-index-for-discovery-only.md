---
title: "0013 PRIDE's search index is for discovery only"
parent: Decisions
nav_order: 13
status: accepted
date: 2026-09-30
decision-makers: the maintainer
---

# PRIDE's search index is for discovery only

## Context and Problem Statement

PRIDE offers a search index and, for each deposit, a project record. The index's organism, organism-part and disease
fields can hold SDRF column headers (for instance `Characteristics[organism]`) instead of values. The census filtered and
queued deposits on those fields. Which source should the engine act on?

## Considered Options

* The search index, cleaned of header text
* The search index to find candidates, and the project record for every value acted on

## Decision Outcome

Chosen option: "The search index to find candidates, and the project record for every value acted on", because the
project record is what the depositor submitted.

* The census reads each relevant deposit's project record (mzLib's `TryGetProjectAsync`) and filters and queues on its
  organisms.
* With no project record, it falls back to the index's organisms minus SDRF headers, and marks the deposit
  `search_fallback`.

### Consequences

* Good, because a header string can no longer be mistaken for an organism.
* Bad, because the census makes one more request per relevant deposit.
