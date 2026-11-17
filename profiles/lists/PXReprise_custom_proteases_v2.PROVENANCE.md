# PXReprise_custom_proteases_v2.tsv

- **What:** v1's list plus one entry (v0.4.50), in the same columns as mzLib's `proteases.tsv`. PXReprise merges them into
  mzLib's `ProteaseDictionary` when the profiles load, and installs the ones a search uses into MetaMorpheus's settings
  folder as `proteases_custom.tsv`. MetaMorpheus reads that file at start (`GlobalVariables.LoadDigestionAgents`, since
  1.1.10), and a per-file `DigestionAgent` resolves through the same dictionary (`FileSpecificParameters`).
- **`trypsin+chymotrypsin`:** as in v1 (`PXReprise_custom_proteases_v1.PROVENANCE.md`): the same entry, byte for byte, so
  a settings folder that already holds it is left as it is.
- **`trypsin+Glu-C`:** cuts after K, R and E.
  - **For:** one digest by Lys-C, trypsin and Glu-C together, first PXD031782 (mouse articular cartilage; aging PXR-A63,
    PXR-A148). Its protocol: in-gel "digestion by a combination of LysC/trypsin/Glu-C enzymes in 25 mM ammonium
    bicarbonate". Its 12 raw files all come from that digest, one per mouse.
  - **E only, not D:** in ammonium bicarbonate Glu-C cuts after E; it cuts after D as well only in phosphate buffer.
    mzLib's `Glu-C` is `E|`, its `Glu-C (with asp)` `E|,D|`.
  - **Lys-C adds nothing:** trypsin already cuts after K.
  - **Proline:** no part has a proline rule. The trypsin part is plain `trypsin`'s, as in `trypsin+chymotrypsin`; the Glu-C
    part is mzLib's `Glu-C`.
  - **Evidence so far:** the protocol. Searched as trypsin only (2026-10-04), PXD031782 gave 60,442 PSMs. In such a digest
    roughly three peptides in five have an E end (E is about 7% of residues, K and R together about 11%), and a trypsin
    search cannot match them. The first search with this entry is its measurement.
- **Named by:** a question's SDRF, either `NT=trypsin+Glu-C` or trypsin and Glu-C (Lys-C beside them changes nothing) in
  one file's `comment[cleavage agent details]` columns.
- **Rule:** never edit an entry in place; searches ran against it. A changed list ships as `_v3.tsv`.
