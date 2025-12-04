using UnityEngine;

// handles day/night cycle and game days
public class TimeManager : MonoBehaviour
{
    public float time = 0f; 
    public float timeSpeed = 1f;
    public int currentDay = 1;
    public int maxDays = 20;

    public delegate void OnDayChanged(int day); 
    public event OnDayChanged onDayChanged;

    public delegate void OnGameOver();
    public event OnGameOver onGameOver;

    void Update()
    {
        time += Time.deltaTime * timeSpeed; // advancing time
        if (time >= 24f) // day is over
        {
            time = 0f;
            currentDay++;
            onDayChanged?.Invoke(currentDay);

            if (currentDay > maxDays) // game over
            {
                onGameOver?.Invoke();
                Time.timeScale = 0f; // stop the game
            }
        }
    }

    public int GetCurrentDay()
    {
        return currentDay;
    }
}