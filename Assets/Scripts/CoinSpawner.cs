using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    [SerializeField] private GameObject coin ;
    
    [SerializeField] private List<Transform> spawnPoints;
    
    private GameObject currentCoin;
    
    void Update()
    {
        GenerateCoin();
    }

    void GenerateCoin()
    {
        if (currentCoin) return;
        
        int position = Random.Range(0, spawnPoints.Count);
        
        Transform coinPosition = spawnPoints[position];
        
        Vector3 spawnPoint = new Vector3 (coinPosition.position.x ,coinPosition.position.y, 0);
        
        currentCoin = Instantiate(coin, spawnPoint, Quaternion.identity);
    }

    public void Notify()
    {
        currentCoin = null;
    }
    
}
