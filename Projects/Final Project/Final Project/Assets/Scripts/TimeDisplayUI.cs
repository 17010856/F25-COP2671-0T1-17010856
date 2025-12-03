using UnityEngine;
using TMPro;

public class TimeDisplayUI : MonoBehaviour
{
    public TimeManager timeManager;
    public TextMeshProUGUI timeText;

    void Update()
    {
        if (timeManager == null || timeText == null) return;

        int hours = Mathf.FloorToInt(timeManager.time);
        int minutes = Mathf.FloorToInt((timeManager.time - hours) * 60);

        string period = hours >= 12 ? "PM" : "AM";
        int displayHours = hours % 12;
        if (displayHours == 0) displayHours = 12;

        timeText.text = string.Format("{0}:{1:00} {2}", displayHours, minutes, period);
    }
}