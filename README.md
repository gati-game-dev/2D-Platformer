# 2D Platformer

A 2D platformer game developed using **Unity and C#**, featuring responsive player movement, enemy AI, environmental interactions, physics-based mechanics, collectibles, and dynamic hazards.

The game combines traditional platforming with interactive environments, enemy encounters, and mechanics that encourage exploration and timing.

<p align="center">
  <img src="Screenshots/gameplay.png" alt="Platformer Gameplay" width="750">
</p>

---

## 🎮 Features

### 🏃 Player Movement & Mechanics
- Responsive horizontal movement and jumping.
- Ground detection using a dedicated ground-check system.
- Rope climbing and physics-based rope swinging.
- Swing using horizontal movement and release with retained momentum.
- Jump-based rope detachment.
- Spring-based boosted jumps.
- Player health and damage system.
- Hit animation triggered upon taking damage.
- Death state with a dedicated **"You Died!"** UI and restart functionality.

### 🤖 Enemy AI

The game features multiple enemies with distinct movement and attack behaviours.

- **Bee AI:** Idle hovering followed by player detection and chasing behaviour.
- **Fish AI:** Periodically jumps out of water and damages the player on contact.
- **Saw Boss AI:** Boss encounter featuring an awakening animation, horizontal player tracking, periodic jumping, and collision-based damage.

Each enemy uses its own movement and detection logic to create different gameplay challenges.

### 🪢 Rope Swinging System
- Physics-based swinging using Unity's `DistanceJoint2D`.
- Player can attach to ropes when within a specified grab range.
- Horizontal movement controls swinging direction.
- Releasing the rope preserves the player's momentum.
- Rope visuals dynamically represent the swinging connection.
- Supports reusable rope prefabs for placement throughout the level.

### 🎯 Interactive Objects
- Lever-based environmental interactions.
- Moving platforms.
- Spring mechanisms.
- Exit door with key-based unlocking.
- Interactive objects that respond to player proximity and input.

### 🔑 Collectibles & Progression
- Collectible keys required to unlock level progression.
- Key collection tracked and displayed through the UI.
- Exit door becomes accessible after collecting all required keys.
- Key collection triggers environmental changes and resets specific objects.

### ❤️ Health & Death System
- Player health system with different damage values depending on the hazard or enemy.
- Damage from enemies and environmental hazards.
- Dedicated hit animation when the player takes damage.
- Death state with a **"You Died!"** UI.
- Restart prompt allowing the player to restart the level.

### 💥 Environmental Hazards

The level features multiple hazards with different behaviours and damage mechanics:

- Spikes.
- Lava pits.
- Falling blocks.
- Falling weights.
- Proximity-based bombs.

Some hazards cause instant death, while others reduce player health.

### 🔄 Environmental Reset System

A centralized game management system handles the resetting of multiple environmental objects when the required progression condition is met.

- Falling blocks return to their original positions and restore their visual states.
- Falling weights are repositioned and their physics states are restored.
- Bombs are reset, including their activation and visual states.
- Spring and other registered environmental objects can be reset.
- Reset operations are coordinated through a persistent `GameManager`.

This allows the environment to respond dynamically to key collection and ensures that reusable objects return to their intended initial states.

### 🐌 Additional Enemy & Object Behaviour
- Moving enemies with patrol-style movement.
- Snail and fish movement using ping-pong positioning.
- Conditional movement behaviour that allows the snail to stop when scared and resume its patrol when no longer scared.

---

## 🛠️ Technical Implementation

Some of the key Unity systems and concepts used in the project include:

- **Unity 2D Physics:** Rigidbody2D, Collider2D, DistanceJoint2D and physics-based interactions.
- **Animator:** State transitions, movement animations, hit animations, and enemy animations.
- **Unity Tilemap System:** Level construction and destructible environmental elements.
- **Singleton Pattern:** Centralized GameManager for managing gameplay state and environmental resets.
- **Persistent Scene Management:** `DontDestroyOnLoad` and scene-loading callbacks.
- **Enemy AI:** Player detection, chasing, patrol movement, timed jumping, and collision-based damage.
- **Prefab System:** Reusable environmental objects and rope mechanics.
- **UI Systems:** Health, collectible tracking, death screens, and progression messages.

---

## 🧰 Built With

- **Unity**
- **C#**
- **Unity 2D Physics**
- **Unity Animator**
- **Unity Tilemap System**

---

## 📌 Project Status

<badge color="success">Completed</badge>

The core gameplay systems, enemy behaviours, environmental interactions, collectibles, hazards, and progression mechanics have been implemented.

The project served as a hands-on exploration of Unity game development, focusing on gameplay programming, physics-based mechanics, AI behaviour, object management, and building interconnected gameplay systems.