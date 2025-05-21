# ✅ Phase 1 – Core Loop Prototype (Movement + Basic Combat)

## 🎯 Objective:

Create a minimal vertical slice where the player:

* Can move (walk, jump, dash)
* Can fire a weapon (projectile)
* Can destroy a static enemy
* Sees their health and energy in a basic HUD

---

## 🔧 Architectural Patterns Used in Phase 1:

| Pattern               | Description                                                                                                        |
| --------------------- | ------------------------------------------------------------------------------------------------------------------ |
| **MVC**               | Used for HUD: `HUDView` (View), `HUDController` (Controller), player stats (Model)                                 |
| **Composition**       | Movement and firing separated into `PlatformerController` and `MechController`                                     |
| **Event-Driven**      | `EventBus` + custom events used to trigger HUD updates, enemy death, etc.                                          |
| **Interfaces**        | `IDamageable`, `IAttacker` used for flexible combat interactions                                                   |
| **ScriptableObjects** | Definitions created but not yet used — stub out `WeaponConfigSO`, `CombatConfigSO`, `GameEventSO<T>` for later use |

---

## 📁 Assets Required (Placeholder versions acceptable)

| Asset Type              | Purpose                          | Examples/Formats               |
| ----------------------- | -------------------------------- | ------------------------------ |
| **Sprite – Player**     | Player idle, run animation       | 2D PNG or Unity SpriteSheet    |
| **Sprite – Enemy**      | Stationary enemy or dummy target | PNG, static                    |
| **Sprite – Projectile** | Basic bullet or laser            | PNG or LineRenderer prefab     |
| **Tilemap/Ground**      | Platforms to walk on             | Unity Tilemap or BoxCollider2D |
| **UI Elements**         | Health bar, energy bar, text     | Unity UI / TextMeshPro         |

---

## 📘 Milestone 1: Project Setup & Base Scene

| Task    | Description                                                                        | Dependencies                                        |
| ------- | ---------------------------------------------------------------------------------- | --------------------------------------------------- |
| **1.1** | Create Unity URP 2D project and folder structure                                   | -                                                   |
| **1.2** | Set up folders: `/Scripts`, `/Prefabs`, `/Scenes`, `/UI`, `/Data`, `/SO`, `/Art`   | -                                                   |
| **1.3** | Create scene `Phase1_TestScene` with basic ground tile and a camera                | Unity URP, 2D Tilemap                               |
| **1.4** | Set up GameObject hierarchy for `GameManager`, `EventBus`, `UI`, `Player`, `Enemy` | `GameBootstrapper`, `GameManager`, `Canvas`         |
| **1.5** | Create prefabs: `PlayerPrefab`, `EnemyPrefab`, `ProjectilePrefab`                  | `PlayerController`, `EnemyController`, `Projectile` |

---

## 🏃 Milestone 2: Movement System (Platforming)

| Task    | Description                                                                           | Class(es) Involved     | Pattern     |
| ------- | ------------------------------------------------------------------------------------- | ---------------------- | ----------- |
| **2.1** | Implement `PlayerController` to handle input                                          | `PlayerController`     | OOP         |
| **2.2** | Implement `PlatformerController` for movement (Rigidbody2D, ground check, jump, dash) | `PlatformerController` | Composition |
| **2.3** | Integrate animator for walking/jumping (optional)                                     | Animator component     | -           |
| **2.4** | Create ground tiles with colliders                                                    | Unity Tilemap          | -           |
| **2.5** | Verify smooth 60fps movement, collisions, jump and dash                               | -                      | QA          |

---

## 🔫 Milestone 3: Basic Weapon Firing & Projectile Logic

| Task    | Description                                                                          | Class(es) Involved                             | Pattern     |
| ------- | ------------------------------------------------------------------------------------ | ---------------------------------------------- | ----------- |
| **3.1** | Create `MechController`, handle fire input                                           | `MechController`                               | Composition |
| **3.2** | Implement `WeaponBase` class (simple fire method with cooldown)                      | `WeaponBase`, `IAttacker`                      | Interface   |
| **3.3** | Create `Projectile` prefab with `Rigidbody2D`, `Collider2D`, simple forward movement | `Projectile`                                   | OOP         |
| **3.4** | On hit: detect collision with `IDamageable` → call `TakeDamage()`                    | `IDamageable`, `EnemyController`, `Projectile` | Interface   |

---

## 🧟 Milestone 4: Dummy Enemy Setup

| Task    | Description                                                        | Class(es) Involved                              | Pattern      |
| ------- | ------------------------------------------------------------------ | ----------------------------------------------- | ------------ |
| **4.1** | Create `EnemyController` with health field                         | `EnemyController`, `IDamageable`                | Interface    |
| **4.2** | On projectile hit: call `TakeDamage()` and destroy when health ≤ 0 | `EnemyController`, `Projectile`, `CombatSystem` | Event-driven |
| **4.3** | Optionally raise `OnEnemyDestroyedEvent` via `EventBus`            | `EventBus`, `GameEventSO<Enemy>`                | Event System |

---

## 💥 Milestone 5: Combat System

| Task    | Description                                                           | Class(es) Involved                 | Pattern                 |
| ------- | --------------------------------------------------------------------- | ---------------------------------- | ----------------------- |
| **5.1** | Create `CombatSystem` with `ApplyDamage()` method                     | `CombatSystem`, `DamageCalculator` | SRP                     |
| **5.2** | Implement `DamageCalculator` with hardcoded damage (e.g., 10 per hit) | `DamageCalculator`                 | SRP                     |
| **5.3** | Connect `Projectile` → `CombatSystem.ApplyDamage()`                   | `Projectile`, `CombatSystem`       | Composition             |
| **5.4** | Register enemy deaths through logs                                    | `EnemyController`, `EventBus`      | Debugging, Event-driven |

---

## 🧑‍💻 Milestone 6: Basic HUD

| Task    | Description                                                            | Class(es) Involved                  | Pattern    |
| ------- | ---------------------------------------------------------------------- | ----------------------------------- | ---------- |
| **6.1** | Create Canvas + HUD layout with health and energy text/sliders         | Unity UI / TMP                      | MVC        |
| **6.2** | Implement `HUDView` to expose UI hooks                                 | `HUDView`                           | View       |
| **6.3** | Implement `HUDController` to update player health and energy           | `HUDController`                     | Controller |
| **6.4** | Hook `PlayerController` → `HUDController` to update UI on state change | `PlayerController`, `HUDController` | MVC        |

---

## 🧠 Milestone 7: Core Game Management

| Task    | Description                                                             | Class(es) Involved             | Pattern          |
| ------- | ----------------------------------------------------------------------- | ------------------------------ | ---------------- |
| **7.1** | Implement `GameManager` with simple GameState enum                      | `GameManager`                  | Singleton        |
| **7.2** | Implement `GameBootstrapper` to connect systems and inject dependencies | `GameBootstrapper`, `EventBus` | Composition Root |
| **7.3** | Confirm scene boot order, prefab instantiation, component linking       | All above                      | QA/Glue          |

---

## 🧪 Milestone 8: Testing & Polish

| Task    | Description                                                   | Output              |
| ------- | ------------------------------------------------------------- | ------------------- |
| **8.1** | Test firing, hit detection, movement timing, and jump physics | Gameplay QA         |
| **8.2** | Tune values (e.g., jump force, fire rate, enemy health)       | Balanced experience |
| **8.3** | Ensure no console errors or unlinked references               | Stability           |
| **8.4** | Create Git commit `phase1_core_loop`                          | Version checkpoint  |

---

## ✅ End State Summary

* Fully playable prototype
* Player can move and dash
* Player can shoot a projectile
* Dummy enemy can be destroyed with a few hits
* Player sees health and energy in UI
* Internal systems modular and test-ready
* Event system, interfaces, and controller architecture in place
* ScriptableObject templates exist for Phase 2 integration