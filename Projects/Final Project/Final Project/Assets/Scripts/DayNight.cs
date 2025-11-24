using UnityEngine;

public class DayNightEvents : MonoBehaviour
{
    public TimeManager timeManager;

    void Update()
    {
        if (timeManager.time >= 6f && timeManager.time < 6.1f)
            Debug.Log("Sunrise!");
        if (timeManager.time >= 18f && timeManager.time < 18.1f)
            Debug.Log("Sunset!");
    }
}
