using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameTimeUI : MonoBehaviour
{
    [Tooltip("인게임 시간을 표시하는 텍스트")]
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
        // 00h 00m 형식
        timeText.text = $"{hour:00}h {minute:00}m";
    }
}
