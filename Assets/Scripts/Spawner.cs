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
            actionOnGet: OnGetCube,
            actionOnRelease: OnReleaseCube,
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
        cube.OnReturnToPoolRequested += HandleReturnToPool;
        return cube;
    }

    private void OnGetCube(Cube cube)
    {
        cube.gameObject.SetActive(true);
        cube.ResetState();
    }

    private void OnReleaseCube(Cube cube)
    {
        cube.gameObject.SetActive(false);
    }

    private void DestroyCube(Cube cube)
    {
        cube.OnReturnToPoolRequested -= HandleReturnToPool;
        Destroy(cube.gameObject);
    }

    private void HandleReturnToPool(Cube cube)
    {
        _pool.Release(cube);
    }

    private IEnumerator SpawnCubeRoutine()
    {
        while (true)
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
        Vector3 size = _platform.transform.localScale;

        float randomX = Random.Range(position.x - size.x / 2, position.x + size.x / 2);
        float spawnY = position.y + _spawnHeight;
        float randomZ = Random.Range(position.z - size.z / 2, position.z + size.z / 2);

        return new Vector3 (randomX, spawnY, randomZ);
    }
}
