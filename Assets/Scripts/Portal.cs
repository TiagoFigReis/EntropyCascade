using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    private float cooldown, lastMonster;
    
    [SerializeField] private List<GameObject> monsters;
    void Start()
    {
        cooldown = Random.Range(2, 4);
    }
    
    void Update()
    {
        Spawn();
    }

    void Spawn()
    {
        if (lastMonster + cooldown > Time.time) return;
        
        lastMonster = Time.time;
        
        int intervals = Mathf.FloorToInt(Time.timeSinceLevelLoad / 20f);
        float spawnRateMultiplier = Mathf.Pow(0.9f, intervals);

        float minCooldown = 6f * spawnRateMultiplier;
        float maxCooldown = 10f * spawnRateMultiplier;
        cooldown = Random.Range(minCooldown, maxCooldown);
        
        int monster = Random.Range(0, monsters.Count);
        
        Instantiate(monsters[monster], transform.position, Quaternion.identity);
    }
}