using UnityEngine;

public class FloatingMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float amplitude = 0.1f;

    [SerializeField] private float frequency = 0.3f;

    [SerializeField] private Vector2 randomPhaseRange = new Vector2(0f, 10f);

    private Vector3 _startPosition;

    private float _elapsedTime;

    private float _phaseOffset;

    private void Start()
    {
        _startPosition = transform.position;

        _phaseOffset = Random.Range(randomPhaseRange.x, randomPhaseRange.y);
    }

    private void Update()
    {
        _elapsedTime += Time.deltaTime * GameTimeManager.Instance.IngameTimeScale;

        float wave = Mathf.Sin((_elapsedTime + _phaseOffset) * frequency * Mathf.PI * 2f);

        float offset = wave * amplitude;

        transform.position = _startPosition + Vector3.up * offset;
    }
}