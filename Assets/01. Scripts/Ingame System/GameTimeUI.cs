using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameTimeUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI timeText;

    private void Awake()
    {
        if (timeText == null)
        {
            Debug.LogError("TimeText is not assigned.");
        }
    }

    private void Start()
    {
        GameTimeManager.Instance.OnTimeChanged += UpdateTimeUI;
    }

    private void OnDestroy()
    {
        GameTimeManager.Instance.OnTimeChanged -= UpdateTimeUI;
    }

    private void UpdateTimeUI(int hour, int minute)
    {
        // 00:00 Çü½Ä
        timeText.text = $"{hour:00}:{minute:00}";
    }
}
