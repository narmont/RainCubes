using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Platform _platform;
    [SerializeField] private Cube _prefabCube;
    [SerializeField] private float _spawnHeight = 10f;
    [SerializeField] private float _spawnInterval = 5f;
    [SerializeField] private int _poolCapacity = 5;
    [SerializeField] private int _poolMaxSize = 5;

    private IObjectPool<Cube> _pool;
    private WaitForSeconds _spawnWait;

    private void Awake()
    {
        _spawnWait = new WaitForSeconds(_spawnInterval);
        InitializePool();
    }

    private void Start()
    {
        StartCoroutine(SpawnCubeRoutine());
    }

    private void InitializePool()
    {
        _pool = new ObjectPool<Cube>(
            createFunc: CreateCube,
            actionOnGet: PrepareCube,
            actionOnRelease: DisableCube,
            actionOnDestroy: DestroyCube,
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize
        );

        for (int i = 0; i < _poolCapacity; i++)
        {
            Cube cube = _pool.Get();
            _pool.Release(cube);
        }
    }

    private Cube CreateCube()
    {
        Cube cube = Instantiate(_prefabCube);
        cube.LifeEnded += HandleLifeEnded;
        return cube;
    }

    private void PrepareCube(Cube cube)
    {
        cube.gameObject.SetActive(true);
        cube.ResetState();
    }

    private void DisableCube(Cube cube)
    {
        cube.gameObject.SetActive(false);
    }

    private void DestroyCube(Cube cube)
    {
        cube.LifeEnded -= HandleLifeEnded;
        Destroy(cube.gameObject);
    }

    private void HandleLifeEnded(Cube cube)
    {
        _pool.Release(cube);
    }

    private IEnumerator SpawnCubeRoutine()
    {
        while (enabled)
        {
            yield return _spawnWait;
            SpawnCube();
        }
    }

    private void SpawnCube()
    {
        Cube cube = _pool.Get();
        
        if (cube != null)
        {
            cube.transform.position = GetRandomPosition();
        }
    }

    private Vector3 GetRandomPosition()
    {
        Vector3 position = _platform.transform.position;
        Vector3 halfScale = _platform.transform.localScale * 0.5f;

        float randomX = Random.Range(position.x - halfScale.x, position.x + halfScale.x);
        float spawnY = position.y + _spawnHeight;
        float randomZ = Random.Range(position.z - halfScale.z, position.z + halfScale.z);

        return new Vector3 (randomX, spawnY, randomZ);
    }
}
