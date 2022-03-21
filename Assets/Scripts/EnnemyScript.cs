using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnnemyScript : MonoBehaviour, IPoolable
{
    protected enum StatusName { ATTACKING, DYING, WALKING, FLEEING, HURT};
    public Animator animator;
    protected Vector3 _directionTowardPlayer;
    protected StatusName _status;
    public LevelManager levelManager;
    protected bool _isFacingPlayer = true;

    public IPool ParentPool { get; set; }

    public abstract void Hit(float damage);

    private void Start()
    {
        SetInitialState();
    }

    protected void FlipSprites()
    {
        if (transform.eulerAngles.y == 180)
            transform.rotation = Quaternion.Euler(transform.rotation.x, 0, transform.rotation.z);
        else
            transform.rotation = Quaternion.Euler(transform.rotation.x, 180, transform.rotation.z);
        _directionTowardPlayer = -_directionTowardPlayer;
        _isFacingPlayer = _isFacingPlayer ? false: true;
    }

    private void ResetStatus()
    {
        animator.SetBool("Attack", false);
        animator.SetBool("Die", false);
    }

    protected void ChangeStatus(StatusName sn)
    {
        _status = sn;
        ResetStatus();
        switch (sn)
        {
            case StatusName.ATTACKING:
                animator.SetBool("Attack", true);
                break;
            case StatusName.DYING:
                animator.SetBool("Die", true);
                break;
            default:
                break;
        }
    }

    public void Destroy()
    {
        if (levelManager)
            levelManager.IncreaseKillCounter();
        if (ParentPool != null)
        {
            ReturnToPool();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void ReturnToPool()
    {
        ParentPool?.PoolObject(this);
    }

    public void SetLevelManager(LevelManager lm)
    {
        levelManager = lm;
    }

    public abstract void SetInitialState();
}
