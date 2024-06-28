using UnityEngine.Pool;
using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Cube _prefabCube;
    [SerializeField] private Transform[] _startPositions;
    [SerializeField] private float _repeatRate = 1f;
    [SerializeField] private int _poolCapacity = 5;
    [SerializeField] private int _poolMaxSize = 5;

    private ObjectPool<Cube> _pool;

    private void Awake()
    {
        _pool = new ObjectPool<Cube>(
        createFunc: () => CreateCube(),
        actionOnGet: PrepareCube,
        actionOnRelease: (cube) => cube.gameObject.SetActive(false),
        actionOnDestroy: (cube) => Destroy(cube),
        collectionCheck: true,
        defaultCapacity: _poolCapacity,
        maxSize: _poolMaxSize);
    }

    private void Start()
    {
        StartCoroutine(SpawnWithRepeatRate());
    }

    private void ReleaseCube(Cube cube)
    {
        _pool.Release(cube);
    }

    private Cube CreateCube()
    {
        Cube cube = Instantiate(_prefabCube);
        cube.Released += ReleaseCube;
        return cube;
    }

    private void GetCube()
    {
        _pool.Get();
    }

    private void PrepareCube(Cube cube)
    {
        cube.InitVelocity(Vector3.up);
        cube.transform.position = _startPositions[Random.Range(0, _startPositions.Length)].position;
        cube.gameObject.SetActive(true);
    }

    private IEnumerator SpawnWithRepeatRate()
    {
        while (true)
        {
            GetCube();
            yield return new WaitForSeconds(_repeatRate);
        }
    }
}
