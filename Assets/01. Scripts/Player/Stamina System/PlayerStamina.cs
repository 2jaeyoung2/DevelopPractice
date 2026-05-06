using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStamina : MonoBehaviour
{
    [SerializeField]
    private PlayerMovement player;

    [SerializeField]
    private float _maxStamina;

    [SerializeField]
    private float _currentStamina;


    public float MaxStamina => _maxStamina;

    public float CurrentStamina => _currentStamina;

    private void Awake()
    {
        StaminaSettings(80, 80);
    }

    private void Start()
    {
        if (player == null)
        {
            player = GetComponent<PlayerMovement>();
        }
    }

    private void StaminaSettings(float current, float max)
    {
        _currentStamina = current;

        _maxStamina = max;
    }
}
