using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    
    [System.Serializable]
    private class SpawnPoints
    {
        public float xI;
        public float xF;
        public float y;
    }
    
    [SerializeField] private GameObject coin ;
    
    [SerializeField] private List<SpawnPoints> spawnPoints;
    
    private GameObject currentCoin;
    
    void Update()
    {
        GenerateCoin();
    }

    void GenerateCoin()
    {
        if (currentCoin) return;
        
        int position = Random.Range(0, spawnPoints.Count);
        
        float x = Random.Range(spawnPoints[position].xI, spawnPoints[position].xF);
        
        Vector3 spawnPoint = new Vector3 (x,spawnPoints[position].y, 0);
        
        currentCoin = Instantiate(coin, spawnPoint, Quaternion.identity);
    }

    public void Notify()
    {
        currentCoin = null;
    }
    
}
