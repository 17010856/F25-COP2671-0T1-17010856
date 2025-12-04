using UnityEngine;
using TMPro;

public class DayDisplayUI : MonoBehaviour
{
    public TimeManager timeManager;
    public TextMeshProUGUI dayText;

    void Start()
    {
        if (timeManager != null)
        {
            timeManager.onDayChanged += UpdateDisplay;
            UpdateDisplay(timeManager.GetCurrentDay());
        }
    }

    void OnDestroy()
    {
        if (timeManager != null)
        {
            timeManager.onDayChanged -= UpdateDisplay;
        }
    }

    void UpdateDisplay(int day)
    {
        if (dayText != null)
        {
            dayText.text = $"Day {day}/{timeManager.maxDays}";
        }
    }
}