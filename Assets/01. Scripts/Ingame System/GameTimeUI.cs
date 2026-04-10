using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameTimeUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI timeText;

    [SerializeField]
    private GameTimeSystem timeSystem;

    private void Awake()
    {
        if (timeText == null)
        {
            Debug.LogError("TimeText is not assigned.");
        }

        if (timeSystem == null)
        {
            Debug.LogError("GameTimeSystem is not assigned.");

            return;
        }

        timeSystem.OnTimeChanged += UpdateTimeUI;
    }

    private void OnDestroy()
    {
        if (timeSystem != null)
        {
            timeSystem.OnTimeChanged -= UpdateTimeUI;
        }
    }

    private void UpdateTimeUI(int hour, int minute)
    {
        // 00:00 Çü½Ä
        timeText.text = $"{hour:00}:{minute:00}";
    }
}
