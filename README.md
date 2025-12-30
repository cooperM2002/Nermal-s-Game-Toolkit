# Project Name
<br />

A Unity project focused on building a **toolkit of reusable, system-based gameplay mechanics** inspired by the [Immersive Sim design philosophy](https://www.youtube.com/watch?v=iSF4xuEGgWs), with an emphasis on Valve-like game feel.
<br />

**Goal:** provide clean, composable *systems* (inventory, interaction, AI perception, damage, etc.) that can be easily extended and customized.
<br /><br />

<div style="margin-left: 20px;">
  <p><b>Interaction System</b></p>
  <ul>
    <li>Purpose: ...</li>
    <li>Demo: ...</li>
  </ul>
</div>

## Core design principles
- **Composable**: systems should interoperate through interfaces/events, not hard references
- **Replaceable**: you can swap implementations (ex: different inventory UI or interaction method)
- **Inspectable**: built-in debug views/logs/gizmos
- **Data-driven**: ScriptableObjects for definitions/configs when it helps
- **Minimal coupling**: avoid “god managers”; prefer local responsibilities + messaging
<br /><br />

## Components 



<details>
  <summary><b>Interaction System</b></summary>

  <table>
    <tr><td>
      <b>Purpose:</b> Raycast-based interaction + prompts.<br><br>
      <b>Key scripts:</b> <code>Interactor.cs</code>, <code>Interactable.cs</code><br>
      <b>Demo:</b> <code>Demo_Interaction.unity</code>
    </td></tr>
  </table>
</details>





