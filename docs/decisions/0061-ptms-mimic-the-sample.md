---
title: "0061 PTMs in the databases and libraries must mimic the sample"
parent: Decisions
nav_order: 61
status: accepted
date: 2026-11-04
decision-makers: the maintainer
---

# PTMs in the databases and libraries must mimic the sample

## Context and Problem Statement

Too many PTM sites that a sample does not carry turn a search of non-enriched data into a variable-modification search,
with poor sensitivity and specificity. The spectral libraries are meant for label-free, library-based DIA, where every
entry is a candidate. UniProt's own modified-residue annotations already include many sites found only in enrichment
studies. Which PTM sites belong in the regular database and library?

## Considered Options

* Keep every UniProt site, and add the corpus's own
* Keep, per PTM class, only the sites seen in data not enriched for that class

## Decision Outcome

Chosen option: "Keep, per class, only the sites seen in data not enriched for that class", because the database and
library should look like the sample being searched.

* The rule is per class. A GG enrichment's phospho sites may stay in the regular database and library; only its GG sites
  go to a GG library.
* Library registration is routed by class.
* **Adopted on principle** (entry 62 of the log, 2026-11-04). A trial found the regular database neither better nor worse
  (peptides -0.6% to +0.9%, false discovery proportion equal or lower, 1-3% of UniProt-site peptides lost), so the
  principle decided. Every PTM database build from then on runs `tools/ptm-db/BuildPtmDb.cs` with
  `--uniprot-sites regular`: a UniProt modified residue is kept only where a deposit not enriched for its class
  identifies it.
* Enrichment databases and enrichment libraries, for enriched deposits, are planned, with code to route outputs to them.

### Consequences

* Good, because a regular search no longer pays for sites its sample does not carry.
* Bad, because a few true UniProt sites that no non-enriched deposit has seen yet are left out until one does.
