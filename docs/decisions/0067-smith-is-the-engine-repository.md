---
title: "0067 This repository is the engine's real home"
parent: Decisions
nav_order: 67
status: accepted
date: 2026-11-13
decision-makers: the maintainer
---

# This repository is the engine's real home

## Context and Problem Statement

Until 2026-11-13 the engine was developed in a private repository, and a script published allowlisted files here as one
snapshot commit per release. A contributor wanted to start contributing. Under the snapshot model, a merged pull request
here would have been undone by the next snapshot, so every contribution had to be ported by hand.

## Considered Options

* Keep the snapshots, and port each contribution to the private repository by hand
* Make this repository the real one, with the engine's real history

## Decision Outcome

Chosen option: "Make this repository the real one", because contributors can then work here as in any repository.

* `master` here is the engine's real history, filtered from the private repository: 260 commits. All 55 release tags
  were moved to their real commits, each checked identical to what it shipped. The old snapshot commits are kept on the
  branch `snapshots`.
* Work from a fork: a branch per change, a pull request here; the maintainer merges.
* The snapshot script is retired.
* The maintainers' project records (plans, conversations with other projects, unpublished results) stay private.

### Consequences

* Good, because contributions are reviewed and merged here, with their history.
* Bad, because two places now hold records: engine decisions here, project records privately
  ([0070](0070-record-engine-decisions-as-madr.md)).

## More Information

This supersedes the earlier model in which this repository received snapshots of a private one.
