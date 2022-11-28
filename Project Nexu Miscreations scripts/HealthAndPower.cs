using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthAndPower : MonoBehaviour
{
    [SerializeField] private SOPlayerSettings playerSettings;
    private Animator animator;  
    private float MaxHealth;    
    public float Health { get; private set; }
    private float MaxPower;
    public float Power { get; private set; }
    [HideInInspector]public bool hasPower = true;
    private bool hasDied;

    void Start()
    {
        animator = GetComponent<Animator>();
        

        MaxHealth = playerSettings.MaxHealth;
        MaxPower = playerSettings.MaxPower;
        Health = MaxHealth;
        Power = MaxPower;

        
    }

   
    void Update()
    {
        if (Health > playerSettings.MaxHealth) Health = MaxHealth;
        if (Power > playerSettings.MaxPower) Power = MaxPower;
        if (Health < 0) Health = 0;

        if (Power < 0)
        {
            hasPower = false;
            Power = 0;
        }
        else hasPower = true;


        if (Input.GetKeyDown(KeyCode.L))
        {
            TakeDamage(20);
        }

        if (Health <= 0 && hasDied == false)
        {
            hasDied = true;
            animator.SetBool("Died", true);
        }

    }

    public void TakeDamage(float amount)
    {
        Health -= amount;
        animator.SetTrigger("Hurt");
    }    

    public void GetHealth(float amount)
    {
        Health += amount;
    }

    public void LosePower(float amount)
    {
        Power -= amount;
    }

    public void GetPower(float amount)
    {
        Power += amount;
    }



}
