---
title: "0014 A new search engine joins as a MetaMorpheus task and a profile"
parent: Decisions
nav_order: 14
status: accepted
date: 2026-09-30
decision-makers: the maintainer
---

# A new search engine joins as a MetaMorpheus task and a profile

## Context and Problem Statement

Other search engines (a DIA engine was the first proposed) could add kinds of data PXReprise cannot search today. How
should a new engine plug in?

## Considered Options

* Each engine as its own command-line program, which PXReprise calls
* Each engine as a task in a released MetaMorpheus command line, plus a PXReprise profile that names it

## Decision Outcome

Chosen option: "A task in a released MetaMorpheus, plus a profile", because PXReprise then keeps one way to run a
search, one provenance shape and one place for settings ([0001](0001-csharp-orchestration-only.md),
[0007](0007-profile-versions-never-edited.md)).

### Consequences

* Good, because a new engine gets calibration, file readers and output tables that already work.
* Bad, because a new engine waits for a MetaMorpheus release before PXReprise can use it.

## More Information

The same log entry kept an optional model-based reader of papers off in PXReprise's runs. That part is superseded by
[0065](0065-sdrf-for-every-deposit.md): the paid evidence reading is a machine option, on where a key is configured.
