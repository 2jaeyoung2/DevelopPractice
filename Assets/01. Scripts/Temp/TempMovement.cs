using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float amplitude = 2f;
    [SerializeField] private float frequency = 1f;

    private Vector3 _startPosition;
    private float _elapsedTime;

    private void Start()
    {
        _startPosition = transform.position;
        _elapsedTime = 0f;
    }

    private void Update()
    {
        _elapsedTime += Time.deltaTime * GameTimeManager.Instance.IngameTimeScale;
        float offset = Mathf.Sin(_elapsedTime * frequency * Mathf.PI * 2f) * amplitude;
        transform.position = _startPosition + new Vector3(0f, offset, 0f);
    }
}
