using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealth : MonoBehaviour
{
    [SerializeField] private SOPlayerSettings playerSettings;
    private Slider HealthSlider;
    private HealthAndPower healthAndPower;
    
    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        healthAndPower = player.GetComponent<HealthAndPower>();
        HealthSlider = GetComponent<Slider>();
        HealthSlider.maxValue = playerSettings.MaxHealth;
        HealthSlider.value = healthAndPower.Health;
    }
    private void Update()
    {          
        SetHealth(healthAndPower.Health);
    }
    public void SetHealth(float health)
    {
        HealthSlider.value = health;
    }
}
