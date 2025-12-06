using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    private float cooldown, lastMonster;
    private float direction;
    
    [SerializeField] private List<GameObject> monsters;
    
    public void Init(float directionSpawn)
    {
        direction = directionSpawn;
    }
    
    void Start()
    {
        cooldown = Random.Range(2, 4);

        if (direction != 0f) return;
        
        if (transform.position.x < 0) direction = 1;
        else direction = -1;
    }
    
    void Update()
    {
        Spawn();
    }

    void Spawn()
    {
        if (lastMonster + cooldown > Time.time) return;
        
        lastMonster = Time.time;
        
        int intervals = Mathf.FloorToInt(Time.timeSinceLevelLoad / 60f);
        float spawnRateMultiplier = Mathf.Pow(0.9f, intervals);

        float minCooldown = 6f * spawnRateMultiplier;
        float maxCooldown = 12f * spawnRateMultiplier;
        cooldown = Random.Range(minCooldown, maxCooldown);
        
        int monster = Random.Range(0, monsters.Count);
        print(monster);
        
        GameObject monsterInstance = Instantiate(monsters[monster], transform.position, Quaternion.identity);
        if (monster == 0)
        {
            Monster monsterInstanceGj = monsterInstance.GetComponent<Monster>();
            monsterInstanceGj.Init(direction);
            return;
        }
        
        Fox FoxInstance = monsterInstance.GetComponent<Fox>();
        FoxInstance.Init(direction);
        
    }
}