<div align="center">

# 🧟 Infected Frontier VR

### A cooperative VR tower-defense shooter inspired by *Orcs Must Die!*

**Defend the portal. Set your traps. Survive the horde.**

[![Unity](https://img.shields.io/badge/Unity-2021.3.10f1-black?logo=unity)](https://unity.com/)
[![Photon PUN 2](https://img.shields.io/badge/Netcode-Photon%20PUN%202-blue?logo=photon)](https://www.photonengine.com/pun)
[![XR Interaction Toolkit](https://img.shields.io/badge/VR-XR%20Interaction%20Toolkit-green?logo=oculus)](https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@2.0/manual/index.html)
[![OpenXR](https://img.shields.io/badge/Platform-OpenXR%20%7C%20Oculus-orange?logo=meta)](https://www.khronos.org/openxr/)
[![License](https://img.shields.io/badge/License-Unlicensed-lightgrey)]()

---

[![Infected Frontier VR – Mechanics Reel](https://img.youtube.com/vi/SIbMcnkYhNk/maxresdefault.jpg)](https://youtu.be/SIbMcnkYhNk "Infected Frontier VR – Watch Mechanics Reel")

▶️ **Click the image above to watch the mechanics & features reel on YouTube**

---

</div>

## 📖 About

**Infected Frontier VR** is a **cooperative multiplayer VR game (1–3 players)** where waves of zombies march toward a portal that players must defend. Inspired by the *Orcs Must Die!* formula, it combines **first-person VR shooting** with **real-time trap placement** across rounds of increasing difficulty.

If the portal's health reaches zero — it's game over for everyone.

This project started as an academic exercise and evolved into a fully playable prototype that tackles some genuinely hard problems: **synchronizing VR interactions over the network**, **physics-based player rigs in multiplayer**, and **real-time trap placement with validation**. It's far from a polished product, but the core systems work and the foundation is solid.

> **Status:** Work In Progress — playable alpha with known rough edges

---

## 🎮 Core Gameplay

| Feature | Description |
|---|---|
| **Wave System** | Progressive rounds with scaling enemy count, health, and enemy-type variety |
| **Trap Placement** | Purchase and deploy **barricades**, **spike wires**, and **explosive mines** in real-time using an in-world economy |
| **Two Enemy Archetypes** | *Runners* — fast, portal-focused; *Fighters* — aggressive, player-targeting with chase AI |
| **Portal Defense** | A shared objective with networked health — every zombie that reaches the portal damages it for all players |
| **VR Weapons** | Physics-based guns with manual reloading, two-handed grip support, and scope mechanics |
| **Economy** | Earn currency by eliminating enemies; spend it on traps via an in-world UI shop |

---

## 🛠️ Technical Highlights

### 🥽 VR Interaction System
- **Physics-based VR rig** with `CapsuleCollider` body tracking, `ConfigurableJoint` hands, and head-tracked collider height
- **Continuous locomotion & snap turn** with physics-driven `Rigidbody` movement (not teleport-only)
- **Climbing system** — velocity-tracked hand movement applied to `CharacterController`
- **Two-handed weapon grip** — custom `XRGrabInteractable` subclass supporting dual-interactor aim (`XRGrabInteractableDoubleGrip`) with dynamic look rotation between hands
- **Hand pose system** — per-object finger bone retargeting via `HandData` / `GrabHandPose` for grab animations
- **Haptic feedback** — serializable haptic profiles per interaction event (hover, grab, activate, release)
- **Secondary actions** — context-sensitive button actions while holding objects (e.g., flashlight toggle)

### 🌐 Multiplayer Networking (Photon PUN 2)
- Full **room-based online multiplayer** for up to 3 concurrent VR players
- **Networked player avatar** — real-time head and hand tracking synchronized via `PhotonView`
- **Networked hand animations** — grip and trigger blendtree values replicated across clients
- **RPC-driven game state** — damage, economy, wave progression, trap placement, and portal health all synchronized via `[PunRPC]` calls
- **Ownership transfer** on grabbable objects — seamless pick-up between players with `RequestOwnership()`
- **Networked grabbables** — custom `XRGrabInteractable` subclasses that disable interaction on remote clients and sync transform via `IPunObservable`

### 🤖 AI & State Machines
- Custom **finite state machine** architecture (`State` → `StateMachine`) — a simple but functional pattern for enemy behavior
- **BaseEnemy (Runner):** `Move → Attack (barricade)` — navigates NavMesh toward the portal, attacks obstacles in its path
- **AttackEnemy (Fighter):** `Move → Chase → Attack` — detects nearby players with `OverlapSphere`, pursues and engages them
- Enemies react to **traps**: spike traps reduce speed and deal DoT, explosive traps deal burst AoE, barricades block paths
- **Ragdoll death** system — toggles `Rigidbody.isKinematic` and disables `Animator`/`NavMeshAgent` on death
- **Wave-synced NavMesh destinations** with randomized spawn points and route variation via `ChangeDestination` triggers
- *The two enemy types currently share a lot of duplicated code — a shared base class is planned to clean this up*

### 🪤 Trap System
- **Ray-based placement** — aim with VR controller, preview holographic trap on valid ground, confirm with trigger
- **Collision validation** — `Physics.CheckBox` prevents overlapping traps and respects obstacle layers
- Three trap types with distinct gameplay roles:
  - 🧱 **Barricade** — blocks enemy paths, has networked destructible health
  - ⚡ **Spike Wire** — area slow + damage-over-time, degrades over time
  - 💣 **Explosive Mine** — triggered on contact, AoE burst damage with VFX

### 🎨 Rendering & VFX
- **Universal Render Pipeline (URP)** optimized for VR performance
- **Particle systems** for muzzle flash, explosions, barricade destruction — all network-synced
- Dynamic **material swapping** for trap placement preview (holographic → solid)

---

## 📁 Project Structure

```
Assets/
├── Scripts/
│   ├── IA/                          # AI state machines, wave manager, lose condition
│   │   ├── State.cs                 # Base state class
│   │   ├── StateMachine.cs          # FSM controller
│   │   ├── BaseEnemy_SM.cs          # Runner enemy (Move → Attack)
│   │   ├── AttackEnemy_SM.cs        # Fighter enemy (Move → Chase → Attack)
│   │   ├── Wave_Manager.cs          # Wave spawning & progression
│   │   ├── LoseCondition.cs         # Portal health & game over
│   │   └── ChangeDestination.cs     # NavMesh route variation triggers
│   ├── NetworkManager.cs            # Photon connection & room setup
│   ├── NetworkPlayer.cs             # Networked VR avatar (head + hands)
│   ├── NetworkPlayerSpawning.cs     # Player instantiation on join
│   ├── ShotOnActivate.cs            # Weapon firing, ammo & reload
│   ├── BulletBehaviour.cs           # Projectile physics & hit detection
│   ├── EconomyManager.cs            # Currency earn/spend system
│   ├── TrapPlacer.cs                # VR ray-based trap placement
│   ├── Trap.cs                      # Trap base (hologram preview → place)
│   ├── BarricateBehaviour.cs        # Destructible barricade logic
│   ├── SpikeTrapBehaviour.cs        # Slow + DoT trap
│   ├── ExplosionTrapBehaviour.cs    # AoE burst trap
│   ├── PlayerLifeManager.cs         # Player health, death & respawn
│   ├── Climber.cs / ClimbInteractable.cs  # VR climbing system
│   ├── ContinuousMovementPhysics.cs # Physics-based VR locomotion
│   ├── PhysicVRRig.cs               # Physics body rig (joints + colliders)
│   ├── XRGrabInteractableTwoHands.cs      # Networked single-hand grab
│   ├── XRGrabInteractableDoubleGrip.cs    # Networked two-hand weapon grip
│   ├── XRGrabNetworkInteractable.cs       # Base networked grabbable
│   ├── GrabHandPose.cs / HandData.cs      # Per-object hand pose system
│   ├── HapticInteraction.cs         # Configurable haptic feedback
│   └── ...                          # UI, menus, flashlight, etc.
├── Prefabs/
│   ├── Zombie_Runner.prefab         # Runner enemy prefab
│   ├── Zombie_Fighter.prefab        # Fighter enemy prefab
│   ├── Gun.prefab / Sniper.prefab   # VR weapon prefabs
│   ├── Traps/                       # Trap prefabs
│   └── Network Prefabs/             # Photon-instantiated prefabs
├── Scenes/
│   ├── MenuScene.unity              # Main menu
│   ├── Lobby.unity                  # Multiplayer lobby
│   └── Terrain.unity                # Main gameplay level
└── ...
```

---

## 🚀 How to try it?

### Prerequisites
- **Oculus / Meta Quest** headset (or any OpenXR-compatible HMD)
- Download the exe [here](https://github.com/metaOkami/InfectedFrontierVR-WIP/releases/tag/0.1)



---

## 🧰 Tech Stack

| Technology | Purpose |
|---|---|
| **Unity 2021.3 LTS** | Game engine |
| **Universal Render Pipeline** | VR-optimized rendering |
| **XR Interaction Toolkit 2.0** | VR input, interaction, locomotion |
| **OpenXR + Oculus SDK** | Cross-platform VR runtime |
| **Photon PUN 2** | Real-time multiplayer networking |
| **NavMesh + NavMeshComponents** | AI pathfinding |
| **TextMesh Pro** | In-world UI text |
| **Animation Rigging** | Procedural hand/body animation |
| **C# State Machines** | Modular AI behavior architecture |

---

## � Known Limitations & Roadmap

This is an honest WIP. Some things work well, others need iteration:

| Area | Current State | Planned Improvement |
|---|---|---|
| **Code architecture** | Some classes handle too many responsibilities (e.g., `EconomyManager` manages money, UI, and trap purchasing) | Refactor toward SOLID — split into focused components |
| **Enemy system** | `BaseEnemy_SM` and `AttackEnemy_SM` share ~80% duplicated code | Extract a shared `EnemyBase` class with common logic |
| **Damage system** | Bullet damage is resolved by checking `GameObject.name` strings | Introduce an `IDamageable` interface and `ScriptableObject`-based bullet data |
| **Runtime lookups** | Several scripts use `Find`/`FindWithTag` in `Update()` | Cache references at initialization or use dependency injection |
| **Wave balancing** | Enemy stats per wave are hardcoded in `if/else` chains | Move to data-driven `ScriptableObject` wave configs |
| **Trap extensibility** | Adding a new trap type requires modifying multiple classes | Abstract trap behavior behind interfaces/base classes |
| **UI** | Volume settings save to `PlayerPrefs` every frame | Save only on value change |

These are identified, documented, and part of the iteration plan — not hidden debt.

---

## 👤 Author

Developed in a team as an academic project exploring **VR interaction design**, **real-time multiplayer networking**, and **AI-driven game systems**. The goal was to build a playable multiplayer VR experience from scratch, dealing firsthand with the challenges of synchronizing physics-based VR rigs over the network. I worked in almost all areas but I focused on the scripting.

---

<div align="center">

*Infected Frontier VR is a work in progress*

</div>
