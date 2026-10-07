# Graph Report - EPEC  (2026-10-06)

## Corpus Check
- cluster-only mode — file stats not available

## Summary
- 1179 nodes · 1606 edges · 92 communities (49 shown, 43 thin omitted)
- Extraction: 98% EXTRACTED · 2% INFERRED · 0% AMBIGUOUS · INFERRED: 26 edges (avg confidence: 0.88)
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `5409371b`
- Run `git rev-parse HEAD` and compare to check if the graph is stale.
- Run `graphify update .` after code changes (no API cost).

## Community Hubs (Navigation)
- unityengine
- manifest.json
- dependencies
- DialogueSystemGlobal
- com.unity.modules.imageconversion
- TMP_FrameRateCounter
- GlobalStateManager
- packages-lock.json
- com.unity.2d.tilemap
- TextMeshProFloatingText
- TMP_TextSelector_B
- TMP_TextEventHandler
- com.unity.burst
- com.unity.modules.physics
- InteractionSystem
- Árvore de Diálogos Prólogo Diagram
- com.unity.modules.audio
- TMP_TextInfoDebugTool
- com.unity.settings-manager
- com.unity.searcher
- dependencies
- MovementScript
- com.unity.modules.jsonserialize
- dependencies
- com.unity.modules.unitywebrequest
- com.unity.render-pipelines.core
- Benchmark03
- CameraController
- ObjectSpin
- TMP_ExampleScript_01
- com.unity.ext.nunit
- TextConsoleSimulator
- VertexJitter
- Directional Key Pressed Check
- InteractionSystem
- Benchmark01
- TMP_TextEventCheck
- VertexShakeA
- VertexShakeB
- VertexZoom
- InputRebindManager
- Benchmark01_UGUI
- Houve comando de direção?
- ShaderPropAnimator
- SkewTextExample
- WarpTextExample
- MonoBehaviour
- ChatController
- EnvMapAnimator
- SceneFollow
- VertexColorCycler
- dependencies
- dependencies
- com.unity.2d.psdimporter
- com.unity.2d.spriteshape
- com.unity.2d.tooling
- com.unity.collab-proxy
- com.unity.profiling.core
- Assembly-CSharp.csproj
- CameraFollow
- WakeUpPrototype
- TeleType
- com.unity.collections
- com.unity.2d.common
- com.unity.2d.sprite
- com.unity.mathematics
- com.unity.modules.accessibility
- com.unity.modules.ai
- com.unity.modules.animation
- com.unity.modules.tetgen
- com.unity.modules.umbra
- com.unity.modules.video
- com.unity.modules.wind
- PostProcessingManager
- Benchmark04
- DropdownSample
- SimpleScript
- CameraFollow
- ChoiceSystemGlobal
- SceneFollow
- General Choice System Flowchart

## God Nodes (most connected - your core abstractions)
1. `TMP_TextEventHandler` - 31 edges
2. `TMPro.Examples` - 28 edges
3. `DialogueSystemGlobal` - 23 edges
4. `TMP_TextSelector_B` - 22 edges
5. `InteractionSystem` - 22 edges
6. `ChoiceSystemGlobal` - 22 edges
7. `TextMeshProFloatingText` - 19 edges
8. `TMP_TextInfoDebugTool` - 17 edges
9. `com.unity.modules.jsonserialize` - 16 edges
10. `GlobalStateManager` - 15 edges

## Surprising Connections (you probably didn't know these)
- `DialogueSystemGlobal` --semantically_similar_to--> `DialogueSystemGlobal`  [INFERRED] [semantically similar]
  Assets/Scripts/README.md → Docs/README.md
- `Interactable` --semantically_similar_to--> `Interactable`  [INFERRED] [semantically similar]
  Assets/Scripts/README.md → Docs/README.md
- `InteractionSystem` --semantically_similar_to--> `InteractionSystem`  [INFERRED] [semantically similar]
  Assets/Scripts/README.md → Docs/README.md
- `MovementScript` --semantically_similar_to--> `MovementScript`  [INFERRED] [semantically similar]
  Assets/Scripts/README.md → Docs/README.md
- `SpriteChange` --semantically_similar_to--> `SpriteChange`  [INFERRED] [semantically similar]
  Assets/Scripts/README.md → Docs/README.md

## Import Cycles
- None detected.

## Hyperedges (group relationships)
- **Interaction Core Flow** — assets_scripts_readme_interactionsystem, assets_scripts_readme_dialoguesystemglobal, assets_scripts_readme_interactable, assets_scripts_readme_movementscript [EXTRACTED 1.00]
- **Available Choices** — docs_fluxogramas_epec_escolhas_png_escolha_1, docs_fluxogramas_epec_escolhas_png_escolha_2, docs_fluxogramas_epec_escolhas_png_escolha_3, docs_fluxogramas_epec_escolhas_png_escolha_4 [EXTRACTED 1.00]
- **Interaction Core Flow** — docs_readme_interactionsystem, docs_readme_dialoguesystemglobal, docs_readme_interactable, docs_readme_movementscript [EXTRACTED 1.00]

## Communities (92 total, 43 thin omitted)

### Community 0 - "unityengine"
Cohesion: 0.07
Nodes (4): TMP_DigitValidator, TMP_PhoneNumberValidator, TMPro.Examples, TMPro

### Community 1 - "manifest.json"
Cohesion: 0.04
Nodes (54): com.unity.2d.sprite, com.unity.2d.tilemap, com.unity.modules.animation, com.unity.modules.assetbundle, com.unity.modules.audio, com.unity.modules.director, com.unity.modules.imageconversion, com.unity.modules.imgui (+46 more)

### Community 2 - "dependencies"
Cohesion: 0.04
Nodes (55): dependencies, com.unity.2d.animation, com.unity.2d.aseprite, com.unity.2d.psdimporter, com.unity.2d.sprite, com.unity.2d.spriteshape, com.unity.2d.tilemap, com.unity.2d.tilemap.extras (+47 more)

### Community 3 - "DialogueSystemGlobal"
Cohesion: 0.06
Nodes (5): CutsceneManager, ChoiceSystemGlobal, DialogueData, DialogueSystemGlobal, DialogueTrigger

### Community 4 - "com.unity.modules.imageconversion"
Cohesion: 0.04
Nodes (46): dependencies, depth, source, url, version, dependencies, depth, source (+38 more)

### Community 5 - "TMP_FrameRateCounter"
Cohesion: 0.06
Nodes (18): FpsCounterAnchorPositions, BottomLeft, BottomRight, TopLeft, TopRight, TMP_FrameRateCounter, FpsCounterAnchorPositions, BottomLeft (+10 more)

### Community 6 - "GlobalStateManager"
Cohesion: 0.07
Nodes (9): GlobalStateManager, Reputacao, VariavelNarrativa, InventoryData, ItemData, ReputationSystem, GameSaveData, SaveManager (+1 more)

### Community 7 - "packages-lock.json"
Cohesion: 0.05
Nodes (39): com.unity.2d.sprite, com.unity.2d.tilemap, com.unity.modules.animation, com.unity.modules.assetbundle, com.unity.modules.audio, com.unity.modules.director, com.unity.modules.imageconversion, com.unity.modules.imgui (+31 more)

### Community 8 - "com.unity.2d.tilemap"
Cohesion: 0.06
Nodes (37): dependencies, depth, dependencies, depth, source, url, version, source (+29 more)

### Community 9 - "TextMeshProFloatingText"
Cohesion: 0.08
Nodes (3): Benchmark02, TextMeshProFloatingText, TextMeshSpawner

### Community 11 - "TMP_TextEventHandler"
Cohesion: 0.09
Nodes (11): CharacterSelectionEvent, LineSelectionEvent, LinkSelectionEvent, SpriteSelectionEvent, TMP_TextEventHandler, onCharacterSelection, onLineSelection, onLinkSelection (+3 more)

### Community 12 - "com.unity.burst"
Cohesion: 0.08
Nodes (27): dependencies, depth, source, version, dependencies, dependencies, depth, source (+19 more)

### Community 13 - "com.unity.modules.physics"
Cohesion: 0.08
Nodes (25): dependencies, depth, source, version, dependencies, depth, source, version (+17 more)

### Community 14 - "InteractionSystem"
Cohesion: 0.12
Nodes (3): IInteractAction, InteractionSystem, ItemPickup

### Community 15 - "Árvore de Diálogos Prólogo Diagram"
Cohesion: 0.10
Nodes (21): acordouPrimeiroDia == false, acordouPrimeiroDia == true, seAlimentou == false, seAlimentou == true, RECEBE CHAMADA DE MARIA, IF: acordouPrimeiroDia == false || respondeuMensagemMaria == false, IF: respondeuChamadaMaria || respondeuMensagemMaria && seAlimentou, Árvore de Diálogos Prólogo Diagram (+13 more)

### Community 16 - "com.unity.modules.audio"
Cohesion: 0.11
Nodes (20): dependencies, depth, source, version, dependencies, depth, source, version (+12 more)

### Community 18 - "com.unity.settings-manager"
Cohesion: 0.12
Nodes (17): dependencies, depth, source, version, dependencies, depth, source, url (+9 more)

### Community 19 - "com.unity.searcher"
Cohesion: 0.12
Nodes (16): dependencies, depth, source, version, dependencies, depth, source, url (+8 more)

### Community 20 - "dependencies"
Cohesion: 0.14
Nodes (16): dependencies, depth, source, version, dependencies, depth, source, version (+8 more)

### Community 22 - "com.unity.modules.jsonserialize"
Cohesion: 0.13
Nodes (15): dependencies, depth, source, version, dependencies, depth, source, version (+7 more)

### Community 23 - "dependencies"
Cohesion: 0.14
Nodes (15): dependencies, depth, source, version, dependencies, depth, source, version (+7 more)

### Community 24 - "com.unity.modules.unitywebrequest"
Cohesion: 0.13
Nodes (15): dependencies, depth, source, version, dependencies, depth, source, version (+7 more)

### Community 25 - "com.unity.render-pipelines.core"
Cohesion: 0.14
Nodes (15): dependencies, depth, source, version, dependencies, depth, source, version (+7 more)

### Community 26 - "Benchmark03"
Cohesion: 0.14
Nodes (7): Benchmark03, BenchmarkType, TEXTMESH_BITMAP, TMP_BITMAP_MOBILE, TMP_SDF, TMP_SDF_MOBILE, TMP_SDF__MOBILE_SSD

### Community 27 - "CameraController"
Cohesion: 0.18
Nodes (5): CameraController, CameraModes, Follow, Free, Isometric

### Community 28 - "ObjectSpin"
Cohesion: 0.17
Nodes (5): MotionType, Rotation, SearchLight, Translation, ObjectSpin

### Community 29 - "TMP_ExampleScript_01"
Cohesion: 0.17
Nodes (4): objectType, TextMeshPro, TextMeshProUGUI, TMP_ExampleScript_01

### Community 30 - "com.unity.ext.nunit"
Cohesion: 0.17
Nodes (12): dependencies, depth, source, version, dependencies, depth, source, url (+4 more)

### Community 33 - "Directional Key Pressed Check"
Cohesion: 0.25
Nodes (11): Store Boolean, Choice 1 (W/Up), Choice 2 (A/Left), Choice 3 (D/Right), Choice 4 (S/Down), Default Neutral Choice, Display Choices on Screen, Read Keyboard Enter (+3 more)

### Community 34 - "InteractionSystem"
Cohesion: 0.33
Nodes (10): DialogueSystemGlobal, Interactable, InteractionSystem, MovementScript, SpriteChange, DialogueSystemGlobal, Interactable, InteractionSystem (+2 more)

### Community 42 - "Houve comando de direção?"
Cohesion: 0.28
Nodes (9): Calcular direção: X, Y, Direção * Vel * DT, End, Houve comando de direção?, Leia Teclado (WASD || ArrowKeys), Realizar movimento, Start, Verificar se colisão (+1 more)

### Community 51 - "dependencies"
Cohesion: 0.33
Nodes (6): dependencies, depth, source, url, version, com.unity.2d.animation

### Community 52 - "dependencies"
Cohesion: 0.33
Nodes (6): dependencies, depth, source, url, version, com.unity.2d.aseprite

### Community 53 - "com.unity.2d.psdimporter"
Cohesion: 0.33
Nodes (6): dependencies, depth, source, url, version, com.unity.2d.psdimporter

### Community 54 - "com.unity.2d.spriteshape"
Cohesion: 0.33
Nodes (6): dependencies, depth, source, url, version, com.unity.2d.spriteshape

### Community 55 - "com.unity.2d.tooling"
Cohesion: 0.33
Nodes (6): dependencies, depth, source, url, version, com.unity.2d.tooling

### Community 56 - "com.unity.collab-proxy"
Cohesion: 0.33
Nodes (6): dependencies, depth, source, url, version, com.unity.collab-proxy

### Community 57 - "com.unity.profiling.core"
Cohesion: 0.33
Nodes (6): dependencies, depth, source, url, version, com.unity.profiling.core

### Community 62 - "com.unity.collections"
Cohesion: 0.40
Nodes (5): dependencies, depth, source, version, com.unity.collections

### Community 63 - "com.unity.2d.common"
Cohesion: 0.40
Nodes (5): depth, source, url, version, com.unity.2d.common

### Community 64 - "com.unity.2d.sprite"
Cohesion: 0.40
Nodes (5): dependencies, depth, source, version, com.unity.2d.sprite

### Community 65 - "com.unity.mathematics"
Cohesion: 0.40
Nodes (5): dependencies, depth, source, version, com.unity.mathematics

### Community 66 - "com.unity.modules.accessibility"
Cohesion: 0.40
Nodes (5): dependencies, depth, source, version, com.unity.modules.accessibility

### Community 67 - "com.unity.modules.ai"
Cohesion: 0.40
Nodes (5): dependencies, depth, source, version, com.unity.modules.ai

### Community 68 - "com.unity.modules.animation"
Cohesion: 0.40
Nodes (5): dependencies, depth, source, version, com.unity.modules.animation

### Community 69 - "com.unity.modules.tetgen"
Cohesion: 0.40
Nodes (5): dependencies, depth, source, version, com.unity.modules.tetgen

### Community 70 - "com.unity.modules.umbra"
Cohesion: 0.40
Nodes (5): dependencies, depth, source, version, com.unity.modules.umbra

### Community 71 - "com.unity.modules.video"
Cohesion: 0.40
Nodes (5): dependencies, depth, source, version, com.unity.modules.video

### Community 72 - "com.unity.modules.wind"
Cohesion: 0.40
Nodes (5): dependencies, depth, source, version, com.unity.modules.wind

## Knowledge Gaps
- **458 isolated node(s):** `VertexAnim`, `com.unity.2d.sprite`, `com.unity.2d.tilemap`, `com.unity.modules.animation`, `com.unity.modules.assetbundle` (+453 more)
  These have ≤1 connection - possible missing edges or undocumented components. (Counts symbols only; 676 node(s) total have ≤1 connection when file, concept and rationale nodes are included.)
- **43 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `dependencies` connect `dependencies` to `com.unity.modules.imageconversion`, `packages-lock.json`, `com.unity.2d.tilemap`, `com.unity.burst`, `com.unity.modules.physics`, `com.unity.modules.audio`, `com.unity.settings-manager`, `com.unity.searcher`, `com.unity.modules.jsonserialize`, `dependencies`, `com.unity.modules.unitywebrequest`, `com.unity.render-pipelines.core`, `com.unity.ext.nunit`, `dependencies`, `dependencies`, `com.unity.2d.psdimporter`, `com.unity.2d.spriteshape`, `com.unity.2d.tooling`, `com.unity.collab-proxy`, `com.unity.profiling.core`, `com.unity.collections`, `com.unity.2d.common`, `com.unity.2d.sprite`, `com.unity.mathematics`, `com.unity.modules.accessibility`, `com.unity.modules.ai`, `com.unity.modules.animation`, `com.unity.modules.tetgen`, `com.unity.modules.umbra`, `com.unity.modules.video`, `com.unity.modules.wind`?**
  _High betweenness centrality (0.095) - this node is a cross-community bridge._
- **What connects `VertexAnim`, `com.unity.2d.sprite`, `com.unity.2d.tilemap` to the rest of the system?**
  _458 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `unityengine` be split into smaller, more focused modules?**
  _Cohesion score 0.07475678443420379 - nodes in this community are weakly interconnected._
- **Why does `TMP_TextEventHandler` connect `TMP_TextEventHandler` to `unityengine`, `TMP_TextSelector_B`, `MonoBehaviour`?**
  _High betweenness centrality (0.027) - this node is a cross-community bridge._
- **Should `manifest.json` be split into smaller, more focused modules?**
  _Cohesion score 0.03636363636363636 - nodes in this community are weakly interconnected._
- **Why does `ChoiceSystemGlobal` connect `DialogueSystemGlobal` to `unityengine`, `MonoBehaviour`, `GlobalStateManager`?**
  _High betweenness centrality (0.023) - this node is a cross-community bridge._
- **Should `dependencies` be split into smaller, more focused modules?**
  _Cohesion score 0.03636363636363636 - nodes in this community are weakly interconnected._