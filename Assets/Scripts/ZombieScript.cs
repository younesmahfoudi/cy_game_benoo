using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieScript : EnnemyScript
{
    public AudioClip attackAC;
    public AudioClip dieAC;
    public GameObject gemPrefab;
    public float maxHealth = 1;
    private float health = 1;
    private float speed = 3;
    public float maxSpeed = 4;
    public float minSpeed = 2;
    private int _gemDropRate = 10;

    //use flee after successful hit
    private float _fleeingSpeed = 20;
    private float _fleeTimer = 0.4f;

    private System.Random _random = ServiceProvider.random;

    public void SetFirstInstance(IPool parPool, float minSpd, float maxSpd, int gemdrop)
    {
        SpriteRenderer[] sprites = this.GetComponentsInChildren<SpriteRenderer>(true);
        int addedLayers = levelManager.GetNewEnnemyLayer();

        //prevent overlapping between ennemies
        for (int i = 0; i < sprites.Length; i++)
            sprites[i].sortingOrder += addedLayers;

        ParentPool = parPool;
        minSpeed = minSpd;
        maxSpeed = maxSpd;
        _gemDropRate = gemdrop;
    }

    public override void SetInitialState()
    {
        health = maxHealth;
        ChangeStatus(StatusName.WALKING);
        SetTargetDirection();

        speed = (float)_random.NextDouble() * (maxSpeed - minSpeed);
        speed += minSpeed;
    }
    private void FixedUpdate()
    {
        //zombies will flee if the game is won
        if (levelManager.gameStatus == GameStatus.GAMEOVER && levelManager.IsGameWon())
        {
            if (_isFacingPlayer)
                FlipSprites();
            _fleeTimer = 100f;
            ChangeStatus(StatusName.FLEEING);
        }

        //Normal walking toward player
        if (_status == StatusName.WALKING)
            transform.position += _directionTowardPlayer * Time.deltaTime * speed;
        //Used after a successful hit, will return to walking after some time
        else if (_status == StatusName.FLEEING)
        {
            _fleeTimer -= Time.deltaTime;
            transform.position += _directionTowardPlayer * Time.deltaTime * _fleeingSpeed;

            if (_fleeTimer <= 0)
            {
                FlipSprites();
                ChangeStatus(StatusName.WALKING);
            }
        }
    }

    //Will go to fleeing after animation ends with animation event
    private void Attack(GameObject player)
    {
        SoundManager.Instance.Play(attackAC, SoundManager.SoundPriority.COMMON);
        ChangeStatus(StatusName.ATTACKING);
        player.GetComponent<HeroScript>().Hit();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag(TagNames.player))
            Attack(collision.gameObject);
    }

    public override void Hit(float damage)
    {
        health -= damage;

        //Call to Destroy() after animation end with animation event
        if (health <= 0)
        {
            SoundManager.Instance.Play(dieAC, SoundManager.SoundPriority.COMMON);
            ChangeStatus(StatusName.DYING);
        }
    }

    //initialize sprite and timer and update status for fleeing
    public void PostAttackMovement()
    {
        //make their attack movement repeat to put impression that they are feasting on the player
        if (levelManager.gameStatus == GameStatus.GAMEOVER || levelManager.gameStatus == GameStatus.EXIT)
            return;

        FlipSprites();
        _fleeTimer = 0.4f;
        ChangeStatus(StatusName.FLEEING);
    }

    private void SetTargetDirection()
    {
        if (transform.position.x < 0)
            _directionTowardPlayer = transform.right;
        else
            _directionTowardPlayer = -transform.right;

        if (transform.eulerAngles.y == 180)
            _directionTowardPlayer = -_directionTowardPlayer;
    }

    //Called by the death animation with anim event
    private void GemDropCheck()
    {
        if (!levelManager || _random.Next(0, 100) > _gemDropRate)
            return;
        GameObject gem = Instantiate(gemPrefab);
        gem.transform.position = this.transform.position;
        gem.GetComponent<GemScript>().Throw(_directionTowardPlayer.x > 0 ? false : true);
        levelManager.IncreaseGemCounter();
    }
}
