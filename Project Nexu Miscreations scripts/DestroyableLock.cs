using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableLock : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        Destroy(gameObject);
    }

}
