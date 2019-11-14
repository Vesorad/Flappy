using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Parallaxer : MonoBehaviour
{
    public class PoolObject
    {
        public Transform transform;
        public bool inUse;
        public PoolObject(Transform t) { transform = t; }
        public void Use() { inUse = true; }
        public void Dispose() { inUse = false; }

    }
    [System.Serializable]
    public struct YSpawnRange 
        {
        public float min;
        public float max;
        }
    

    public GameObject PreFab;
    public int poolSize;
    public float shiftSpeed;
    public float spawnRate;

    public YSpawnRange ySpawnRange;
    public Vector3 defultSpawnPos;
    public bool spawnImmediate;
    public Vector3 ImmediateSpawnPos;
    public Vector2 targerAspectRatio;

    float spawnTimer;
    float targerAspect;
    PoolObject[] poolObjects;
    GameMenager game;

    private void Awake()
    {
        Configure();  
    }
    private void Start()
    {
        game = GameMenager.Instance;
    }
    private void OnEnable()
    {
        GameMenager.OnGameOverConfirmed += OnGameOverConfirmed;
    }
    private void OnDisable()
    {
        GameMenager.OnGameOverConfirmed += OnGameOverConfirmed;
    }
    void OnGameOverConfirmed()
    {
        for (int i = 0; i < poolObjects.Length; i++)
        {
            poolObjects[i].Dispose();
            poolObjects[i].transform.position = Vector3.one * 1000;
        }
        if (spawnImmediate)
        {
            SpawnImmediate();
        }
    }
    private void Update()
    {
        if (game.GameOver1) return;
        Shift();
        spawnTimer += Time.deltaTime;
        
        if (spawnTimer > spawnRate)
        {
            Spawn();
            spawnTimer = 0;
        }
        
    }
    void Configure()
    {
        targerAspect = targerAspectRatio.x / targerAspectRatio.y;
        poolObjects = new PoolObject[poolSize];
        
        for (int i = 0; i < poolObjects.Length; i++)
        {
            GameObject go = Instantiate(PreFab) as GameObject;
            Transform t = go.transform;
            t.SetParent(transform);
            t.position = Vector3.one * 1000;
            poolObjects[i] = new PoolObject(t);
        }
        if (spawnImmediate)
        {
            SpawnImmediate();
        }
    }
    void Spawn()
    {
        Transform t = GetPoolObject();
        if (t == null) return;
        {
            Vector3 pos = Vector3.zero;
            pos.x = (defultSpawnPos.x * Camera.main.aspect) / targerAspect;
            pos.y = Random.Range(ySpawnRange.min, ySpawnRange.max);
            t.position = pos;
        }
    }
    void SpawnImmediate()
    {
        Transform t = GetPoolObject();
        if (t == null) return;
        {
            Vector3 pos = Vector3.zero;
            pos.x = (ImmediateSpawnPos.x * Camera.main.aspect) / targerAspect;
            pos.y = Random.Range(ySpawnRange.min, ySpawnRange.max);
            t.position = pos;
            Spawn();
        }
    }
    void Shift()
    {
        for (int i = 0; i < poolObjects.Length; i++)
        {
            poolObjects[i].transform.localPosition += -Vector3.right * shiftSpeed * Time.deltaTime;
            CheckDisposeObject(poolObjects[i]);
        }
    }
    void CheckDisposeObject(PoolObject poolObject)
    {
if (poolObject.transform.position.x < (-defultSpawnPos.x * Camera.main.aspect) / targerAspect)
        {
            poolObject.Dispose();
            poolObject.transform.position = Vector3.one * 1000;
        }
    }
    Transform GetPoolObject()
    {
        for (int i = 0; i < poolObjects.Length; i++)
        {
            if (!poolObjects[i].inUse)
            {
                poolObjects[i].Use();
                return poolObjects[i].transform;
            }
        }
        return null;
    }

}
