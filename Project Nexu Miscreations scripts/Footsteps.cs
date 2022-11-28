using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Footsteps : MonoBehaviour
{
    AudioSource audioSource;
    Animator animator;

    [SerializeField] AudioClip[] concreteFootsteps;
    [SerializeField] AudioClip[] grassFootsteps;
    [SerializeField] AudioClip[] waterFootsteps;
    [SerializeField] AudioClip[] solidMetalFootsteps;
    [SerializeField] AudioClip[] hollowMetalFootsteps;
    [SerializeField] AudioClip[] woodFootsteps;

    [SerializeField] Transform groundCheck;

    float timer;
    [SerializeField] float timeBetweenSteps = 0.4f;
    public bool isFootstepping;

    [SerializeField] LayerMask layerMask;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponentInParent<Animator>();        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Walking")) isFootstepping = true;
        else isFootstepping = false;
        if (isFootstepping) GetFootsteps();
        
    }

    private void GetFootsteps()
    {
        RaycastHit hit;
        Collider[] hitGround = Physics.OverlapSphere(groundCheck.position, 0.4f, layerMask);
        if (hitGround.Length > 0 && (Input.GetAxisRaw("Horizontal") != 0 | Input.GetAxisRaw("Vertical") != 0))
        {            
            if (timer > timeBetweenSteps)
            {
                if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out hit, Mathf.Infinity, layerMask))
                {
                    if (hit.collider.tag == "Untagged") PlayFootsteps(concreteFootsteps);
                    if (hit.collider.tag == "Grass") PlayFootsteps(grassFootsteps);
                    if (hit.collider.tag == "Water") PlayFootsteps(waterFootsteps);
                    if (hit.collider.tag == "SolidMetal") PlayFootsteps(solidMetalFootsteps);
                    if (hit.collider.tag == "HollowMetal") PlayFootsteps(hollowMetalFootsteps);
                    if (hit.collider.tag == "Wood") PlayFootsteps(woodFootsteps);
                }
            }
        }
    }

    private void PlayFootsteps(AudioClip[] footstepArray)
    {
        timer = 0;
        AudioClip clip = footstepArray[Random.Range(0, footstepArray.Length)];
        audioSource.pitch = Random.Range(0.7f, 1.3f);
        audioSource.PlayOneShot(clip);
    }
}
