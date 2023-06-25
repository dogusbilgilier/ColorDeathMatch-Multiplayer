using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollController : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] Rigidbody[] rigidbodies;
    [SerializeField] Collider[] colliders;
    [SerializeField] Collider playerCollider;

    [Button]
    void EnableRagdoll()
    {
        playerCollider.enabled = false;
        animator.enabled = false;
        foreach (Rigidbody rb in rigidbodies)
        {
            rb.isKinematic = false;
            
        }
        foreach (Collider collider in colliders)
        {
            collider.enabled = true;
        }
    }
    [Button]
    void DisableRagdoll()
    {
        playerCollider.enabled = true;
        animator.enabled = true;
        foreach (Rigidbody rb in rigidbodies)
        {
            rb.isKinematic = true;
        }
        foreach (Collider collider in colliders)
        {
            collider.enabled = false;
        }
    }
}
