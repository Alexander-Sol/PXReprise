---
title: "0008 No Python"
parent: Decisions
nav_order: 8
status: accepted
date: 2026-09-29
decision-makers: the maintainer
---

# No Python

## Context and Problem Statement

Someone who installs PXReprise should not need a particular version of Python, and the pipeline's results should not
depend on a Python environment that can drift. Where may Python be used?

## Considered Options

* Python where convenient, with a pinned environment
* No Python on a user's path; Python only where another project owns a tool, and only as an option
* No Python for anything important at all

## Decision Outcome

Chosen option: first "No Python on a user's path" (2026-09-29), then "No Python for anything important" (2026-09-30,
entry 12 of the log), because an install should need only .NET, git and MetaMorpheus.

* A new user needs .NET, git and MetaMorpheus. Nothing else.
* Nothing important is done in Python: nothing that feeds the public site, a figure or a paper.
* New scripts are C#, run as `dotnet run script.cs` (with `#:package` lines for their packages).
* When the data repository's code moved into this repository ([0069](0069-datarepo-code-in-pxreprise.md)), its
  Python stayed behind.

### Consequences

* Good, because one runtime covers the engine, its tools and the data repository.
* Bad, because existing Python tools had to be ported, and some libraries have no .NET equivalent.
