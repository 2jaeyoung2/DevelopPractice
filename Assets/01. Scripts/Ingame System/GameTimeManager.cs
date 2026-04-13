using System;
using System.Collections;
using UnityEngine;

public class GameTimeManager : MonoBehaviour
{
    // singleton
    public static GameTimeManager Instance { get; private set; }

    [Header("Time Settings")]
    [SerializeField]
    private float secondsPerMinute = 2.5f;

    [SerializeField]
    private float timeScaleTransitionsDuration = 0.8f;


    private float accumulatedTime;

    private int currentHour = 0;

    private int currentMinute = 0;

    private float _ingameTimeScale;

    private bool _isMoving = false;


    private Coroutine _timeScaleCoroutine;


    public event Action<int, int> OnTimeChanged;


    public bool IsMoving => _isMoving;

    public float IngameTimeScale => _ingameTimeScale;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);

            return;
        }
        else
        {
            Instance = this;

            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        OnTimeChanged?.Invoke(currentHour, currentMinute);
    }

    private void Update()
    {
        UpdateIngameTime();
    }

    private void UpdateIngameTime()
    {
        if (_ingameTimeScale == 0f)
        {
            return;
        }

        accumulatedTime += Time.deltaTime * _ingameTimeScale;

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
        _isMoving = moving;

        if (_timeScaleCoroutine != null)
        {
            StopCoroutine(_timeScaleCoroutine);
        }

        float target = moving ? 1f : 0f;

        _timeScaleCoroutine = StartCoroutine(TransitionTimeScale(target));
    }

    private IEnumerator TransitionTimeScale(float target)
    {
        float start = _ingameTimeScale;

        float elapsed = 0f;

        float duration = timeScaleTransitionsDuration * Mathf.Abs(target - start);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            _ingameTimeScale = Mathf.Lerp(start, target, elapsed / duration);

            yield return null;
        }

        _ingameTimeScale = target;

        _timeScaleCoroutine = null;
    }
}
