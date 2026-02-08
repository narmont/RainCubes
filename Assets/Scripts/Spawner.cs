using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Platform _platform;
    [SerializeField] private CubePool _cubePool;

    [SerializeField] private float _spawnHeight = 10f;
    [SerializeField] private float _spawnInterval = 5f;

    private void Start()
    {
        StartCoroutine(SpawnCubeRoutine());
    }

    private IEnumerator SpawnCubeRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(_spawnInterval);

            Cube cube = _cubePool.GetCube();

            if (cube != null)
            {
                cube.transform.position = GetRandomPosition();
            }
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
