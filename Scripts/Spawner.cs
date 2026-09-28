using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Platform platform;
    [SerializeField] private Cube _cubePrefab;
    private int _poolCapacity = 18;
    private int _poolMaxSize = 18;
    private ObjectPool<Cube> _cubePool;

    private void Awake()
    {
        _cubePool = new ObjectPool<Cube>(
            createFunc: () => Instantiate(_cubePrefab),
            actionOnGet: (cube) => ActionOnGet(cube),
            actionOnRelease: (cube) => cube.gameObject.SetActive(false),
            actionOnDestroy: (cube) => Destroy(cube),
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize);    
    }

    private void Start()
    {
        StartCoroutine(SpawnWithDelay());
    }

    private Cube GetCube()
    {
        Cube cube = _cubePool.Get();
        return cube;
    }

    private IEnumerator SpawnWithDelay()
    {
        bool corutineWork = true;

        while (corutineWork)
        {
            float timeRepeatRate = 0.5f;
            yield return new WaitForSeconds(timeRepeatRate);
            GetCube();
        }        
    }

    public void DestroyCube(Cube cube)
    {
        cube.Felled -= DestroyCube;
        _cubePool.Release(cube);
    }

    private void ActionOnGet(Cube cube)
    {
        float coefficientDivision = 2f;

        float _spawnPointX = platform.PositionByX / coefficientDivision;
        float _spawnPointY = 10f;
        float _spawnPointZ = platform.PositionByZ / coefficientDivision;

        cube.transform.position = new Vector3(UnityEngine.Random.Range(-_spawnPointX, _spawnPointX), _spawnPointY, UnityEngine.Random.Range(-_spawnPointZ, _spawnPointZ));
        cube.SetStandartColor();
        cube.gameObject.SetActive(true);

        cube.Felled += DestroyCube;
    }
}
