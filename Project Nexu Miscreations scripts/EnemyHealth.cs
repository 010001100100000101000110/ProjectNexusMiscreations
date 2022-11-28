using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{

    public float EnemHealth = 100;
    
    void Update()
    {
        if (EnemHealth <= 0) gameObject.SetActive(false);
    }
    public void EnemyTakeDamage(float amount)
    {
        EnemHealth -= amount;
    }

}
