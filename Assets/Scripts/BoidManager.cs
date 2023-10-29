using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidManager : MonoBehaviour
{
    //public Vector3 DesCohesionDist = new Vector3(0, 0, 40);
    public float DesCohesionDist = 40;
    public GameObject Boidprefab;
    public GameObject Camera;
    public int numBoid = 20;
    public GameObject[] allBoids;
    public Vector3 WalkLimits = new Vector3(30, 0, 30);
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
    public float obsticalDist = 30f;
    [Header("Boid settings")]
    [Range(0f, 50f)]
    public float minspeed;
    [Range(0f, 50f)]
    public float maxspeed;
    [Range(1f, 1000f)]
    public float neighbourDistance;
    [Range(0f, 50f)]
    public float rotationSpeed;
    public Vector3 Offset = new Vector3(0, 20, 0);

    // Start is called before the first frame update
    void Awake()
    {
        allBoids = new GameObject[numBoid];
        for (int i = 0; i < numBoid; i++)
        {
            Vector3 pos = this.transform.position = new Vector3(Random.Range(-WalkLimits.x, WalkLimits.x),
                                                                WalkLimits.y,
                                                                Random.Range(-WalkLimits.z, WalkLimits.z));
            allBoids[i] = (GameObject)Instantiate(Boidprefab, pos, Quaternion.identity);
            allBoids[i].GetComponent<BoidController>().Man = this;
        }

    }

    private void Update()
    {
        CameraPosition();
    }

    public void CameraPosition()
    {
        Vector3 location = Vector3.zero;
        for (int i = 0; i < numBoid; i++)
        {

            location += allBoids[i].transform.position;

        }
        location = location / numBoid;
        Camera.transform.position = location + Offset;

    }
}


