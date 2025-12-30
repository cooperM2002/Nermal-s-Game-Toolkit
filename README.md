# [Project Name] — A Reusable Systems Toolkit for Unity (Immersive-Sim Style)

A Unity project focused on building a **toolkit of reusable, system-based gameplay mechanics** that draw heavy inspiration from the Immersive Sim design philosophy with an emphasis on Valve-like game feel.  
**Goal:** provide clean, composable *systems* (inventory, interaction, AI perception, damage, etc...) that can be easily extended and customized.



## What this is / isn’t

### What this is:
- A collection of **reusable gameplay systems** implemented as modular components
- A **reference project** with working scenes, examples, and test setups



## Core design principles
- **Composable**: systems should interoperate through interfaces/events, not hard references
- **Replaceable**: you can swap implementations (ex: different inventory UI or interaction method)
- **Inspectable**: built-in debug views/logs/gizmos
- **Data-driven**: ScriptableObjects for definitions/configs when it helps
- **Minimal coupling**: avoid “god managers”; prefer local responsibilities + messaging



## Quick start
1. Clone:
   ```bash
   git clone https://github.com/[you]/[repo].git
