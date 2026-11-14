---
title: "0002 Profiles, questions and one shared corpus"
parent: Decisions
nav_order: 2
status: accepted
date: 2026-09-27
decision-makers: the maintainer
---

# Profiles, questions and one shared corpus

## Context and Problem Statement

The engine must serve any biological question, now and later, without a code change for each one. How should "how data
is searched" be kept apart from "what is being studied"?

## Considered Options

* One configuration per question, holding both its search settings and its scope
* Separate objects: search methods in the engine, questions as data in each question's own project

## Decision Outcome

Chosen option: "Separate objects", because a new question then needs no engine change, and one search can serve every
question.

* **Profile:** how one kind of data is searched. Versioned (`name@N`, see [0007](0007-profile-versions-never-edited.md)).
  Profiles live in this repository, under `profiles/`.
* **Question:** what is studied. It is data (`question.toml`, `decisions.tsv`, `traits/`) kept in the question's own
  project, never code.
* **Corpus:** the searches, shared by every question, keyed by deposit, profile version and engine.
* **Study layer:** a question's traits and analyses.
* Routing sends each deposit to the first profile the question allows that accepts the deposit's acquisition. A deposit
  no allowed profile accepts is recorded as waiting on a capability.
* Per-deposit exceptions (lifted size caps, files included or excluded by name, and later keys) are question data too,
  in `[exceptions]`, and are recorded in provenance. (From entry 28 of the log.)

### Consequences

* Good, because a question is a small text file that a person can review.
* Bad, because a question that needs a new kind of search waits until a profile for it exists.
