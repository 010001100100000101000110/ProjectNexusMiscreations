using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerItemS : MonoBehaviour, IInteractable
{
    private HealthAndPower healthAndPower;
    [SerializeField] private SOPlayerSettings playerSettings;
    private bool beenConsumed = true;
    private Animator animator;

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        healthAndPower = player.GetComponent<HealthAndPower>();
        animator = player.GetComponent<Animator>();
    }
   
    public void Interact()
    {
        if (beenConsumed == true)
        {
            beenConsumed = false;
            healthAndPower.GetPower(playerSettings.PowerItemSSize);
            animator.SetTrigger("SuckPower");
        }
    }
}
