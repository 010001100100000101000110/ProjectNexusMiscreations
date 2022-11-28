using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayerNoises : MonoBehaviour
{
    private AudioSource audioSource;
    Animator animator;

    //Human noises
    [SerializeField] SOPlayerSettings playerSettings;
    [SerializeField] AudioClip deathSound;
    [SerializeField] AudioClip[] hurtSounds;
    [SerializeField] AudioClip[] jumpSounds;
    //[SerializeField] AudioClip[] breathingSounds;
    [SerializeField] AudioClip[] attackSounds;

    bool hasDied;
    bool gotHurt;
    bool hasJumped;

    float breathStart;
    float breathCooldown;

    bool attack1Done;
    bool attack2Done;
    bool attack3Done;

    //Electricity noises
    [SerializeField] AudioClip takePowerSound;
    [SerializeField] AudioClip[] whipSounds;
    [SerializeField] AudioClip dashSound;

    bool hasTakenPower;
    bool hasDashed;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        breathCooldown = Random.Range(4, 8);
        breathStart = Time.time + breathCooldown;
    }

    void Update()
    {
        ResetBools();
        DeathSound();
        HurtSound();
        JumpSound();
        //BreathingSounds();
        AttackSounds();
        PlaySuckPowerSound();
        DashWoosh();
    }

    //Human noises
    private void DeathSound()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("DEATH") && hasDied == false)
        {
            hasDied = true;
            audioSource.PlayOneShot(deathSound);
        }
    }
    private void HurtSound()
    {               
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("GetHurt") && gotHurt == false)
        {
            gotHurt = true;
            AudioClip clip = hurtSounds[Random.Range(0, hurtSounds.Length)];
            audioSource.PlayOneShot(clip);
        }       
    }
    private void JumpSound()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("JumpBoing") && hasJumped == false)
        {
            hasJumped = true;
            AudioClip clip = jumpSounds[Random.Range(0, jumpSounds.Length)];
            audioSource.PlayOneShot(clip);
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("JumpLand")) hasJumped = false;
    }

    //private void BreathingSounds()
    //{
    //    if (animator.GetCurrentAnimatorStateInfo(0).IsName("Walking"))
    //    {
    //        if (breathStart <= Time.time)
    //        {
    //            breathStart = Time.time + breathCooldown;
    //            AudioClip clip = breathingSounds[Random.Range(0, breathingSounds.Length)];
    //            audioSource.PlayOneShot(clip);
    //        }
    //    }
           
    //}

    private void DashWoosh()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Dash") && hasDashed == false)
        {
            hasDashed = true;            
            audioSource.PlayOneShot(dashSound);
        }
    }

    //human noise and whip noise
    private void AttackSounds()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") && attack1Done == false)
        {
            attack1Done = true;
            StartCoroutine(AttackNoiseWithDelay(playerSettings.Attack1GruntDelay, attackSounds, whipSounds));         
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack2") && attack2Done == false)
        {
            attack2Done = true;
            StartCoroutine(AttackNoiseWithDelay(playerSettings.Attack2GruntDelay, attackSounds, whipSounds));
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack3") && attack3Done == false)
        {
            attack3Done = true;
            StartCoroutine(AttackNoiseWithDelay(playerSettings.Attack3GruntDelay, attackSounds, whipSounds));
        }

    }

    private IEnumerator AttackNoiseWithDelay(float seconds, AudioClip[] attackAudioClips, AudioClip[] whipSounds)
    {
        yield return new WaitForSeconds(seconds);
        AudioClip clip = attackAudioClips[Random.Range(0, attackAudioClips.Length)];
        AudioClip clip2 = whipSounds[Random.Range(0, whipSounds.Length)];
        audioSource.PlayOneShot(clip);       
        audioSource.PlayOneShot(clip2);        
    }


    //Electricity noises

    private void PlaySuckPowerSound()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("SuckPower") && hasTakenPower == false)
        {
            hasTakenPower = true;
            audioSource.PlayOneShot(takePowerSound);
        }
    }

    private void ResetBools()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Walking"))
        {
            gotHurt = false;
            hasJumped = false;
            hasDashed = false;

            attack1Done = false;
            attack2Done = false;
            attack3Done = false;

            hasTakenPower = false;

        }
    }
}
