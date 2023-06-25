using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public Animator animator;
    [SerializeField] float fastRunSpeed,runspeed;

    public void RunAnim(bool state)
    {
        animator.SetBool("Run", state);
        animator.SetFloat("RunSpeed", runspeed);
    }

    public void SetRunAnimMovement(Vector2 moveVector)
    {
        animator.SetFloat("XSpeed", moveVector.x);
        animator.SetFloat("YSpeed", moveVector.y);
        Vector2 myPos = new Vector2(transform.position.x, transform.position.z);
        Vector2 offset = myPos + moveVector.normalized;
    }

    public void IdleAnim(bool state)
    {
        animator.SetBool("Idle", state);
    }

    public void BoostAnim(bool state)
    {
        animator.SetBool("Run", state);
        animator.SetFloat("RunSpeed", fastRunSpeed);
    }

    public void JumpAnim()
    {
        animator.SetTrigger("Jump");
    }

    public void AimAnim(bool state)
    {
        animator.SetBool("Aim",state);
    }

    float MoveAngle(Vector2 dir)
    {
     
            return 180 - (Vector2.SignedAngle(Vector2.up, dir.normalized));
        
        
    }
}
