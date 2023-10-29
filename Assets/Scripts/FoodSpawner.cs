using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FoodSpawner : MonoBehaviour
{
    public Vector3 spawnLimits = new Vector3(300, 0, 300);
    public int numVegetation = 40;
    public int maxVegetation = 300;
    public int spawnRate = 2;
    public int DespawnRate = 2;
    public GameObject vegetation;
    List<GameObject> stuff = new List<GameObject>();
    void Awake()
    {
        for (int i = 0; i < numVegetation; i++)
        {
            Vector3 pos = this.transform.position = new Vector3(Random.Range(-spawnLimits.x, spawnLimits.x),
                                                                spawnLimits.y,
                                                                Random.Range(-spawnLimits.z, spawnLimits.z));
            stuff.Add((GameObject)Instantiate(vegetation, pos, Quaternion.identity));
            
        }
        StartCoroutine(growth());
    }

    // Update is called once per frame

    private void Update()
    {
        if(stuff.Count > maxVegetation)
        {
            Destroy(stuff[Random.Range(0, stuff.Count - 1)]);
        }
        for (int i = 0; i < stuff.Count - 1; i++)
        {
            if (stuff[i] == null)
            {
                stuff.RemoveAt(i);
            }
            else if (stuff[i].activeSelf == false)
            {
                Destroy(stuff[i]);
            }

        }
    }
    IEnumerator growth()
    {
        while (true)
        {
            for(int i = 0; i < spawnRate; i++)
            {
                 Vector3 pos = this.transform.position = new Vector3(Random.Range(-spawnLimits.x, spawnLimits.x),
                                                                       spawnLimits.y,
                                                                       Random.Range(-spawnLimits.z, spawnLimits.z));
                 stuff.Add((GameObject)Instantiate(vegetation, pos, Quaternion.identity));
            
                 pos = this.transform.position = new Vector3(Random.Range(-spawnLimits.x, spawnLimits.x),
                                                                       spawnLimits.y,
                                                                       Random.Range(-spawnLimits.z, spawnLimits.z));
                 stuff.Add((GameObject)Instantiate(vegetation, pos, Quaternion.identity));
        
           //Destroy(stuff[Random.Range(0, stuff.Count - 1)]);
          
            }
            for (int i = 0; i < DespawnRate; i++)
            {
                stuff[Random.Range(0, stuff.Count - 1)].SetActive(false); 
            }




                yield return new WaitForSeconds(15);
        }
        
    }
}
