using UnityEngine;
using System.Collections;

public class Clock : MonoBehaviour
{
    [SerializeField]
    private int minutes = 0;

    [SerializeField]
    private int hour = 0;

    [SerializeField]
    private int seconds = 0;

    [SerializeField]
    private bool realTime = true;

    [SerializeField]
    private GameObject pointerSeconds;

    [SerializeField]
    private GameObject pointerMinutes;

    [SerializeField]
    private GameObject pointerHours;

    [SerializeField]
    private float clockSpeed = 1.0f;

    float msecs = 0;

    void Start()
    {
        SetRandomTime();
    }

    void Update()
    {
        ClockMovement();
    }

    private void SetRandomTime()
    {
        if (realTime == true)
        {
            hour = Random.Range(0, 24);

            minutes = Random.Range(0, 60);

            seconds = Random.Range(0, 60);
        }
    }

    private void ClockMovement()
    {
        // 시간 계산
        msecs += Time.deltaTime * GameTimeManager.Instance.IngameTimeScale * clockSpeed;

        if (msecs >= 1.0f)
        {
            msecs -= 1.0f;

            seconds++;

            if (seconds >= 60)
            {
                seconds = 0;

                minutes++;

                if (minutes >= 60)
                {
                    minutes = 0;

                    hour++;

                    if (hour >= 24)
                    {
                        hour = 0;
                    }
                }
            }
        }

        //-- smooth values
        float smoothSeconds = seconds + msecs;

        float smoothMinutes = minutes + (smoothSeconds / 60.0f);

        float smoothHours = hour + (smoothMinutes / 60.0f);

        //-- calculate pointer angles
        float rotationSeconds = (360.0f / 60.0f) * seconds;

        float rotationMinutes = (360.0f / 60.0f) * minutes;

        float rotationHours = ((360.0f / 12.0f) * hour) + ((360.0f / (60.0f * 12.0f)) * minutes);

        //-- draw pointers
        pointerSeconds.transform.localEulerAngles = new Vector3(0.0f, 0.0f, rotationSeconds);

        pointerMinutes.transform.localEulerAngles = new Vector3(0.0f, 0.0f, rotationMinutes);

        pointerHours.transform.localEulerAngles = new Vector3(0.0f, 0.0f, rotationHours);
    }
}
