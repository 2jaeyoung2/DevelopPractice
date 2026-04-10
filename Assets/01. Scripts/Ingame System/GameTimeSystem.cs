using System;
using UnityEngine;

public class GameTimeSystem : MonoBehaviour
{
    [Header("Time Settings")]
    [SerializeField]
    private float secondsPerMinute = 4f;

    private float accumulatedTime;

    private int currentHour = 0;

    private int currentMinute = 0;

    private bool isMoving = false;

    public event Action<int, int> OnTimeChanged;


    private void Start()
    {
        OnTimeChanged?.Invoke(currentHour, currentMinute);
    }

    private void Update()
    {
        if (isMoving == false)
        {
            return;
        }

        accumulatedTime += Time.deltaTime;

        if (accumulatedTime >= secondsPerMinute)
        {
            accumulatedTime -= secondsPerMinute;

            AddMinute();
        }
    }

    private void AddMinute()
    {
        currentMinute++;

        if (currentMinute >= 60)
        {
            currentMinute = 0;

            currentHour++;
        }

        if (currentHour >= 24)
        {
            currentHour = 0;
        }

        OnTimeChanged?.Invoke(currentHour, currentMinute);
    }

    public void SetMoving(bool moving)
    {
        isMoving = moving;
    }
}
