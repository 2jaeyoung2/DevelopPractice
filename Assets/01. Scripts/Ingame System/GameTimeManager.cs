using System;
using System.Collections;
using UnityEngine;

public class GameTimeManager : MonoBehaviour
{
    // singleton
    public static GameTimeManager Instance { get; private set; }

    [Header("Time Settings")]
    [Tooltip("현실 시간 기준 '게임 내 1분'이 흐르는 데 걸리는 시간(초). 값이 작을수록 게임 시간이 더 빠르게 흐름 (예: 2 = 2초에 1분)")]
    [SerializeField]
    private float secondsPerMinute = 2f;

    [Tooltip("시간 흐름 배율(_ingameTimeScale)이 목표값으로 전환될 때 걸리는 기본 시간(초). 이동 시작/정지 시 시간 흐름이 부드럽게 변화하는 속도를 제어")]
    [SerializeField]
    private float timeScaleTransitionsDuration = 0.3f;


    private float accumulatedTime;

    private int currentHour = 0;

    private int currentMinute = 0;

    private float _ingameTimeScale;

    private bool _isMoving = false;


    private Coroutine _timeScaleCoroutine;


    public event Action<int, int> OnTimeChanged;


    public bool IsMoving => _isMoving;

    // 모든 움직이는 물체에 곱해야 함 (Update로 움직이는 것들)
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

        // 가만히 있을 때 0으로 시간 안흐르게 함.
        // 기획에 따라 가만히 있을 때에도 시간이 조금은 흐르게 해야하나 싶으넫
        // TODO: 0f >> nf
        float target = moving ? 1f : 0.2f;

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
