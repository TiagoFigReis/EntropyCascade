using System;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

[System.Serializable]
public struct SawPath
{
    public Transform start;
    public Transform end;
}

public class SawSpawner : MonoBehaviour
{
    [SerializeField] private GameObject sawPrefab, warningPrefab;
    [SerializeField] private SawPath[] sawPaths;

    [SerializeField] private float minSpawnTime = 5f;
    [SerializeField] private float maxSpawnTime = 30f;

    private float spawnTimer;

    void Start()
    {
        SetNextSpawnTime();
    }

    void Update()
    {
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0)
        {
            SpawnSaw();
            SetNextSpawnTime();
        }
    }

    void SetNextSpawnTime()
    {
        spawnTimer = Random.Range(minSpawnTime, maxSpawnTime);
    }

    void SpawnSaw()
    {
        if (sawPaths == null || sawPaths.Length == 0)
        {
            return;
        }
        
        int index = Random.Range(0, sawPaths.Length);
        SawPath selectedPath = sawPaths[index];
        
        StartCoroutine(SpawnSequence(selectedPath));
    }
    
    private IEnumerator SpawnSequence(SawPath path)
    {
        Vector2 spawnPos = new Vector2(path.start.position.x, path.start.position.y);
        Vector2 targetPos = new Vector2(path.end.position.x, path.end.position.y);
        
        Vector2 warningPos = new Vector2(path.start.position.x, path.start.position.y + 0.2f);
        
        GameObject warningInstance = Instantiate(warningPrefab, warningPos, Quaternion.identity);
        
        Destroy(warningInstance, 0.8f);
        
        yield return new WaitForSeconds(2f);
        
        GameObject sawInstance = Instantiate(sawPrefab, spawnPos, Quaternion.identity);
        
        Saw controller = sawInstance.GetComponentInChildren<Saw>();
        if (controller != null)
        {
            controller.Initialize(targetPos);
        }
    }
}