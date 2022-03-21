using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BroadswordScript : WeaponScript
{
    public AudioClip attackAC;
    bool _hasHitEnnemy = false;
    float _damage = 1;

    public override void Attack()
    {
        SoundManager.Instance.Play(attackAC, SoundManager.SoundPriority.NORMAL);
        _hasHitEnnemy = false;
        ChangeStatus(StatusName.ATTACKING);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(TagNames.ennemy))
        {
            _hasHitEnnemy = true;
            collision.GetComponent<EnnemyScript>().Hit(_damage);
        }
    }

    //called in end of attack animation by animation event
    public void PostAttackCheck()
    {
        if (!_hasHitEnnemy)
        {
            ChangeStatus(StatusName.MISSED);
            StartCoroutine(Missed());
        }
        else
            Idle();
    }

    public override void Idle()
    {
        ChangeStatus(StatusName.IDLE);
    }

    public override void Die()
    {
        ChangeStatus(StatusName.DIE);
    }

    private IEnumerator Missed()
    {
        yield return new WaitForSeconds(1);
        ChangeStatus(StatusName.IDLE);
    }
}
