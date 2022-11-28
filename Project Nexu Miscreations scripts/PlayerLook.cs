using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLook : MonoBehaviour
{
    private Animator animator;
    private MovementController movement;
    [SerializeField] LayerMask layerMask;
    [SerializeField] int powerItemLayerNumber;
    [SerializeField] int lockItemLayerNumber;
   

    private Collider[] hitColliders;
    [SerializeField] GameObject interactionPanel;
    void Start()
    {
        animator = GetComponent<Animator>();
        movement = GetComponent<MovementController>();       
    }

    void OnDrawGizmosSelected()
    {        
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position + (transform.forward * 0.5f), 0.8f);
    }
    void Update()
    {
        hitColliders = Physics.OverlapSphere(transform.position + (transform.forward * 0.5f), 0.8f, LayerMask.GetMask("Interactable"));

        if (hitColliders.Length > 0)
        {
            interactionPanel.SetActive(true);
            //interactionPanel.GetComponentInChildren<Text>().text = "Press E to Interact";
            if (Input.GetKeyDown(KeyCode.E))
            {
                for (int i = 0; i < hitColliders.Length; i++)
                {
                    if (hitColliders[i].transform.TryGetComponent<IInteractable>(out IInteractable iInteractable))
                    {
                        iInteractable.Interact();
                    }
                }
            }
        }
        else interactionPanel.SetActive(false);
        

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("SuckPower"))
        {
            movement.canMove = false;
            animator.ResetTrigger("Jump");
        }
        
    }
}
