using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DayNightLighting : MonoBehaviour
{
    public Light2D sceneLight;
    public TimeManager timeManager;

    [Header("Intensity Settings")]
    public AnimationCurve intensityCurve;

    [Header("Color Settings")]
    public Gradient lightColorGradient;

    void Start()
    {
    
        if (intensityCurve == null || intensityCurve.keys.Length == 0)
        {
            intensityCurve = new AnimationCurve(
                new Keyframe(0f, 0.5f),    
                new Keyframe(0.15f, 0.7f), 
                new Keyframe(0.35f, 1.2f), 
                new Keyframe(0.65f, 1.2f), 
                new Keyframe(0.85f, 0.7f), 
                new Keyframe(1f, 0.5f)     
            );
        }
    }

    void Update()
    {
        if (sceneLight == null || timeManager == null) return;

        float t = timeManager.time / 24f; 

      
        sceneLight.intensity = intensityCurve.Evaluate(t);

      
        if (lightColorGradient != null)
            sceneLight.color = lightColorGradient.Evaluate(t);
    }
}
