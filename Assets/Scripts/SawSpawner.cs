using UnityEngine;

[System.Serializable]
public class SawPath
{
    public float xInitial;
    public float xFinal;
    public float y;
}

public class SawSpawner : MonoBehaviour
{
    [SerializeField] private GameObject sawPrefab;
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
        
        Vector2 spawnPos = new Vector2(selectedPath.xInitial, selectedPath.y);
        Vector2 targetPos = new Vector2(selectedPath.xFinal, selectedPath.y);
        
        
        GameObject sawInstance = Instantiate(sawPrefab, spawnPos, Quaternion.identity);
        
        Saw controller = sawInstance.GetComponentInChildren<Saw>();
        if (controller != null)
        {
            controller.Initialize(targetPos);
        }
    }
}
