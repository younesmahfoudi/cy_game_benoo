using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPool
{
    void PoolObject(IPoolable poolable);
}

public interface IPoolable
{
    IPool ParentPool { get; }
    void ReturnToPool();
    GameObject gameObject { get; }
}

public class ZombieSpawner : MonoBehaviour, IPool
{
    public GameObject[] zombieList;

    public LevelManager levelManager = null;
    public float minSpawnRatePerSecond = 0.1f;
    public float maxSpawnRatePerSecond = 0.7f;
    public float zombieMinSpeed = 0.3f;
    public float zombieMaxSpeed = 0.9f;
    public int gemDropRating = 10;
    private float _timeBetweenSpawns = 0f;
    private float _spawnCounter = 0f;
    private System.Random _random = ServiceProvider.random;

    //pooling system
    Stack<GameObject> pool = new Stack<GameObject>();

    void Start()
    {
        if (GameSaveInstance.Instance)
        {
            minSpawnRatePerSecond = GameSaveInstance.Instance.currentDifficultySettings.spawnerMinFrequency;
            maxSpawnRatePerSecond = GameSaveInstance.Instance.currentDifficultySettings.spawnerMaxFrequency;
            zombieMinSpeed = GameSaveInstance.Instance.currentDifficultySettings.zombiesMinSpeed;
            zombieMaxSpeed = GameSaveInstance.Instance.currentDifficultySettings.zombiesMaxSpeed;
            gemDropRating = GameSaveInstance.Instance.currentDifficultySettings.gemDropRating;
        }

        SetRandomSpawnTime();
    }

    void Update()
    {
        if (levelManager.gameStatus == GameStatus.GAMEOVER)
            return;
        _spawnCounter += Time.deltaTime;

        if (_spawnCounter > _timeBetweenSpawns)
        {
            _spawnCounter = 0;
            Spawn();
            SetRandomSpawnTime();
        }
    }

    private void SetRandomSpawnTime()
    {
        float _spawnRate = (float)_random.NextDouble() * (maxSpawnRatePerSecond - minSpawnRatePerSecond);
        _spawnRate += minSpawnRatePerSecond;
        _timeBetweenSpawns = 1 / _spawnRate;
    }

    private void Spawn()
    {
        GameObject _zombie;
        Vector3 position = GetSpawnLocation();
        Quaternion rotation = GetFacingDirection(position);

        if (pool.Count > 0)
        {
            _zombie = pool.Pop();
            _zombie.gameObject.SetActive(true);
            _zombie.transform.position = position;
            _zombie.transform.rotation = rotation;
            _zombie.GetComponent<ZombieScript>().SetInitialState();
        }
        else
        {
            int randNbr = _random.Next(zombieList.Length);
            _zombie = Instantiate(zombieList[randNbr], position, rotation);
            _zombie.GetComponent<ZombieScript>().SetLevelManager(levelManager);
            _zombie.GetComponent<ZombieScript>().SetFirstInstance(this, zombieMinSpeed, zombieMaxSpeed, gemDropRating);
        }
    }

    private Vector3 GetSpawnLocation()
    {
        float span = 7f;
        var randomModifier = Random.Range(0.0f, 1.0f);
        randomModifier *= span;
        randomModifier -= span / 2;

        var current = transform.position;

        return new Vector3(current.x + (float)randomModifier, current.y, 0);
    }

    private Quaternion GetFacingDirection(Vector3 direction)
    {
        if (direction.x > 0)
            return Quaternion.Euler(0, 0, 0);
        return Quaternion.Euler(0, 180, 0);
    }

    //IPool interface
    public void PoolObject(IPoolable poolable)
    {
        poolable.gameObject.SetActive(false);
        pool.Push(poolable.gameObject);
    }
}
