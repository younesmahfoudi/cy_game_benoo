using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponScript : MonoBehaviour
{
    protected enum StatusName { ATTACKING, IDLE, DIE, MISSED };
    public HeroScript heroScript;
    public Animator animator;

    public void ResetStatus()
    {
        animator.ResetTrigger("Attacking");
        animator.SetBool("Die", false);
        animator.SetBool("Missed", false);
    }

    protected void ChangeStatus(StatusName sn)
    {
        ResetStatus();
        switch (sn)
        {
            case StatusName.ATTACKING:
                animator.SetTrigger("Attacking");
                break;
            case StatusName.IDLE:
                heroScript.ChangeStatus(HeroStatus.Idle);
                break;
            case StatusName.DIE:
                animator.SetBool("Die", true);
                break;
            case StatusName.MISSED:
                heroScript.ChangeStatus(HeroStatus.Missed);
                animator.SetBool("Missed", true);
                break;
            default:
                break;
        }
    }

    public abstract void Die();

    public abstract void Attack();

    public abstract void Idle();

}
