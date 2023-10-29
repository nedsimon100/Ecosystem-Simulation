using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class CowController : MonoBehaviour
{
    public AudioSource Noise;
    public CowManager Man;
    public Vector3 position;
    public float speed;
    public Vector3 Target;
    public float hunger = 0f, thirst = 0f, need2Breed = -20f, fear = 0f, lonely = 0f, pointOfDeath = 200f;
    public float hungerMult, thirstMult, need2BreedMult, fearMult, lonelyMult;
    public bool child;
    public string objective = ("NOTHING");
    public bool breedingtime;
    public GameObject breedingpartner;
    void Start()
    {

        speed = Random.Range(Man.minspeed,Man.maxspeed);
        if (child == false)
        {
            hungerMult = Random.Range(Man.HungerMin, Man.HungerMax);
            thirstMult = Random.Range(Man.ThirstMin, Man.ThirstMax);
            need2BreedMult = Random.Range(Man.BirthingMin, Man.BirthingMax);
            lonelyMult = Random.Range(Man.SocialMin, Man.SocialMax);
            fearMult = Random.Range(Man.FearMin, Man.FearMax);
        }
        StartCoroutine(NeedStuff());
        StartCoroutine(MakeNoise());
    }
    IEnumerator MakeNoise()
    {
        while (true)
        {
            Noise.Play();
            yield return new WaitForSeconds(Random.Range(2f, 20f));
        }
    }

    void Update()
    {
        ApplyRules();
        CheckDeath();

    }
    IEnumerator NeedStuff()
    {
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
            hunger+=1f;
            thirst+=1f;
            need2Breed+=1f;
            pointOfDeath -= 0.2f;
            yield return new WaitForSeconds(3);
        }
        
    }

    public void CheckDeath()
    {
        if (hunger > pointOfDeath || thirst > pointOfDeath)
        {
            Die();
        }
    }
    public void Die()
    {
        this.gameObject.SetActive(false);
    }
    private Vector3 inp;
    void ApplyRules()
    {

        List<GameObject> gos;
        List<GameObject> animal = Man.animal;
        gos = animal;

        Vector3 DestCohesion = Vector3.zero;
        Vector3 NearestFood = Vector3.zero;
        Vector3 NearestWater = Vector3.zero;
        Vector3 NearestMate = Vector3.zero;
        Vector3 Terror = Vector3.zero;

        
        float dtonearestfood = 1000;
        float dtonearestmate = 1000;
        float dtonearestwater = 1000;
        float nDistance;
        int groupSize = 0;
        Vector3 ObsticalAvoidance = Vector3.zero;
        Vector3[] ObsAvoid = new Vector3[Man.sensorsMult * 8];

        foreach (GameObject go in gos)
        {
            if (go != this.gameObject)
            {
                if (go != null)
                {
                    nDistance = Vector3.Distance(go.transform.position, this.transform.position);
                    if (nDistance <= Man.neighbourDistance)
                    {
                        groupSize++;
                        // Pre Cohesion


                        // transform.TransformPoint
                        DestCohesion += go.transform.position + (go.transform.forward * Man.DesCohesionDist);

                        //
                        // DestCohesion = Vector3.zero;
                        //

                        //Nearest food









                        //
                        //ObsticalAvoidance = Vector3.zero;
                        //

                        // ObsticalAvoidance += transform.position;
                        // ObsticalAvoidance.y = 0;
                        // Debug.DrawLine(r.origin, ObsticalAvoidance , Color.black);


                        if (nDistance < Man.obsDist)
                        {
                            //transform.TransformPoint

                            // Debug.DrawLine(this.transform.position,go.transform.position,Color.white);

                        }



                    }
                }
            }
        }


        if (groupSize == 0)
        {
          DestCohesion += transform.position + (transform.forward * (Man.DesCohesionDist/2));
          DestCohesion = DestCohesion / (groupSize + 1);
        }
        else
        {
            DestCohesion = DestCohesion / (groupSize);
        }
        // obstical avoidance
        float nm = Man.sensorsMult;
        Vector3 nmb = Vector3.zero;
        float a, b;
        a = b = 0;
        int amber = 0;

        for (int i = 0; i < ObsAvoid.Length; i++)
        {
            if (i < ObsAvoid.Length)
            {

                a = ((i - 1) - (7 * nm));
                b = 1;

            }
            if (i < nm * 7)
            {
                a = -((i - 1) - (6 * nm));
                b = 1;
            }
            if (i < nm * 6)
            {
                a = ((i - 1) - (5 * nm));
                b = -1;
            }
            if (i < nm * 5)
            {
                a = -((i - 1) - (4 * nm));
                b = -1;
            }
            if (i < nm * 4)
            {
                a = 1;
                b = ((i - 1) - (3 * nm));
            }
            if (i < nm * 3)
            {
                a = -1;
                b = ((i - 1) - (2 * nm));
            }
            if (i < nm * 2)
            {
                a = 1;
                b = -((i - 1) - (1 * nm));
            }
            if (i < nm)
            {
                a = -1;
                b = -((i - 1));
            }

            // if (i < ObsAvoid.Length)
            // {
            //
            //     a = ((i - 1) - (7 * nm));
            //     b = 1;
            //
            // }
            // 
            // if (i < nm * 6)
            // {
            //     a = ((i - 1) - (5 * nm));
            //     b = -1;
            // }
            //
            // if (i < nm * 4)
            // {
            //     a = 1;
            //     b = ((i - 1) - (3 * nm));
            // }
            // 
            // if (i < nm * 2)
            // {
            //     a = 1;
            //     b = -((i - 1) - (1 * nm));
            // }

            ObsAvoid[i] = (a * transform.forward + b * transform.right);
            Ray r = new Ray(transform.position + transform.forward * Man.obsticalOffset, ObsAvoid[i]);
            RaycastHit hit;
            //Debug.DrawLine(r.origin, r.direction , Color.grey);
            if (Physics.Raycast(r, out hit, Man.obsDist))
            {
                if (hit.transform.gameObject == this.gameObject)
                {

                }
                else
                {
                    ObsAvoid[i] = ((Man.obsticalMultiplier / (((hit.point - transform.position).magnitude) * ((hit.point - transform.position).magnitude)))) * (transform.position - hit.point);
                    Debug.DrawLine(r.origin, hit.point, Color.grey);
                    Debug.DrawLine(r.origin, transform.position + ObsAvoid[i], Color.red);
                    amber++;
                    nmb += ObsAvoid[i];
                }


            }


        }
        if (amber != 0)
        {
            ObsticalAvoidance = ((nmb) / amber);
        }
        else
        {
            ObsticalAvoidance = Vector3.zero;

        }

        Vector3 j = (transform.forward);
        Ray l = new Ray(transform.position, j);
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, Man.viewDist);
        bool safe = true;
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
                    
                    fear = (fearMult /(dstToTarget/Man.viewDist)) * fearMult*fearMult;
                    Terror = (-dirToTarget)*fear;
                    Debug.DrawLine(transform.position, Terror + transform.position, Color.yellow);
                }
            }
        }
        if (safe == true)
        {
            fear = 0;
        }
        //DestCohesion += transform.position + ((transform.forward * Man.DesCohesionDist) * Man.cohesionMultiplier);

        


        Vector3 direction = new Vector3(0, 0, 0);
        //-transform.position



        if ((thirst * thirstMult) > (hunger * hungerMult) && (lonely * lonelyMult) < (thirst * thirstMult) && (thirst * thirstMult) > (need2Breed * need2BreedMult) && (thirst * thirstMult) > (fear * fearMult))
        {
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
            }
            else
            {
                lonely -= 0.2f * groupSize;
                Target = ((DestCohesion) - transform.position) * Man.cohesionMultiplier;
                objective = ("FRIENDSHIP");
            }
            objective = ("WATER");
            Gizmos.color = Color.blue;
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
                // * hunger * hungerMult
            }
            else
            {
                lonely -= 0.2f;
                Target = ((DestCohesion) - transform.position) * Man.cohesionMultiplier;
                objective = ("FRIENDSHIP");
            }
            Gizmos.color = Color.magenta;
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
            Gizmos.color = Color.green;
        }
        else if ((lonely * lonelyMult) < (fear ) && (fear) > (thirst * thirstMult) && (hunger * hungerMult) < (fear) && (need2Breed * need2BreedMult) < (fear * fearMult))
        { 
            objective = ("ESCAPE");
            Target = Terror;
            Gizmos.color = Color.red;
        }
        else
        {
            lonely -= 0.2f * groupSize;
            Target = ((DestCohesion) - transform.position) * Man.cohesionMultiplier;
            objective = ("FRIENDSHIP");
            Gizmos.color = Color.white;
        }
            direction = Target + ObsticalAvoidance;
        Gizmos.DrawSphere(transform.position + new Vector3(0, 0, 1), 0.2f);

            
    
            // + (obsticalAvoidance)
            direction.y = 0;
            
            Debug.DrawLine(transform.position, direction + transform.position, Color.blue);
            Debug.DrawLine(transform.position, Target + transform.position, Color.magenta);
            
            Debug.DrawLine(transform.position, ObsticalAvoidance + transform.position, Color.cyan);

           // Debug.Log("direction.x = " + direction.x + "direction.y = " + direction.y + "direction.z = " + direction.z);

            if (direction != Vector3.zero)
            {
                 transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Man.rotationSpeed * Time.deltaTime);
            }
            
            inp = Vector3.Lerp(Vector3.zero, new Vector3(0, 0, speed), 0.02f);
            inp = transform.TransformDirection(inp);
            inp.y = 0;
            transform.position += inp;
        


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
            
        

    }
    public void OnDrawGizmos()
    {
        
        Gizmos.DrawSphere(transform.position + new Vector3(0, 0, 1), 0.2f);
    }
    private void OnCollisionEnter(Collision collision)
    {
        GameObject Target = collision.collider.gameObject;
        if (Target.layer == 4)
        {
            thirst -= 40;
        }
        transform.position = transform.position - inp;
        transform.position = transform.position - inp;
        transform.position = transform.position - inp;
        transform.position = transform.position - inp;
    }
    IEnumerator breedinglimiter()
    {
        // a repeating test to see whether the user is walking
        while (true)
        {
            if (breedingtime == true)
            {
                Man.newBirthing(this.gameObject, breedingpartner);
                breedingtime = false;
                yield return new WaitForSeconds(5f);
            }
            else
            {
                yield return new WaitForSeconds(0);
            }
        }
    }
}

