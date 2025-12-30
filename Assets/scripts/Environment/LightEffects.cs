using UnityEngine;


public class LightEffects : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Light lightsource;
    private float baseIntensity;
    private float intensityMultiplier;

    public float frequency;     // 2 pulses/sec
    public float minPulse = 0.5f;    // 20% of max
    public float maxPulse = 1.1f;    // 100% of max
    public float phase = 0f;

    public float minCycleSeconds = 1f;
    public float maxCycleSeconds = 5f;

    float phaseRange;          // 0..2π
    float cycleSeconds;   // current cycle duration
    float omega;          // angular speed = 2π / cycleSeconds

    void Start()
    {
        lightsource = GetComponent<Light>();
        if (lightsource == null) {
            Debug.Log("no light component in Light gameobject");
        }

        baseIntensity = lightsource.intensity;
        intensityMultiplier = baseIntensity*0.125f;
        PickNewCycle();
        phaseRange = 0f;

    }

// Update is called once per frame
    void Update()
    {

        // advance phase based on current cycle duration
        phaseRange += Time.deltaTime * omega;

        // if we completed a cycle, wrap and pick a new duration
        if (phaseRange >= Mathf.PI * 2f)
        {
            phaseRange -= Mathf.PI * 2f;
            PickNewCycle();
        }

        // wave in 0..1
        float wave01 = (Mathf.Sin(phase) + 1f) * 0.5f;
        lightsource.intensity= baseIntensity * intensityMultiplier * Mathf.Lerp(minPulse, maxPulse, wave01);

    }

    void PickNewCycle()
    {
        cycleSeconds = Random.Range(minCycleSeconds, maxCycleSeconds); // float: min..max
        omega = (Mathf.PI * 2f) / cycleSeconds;
    }
}


