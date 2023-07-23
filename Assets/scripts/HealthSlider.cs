using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthSlider : MonoBehaviour
{
    private static HealthSlider instance;
    [SerializeField] private Slider _HealthSlider;
    public static HealthSlider Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<HealthSlider>();
            }
            return instance;
        }
    }
    void Start()
    {
        SetMax(100);
    }
    public void SetMax(int maxHealth)
    {
        _HealthSlider.maxValue = maxHealth;
        _HealthSlider.value = maxHealth;
    }
    public void SetHealth(int heath)
    {
        _HealthSlider.value = heath;
    }
    public bool AmiDeadAlready()
    {
        return _HealthSlider.value <= 0;
    }
}
