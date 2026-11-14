---
title: "0038 Search UniProt's canonical proteomes"
parent: Decisions
nav_order: 38
status: accepted
date: 2026-10-07
decision-makers: the maintainer
---

# Search UniProt's canonical proteomes

## Context and Problem Statement

Searches used UniProt's reviewed entries only (Swiss-Prot). Many genes, especially in rat, have no reviewed entry, so
their peptides could not be identified.

## Considered Options

* Reviewed entries only (Swiss-Prot)
* UniProt's canonical reference proteome: one protein per gene, reviewed and unreviewed
* Unreviewed entries only (TrEMBL)

## Decision Outcome

Chosen option: "UniProt's canonical reference proteome", because it covers every gene once. The evidence: the PAW
pipeline (Wilmarth) uses it, and a rat test gave 15-49% more peptides, with the rat share of identifications rising from
63-88% to 90-95%.

* Every search database is UniProt's canonical reference proteome, one protein per gene (REST
  `proteome:UP... AND is_gene_centric:true`, which equals the FTP `<UP>_<taxon>.xml` exactly). Release 2026_03: human
  20,652 proteins, mouse 21,860, rat 21,471.
* It applies to every organism profile and ships as new profile versions, with the PTM databases rebuilt on it
  ([0029](0029-rebuild-ptm-databases-each-build.md)). Consumers are told before the first canonical search is
  delivered.
* The exact database of every search is recorded, so results on reviewed and canonical databases stay separable.
* Never unreviewed entries alone.
* **No protein is kept apart.** An unreviewed entry is a full member of the database: no special treatment in
  quantification, inference, counts, GO or controls.

### Consequences

* Good, because more of each proteome is searchable.
* Bad, because every deposit searched on a reviewed-only database became due for a re-search
  ([0039](0039-catch-up-re-search.md)).
