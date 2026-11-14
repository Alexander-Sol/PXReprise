---
title: "0041 Public data architecture: add-only bundles, one corpus, a catalog of views"
parent: Decisions
nav_order: 41
status: accepted
date: 2026-10-11
decision-makers: the maintainer
---

# Public data architecture: add-only bundles, one corpus, a catalog of views

## Context and Problem Statement

The data repository first built one materialised artifact per question, rebuilt after every ingest. It did not scale:
82% of 8.06 GB of Parquet was PSMs; the catalog reached 28 GB; a publish grew from 4 to 49 minutes; two publishes crashed
at the final file move (a reader held the file), leaving 64 GB of orphans; and the catalog was never public. How should
results be published?

## Considered Options

* Keep one materialised artifact per question, and make it faster
* Add-only per-deposit bundles shared by every question, with a small catalog of views over them

## Decision Outcome

Chosen option: "Add-only per-deposit bundles", because each deposit's work is then done once and every question shares
it. PXReprise is responsible for the generic pipeline and its architecture; the data repository is one generic component
of it; a question is an instance.

1. The immutable per-deposit Parquet bundle is the unit of publication. Bundles are only ever added.
2. One corpus for every question. Bundles are keyed by deposit and search configuration. A question is a membership
   list plus its own derived results, never its own store, catalog or site.
3. Tiers: answers (protein groups, quantities, PTM sites, samples, definitions, findings, statistics) are served
   everywhere; peptides are queried remotely; PSMs are archived and never in a catalog.
4. The catalog is views plus corpus-wide indexes, rebuilt whole at each publish as `catalog-<id>.duckdb` with a `latest`
   pointer. A file a reader may hold is never replaced.
5. Per-deposit work is done once, at ingest.
6. Publishing runs on a schedule, decoupled from delivery.
7. Static hosting with HTTP range reads and CORS, no servers.
8. Interfaces: Parquet and DuckDB first; then the site, with DuckDB in the browser and `llms.txt`; then MCP as a thin
   local adapter over the remote data.
9. A release id is a hash of its bundle ids and the schema version.
10. Evaluate DuckLake before inventing a snapshot format.

### Consequences

* Good, because a publish no longer rebuilds what has not changed, and readers are never cut off mid-read.
* Bad, because the old per-question model, and publishing after every ingest, had to be replaced.

## More Information

* Rule 7 was later relaxed for interactive use only: a small service may answer MCP over HTTP; the data stays static.
* For now, the second question (type 2 diabetes) runs its own store ([0064](0064-each-question-builds-its-own.md)).
