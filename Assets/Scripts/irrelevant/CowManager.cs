using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class CowManager : MonoBehaviour
{
    public Text numOfAnimal;

    //public Vector3 DesCohesionDist = new Vector3(0, 0, 40);
    public float DesCohesionDist = 40;
    public GameObject cowprefab;
    public int numCow = 20;
    public int Maxcow = 30;


    public Vector3 WalkLimits = new Vector3(30, 0, 30);
    [Header("controls")]

    [Range(0f, 50f)]
    public float HungerMax=5f;
    [Range(0f, 50f)]
    public float HungerMin=0.1f;

    [Range(0f, 50f)]
    public float ThirstMax=5f;
    [Range(0f, 50f)]
    public float ThirstMin=0.1f;

    [Range(0f, 50f)]
    public float BirthingMax=5f;
    [Range(0f, 50f)]
    public float BirthingMin=0.1f;

    [Range(0f, 50f)]
    public float SocialMax=5f;
    [Range(0f, 50f)]
    public float SocialMin=0.1f;

    [Range(0f, 50f)]
    public float FearMax = 20f;
    [Range(0f, 50f)]
    public float FearMin = 0.1f;


    [Header("Boid Controls")]

    [Range(0f, 100f)]
    public float cohesionMultiplier = 1f;

    [Range(0f, 100f)]
    public float SpeedMultiplier = 1f;

    [Range(0f, 500f)]
    public float obsticalMultiplier = 1f;

    [Range(0f, 500f)]
    public float obsticalOffset =1f;

    [Range(0, 100)]
    public int sensorsMult = 1;

    [Range(0f, 100f)]
    public float viewDist = 30f;

    [Range(0f, 100f)]
    public float obsDist = 30f;

    [Range(0f, 360)]
    public int ViewAngle = 120;
    private float AnimalCount;

    [Header("cow settings")]
    [Range(0f, 50f)]
    public float minspeed;
    [Range(0f, 50f)]
    public float maxspeed;
    [Range(1f, 1000f)]
    public float neighbourDistance;
    [Range(0f, 50f)]
    public float rotationSpeed;
    public Vector3 Offset = new Vector3(0, 20, 0);
    //public List<GameObject> animal;
    // Start is called before the first frame update      
    public List<GameObject> animal = new List<GameObject>();
    void Awake()
    {
        

        for(int i = 0; i<= animal.Count - 1; i++)
        {
            animal[i].SetActive(false);
            Destroy(animal[i].gameObject);
        }


        for (int i = 0; i < numCow; i++)
        {
            Vector3 pos = this.transform.position = new Vector3(Random.Range(-WalkLimits.x, WalkLimits.x),
                                                                WalkLimits.y,
                                                                Random.Range(-WalkLimits.z, WalkLimits.z));
            animal.Add((GameObject)Instantiate(cowprefab, pos, Quaternion.identity));
            animal[i].GetComponent<CowController>().child = false;
            animal[i].GetComponent<CowController>().Man = this;
        }

    }

    public void newBirthing(GameObject A, GameObject B)
    {
        float p;
        animal.Add((GameObject)Instantiate(cowprefab, A.transform.position, Quaternion.identity));
        animal[animal.Count - 1].GetComponent<CowController>().child = true;
        animal[animal.Count - 1].GetComponent<CowController>().Man = this;

        p = Random.Range(0f,3f);
        //subjuect to change
        if (p < 5)
        {
            animal[animal.Count - 1].GetComponent<CowController>().hungerMult = A.GetComponent<CowController>().hungerMult;
        }
        else if (p < 2)
        {
            animal[animal.Count - 1].GetComponent<CowController>().hungerMult = B.GetComponent<CowController>().hungerMult;
        }
        else
        {
            animal[animal.Count - 1].GetComponent<CowController>().hungerMult = (B.GetComponent<CowController>().hungerMult+ A.GetComponent<CowController>().hungerMult)/2;
        }
        p = Random.Range(0f, 1f);
        if (p < 0.05)
        {
            p = Random.Range(-1f, 1f);
            animal[animal.Count - 1].GetComponent<CowController>().hungerMult += p;
        }
        if(animal[animal.Count - 1].GetComponent<CowController>().hungerMult == 0)
        {
            animal[animal.Count - 1].GetComponent<CowController>().hungerMult = Random.Range(HungerMin, HungerMax);
        }
        p = Random.Range(0f, 3f);
        if (p < 5)
        {
            animal[animal.Count - 1].GetComponent<CowController>().thirstMult = A.GetComponent<CowController>().thirstMult;
        }                                                         
        else if (p < 2)                                           
        {                                                         
            animal[animal.Count - 1].GetComponent<CowController>().thirstMult = B.GetComponent<CowController>().thirstMult;
        }                                                       
        else                                                    
        {                                                       
            animal[animal.Count - 1].GetComponent<CowController>().thirstMult = (B.GetComponent<CowController>().thirstMult + A.GetComponent<CowController>().thirstMult) / 2;
        }                                                          
        p = Random.Range(0f, 1f);                                  
        if (p < 0.05)                                              
        {                                                          
            p = Random.Range(-1f, 1f);                             
            animal[animal.Count - 1].GetComponent<CowController>().thirstMult += p;
        }
        if (animal[animal.Count - 1].GetComponent<CowController>().thirstMult == 0)
        {
            animal[animal.Count - 1].GetComponent<CowController>().thirstMult = Random.Range(ThirstMin, ThirstMax);
        }
        p = Random.Range(0f, 3f);
        if (p < 5)
        {
            animal[animal.Count - 1].GetComponent<CowController>().need2BreedMult = A.GetComponent<CowController>().need2BreedMult;
        }
        else if (p < 2)
        {
            animal[animal.Count - 1].GetComponent<CowController>().need2BreedMult = B.GetComponent<CowController>().need2BreedMult;
        }
        else
        {
            animal[animal.Count - 1].GetComponent<CowController>().need2BreedMult = (B.GetComponent<CowController>().need2BreedMult + A.GetComponent<CowController>().need2BreedMult) / 2;
        }
        p = Random.Range(0f, 1f);
        if (p < 0.05)
        {
            p = Random.Range(-1f, 1f);
            animal[animal.Count - 1].GetComponent<CowController>().need2BreedMult += p;
        }
        if (animal[animal.Count - 1].GetComponent<CowController>().need2BreedMult == 0)
        {
            animal[animal.Count - 1].GetComponent<CowController>().need2BreedMult = Random.Range(BirthingMin, BirthingMax);
        }
        p = Random.Range(0f, 3f);
        if (p < 5)
        {
            animal[animal.Count - 1].GetComponent<CowController>().fearMult = A.GetComponent<CowController>().fearMult;
        }
        else if (p < 2)
        {
            animal[animal.Count - 1].GetComponent<CowController>().fearMult = B.GetComponent<CowController>().fearMult;
        }
        else
        {
            animal[animal.Count - 1].GetComponent<CowController>().fearMult = (B.GetComponent<CowController>().fearMult + A.GetComponent<CowController>().fearMult) / 2;
        }
        p = Random.Range(0f, 1f);
        if (p < 0.05)
        {
            p = Random.Range(-1f, 1f);
            animal[animal.Count - 1].GetComponent<CowController>().fearMult += p;
        }
        if (animal[animal.Count - 1].GetComponent<CowController>().fearMult == 0)
        {
            animal[animal.Count - 1].GetComponent<CowController>().fearMult = Random.Range(FearMin, FearMax);
        }
        p = Random.Range(0f, 3f);
        if (p < 5)
        {
            animal[animal.Count - 1].GetComponent<CowController>().lonelyMult = A.GetComponent<CowController>().lonelyMult;
        }
        else if (p < 2)
        {
            animal[animal.Count - 1].GetComponent<CowController>().lonelyMult = B.GetComponent<CowController>().lonelyMult;
        }
        else
        {
            animal[animal.Count - 1].GetComponent<CowController>().lonelyMult = (B.GetComponent<CowController>().lonelyMult + A.GetComponent<CowController>().lonelyMult) / 2;
        }
        p = Random.Range(0f, 1f);
        if (p < 0.05)
        {
            p = Random.Range(-1f, 1f);
            animal[animal.Count - 1].GetComponent<CowController>().lonelyMult += p;
        }
        if (animal[animal.Count - 1].GetComponent<CowController>().lonelyMult == 0)
        {
            animal[animal.Count - 1].GetComponent<CowController>().lonelyMult = Random.Range(SocialMin, SocialMax);
        }
    }

    private void FixedUpdate()
    {
        AnimalCount = animal.Count;
        Debug.Log(AnimalCount.ToString("0"));
        numOfAnimal.text = AnimalCount.ToString("0");
        RemoveFromLists();
        CameraPosition();
        if (animal.Count > Maxcow)
        {
            Destroy(animal[Random.Range(0, animal.Count - 1)]);
        }
    }
    public void RemoveFromLists()
    {
        for(int i = 0; i < animal.Count - 1; i++)
        {

            if (animal[i]==null)
            {
                animal.RemoveAt(i);
            }
            else if (animal[i].activeSelf == false)
            {
                Destroy(animal[i]);
            }
        }
    }
    public void CameraPosition()
    {
     //   int i= 0;
     //   Vector3 location = Vector3.zero;
     //   foreach (GameObject go in animal)
     //   {
     //
     //       location += go.transform.position;
     //       i++;
     //   }
     //   location = location / i;
     //   Camera.transform.position = location + Offset;
        
    }
}


