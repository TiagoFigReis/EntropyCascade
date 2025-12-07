using System.Collections.Generic;
using UnityEngine;

public class PortalSpawner : MonoBehaviour
{
    [SerializeField] private float portalTimer;
    private float lastSpawned;
    [SerializeField] private List<GameObject> portalSpawnList;
    private int portalCount;
    [SerializeField] private Portal portalPrefab;

    void Update()
    {
        spawnPortal();
    }

    void spawnPortal()
    {
        if (lastSpawned + portalTimer > Time.timeSinceLevelLoad || portalSpawnList.Count <= portalCount)
        {
            return;
        }
        
        lastSpawned = Time.timeSinceLevelLoad;
        Vector3 position = portalSpawnList[portalCount].transform.position;
        GameObject newPortal = Instantiate(portalPrefab.gameObject, position, Quaternion.identity);
        Portal portalInstance = newPortal.GetComponent<Portal>();
        
        if (portalCount > 0)
        {
            newPortal.transform.localScale = new Vector3(5, 5, 1);
            portalInstance.Init(1);
        }
        else
        {
            newPortal.transform.localScale = new Vector3(-5, 5, 1);
            portalInstance.Init(-1);
        }

        portalCount++;
    }
    
}
