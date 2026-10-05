# Graph Report - EPEC  (2026-10-05)

## Corpus Check
- 94 files · ~111,877 words
- Verdict: corpus is large enough that graph structure adds value.
- Unclassified: 406 file(s) not represented in the graph (top: .meta 241, .asset 54, .unity 35)

## Summary
- 1082 nodes · 1470 edges · 77 communities (46 shown, 31 thin omitted)
- Extraction: 99% EXTRACTED · 1% INFERRED · 0% AMBIGUOUS · INFERRED: 19 edges (avg confidence: 0.9)
- Token cost: 0 input · 0 output

## Community Hubs (Navigation)
- Ref Modules
- Manifest Dependencies
- Examples Textmesh
- Lock Modules
- Ref Lock
- Lock Modules
- Lock Dependencies
- Tmp Examples
- Tmp Texteventhandler
- Examples Textmesh
- Lock Tilemap
- Lock Modules
- Arvorededialogosprologo False
- Tmp Examples
- Lock Dependencies
- Inventorydata Globalstatemanager
- Examples Textmesh
- Lock Dependencies
- Scenefollow Camerafollow
- Dialoguesystemglobal Choicesystemglobal
- Examples Benchmark03
- Tmp Textmesh
- Lock Render
- Choicesystemglobal Dialoguesystemglobal
- Interactionsystem Gameobject
- Examples Cameracontroller
- Examples Objectspin
- Movementscript Spritechange
- Examples Tmp
- Examples Vertexjitter
- Examples Benchmark01
- Examples Textconsolesimulator
- Tmp Examples
- Examples Vertexshakea
- Examples Vertexshakeb
- Escolhas Fluxogramas
- Lock Animation
- Lock Dependencies
- Lock Tooling
- Lock Psdimporter
- Lock Spriteshape
- Lock Modules
- Dialoguesystemglobal Interactable
- Examples Vertexzoom
- Lock Modules
- Lock Modules
- Lock Modules
- Lock Modules
- Lock Modules
- Examples Benchmark01
- Examples Shaderpropanimator
- Examples Skewtextexample
- Examples Warptextexample
- Movimento Fluxogramas
- Envmapanimator Textmesh
- Chatcontroller Textmesh
- Examples Vertexcolorcycler
- Fluxogramas Epec
- Assembly Csharp
- Examples Teletype
- Lock Aseprite
- Lock Collab
- Lock Profiling
- Examples Benchmark04
- Dropdownsample Textmesh
- Examples Simplescript
- Lock Modules
- Lock Modules
- Lock Modules
- Lock Modules
- Lock Modules
- Camerafollow
- Choicesystemglobal
- Scenefollow
- Fluxogramas Epec

## God Nodes (most connected - your core abstractions)
1. `TMP_TextEventHandler` - 31 edges
2. `TMPro.Examples` - 28 edges
3. `TMP_TextSelector_B` - 22 edges
4. `DialogueSystemGlobal` - 19 edges
5. `TextMeshProFloatingText` - 19 edges
6. `ChoiceSystemGlobal` - 17 edges
7. `TMP_TextInfoDebugTool` - 17 edges
8. `com.unity.modules.jsonserialize` - 16 edges
9. `InteractionSystem` - 14 edges
10. `MovementScript` - 11 edges

## Surprising Connections (you probably didn't know these)
- `MovementScript` --semantically_similar_to--> `MovementScript`  [INFERRED] [semantically similar]
  Assets/Scripts/README.md → Docs/README.md
- `SpriteChange` --semantically_similar_to--> `SpriteChange`  [INFERRED] [semantically similar]
  Assets/Scripts/README.md → Docs/README.md
- `CameraFollow` --semantically_similar_to--> `CameraFollow`  [INFERRED] [semantically similar]
  Assets/Scripts/README.md → Docs/README.md
- `SceneFollow` --semantically_similar_to--> `SceneFollow`  [INFERRED] [semantically similar]
  Assets/Scripts/README.md → Docs/README.md
- `Interactable` --semantically_similar_to--> `Interactable`  [INFERRED] [semantically similar]
  Assets/Scripts/README.md → Docs/README.md

## Import Cycles
- None detected.

## Hyperedges (group relationships)
- **Interaction Core Flow** — assets_scripts_readme_interactionsystem, assets_scripts_readme_dialoguesystemglobal, assets_scripts_readme_interactable, assets_scripts_readme_movementscript [EXTRACTED 1.00]
- **Interaction Core Flow** — docs_readme_interactionsystem, docs_readme_dialoguesystemglobal, docs_readme_interactable, docs_readme_movementscript [EXTRACTED 1.00]
- **Available Choices** — docs_fluxogramas_epec_escolhas_png_escolha_1, docs_fluxogramas_epec_escolhas_png_escolha_2, docs_fluxogramas_epec_escolhas_png_escolha_3, docs_fluxogramas_epec_escolhas_png_escolha_4 [EXTRACTED 1.00]
- **Player Interaction Flow** — docs_fluxogramas_epec_interao_check_proximity, docs_fluxogramas_epec_interao_check_looking_at, docs_fluxogramas_epec_interao_show_interaction_prompt, docs_fluxogramas_epec_interao_check_input, docs_fluxogramas_epec_interao_start_interaction, docs_fluxogramas_epec_interao_cancel_interaction [EXTRACTED 1.00]

## Communities (77 total, 31 thin omitted)

### Community 0 - "Ref Modules"
Cohesion: 0.04
Nodes (54): com.unity.2d.sprite, com.unity.2d.tilemap, com.unity.modules.animation, com.unity.modules.assetbundle, com.unity.modules.audio, com.unity.modules.director, com.unity.modules.imageconversion, com.unity.modules.imgui (+46 more)

### Community 1 - "Manifest Dependencies"
Cohesion: 0.04
Nodes (55): dependencies, com.unity.2d.animation, com.unity.2d.aseprite, com.unity.2d.psdimporter, com.unity.2d.sprite, com.unity.2d.spriteshape, com.unity.2d.tilemap, com.unity.2d.tilemap.extras (+47 more)

### Community 2 - "Examples Textmesh"
Cohesion: 0.05
Nodes (18): FpsCounterAnchorPositions, BottomLeft, BottomRight, TopLeft, TopRight, TMP_FrameRateCounter, FpsCounterAnchorPositions, BottomLeft (+10 more)

### Community 3 - "Lock Modules"
Cohesion: 0.04
Nodes (47): dependencies, depth, source, url, version, dependencies, depth, source (+39 more)

### Community 4 - "Ref Lock"
Cohesion: 0.05
Nodes (39): com.unity.2d.sprite, com.unity.2d.tilemap, com.unity.modules.animation, com.unity.modules.assetbundle, com.unity.modules.audio, com.unity.modules.director, com.unity.modules.imageconversion, com.unity.modules.imgui (+31 more)

### Community 5 - "Lock Modules"
Cohesion: 0.06
Nodes (35): dependencies, depth, source, version, dependencies, depth, source, version (+27 more)

### Community 6 - "Lock Dependencies"
Cohesion: 0.06
Nodes (34): dependencies, dependencies, depth, source, version, dependencies, depth, source (+26 more)

### Community 8 - "Tmp Texteventhandler"
Cohesion: 0.09
Nodes (11): CharacterSelectionEvent, LineSelectionEvent, LinkSelectionEvent, SpriteSelectionEvent, TMP_TextEventHandler, onCharacterSelection, onLineSelection, onLinkSelection (+3 more)

### Community 10 - "Lock Tilemap"
Cohesion: 0.08
Nodes (26): dependencies, depth, dependencies, depth, source, url, version, source (+18 more)

### Community 11 - "Lock Modules"
Cohesion: 0.08
Nodes (25): dependencies, depth, source, version, dependencies, depth, source, version (+17 more)

### Community 12 - "Arvorededialogosprologo False"
Cohesion: 0.10
Nodes (21): acordouPrimeiroDia == false, acordouPrimeiroDia == true, seAlimentou == false, seAlimentou == true, RECEBE CHAMADA DE MARIA, IF: acordouPrimeiroDia == false || respondeuMensagemMaria == false, IF: respondeuChamadaMaria || respondeuMensagemMaria && seAlimentou, Árvore de Diálogos Prólogo Diagram (+13 more)

### Community 14 - "Lock Dependencies"
Cohesion: 0.12
Nodes (17): dependencies, depth, source, version, dependencies, depth, source, url (+9 more)

### Community 15 - "Inventorydata Globalstatemanager"
Cohesion: 0.16
Nodes (6): GlobalStateManager, VariavelNarrativa, InventoryData, ItemData, Welcome2DScript, Unity.U2D.Welcome

### Community 16 - "Examples Textmesh"
Cohesion: 0.18
Nodes (3): Benchmark02, TextMeshSpawner, TMPro.Examples

### Community 17 - "Lock Dependencies"
Cohesion: 0.12
Nodes (16): dependencies, depth, source, version, dependencies, depth, source, url (+8 more)

### Community 18 - "Scenefollow Camerafollow"
Cohesion: 0.14
Nodes (3): CameraFollow, Interactable, SceneFollow

### Community 20 - "Examples Benchmark03"
Cohesion: 0.14
Nodes (7): Benchmark03, BenchmarkType, TEXTMESH_BITMAP, TMP_BITMAP_MOBILE, TMP_SDF, TMP_SDF_MOBILE, TMP_SDF__MOBILE_SSD

### Community 21 - "Tmp Textmesh"
Cohesion: 0.19
Nodes (3): TMP_DigitValidator, TMP_PhoneNumberValidator, TMPro

### Community 22 - "Lock Render"
Cohesion: 0.15
Nodes (14): depth, source, version, dependencies, depth, source, version, dependencies (+6 more)

### Community 25 - "Examples Cameracontroller"
Cohesion: 0.17
Nodes (5): CameraController, CameraModes, Follow, Free, Isometric

### Community 26 - "Examples Objectspin"
Cohesion: 0.15
Nodes (5): MotionType, Rotation, SearchLight, Translation, ObjectSpin

### Community 28 - "Examples Tmp"
Cohesion: 0.17
Nodes (4): objectType, TextMeshPro, TextMeshProUGUI, TMP_ExampleScript_01

### Community 36 - "Escolhas Fluxogramas"
Cohesion: 0.25
Nodes (11): Store Boolean, Choice 1 (W/Up), Choice 2 (A/Left), Choice 3 (D/Right), Choice 4 (S/Down), Default Neutral Choice, Display Choices on Screen, Read Keyboard Enter (+3 more)

### Community 37 - "Lock Animation"
Cohesion: 0.18
Nodes (11): dependencies, depth, source, url, version, dependencies, depth, source (+3 more)

### Community 38 - "Lock Dependencies"
Cohesion: 0.20
Nodes (11): dependencies, dependencies, depth, source, version, depth, source, version (+3 more)

### Community 39 - "Lock Tooling"
Cohesion: 0.18
Nodes (11): depth, source, url, version, dependencies, depth, source, url (+3 more)

### Community 40 - "Lock Psdimporter"
Cohesion: 0.18
Nodes (11): dependencies, depth, source, url, version, dependencies, depth, source (+3 more)

### Community 41 - "Lock Spriteshape"
Cohesion: 0.18
Nodes (11): dependencies, depth, source, url, version, dependencies, depth, source (+3 more)

### Community 42 - "Lock Modules"
Cohesion: 0.20
Nodes (11): dependencies, depth, source, version, dependencies, depth, source, version (+3 more)

### Community 43 - "Dialoguesystemglobal Interactable"
Cohesion: 0.33
Nodes (10): DialogueSystemGlobal, Interactable, InteractionSystem, MovementScript, SpriteChange, DialogueSystemGlobal, Interactable, InteractionSystem (+2 more)

### Community 45 - "Lock Modules"
Cohesion: 0.20
Nodes (10): dependencies, depth, source, version, dependencies, depth, source, version (+2 more)

### Community 46 - "Lock Modules"
Cohesion: 0.20
Nodes (10): dependencies, depth, source, version, dependencies, depth, source, version (+2 more)

### Community 47 - "Lock Modules"
Cohesion: 0.20
Nodes (10): dependencies, depth, source, version, dependencies, depth, source, version (+2 more)

### Community 48 - "Lock Modules"
Cohesion: 0.20
Nodes (10): dependencies, depth, source, version, dependencies, depth, source, version (+2 more)

### Community 49 - "Lock Modules"
Cohesion: 0.20
Nodes (10): depth, source, version, dependencies, depth, source, version, dependencies (+2 more)

### Community 54 - "Movimento Fluxogramas"
Cohesion: 0.28
Nodes (9): Calcular direção: X, Y, Direção * Vel * DT, End, Houve comando de direção?, Leia Teclado (WASD || ArrowKeys), Realizar movimento, Start, Verificar se colisão (+1 more)

### Community 58 - "Fluxogramas Epec"
Cohesion: 0.43
Nodes (7): Cancelar/Fim da interação, Verificar input (Pressionado?), Verificar se está olhando para o objeto, Verificar proximidade com objeto, Fluxograma de Interação EPEC, Mostrar prompt Pressione Enter para interagir, Iniciar interação

### Community 59 - "Assembly Csharp"
Cohesion: 0.47
Nodes (3): Assembly-CSharp, netstandard2.1, netstandard2.1

### Community 61 - "Lock Aseprite"
Cohesion: 0.33
Nodes (6): dependencies, depth, source, url, version, com.unity.2d.aseprite

### Community 62 - "Lock Collab"
Cohesion: 0.33
Nodes (6): dependencies, depth, source, url, version, com.unity.collab-proxy

### Community 63 - "Lock Profiling"
Cohesion: 0.33
Nodes (6): dependencies, depth, source, url, version, com.unity.profiling.core

### Community 67 - "Lock Modules"
Cohesion: 0.40
Nodes (5): dependencies, depth, source, version, com.unity.modules.accessibility

### Community 68 - "Lock Modules"
Cohesion: 0.40
Nodes (5): dependencies, depth, source, version, com.unity.modules.ai

### Community 69 - "Lock Modules"
Cohesion: 0.40
Nodes (5): dependencies, depth, source, version, com.unity.modules.tetgen

### Community 70 - "Lock Modules"
Cohesion: 0.40
Nodes (5): dependencies, depth, source, version, com.unity.modules.umbra

### Community 71 - "Lock Modules"
Cohesion: 0.40
Nodes (5): dependencies, depth, source, version, com.unity.modules.unitywebrequestaudio

## Knowledge Gaps
- **462 isolated node(s):** `netstandard2.1`, `TMP_SDF_MOBILE`, `TMP_SDF__MOBILE_SSD`, `TMP_SDF`, `TMP_BITMAP_MOBILE` (+457 more)
  These have ≤1 connection - possible missing edges or undocumented components. (Counts symbols only; 633 node(s) total have ≤1 connection when file, concept and rationale nodes are included.)
- **31 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `dependencies` connect `Lock Modules` to `Lock Modules`, `Ref Lock`, `Lock Modules`, `Lock Dependencies`, `Lock Tilemap`, `Lock Modules`, `Lock Dependencies`, `Lock Dependencies`, `Lock Render`, `Lock Animation`, `Lock Dependencies`, `Lock Tooling`, `Lock Psdimporter`, `Lock Spriteshape`, `Lock Modules`, `Lock Modules`, `Lock Modules`, `Lock Modules`, `Lock Modules`, `Lock Aseprite`, `Lock Collab`, `Lock Profiling`, `Lock Modules`, `Lock Modules`, `Lock Modules`, `Lock Modules`, `Lock Modules`?**
  _High betweenness centrality (0.120) - this node is a cross-community bridge._
- **What connects `netstandard2.1`, `TMP_SDF_MOBILE`, `TMP_SDF__MOBILE_SSD` to the rest of the system?**
  _462 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `Ref Modules` be split into smaller, more focused modules?**
  _Cohesion score 0.03636363636363636 - nodes in this community are weakly interconnected._
- **Why does `TMP_TextSelector_B` connect `Tmp Examples` to `Scenefollow Camerafollow`, `Tmp Textmesh`?**
  _High betweenness centrality (0.032) - this node is a cross-community bridge._
- **Should `Manifest Dependencies` be split into smaller, more focused modules?**
  _Cohesion score 0.03636363636363636 - nodes in this community are weakly interconnected._
- **Why does `TMP_TextInfoDebugTool` connect `Tmp Examples` to `Examples Textmesh`, `Scenefollow Camerafollow`?**
  _High betweenness centrality (0.021) - this node is a cross-community bridge._
- **Should `Examples Textmesh` be split into smaller, more focused modules?**
  _Cohesion score 0.0545790934320074 - nodes in this community are weakly interconnected._