# <p>Project Name</p>

A Unity project focused on building a **toolkit of reusable, system-based gameplay mechanics** inspired by the [Immersive Sim design philosophy](https://www.youtube.com/watch?v=iSF4xuEGgWs), with an emphasis on Valve-like game feel.
<br />

**Goal:** provide clean, composable *systems* (inventory, interaction, AI perception, damage, etc.) that can be easily extended and customized.
<br /><br />



## <p>Core design principles</p>
- **Composable**: systems should interoperate through interfaces/events, not hard references
- **Replaceable**: you can swap implementations (ex: different inventory UI or interaction method)
- **Inspectable**: built-in debug views/logs/gizmos
- **Data-driven**: ScriptableObjects for definitions/configs when it helps
- **Minimal coupling**: avoid “god managers”; prefer local responsibilities + messaging
<br /><br />

## <p>Components</p> 


### <p>1. Interaction System</p>



<table>
    <tr><td>
        <details>
            <summary><i>details</i></summary>
            <div>
            <p><b>Purpose:</b> Raycast-based interaction + prompts.</p>
            <ul>
                <li><b>Key scripts:</b> <code>Interactor.cs</code>, <code>Interactable.cs</code></li>
                <li><b>Demo:</b> <code>Demo_Interaction.unity</code></li>
                <li><b>Extension:</b> implement <code>IInteractable</code></li>
            </ul>
            </div>
        </details>
    </td></tr>
</table>
<table>
    <tr><td>
        <details>
            <summary><i>details</i></summary>
            <div>
            <p><b>Purpose:</b> Raycast-based interaction + prompts.</p>
            <ul>
                <li><b>Key scripts:</b> <code>Interactor.cs</code>, <code>Interactable.cs</code></li>
                <li><b>Demo:</b> <code>Demo_Interaction.unity</code></li>
                <li><b>Extension:</b> implement <code>IInteractable</code></li>
            </ul>
            </div>
        </details>
    </td></tr>
</table>
<table>
    <tr><td>
        <details>
            <summary><i>details</i></summary>
            <div>
            <p><b>Purpose:</b> Raycast-based interaction + prompts.</p>
            <ul>
                <li><b>Key scripts:</b> <code>Interactor.cs</code>, <code>Interactable.cs</code></li>
                <li><b>Demo:</b> <code>Demo_Interaction.unity</code></li>
                <li><b>Extension:</b> implement <code>IInteractable</code></li>
            </ul>
            </div>
        </details>
    </td></tr>
</table>










