using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidController : MonoBehaviour
{

    public BoidManager Man;

    public Vector3 position;
    public float speed = 5;

    void Start()
    {
         speed = Random.Range(Man.minspeed, Man.maxspeed);

    }



    void Update()
    {

        ApplyRules();
    }
    private Vector3 inp;
    void ApplyRules()
    {
        GameObject[] gos;
        gos = Man.allBoids;

        Vector3 DestCohesion = Vector3.zero;

        float gSpeed = 0.01f;
        float nDistance;
        int groupSize = 0;
        Vector3 ObsticalAvoidance = Vector3.zero;
        Vector3[] ObsAvoid = new Vector3[Man.sensorsMult * 8];

        foreach (GameObject go in gos)
        {
            // checks if it is running the loop for itself
            if (go != this.gameObject)
            {
                if (go != null)
                {
                    // measures distance between the current game object and another givern game object to find neighbour distance
                    nDistance = Vector3.Distance(go.transform.position, this.transform.position);

                    // checks if the givern game object is close enough to form a group of boids
                    if (nDistance <= Man.neighbourDistance)
                    {
                        // increases gorup size for each game object within range
                        groupSize++;

                        // the sum of all of all of the game objects within ranges trasforms with an offset
                        DestCohesion += go.transform.position + (go.transform.forward * Man.DesCohesionDist);

                    }
                }
            }
        }
        // a system to check weather there were any game objects within range (to prevent dividing by 0)

            DestCohesion += transform.position + (transform.forward * (Man.DesCohesionDist / 2));
            DestCohesion = DestCohesion / (groupSize + 1);
            // if there are no obsticals the game object will just walk forward until it finds a group to join

        Debug.DrawLine(this.transform.position, DestCohesion, Color.magenta);

        // obstical avoidance scripts
        Vector3 SumOfSensors = Vector3.zero;
        float forwardMult, rightMult;
        forwardMult = rightMult = 0;
        int SensorsHit = 0;

        for (int i = 0; i < ObsAvoid.Length; i++)
        {
            // splits all of the sensors between octants by finding multipliers for forward and right rays
            if (i < ObsAvoid.Length)
            {
                forwardMult = ((i - 1) - (7 * Man.sensorsMult));
                rightMult = 1;
            }
            if (i < Man.sensorsMult * 7)
            {
                forwardMult = -((i - 1) - (6 * Man.sensorsMult));
                rightMult = 1;
            }
            if (i < Man.sensorsMult * 6)
            {
                forwardMult = ((i - 1) - (5 * Man.sensorsMult));
                rightMult = -1;
            }
            if (i < Man.sensorsMult * 5)
            {
                forwardMult = -((i - 1) - (4 * Man.sensorsMult));
                rightMult = -1;
            }
            if (i < Man.sensorsMult * 4)
            {
                forwardMult = 1;
                rightMult = ((i - 1) - (3 * Man.sensorsMult));
            }
            if (i < Man.sensorsMult * 3)
            {
                forwardMult = -1;
                rightMult = ((i - 1) - (2 * Man.sensorsMult));
            }
            if (i < Man.sensorsMult * 2)
            {
                forwardMult = 1;
                rightMult = -((i - 1) - (1 * Man.sensorsMult));
            }
            if (i < Man.sensorsMult)
            {
                forwardMult = -1;
                rightMult = -((i - 1));
            }
            ObsAvoid[i] = (forwardMult * transform.forward + rightMult * transform.right);
            Ray r = new Ray(transform.position + transform.forward * Man.obsticalOffset, ObsAvoid[i]);
            RaycastHit hit;
            if (Physics.Raycast(r, out hit, Man.obsticalDist))
            {
                if (hit.transform.gameObject == this.gameObject)
                {
                    // if the raycast hits nothing, no action is taken
                }
                else
                {
                    ObsAvoid[i] = ((Man.obsticalMultiplier / (((hit.point - transform.position).magnitude) * ((hit.point - transform.position).magnitude)))) * (transform.position - hit.point);
                    //Debug.DrawLine(r.origin, hit.point, Color.magenta);
                    //Debug.DrawLine(r.origin, transform.position + ObsAvoid[i], Color.red);
                    SensorsHit++;
                    SumOfSensors += ObsAvoid[i];
                    // works out the sum of all rays hit within a certain proximity of agent
                }
            }
        }
        if (SensorsHit != 0)
        {
            ObsticalAvoidance = ((SumOfSensors) / SensorsHit);
            // finds ideal position to get away from all obsticals
        }
        else
        {
            ObsticalAvoidance = Vector3.zero;
            // dosent worry about avoiding obsticals at all

        }
        Debug.DrawLine(transform.position, transform.position + ObsticalAvoidance, Color.red);
        gSpeed = gSpeed / groupSize;

        Vector3 direction = new Vector3(0, 0, 0);
        direction = (((DestCohesion) - transform.position) * Man.cohesionMultiplier) + ((ObsticalAvoidance));




        // + (obsticalAvoidance)
        direction.y = 0;

        Debug.DrawLine(transform.position, direction + transform.position, Color.blue);
        //Debug.DrawLine(transform.position, DestCohesion, Color.magenta);
        //
        //Debug.DrawLine(transform.position, ObsticalAvoidance + transform.position, Color.cyan);
        //
        //Debug.Log("direction.x = " + direction.x + "direction.y = " + direction.y + "direction.z = " + direction.z);

        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Man.rotationSpeed * Time.deltaTime);
        }

        inp = Vector3.Lerp(Vector3.zero, new Vector3(0, 0, speed), 0.02f);
        inp = transform.TransformDirection(inp);
        inp.y = 0;
        transform.position += inp;



       // if (Man.SpeedMultiplier * 1/(direction).magnitude > Man.maxspeed)
       // {
       //
       //     speed = Man.maxspeed;
       //
       // }
       // else if (Man.SpeedMultiplier * 1/(direction).magnitude < Man.minspeed)
       // {
       //
       //     speed = Man.minspeed;
       //
       // }
       // else
       // {
       //
       //     speed = Man.SpeedMultiplier * 1/(direction).magnitude;
       //
       // }



    }

    private void OnCollisionEnter(Collision collision)
    {
        transform.position = transform.position - inp;
        transform.position = transform.position - inp;
        transform.position = transform.position - inp;
        transform.position = transform.position - inp;
        transform.rotation = Quaternion.LookRotation(-transform.forward);
        if(Random.Range(0f, 1f) > 0.5f)
        {
        speed = Random.Range(Man.minspeed, Man.maxspeed);
        }

    }

}
