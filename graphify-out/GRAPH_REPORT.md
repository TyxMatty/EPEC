# Graph Report - EPEC  (2026-10-07)

## Corpus Check
- cluster-only mode — file stats not available

## Summary
- 1190 nodes · 1627 edges · 100 communities (46 shown, 54 thin omitted)
- Extraction: 98% EXTRACTED · 2% INFERRED · 0% AMBIGUOUS · INFERRED: 27 edges (avg confidence: 0.88)
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `5409371b`
- Run `git rev-parse HEAD` and compare to check if the graph is stale.
- Run `graphify update .` after code changes (no API cost).

## Community Hubs (Navigation)
- manifest.json
- dependencies
- GlobalStateManager
- DialogueSystemGlobal
- TMP_FrameRateCounter
- packages-lock.json
- com.unity.test-framework
- TextMeshProFloatingText
- InteractionSystem
- com.unity.render-pipelines.core
- com.unity.2d.tilemap
- TMP_TextEventHandler
- TMP_TextSelector_B
- Árvore de Diálogos Prólogo Diagram
- TMP_TextInfoDebugTool
- system_collections
- com.unity.settings-manager
- com.unity.modules.imgui
- com.unity.modules.subsystems
- com.unity.modules.jsonserialize
- com.unity.modules.animation
- dependencies
- com.unity.modules.audio
- com.unity.modules.physics
- com.unity.modules.imageconversion
- com.unity.modules.unitywebrequest
- Benchmark03
- CameraController
- ObjectSpin
- unityengine
- TextConsoleSimulator
- TMP_ExampleScript_01
- VertexJitter
- InputRebindManager
- Benchmark01
- TMP_TextEventCheck
- VertexShakeA
- VertexShakeB
- Directional Key Pressed Check
- com.unity.burst
- com.unity.2d.common
- com.unity.2d.sprite
- com.unity.mathematics
- dependencies
- InteractionSystem
- TMPro
- TMP_TextInfoDebugTool.cs
- TMP_TextSelector_A
- VertexZoom
- com.unity.modules.uielements
- com.unity.modules.terrain
- com.unity.ugui
- MovementScript
- Benchmark01_UGUI
- ShaderPropAnimator
- SkewTextExample
- WarpTextExample
- Houve comando de direção?
- ChatController
- EnvMapAnimator
- VertexColorCycler
- SceneFollow
- SpriteChange
- WakeUpPrototype
- TeleType
- UnityEvent
- dependencies
- dependencies
- com.unity.collab-proxy
- com.unity.profiling.core
- Assembly-CSharp.csproj
- CameraFollow
- MonoBehaviour
- Benchmark04.cs
- SimpleScript
- com.unity.modules.particlesystem
- com.unity.modules.tetgen
- com.unity.modules.umbra
- com.unity.modules.wind
- CameraFollow
- ChoiceSystemGlobal
- SceneFollow
- General Choice System Flowchart

## God Nodes (most connected - your core abstractions)
1. `TMP_TextEventHandler` - 31 edges
2. `TMPro.Examples` - 28 edges
3. `DialogueSystemGlobal` - 23 edges
4. `TMP_TextSelector_B` - 22 edges
5. `ChoiceSystemGlobal` - 22 edges
6. `InteractionSystem` - 22 edges
7. `TextMeshProFloatingText` - 19 edges
8. `GlobalStateManager` - 18 edges
9. `TMP_TextInfoDebugTool` - 17 edges
10. `com.unity.modules.jsonserialize` - 16 edges

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

## Communities (100 total, 54 thin omitted)

### Community 0 - "manifest.json"
Cohesion: 0.04
Nodes (54): com.unity.2d.sprite, com.unity.2d.tilemap, com.unity.modules.animation, com.unity.modules.assetbundle, com.unity.modules.audio, com.unity.modules.director, com.unity.modules.imageconversion, com.unity.modules.imgui (+46 more)

### Community 1 - "dependencies"
Cohesion: 0.04
Nodes (55): dependencies, com.unity.2d.animation, com.unity.2d.aseprite, com.unity.2d.psdimporter, com.unity.2d.sprite, com.unity.2d.spriteshape, com.unity.2d.tilemap, com.unity.2d.tilemap.extras (+47 more)

### Community 2 - "GlobalStateManager"
Cohesion: 0.06
Nodes (10): FlowerStateListener, GlobalStateManager, Reputacao, VariavelNarrativa, InventoryData, ItemData, ReputationSystem, GameSaveData (+2 more)

### Community 3 - "DialogueSystemGlobal"
Cohesion: 0.06
Nodes (3): CutsceneManager, ChoiceSystemGlobal, DialogueSystemGlobal

### Community 4 - "TMP_FrameRateCounter"
Cohesion: 0.05
Nodes (18): FpsCounterAnchorPositions, BottomLeft, BottomRight, TopLeft, TopRight, TMP_FrameRateCounter, FpsCounterAnchorPositions, BottomLeft (+10 more)

### Community 5 - "packages-lock.json"
Cohesion: 0.05
Nodes (39): com.unity.2d.sprite, com.unity.2d.tilemap, com.unity.modules.animation, com.unity.modules.assetbundle, com.unity.modules.audio, com.unity.modules.director, com.unity.modules.imageconversion, com.unity.modules.imgui (+31 more)

### Community 6 - "com.unity.test-framework"
Cohesion: 0.06
Nodes (34): dependencies, dependencies, depth, source, version, dependencies, depth, source (+26 more)

### Community 8 - "InteractionSystem"
Cohesion: 0.09
Nodes (4): DialogueTrigger, IInteractAction, InteractionSystem, ItemPickup

### Community 9 - "com.unity.render-pipelines.core"
Cohesion: 0.07
Nodes (30): dependencies, depth, source, version, depth, source, version, dependencies (+22 more)

### Community 10 - "com.unity.2d.tilemap"
Cohesion: 0.08
Nodes (26): dependencies, depth, dependencies, depth, source, url, version, source (+18 more)

### Community 11 - "TMP_TextEventHandler"
Cohesion: 0.13
Nodes (6): TMP_TextEventHandler, onCharacterSelection, onLineSelection, onLinkSelection, onSpriteSelection, onWordSelection

### Community 13 - "Árvore de Diálogos Prólogo Diagram"
Cohesion: 0.10
Nodes (21): acordouPrimeiroDia == false, acordouPrimeiroDia == true, seAlimentou == false, seAlimentou == true, RECEBE CHAMADA DE MARIA, IF: acordouPrimeiroDia == false || respondeuMensagemMaria == false, IF: respondeuChamadaMaria || respondeuMensagemMaria && seAlimentou, Árvore de Diálogos Prólogo Diagram (+13 more)

### Community 16 - "com.unity.settings-manager"
Cohesion: 0.12
Nodes (17): dependencies, depth, source, version, dependencies, depth, source, url (+9 more)

### Community 17 - "com.unity.modules.imgui"
Cohesion: 0.12
Nodes (17): dependencies, depth, source, version, dependencies, depth, source, version (+9 more)

### Community 18 - "com.unity.modules.subsystems"
Cohesion: 0.13
Nodes (15): dependencies, depth, source, version, dependencies, depth, source, version (+7 more)

### Community 19 - "com.unity.modules.jsonserialize"
Cohesion: 0.13
Nodes (15): dependencies, depth, source, version, dependencies, depth, source, version (+7 more)

### Community 20 - "com.unity.modules.animation"
Cohesion: 0.14
Nodes (15): dependencies, depth, source, version, dependencies, depth, source, version (+7 more)

### Community 21 - "dependencies"
Cohesion: 0.14
Nodes (15): dependencies, depth, source, version, dependencies, depth, source, version (+7 more)

### Community 22 - "com.unity.modules.audio"
Cohesion: 0.13
Nodes (15): dependencies, depth, source, version, dependencies, depth, source, version (+7 more)

### Community 23 - "com.unity.modules.physics"
Cohesion: 0.13
Nodes (15): dependencies, depth, source, version, dependencies, depth, source, version (+7 more)

### Community 24 - "com.unity.modules.imageconversion"
Cohesion: 0.13
Nodes (15): dependencies, depth, source, version, dependencies, depth, source, version (+7 more)

### Community 25 - "com.unity.modules.unitywebrequest"
Cohesion: 0.13
Nodes (15): dependencies, depth, source, version, dependencies, depth, source, version (+7 more)

### Community 26 - "Benchmark03"
Cohesion: 0.14
Nodes (7): Benchmark03, BenchmarkType, TEXTMESH_BITMAP, TMP_BITMAP_MOBILE, TMP_SDF, TMP_SDF_MOBILE, TMP_SDF__MOBILE_SSD

### Community 27 - "CameraController"
Cohesion: 0.17
Nodes (5): CameraController, CameraModes, Follow, Free, Isometric

### Community 28 - "ObjectSpin"
Cohesion: 0.15
Nodes (5): MotionType, Rotation, SearchLight, Translation, ObjectSpin

### Community 31 - "TMP_ExampleScript_01"
Cohesion: 0.17
Nodes (4): objectType, TextMeshPro, TextMeshProUGUI, TMP_ExampleScript_01

### Community 38 - "Directional Key Pressed Check"
Cohesion: 0.25
Nodes (11): Store Boolean, Choice 1 (W/Up), Choice 2 (A/Left), Choice 3 (D/Right), Choice 4 (S/Down), Default Neutral Choice, Display Choices on Screen, Read Keyboard Enter (+3 more)

### Community 39 - "com.unity.burst"
Cohesion: 0.20
Nodes (11): dependencies, dependencies, depth, source, version, depth, source, version (+3 more)

### Community 40 - "com.unity.2d.common"
Cohesion: 0.18
Nodes (11): depth, source, url, version, dependencies, depth, source, url (+3 more)

### Community 41 - "com.unity.2d.sprite"
Cohesion: 0.18
Nodes (11): dependencies, depth, source, url, version, dependencies, depth, source (+3 more)

### Community 42 - "com.unity.mathematics"
Cohesion: 0.18
Nodes (11): dependencies, depth, source, url, version, dependencies, depth, source (+3 more)

### Community 43 - "dependencies"
Cohesion: 0.18
Nodes (11): dependencies, depth, source, version, dependencies, depth, source, version (+3 more)

### Community 44 - "InteractionSystem"
Cohesion: 0.33
Nodes (10): DialogueSystemGlobal, Interactable, InteractionSystem, MovementScript, SpriteChange, DialogueSystemGlobal, Interactable, InteractionSystem (+2 more)

### Community 49 - "com.unity.modules.uielements"
Cohesion: 0.20
Nodes (10): dependencies, depth, source, url, version, depth, source, version (+2 more)

### Community 50 - "com.unity.modules.terrain"
Cohesion: 0.20
Nodes (10): dependencies, depth, source, version, dependencies, depth, source, version (+2 more)

### Community 51 - "com.unity.ugui"
Cohesion: 0.20
Nodes (10): depth, source, version, dependencies, depth, source, url, version (+2 more)

### Community 57 - "Houve comando de direção?"
Cohesion: 0.28
Nodes (9): Calcular direção: X, Y, Direção * Vel * DT, End, Houve comando de direção?, Leia Teclado (WASD || ArrowKeys), Realizar movimento, Start, Verificar se colisão (+1 more)

### Community 65 - "UnityEvent"
Cohesion: 0.33
Nodes (5): CharacterSelectionEvent, LineSelectionEvent, LinkSelectionEvent, SpriteSelectionEvent, WordSelectionEvent

### Community 66 - "dependencies"
Cohesion: 0.33
Nodes (6): dependencies, depth, source, url, version, com.unity.2d.animation

### Community 67 - "dependencies"
Cohesion: 0.33
Nodes (6): dependencies, depth, source, url, version, com.unity.2d.aseprite

### Community 68 - "com.unity.collab-proxy"
Cohesion: 0.33
Nodes (6): dependencies, depth, source, url, version, com.unity.collab-proxy

### Community 69 - "com.unity.profiling.core"
Cohesion: 0.33
Nodes (6): dependencies, depth, source, url, version, com.unity.profiling.core

### Community 75 - "com.unity.modules.particlesystem"
Cohesion: 0.40
Nodes (5): dependencies, depth, source, version, com.unity.modules.particlesystem

### Community 76 - "com.unity.modules.tetgen"
Cohesion: 0.40
Nodes (5): dependencies, depth, source, version, com.unity.modules.tetgen

### Community 77 - "com.unity.modules.umbra"
Cohesion: 0.40
Nodes (5): dependencies, depth, source, version, com.unity.modules.umbra

### Community 78 - "com.unity.modules.wind"
Cohesion: 0.40
Nodes (5): dependencies, depth, source, version, com.unity.modules.wind

## Knowledge Gaps
- **458 isolated node(s):** `VertexAnim`, `com.unity.2d.sprite`, `com.unity.2d.tilemap`, `com.unity.modules.animation`, `com.unity.modules.assetbundle` (+453 more)
  These have ≤1 connection - possible missing edges or undocumented components. (Counts symbols only; 675 node(s) total have ≤1 connection when file, concept and rationale nodes are included.)
- **54 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `dependencies` connect `dependencies` to `packages-lock.json`, `com.unity.test-framework`, `com.unity.render-pipelines.core`, `com.unity.2d.tilemap`, `com.unity.settings-manager`, `com.unity.modules.imgui`, `com.unity.modules.subsystems`, `com.unity.modules.jsonserialize`, `com.unity.modules.animation`, `dependencies`, `com.unity.modules.audio`, `com.unity.modules.physics`, `com.unity.modules.imageconversion`, `com.unity.modules.unitywebrequest`, `com.unity.burst`, `com.unity.2d.common`, `com.unity.2d.sprite`, `com.unity.mathematics`, `com.unity.modules.uielements`, `com.unity.modules.terrain`, `com.unity.ugui`, `dependencies`, `dependencies`, `com.unity.collab-proxy`, `com.unity.profiling.core`, `com.unity.modules.particlesystem`, `com.unity.modules.tetgen`, `com.unity.modules.umbra`, `com.unity.modules.wind`?**
  _High betweenness centrality (0.103) - this node is a cross-community bridge._
- **What connects `VertexAnim`, `com.unity.2d.sprite`, `com.unity.2d.tilemap` to the rest of the system?**
  _458 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `manifest.json` be split into smaller, more focused modules?**
  _Cohesion score 0.03636363636363636 - nodes in this community are weakly interconnected._
- **Why does `TMP_TextEventHandler` connect `TMP_TextEventHandler` to `UnityEvent`, `MonoBehaviour`, `TMP_TextSelector_A`, `PointerEventData`, `system_collections`?**
  _High betweenness centrality (0.029) - this node is a cross-community bridge._
- **Should `dependencies` be split into smaller, more focused modules?**
  _Cohesion score 0.03636363636363636 - nodes in this community are weakly interconnected._
- **Why does `TMP_UiFrameRateCounter` connect `TMP_FrameRateCounter` to `MonoBehaviour`?**
  _High betweenness centrality (0.025) - this node is a cross-community bridge._
- **Should `GlobalStateManager` be split into smaller, more focused modules?**
  _Cohesion score 0.06274509803921569 - nodes in this community are weakly interconnected._