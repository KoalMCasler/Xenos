using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableManager : MonoBehaviour
{
    public int spwanRate;
    public float minXRange;
    public float maxXRange;
    public float minYRange;
    public float maxYRange;
    public float minZRange;
    public float maxZRange;
    public int coinToFuelRatio;
    public GameObject[] collectables;
    public List<GameObject> activeCollectables;

    /// <summary>
    /// Used to spawn collectables for each run. 
    /// </summary>
    public void SpawnCollectables()
    {
        ClearCollectables();
        int coinCounter = 0;
        for(int i = 0; i < spwanRate; i++)
        {
            Vector3 randomSpawnPosition = new Vector3(Random.Range(minXRange, maxXRange), Random.Range(minYRange,maxYRange), Random.Range(minZRange, maxZRange));
            if(coinCounter < coinToFuelRatio)
            {
                activeCollectables.Add(Instantiate(collectables[0], randomSpawnPosition, Quaternion.identity,this.transform));
                coinCounter++;
            }
            else
            {
                activeCollectables.Add(Instantiate(collectables[1], randomSpawnPosition, Quaternion.identity,this.transform));
                coinCounter = 0;
            }
        }
    }
    /// <summary>
    /// Clears All collectables
    /// </summary>
    public void ClearCollectables()
    {
        for(int i = 0; i < activeCollectables.Count; i++)
        {
            Destroy(activeCollectables[i]);
        }
    }
}
