using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatController : MonoBehaviour
{
    [SerializeField] private SOPlayerSettings playerSettings;
    private Animator animator;    
    private MovementController playerMovement;
    private EnemyHealth enemyHealth;    
    [SerializeField] Transform raycastPlacement;
    [SerializeField]  float lockRange = 4;
    int numberOfClicks =0;
    float lastClickedTime = 0;
    [SerializeField] float maxComboDelay = 0.9f;

    bool attack1Done;
    bool attack2Done;
    bool attack3Done;

    void Start()
    {
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<MovementController>(); 
    }

    void Update()
    {
        Combo();
        
    }

  
    private void Combo()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") || animator.GetCurrentAnimatorStateInfo(0).IsName("Attack2") || animator.GetCurrentAnimatorStateInfo(0).IsName("Attack3"))
        {
            playerMovement.velocity.y = -7;
            playerMovement.movementSpeed = playerSettings.PlayerSpeedInAttackState;
            playerMovement.canDash = false;
        }
        

        if (Time.time - lastClickedTime > maxComboDelay)
        {
            numberOfClicks = 0;
        }

        if (Input.GetMouseButtonDown(0))
        {
            
            
            lastClickedTime = Time.time;
            numberOfClicks++;
            if (numberOfClicks == 1)
            {
                animator.SetBool("Attack1", true);
            }
            numberOfClicks = Mathf.Clamp(numberOfClicks, 0, 3);
            playerMovement.canJump = false;
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Walking"))
        {
            numberOfClicks = 0;
        }
        ComboAttackDamage();
    }

    private void ComboAttackDamage()
    {    
        RaycastHit hit;        
        if (Physics.Raycast(raycastPlacement.position, transform.TransformDirection(Vector3.forward), out hit, lockRange, LayerMask.GetMask("Enemy")))
        {
            enemyHealth = hit.collider.gameObject.GetComponent<EnemyHealth>();     
            
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") && attack1Done == false)
            {
                StartCoroutine(AttackWithDelay(playerSettings.Attack1DamageDelay, 20));                
                attack1Done = true;

                
            }
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack2") && attack2Done == false)
            {
                StartCoroutine(AttackWithDelay(playerSettings.Attack2DamageDelay, 20));                
                attack2Done = true;
            }
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack3") && attack3Done == false)
            {
                StartCoroutine(AttackWithDelay(playerSettings.Attack3DamageDelay, 30));                
                attack3Done = true;
            }
        }
        else
        {
            
            enemyHealth = null;           
        }

   
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Walking"))
        {
            attack1Done = false;
            attack2Done = false;
            attack3Done = false;
        }
        
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack3"))
        {
            animator.SetBool("Attack1", false);
        }        
    }

    private IEnumerator AttackWithDelay(float seconds, float damage)
    {
        yield return new WaitForSeconds(seconds);
        if (enemyHealth != null) enemyHealth.EnemyTakeDamage(damage);
    }

   
    public void return1()
    {
        if (numberOfClicks >= 2)
        {
            animator.SetBool("Attack2", true);
        }
        else
        {
            animator.SetBool("Attack1", false);
            numberOfClicks = 0;
        }
    }
    public void return2()
    {
        if (numberOfClicks >= 3)
        {
            animator.SetBool("Attack3", true);
        }
        else
        {
            animator.SetBool("Attack1", false);
            animator.SetBool("Attack2", false);
            numberOfClicks = 0;
        }
    }
    public void return3()
    {
        animator.SetBool("Attack1", false);
        animator.SetBool("Attack2", false);
        animator.SetBool("Attack3", false);
        numberOfClicks = 0;
    }

   
}
