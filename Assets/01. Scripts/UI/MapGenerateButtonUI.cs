using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapGenerateButtonUI : MonoBehaviour
{
    [SerializeField]
    private MapGenerator mapGenerator;

    private Button mapGeneratorButton;

    private void Awake()
    {
        mapGeneratorButton = GetComponent<Button>();
    }

    private void OnEnable()
    {
        mapGeneratorButton.onClick.AddListener(mapGenerator.GenerateMap);
    }

    private void OnDisable()
    {
        mapGeneratorButton.onClick.RemoveListener(mapGenerator.GenerateMap);
    }
}
