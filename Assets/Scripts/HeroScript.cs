using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HeroStatus { Attack, Idle, Dead, Missed };

public class HeroScript : MonoBehaviour
{
    public AudioClip missedAC;
    public AudioClip dieAC;
    public WeaponScript weaponGO;
    public Animator heroAnimator;
    public Transform parentGOTrans;
    public LevelManager levelManager;
    private HeroStatus _status = HeroStatus.Idle;


    void Update()
    {
        if (levelManager.gameStatus != GameStatus.PLAY)
            return;
        if (_status == HeroStatus.Idle && InputManager.Instance.GetcurrentKey(onlyPressed: true) == InputNames.left)
        {
            ChangeSide(isLeftWanted: true);
            ChangeStatus(HeroStatus.Attack);
        }
        if (_status == HeroStatus.Idle && InputManager.Instance.GetcurrentKey(onlyPressed: true) == InputNames.right)
        {
            ChangeSide(isLeftWanted: false);
            ChangeStatus(HeroStatus.Attack);
        }
    }

    private void ChangeSide(bool isLeftWanted)
    {
        int dir = isLeftWanted ? 0 : 180;
        if (parentGOTrans.eulerAngles.y != dir)
            parentGOTrans.rotation = Quaternion.Euler(0, dir, 0);
    }

    public void ChangeStatus(HeroStatus newStatus)
    {
        ResetStatus();
        switch (newStatus)
        {
            case HeroStatus.Attack:
                heroAnimator.SetBool("Attack", true);
                weaponGO.Attack();
                break;
            case HeroStatus.Dead:
                SoundManager.Instance.Play(dieAC, SoundManager.SoundPriority.IMPORTANT);
                heroAnimator.SetBool("Die", true);
                weaponGO.Die();
                levelManager.ChangeGameStatus(GameStatus.GAMEOVER);
                break;
            case HeroStatus.Missed:
                SoundManager.Instance.Play(missedAC, SoundManager.SoundPriority.IMPORTANT);
                heroAnimator.SetBool("Attack", true);
                heroAnimator.SetBool("Missed", true);
                break;
            default:
                break;
        }

        _status = newStatus;
    }

    public void Hit()
    {
        if (levelManager.gameStatus == GameStatus.PLAY)
            ChangeStatus(HeroStatus.Dead);
    }

    private void ResetStatus()
    {
        heroAnimator.SetBool("Attack", false);
        //heroAnimator.SetBool("Die", false);
        heroAnimator.SetBool("Missed", false);

    }
}