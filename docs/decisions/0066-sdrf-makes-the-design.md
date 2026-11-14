---
title: "0066 PXReprise's SDRF makes the design; the default design retires"
parent: Decisions
nav_order: 66
status: accepted
date: 2026-11-12
decision-makers: the maintainer
---

# PXReprise's SDRF makes the design; the default design retires

## Context and Problem Statement

Under [0031](0031-quantify-inside-metamorpheus.md), a deposit with no usable SDRF was quantified under a default design:
one group, each file its own replicate. Measured on 2026-11-12: 180 of 200 label-free deliveries had used that default.
With an SDRF for every deposit ([0065](0065-sdrf-for-every-deposit.md)), when should it be made, and what should make the
design?

## Considered Options

* Keep the default design where an SDRF gives no usable design
* Make the SDRF before the full download, let it make the design, and retire the default

## Decision Outcome

Chosen option: "Make the SDRF before the full download, let it make the design", because a one-group design cannot
compare anything.

* Every deposit's SDRF is made after its probe and before its full download.
* That SDRF makes the design. The default design retires.
* A deposit whose SDRF gives no usable design waits for a person.
* Measured first: as drafted, 53 of 181 label-free SDRFs gave a usable design under mzLib's `SdrfSearchDesign`. The
  drafter takes a condition only from a claim it rates certain; applying the paid reading's quoted "likely" conditions
  in PXReprise, only where every file has one agreeing value, made 79 usable and broke none.

### Consequences

* Good, because deposits are quantified under the design their samples actually have.
* Bad, because deposits whose SDRF gives no usable design wait for a person instead of being quantified under the
  default.

## More Information

This supersedes the default-design part of [0031](0031-quantify-inside-metamorpheus.md).
