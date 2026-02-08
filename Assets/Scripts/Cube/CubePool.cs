using UnityEngine;
using UnityEngine.Pool;

public class CubePool : MonoBehaviour
{
    [SerializeField] private Cube _prefabCube;
    [SerializeField] private int _poolCapacity = 5;
    [SerializeField] private int _poolMaxSize = 5;

    private IObjectPool<Cube> _pool;

    private void Awake()
    {
        _pool = new ObjectPool<Cube>(
        createFunc: () =>
                    {
                        Cube cube = Instantiate(_prefabCube);
                        cube.SetPool(this);
                        return cube;
                    },
        actionOnGet: cube => {

            cube.gameObject.SetActive(true);
            cube.ResetState();
        },
        actionOnRelease: cube => cube.gameObject.SetActive(false),
        actionOnDestroy: cube => Destroy(cube.gameObject),
        collectionCheck: true,
        defaultCapacity: _poolCapacity,
        maxSize: _poolMaxSize
        );

        CreatePool();
    }

    public Cube GetCube() => _pool.Get();
    public void ReturnCube(Cube cube) => _pool.Release(cube);

    private void CreatePool()
    {
        Cube[] cubes = new Cube[_poolCapacity];

        for(int i = 0; i < _poolCapacity; i++)
        {
            cubes[i] = GetCube();
            _pool.Release(cubes[i]);
        }
    }
}
