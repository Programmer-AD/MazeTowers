using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CastleHealthShower : MonoBehaviour
{
    [SerializeField] private Slider progressBar;
    [SerializeField] private TMP_Text text;

    private void Awake()
    {
        progressBar.minValue = 0;
        progressBar.maxValue = 1;
        progressBar.value = 1;

        text.text = string.Empty;
    }

    public void OnHealthChanged(int health, int maxHealth)
    {
        progressBar.maxValue = maxHealth;
        progressBar.value = health;
        text.text = $"{health}/{maxHealth}";
    }
}
