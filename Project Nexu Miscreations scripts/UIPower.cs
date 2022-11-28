using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPower : MonoBehaviour
{
    [SerializeField] private SOPlayerSettings playerSettings;
    private Slider PowerSlider;
    [SerializeField] private HealthAndPower healthAndPower;

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        healthAndPower = player.GetComponent<HealthAndPower>();
        PowerSlider = GetComponent<Slider>();
        PowerSlider.maxValue = playerSettings.MaxPower;
        PowerSlider.value = healthAndPower.Power;
    }
    private void Update()
    {
        SetPower(healthAndPower.Power);
    }
    public void SetPower(float power)
    {
        PowerSlider.value = power;
    }
}
