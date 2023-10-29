using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

// class inherited from MonoBehaviour which allows for ienumerators, start, update and it allows for the script to be assigned to an object in game.
public class AnimalController : MonoBehaviour
{

    public AudioSource Noise; // the noise the animal makes, this makes them easier to track when there off screen and it adds life to my game
    public AnimalManager Man; // the manager which stores all of the base values for the group, this also makes it easier to edit my animals in the inspector
    public float speed; // the movement velocity of each agent
    public Vector3 Target; // the desired location of each agent
    public float hunger = 0f, thirst = 0f, need2Breed = -5f, fear = 0f, lonely = 0f, pointOfDeath = 200f; // the needs of each agent
    public float hungerMult, thirstMult, need2BreedMult, fearMult, lonelyMult; // the urgensy of the needs of each agent
    public bool child; // if the agent is part of the first generation
    public string objective = ("NOTHING"); // the agents current most urgent need
    public bool breedingtime; // if all the conditions for breeding are met
    public GameObject breedingpartner; // the agent that it splices its need multipliers with to give to the child
    void Start()
    {

        speed = Random.Range(Man.minspeed, Man.maxspeed); // initial speed at birth
        if (child == false)
        {
            // sets all of the needs for the first generation to give genetic variety

            hungerMult = Random.Range(Man.HungerMin, Man.HungerMax);
            thirstMult = Random.Range(Man.ThirstMin, Man.ThirstMax);
            need2BreedMult = Random.Range(Man.BirthingMin, Man.BirthingMax);
            lonelyMult = Random.Range(Man.SocialMin, Man.SocialMax);
            fearMult = Random.Range(Man.FearMin, Man.FearMax);
        }
        // starts all timers and time sensitive iterative algorithms
        StartCoroutine(NeedStuff());
        StartCoroutine(MakeNoise());
        StartCoroutine(breedinglimiter());
    }
    IEnumerator MakeNoise()
    {
        while (true)
        {
            // plays the animals noise at random intervals
            Noise.Play();
            yield return new WaitForSeconds(Random.Range(2f, 20f));
        }
    }

    void Update()
    {
        //applies all of the boid algorithms and ai systems in place as well as checking if the agent has "died"
        ApplyRules();
        CheckDeath();

    }
    IEnumerator NeedStuff()
    {
        //increments all needs anf lowers the amount of hunger and thirst required to "kill" the agent
        while (true)
        {
            if (lonely < 0)
            {
                lonely = 0;
            }
            if (hunger < 0)
            {
                hunger = 0;
            }
            if (thirst < 0)
            {
                thirst = 0;
            }
            lonely += 1f;
            hunger += 1f;
            thirst += 1f;
            need2Breed += 1.5f;
            pointOfDeath -= 0.5f;
            yield return new WaitForSeconds(3);
        }

    }

    public void CheckDeath()
    {
        // checks to see if agent has died of starvation or thirst
        if (hunger > pointOfDeath || thirst > pointOfDeath)
        {
            Die();
        }
    }
    public void Die()
    {
        // "kills" agent
        this.gameObject.SetActive(false);
    }

    private Vector3 inp; // movement distance
    void ApplyRules()
    {

        // variables for the position of each desired target position
        Vector3 DestCohesion = Vector3.zero;
        Vector3 NearestFood = Vector3.zero;
        Vector3 NearestWater = Vector3.zero;
        Vector3 NearestMate = Vector3.zero;
        Vector3 Terror = Vector3.zero;
        Vector3 ObsticalAvoidance = Vector3.zero;

        // arbritary values designed to be larger than the furthest the animals can see
        float dtonearestfood = 1000;
        float dtonearestmate = 1000;
        float dtonearestwater = 1000;
        // distance between each agent
        float nDistance;
        // the amount of agents close enough to one another to form a group of Boids
        int groupSize = 0;
        //the amount of ray casts used to avoid obsticals taken from the manager script
        Vector3[] ObsAvoid = new Vector3[Man.sensorsMult * 8];
        // puts every game object of a species into one list so they can recognise eachother better
        List<GameObject> gos = Man.animal;
        // loops for each agent in a set species
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
        if (groupSize == 0)
        {
            DestCohesion += transform.position + (transform.forward * (Man.DesCohesionDist / 2));
            DestCohesion = DestCohesion / (groupSize + 1);
            // if there are no obsticals the game object will just walk forward until it finds a group to join
        }
        else
        {
            // finds the average position of all of the game objects within ranges trasforms with an offset
            DestCohesion = DestCohesion / (groupSize);
        }


        // obstical avoidance scripts
        Vector3 SumOfSensors = Vector3.zero;
        float forwardMult, rightMult;
        forwardMult=rightMult= 0;
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
               forwardMult= 1;
               rightMult= ((i - 1) - (3 * Man.sensorsMult));
            }
            if (i < Man.sensorsMult * 3)
            {
               forwardMult= -1;
               rightMult= ((i - 1) - (2 * Man.sensorsMult));
            }
            if (i < Man.sensorsMult * 2)
            {
               forwardMult= 1;
               rightMult= -((i - 1) - (1 * Man.sensorsMult));
            }
            if (i < Man.sensorsMult)
            {
               forwardMult= -1;
               rightMult= -((i - 1));
            }
            ObsAvoid[i] = (forwardMult * transform.forward +rightMult* transform.right);
            Ray r = new Ray(transform.position + transform.forward * Man.obsticalOffset, ObsAvoid[i]);
            RaycastHit hit;
            if (Physics.Raycast(r, out hit, Man.obsDist))
            {
                if (hit.transform.gameObject == this.gameObject)
                {
                    // if the raycast hits nothing, no action is taken
                }
                else
                {
                    ObsAvoid[i] = ((Man.obsticalMultiplier / (((hit.point - transform.position).magnitude) * ((hit.point - transform.position).magnitude)))) * (transform.position - hit.point);
                    Debug.DrawLine(r.origin, hit.point, Color.grey);
                    Debug.DrawLine(r.origin, transform.position + ObsAvoid[i], Color.red);
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

        Vector3 j = (transform.forward);
        Ray l = new Ray(transform.position, j);
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, Man.viewDist);
        bool safe = true;
        // checks if there is danger nearby
        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            if (target.gameObject.layer > this.gameObject.layer)
            {

                safe = false;

                Vector3 dirToTarget = (target.position - transform.position).normalized;
                if (Vector3.Angle(transform.forward, dirToTarget) < Man.ViewAngle / 2)
                {
                    float dstToTarget = Vector3.Distance(transform.position, target.position);

                    fear = (fearMult / (dstToTarget / Man.viewDist)) * fearMult * fearMult;
                    Terror = (-dirToTarget) * fear;
                    Debug.DrawLine(transform.position, Terror + transform.position, Color.yellow);
                }
            }
        }
        if (safe == true)
        {
            fear = 0;
        }




        Vector3 direction = new Vector3(0, 0, 0);
        // sets movement direction to 0


        // compares all needs with their multipliers
        if ((thirst * thirstMult) > (hunger * hungerMult) && (lonely * lonelyMult) < (thirst * thirstMult) && (thirst * thirstMult) > (need2Breed * need2BreedMult) && (thirst * thirstMult) > (fear * fearMult))
        {
            // finds nearest water
            for (int i = 0; i < targetsInViewRadius.Length; i++)
            {
                Transform target = targetsInViewRadius[i].transform;
                if (target.gameObject.layer == 4)
                {
                    Vector3 dirToTarget = (target.position - transform.position).normalized;
                    if (Vector3.Angle(transform.forward, dirToTarget) < Man.ViewAngle / 2)
                    {
                        float dstToTarget = Vector3.Distance(transform.position, target.position);
                        if (dstToTarget < dtonearestwater)
                        {
                            NearestWater = target.position;
                            dtonearestwater = dstToTarget;

                        }
                        if (dtonearestwater < 20)
                        {
                            thirst -= 40;

                        }

                    }
                }
            }
            if (dtonearestwater < Man.viewDist)
            {
                Target = (NearestWater - transform.position) * thirst * thirstMult;
                // if it can see water it leaves the group it is traveling with to get some to drink before returning to group
            }
            else
            {
                lonely -= 0.1f * groupSize;
                Target = ((DestCohesion) - transform.position) * Man.cohesionMultiplier;
                objective = ("FRIENDSHIP");
            }
            objective = ("WATER");
        }
        else if ((lonely * lonelyMult) < (hunger * hungerMult) && (hunger * hungerMult) > (thirst * thirstMult) && (hunger * hungerMult) > (need2Breed * need2BreedMult) && (hunger * hungerMult) > (fear * fearMult))
        {
            for (int i = 0; i < targetsInViewRadius.Length; i++)
            {
                Transform target = targetsInViewRadius[i].transform;
                if (target.gameObject.layer == this.gameObject.layer - 1 || target.gameObject.layer == this.gameObject.layer - 2)
                {
                    Vector3 dirToTarget = (target.position - transform.position).normalized;
                    if (Vector3.Angle(transform.forward, dirToTarget) < Man.ViewAngle / 2)
                    {
                        float dstToTarget = Vector3.Distance(transform.position, target.position);
                        if (dstToTarget < dtonearestfood)
                        {
                            NearestFood = target.position;
                            dtonearestfood = dstToTarget;

                        }
                        if (dtonearestfood < 10)
                        {
                            hunger -= 40;
                            target.gameObject.SetActive(false);
                        }
                    }
                }

            }
            if (dtonearestfood < Man.viewDist)
            {
                Target = (NearestFood - transform.position);
            }
            else
            {
                lonely -= 0.2f;
                Target = ((DestCohesion) - transform.position) * Man.cohesionMultiplier;
                objective = ("FRIENDSHIP");
            }

            objective = ("FOOD");
        }
        else if ((lonely * lonelyMult) < (need2Breed * need2BreedMult) && (need2Breed * need2BreedMult) > (thirst * thirstMult) && (hunger * hungerMult) < (need2Breed * need2BreedMult) && (need2Breed * need2BreedMult) > (fear * fearMult))
        {

            for (int i = 0; i < targetsInViewRadius.Length; i++)
            {
                Transform target = targetsInViewRadius[i].transform;
                GameObject parent = targetsInViewRadius[i].gameObject;
                if (target.gameObject.layer == this.gameObject.layer && target.gameObject != this.gameObject)
                {
                    Vector3 dirToTarget = (target.position - transform.position).normalized;
                    if (Vector3.Angle(transform.forward, dirToTarget) < Man.ViewAngle / 2)
                    {
                        float dstToTarget = Vector3.Distance(transform.position, target.position);
                        if (dstToTarget < dtonearestmate)
                        {
                            NearestMate = target.position;
                            dtonearestmate = dstToTarget;

                        }
                        if (dtonearestmate < 2)
                        {
                            need2Breed -= 40;
                            breedingtime = true;
                            breedingpartner = parent;
                        }
                    }
                }
            }
            if (dtonearestmate < Man.viewDist)
            {
                Target = (NearestMate - transform.position) * need2Breed * need2BreedMult;
            }
            else
            {
                lonely -= 0.2f;
                Target = ((DestCohesion) - transform.position) * Man.cohesionMultiplier;
                objective = ("FRIENDSHIP");
            }
            objective = ("BREEDING");

        }
        else if ((lonely * lonelyMult) < (fear * fearMult) && (fear * fearMult) > (thirst * thirstMult) && (hunger * hungerMult) < (fear * fearMult) && (need2Breed * need2BreedMult) < (fear * fearMult))
        {
            objective = ("ESCAPE");
            Target = Terror;

        }
        else
        {
            lonely -= 0.2f * groupSize;
            Target = ((DestCohesion) - transform.position) * Man.cohesionMultiplier;
            objective = ("FRIENDSHIP");

        }
        // tells the game object how to reach its target with minimal collisions
        direction = Target + ObsticalAvoidance;

        // so that the agent dosent glitch and move 3 dimentionally
        direction.y = 0;

        // some debugging so i can see how it works in gizmos during runtime
        Debug.DrawLine(transform.position, direction + transform.position, Color.blue);
        Debug.DrawLine(transform.position, Target + transform.position, Color.magenta);
        Debug.DrawLine(transform.position, ObsticalAvoidance + transform.position, Color.cyan);
        //Debug.Log("direction.x = " + direction.x + "direction.y = " + direction.y + "direction.z = " + direction.z); // Dubugging to check it all works, i dont use this unless there is only one agent because otherwise there can be way to many to be useful

        if (direction != Vector3.zero)
        {
            // slowly turns to face its new direction
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Man.rotationSpeed * Time.deltaTime);
        }

        // movement
        inp = Vector3.Lerp(Vector3.zero, new Vector3(0, 0, speed), 0.02f);
        inp = transform.TransformDirection(inp);
        inp.y = 0;
        transform.position += inp;


        //decides speed based off of how far the agent needs to travel to reach its destination
        if (Man.SpeedMultiplier * (direction).magnitude > Man.maxspeed)
        {

            speed = Man.maxspeed;

        }
        else if (Man.SpeedMultiplier * (direction).magnitude < Man.minspeed)
        {

            speed = Man.minspeed;

        }
        else
        {

            speed = Man.SpeedMultiplier * (direction).magnitude;

        }

        // so that the animals dont maintain there normal speed when i activate slow-mo
        speed = speed * Time.timeScale;






    }

    private void OnCollisionEnter(Collision collision)
    {
        // in the event of a collision this prevents the agents from walking through walls

        GameObject Target = collision.collider.gameObject;
        if (Target.layer == 4)
        {
            thirst -= 40;
        }
        transform.position = transform.position - inp;
        transform.position = transform.position - inp;
        transform.position = transform.position - inp;
        transform.position = transform.position - inp;
        transform.rotation = Quaternion.LookRotation(-transform.forward);
    }
    IEnumerator breedinglimiter()
    {
        //a cooldown timer for breeding

        // if the agent breeds, it cant do it again for at least 5 seconds, this is to fix a glitch
        // i got early on where every time the animals went to multiply far to many came at once 
        // and it crashed my laptop.

        while (true)
        {
            if (breedingtime == true)
            {
                Man.newBirthing(this.gameObject, breedingpartner);
                yield return new WaitForSeconds(5f);
                breedingtime = false;
                
            }
            else
            {
                yield return new WaitForSeconds(0);
            }
        }
    }
}