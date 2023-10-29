using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class AnimalManager : MonoBehaviour
{
    public Text numOfAnimal; // this is written on the hud to keep track

    
    public float DesCohesionDist = 40;
    public GameObject animalprefab;
    public int numAnimal = 20;
    public int Maxanimal = 30;

    
    public Vector3 WalkLimits = new Vector3(30, 0, 30);
    [Header("controls")]

    [Range(0f, 50f)]
    public float HungerMax = 5f;
    [Range(0f, 50f)]
    public float HungerMin = 0.1f;

    [Range(0f, 50f)]
    public float ThirstMax = 5f;
    [Range(0f, 50f)]
    public float ThirstMin = 0.1f;

    [Range(0f, 50f)]
    public float BirthingMax = 5f;
    [Range(0f, 50f)]
    public float BirthingMin = 0.1f;

    [Range(0f, 50f)]
    public float SocialMax = 5f;
    [Range(0f, 50f)]
    public float SocialMin = 0.1f;

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
    public float obsticalOffset = 1f;

    [Range(0, 100)]
    public int sensorsMult = 1;

    [Range(0f, 100f)]
    public float viewDist = 30f;

    [Range(0f, 100f)]
    public float obsDist = 30f;

    [Range(0f, 360)]
    public int ViewAngle = 120;
    private float AnimalCount;

    [Header("animal settings")]
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
        StartCoroutine(RandomDeath());

        for (int i = 0; i <= animal.Count - 1; i++)
        {
            animal[i].SetActive(false);
            Destroy(animal[i].gameObject);
        }


        for (int i = 0; i < numAnimal; i++)
        {
            Vector3 pos = this.transform.position = new Vector3(Random.Range(-WalkLimits.x, WalkLimits.x),
                                                                             WalkLimits.y,
                                                                Random.Range(-WalkLimits.z, WalkLimits.z));
            animal.Add((GameObject)Instantiate(animalprefab, pos, Quaternion.identity));
            animal[i].GetComponent<AnimalController>().child = false;
            animal[i].GetComponent<AnimalController>().Man = this;
        }

    }

    public void newBirthing(GameObject A, GameObject B)
    {
        float p;
        animal.Add((GameObject)Instantiate(animalprefab, A.transform.position, Quaternion.identity));
        animal[animal.Count - 1].GetComponent<AnimalController>().child = true;
        animal[animal.Count - 1].GetComponent<AnimalController>().Man = this;

        p = Random.Range(0f, 3f);
        //subjuect to change
        if (p < 1f)
        {
            animal[animal.Count - 1].GetComponent<AnimalController>().hungerMult = A.GetComponent<AnimalController>().hungerMult;
        }
        else if (p < 2f)
        {
            animal[animal.Count - 1].GetComponent<AnimalController>().hungerMult = B.GetComponent<AnimalController>().hungerMult;
        }
        else
        {
            animal[animal.Count - 1].GetComponent<AnimalController>().hungerMult = (B.GetComponent<AnimalController>().hungerMult + A.GetComponent<AnimalController>().hungerMult) / 2;
        }
        p = Random.Range(0f, 1f);
        if (p < 0.05)
        {
            p = Random.Range(-1f, 1f);
            animal[animal.Count - 1].GetComponent<AnimalController>().hungerMult += p;
        }
        if (animal[animal.Count - 1].GetComponent<AnimalController>().hungerMult == 0)
        {
            animal[animal.Count - 1].GetComponent<AnimalController>().hungerMult = Random.Range(HungerMin, HungerMax);
        }
        p = Random.Range(0f, 3f);
        if (p < 1f)
        {
            animal[animal.Count - 1].GetComponent<AnimalController>().thirstMult = A.GetComponent<AnimalController>().thirstMult;
        }
        else if (p < 2f)
        {
            animal[animal.Count - 1].GetComponent<AnimalController>().thirstMult = B.GetComponent<AnimalController>().thirstMult;
        }
        else
        {
            animal[animal.Count - 1].GetComponent<AnimalController>().thirstMult = (B.GetComponent<AnimalController>().thirstMult + A.GetComponent<AnimalController>().thirstMult) / 2;
        }
        p = Random.Range(0f, 1f);
        if (p < 0.05)
        {
            p = Random.Range(-1f, 1f);
            animal[animal.Count - 1].GetComponent<AnimalController>().thirstMult += p;
        }
        if (animal[animal.Count - 1].GetComponent<AnimalController>().thirstMult == 0)
        {
            animal[animal.Count - 1].GetComponent<AnimalController>().thirstMult = Random.Range(ThirstMin, ThirstMax);
        }
        p = Random.Range(0f, 3f);
        if (p < 1f)
        {
            animal[animal.Count - 1].GetComponent<AnimalController>().need2BreedMult = A.GetComponent<AnimalController>().need2BreedMult;
        }
        else if (p < 2f)
        {
            animal[animal.Count - 1].GetComponent<AnimalController>().need2BreedMult = B.GetComponent<AnimalController>().need2BreedMult;
        }
        else
        {
            animal[animal.Count - 1].GetComponent<AnimalController>().need2BreedMult = (B.GetComponent<AnimalController>().need2BreedMult + A.GetComponent<AnimalController>().need2BreedMult) / 2;
        }
        p = Random.Range(0f, 1f);
        if (p < 0.05)
        {
            p = Random.Range(-1f, 1f);
            animal[animal.Count - 1].GetComponent<AnimalController>().need2BreedMult += p;
        }
        if (animal[animal.Count - 1].GetComponent<AnimalController>().need2BreedMult == 0)
        {
            animal[animal.Count - 1].GetComponent<AnimalController>().need2BreedMult = Random.Range(BirthingMin, BirthingMax);
        }
        p = Random.Range(0f, 3f);
        if (p < 1f)
        {
            animal[animal.Count - 1].GetComponent<AnimalController>().fearMult = A.GetComponent<AnimalController>().fearMult;
        }
        else if (p < 2f)
        {
            animal[animal.Count - 1].GetComponent<AnimalController>().fearMult = B.GetComponent<AnimalController>().fearMult;
        }
        else
        {
            animal[animal.Count - 1].GetComponent<AnimalController>().fearMult = (B.GetComponent<AnimalController>().fearMult + A.GetComponent<AnimalController>().fearMult) / 2;
        }
        p = Random.Range(0f, 1f);
        if (p < 0.05)
        {
            p = Random.Range(-1f, 1f);
            animal[animal.Count - 1].GetComponent<AnimalController>().fearMult += p;
        }
        if (animal[animal.Count - 1].GetComponent<AnimalController>().fearMult == 0)
        {
            animal[animal.Count - 1].GetComponent<AnimalController>().fearMult = Random.Range(FearMin, FearMax);
        }
        p = Random.Range(0f, 3f);
        if (p < 1f)
        {
            animal[animal.Count - 1].GetComponent<AnimalController>().lonelyMult = A.GetComponent<AnimalController>().lonelyMult;
        }
        else if (p < 2f)
        {
            animal[animal.Count - 1].GetComponent<AnimalController>().lonelyMult = B.GetComponent<AnimalController>().lonelyMult;
        }
        else
        {
            animal[animal.Count - 1].GetComponent<AnimalController>().lonelyMult = (B.GetComponent<AnimalController>().lonelyMult + A.GetComponent<AnimalController>().lonelyMult) / 2;
        }
        p = Random.Range(0f, 1f);
        if (p < 0.05)
        {
            p = Random.Range(-1f, 1f);
            animal[animal.Count - 1].GetComponent<AnimalController>().lonelyMult += p;
        }
        if (animal[animal.Count - 1].GetComponent<AnimalController>().lonelyMult == 0)
        {
            animal[animal.Count - 1].GetComponent<AnimalController>().lonelyMult = Random.Range(SocialMin, SocialMax);
        }
    }

    private void FixedUpdate()
    {
        AnimalCount = animal.Count;
        Debug.Log(AnimalCount.ToString("0"));
        numOfAnimal.text = AnimalCount.ToString("0");
        RemoveFromLists();
    }
    IEnumerator RandomDeath()
    {
        while (true)
        {
            if (animal.Count > Maxanimal)
            {
                Destroy(animal[Random.Range(0, animal.Count - 1)]);
                yield return new WaitForSeconds(Random.Range(0,Maxanimal/(animal.Count-Maxanimal)));
            }
            yield return new WaitForSeconds(0);
        }
        
    }

    public void RemoveFromLists()
    {
        for (int i = 0; i < animal.Count - 1; i++)
        {

            if (animal[i] == null)
            {
                animal.RemoveAt(i);
            }
            else if (animal[i].activeSelf == false)
            {
                Destroy(animal[i]);
            }
        }
    }

}


