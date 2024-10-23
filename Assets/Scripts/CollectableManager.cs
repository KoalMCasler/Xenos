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
    public GameObject[] collectables;
    public List<GameObject> activeCollectables;

    public void SpawnCollectables()
    {
        activeCollectables.Clear();
        int coinCounter = 0;
        for(int i = 0; i < spwanRate; i++)
        {
            Vector3 randomSpawnPosition = new Vector3(Random.Range(minXRange, maxXRange), Random.Range(minYRange,maxYRange), Random.Range(minZRange, maxZRange));
            if(coinCounter < 5)
            {
                activeCollectables.Add(Instantiate(collectables[0], randomSpawnPosition, Quaternion.identity));
                coinCounter++;
            }
            else
            {
                activeCollectables.Add(Instantiate(collectables[1], randomSpawnPosition, Quaternion.identity));
                coinCounter = 0;
            }
        }
    }
}
