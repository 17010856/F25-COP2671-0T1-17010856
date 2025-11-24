using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public float time = 0f; 
    public float timeSpeed = 1f; 

    void Update()
    {
        time += Time.deltaTime * timeSpeed;
        if (time >= 24f)
            time = 0f; 
    }
}
