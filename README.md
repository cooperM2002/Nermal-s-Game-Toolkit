# Project Name
<p></p>
A Unity project focused on building a **toolkit of reusable, system-based gameplay mechanics** inspired by the [Immersive Sim design philosophy](https://www.youtube.com/watch?v=iSF4xuEGgWs), with an emphasis on Valve-like game feel.

**Goal:** provide clean, composable *systems* (inventory, interaction, AI perception, damage, etc.) that can be easily extended and customized.

## Core design principles
- **Composable**: systems should interoperate through interfaces/events, not hard references
- **Replaceable**: you can swap implementations (ex: different inventory UI or interaction method)
- **Inspectable**: built-in debug views/logs/gizmos
- **Data-driven**: ScriptableObjects for definitions/configs when it helps
- **Minimal coupling**: avoid “god managers”; prefer local responsibilities + messaging








