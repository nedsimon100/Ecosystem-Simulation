using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public AudioSource Bang;
    public AudioSource Walk;
    public GameObject spawnPoint;
    public GameObject NormalCamera;
    public GameObject BirdsEyeCamera;
    public TrailRenderer TR;
    public float moveSpeed = 10f;
    public float rotationSpeed = 50f;
    private Vector3 inp;
    public GameObject bulletPrefab;
    List<GameObject> bullet = new List<GameObject>();
    public PlayerManager man;
    public GameObject Manager;

    private void Awake()
    {
        spawn();

    }
    public void spawn()
    {  
        StartCoroutine(ShootTimer());
        StartCoroutine(WalkNoise());
        TR.enabled = false;
        transform.position = spawnPoint.transform.position;
        TR.enabled = true;

        Manager.GetComponent<PlayerManager>().kills(0);
    }
    public int bulletsFired = 0;

    private void Update()
    {
         Time.timeScale = 1f;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Time.timeScale = 0.25f;
        }
    }
    void FixedUpdate()
    {
       

        NormalCamera.SetActive(true);
        BirdsEyeCamera.SetActive(false);
        if (Input.GetKey("w") || Input.GetKey(KeyCode.UpArrow))
        {
            inp = Vector3.Lerp(Vector3.zero, new Vector3(0, 0, moveSpeed), 0.02f);
            inp = transform.TransformDirection(inp);
            inp.y = 0;
            transform.position += inp;
        }
        if (Input.GetKey("a") || Input.GetKey(KeyCode.LeftArrow))
        {
            transform.eulerAngles += new Vector3(0, (-rotationSpeed) * 0.02f, 0);
        }
        if (Input.GetKey("d") || Input.GetKey(KeyCode.RightArrow))
        {
            transform.eulerAngles += new Vector3(0, (rotationSpeed) * 0.02f, 0);
        }
        if (Input.GetKey("s") || Input.GetKey(KeyCode.DownArrow))
        {
            inp = Vector3.Lerp(Vector3.zero, new Vector3(0, 0, -moveSpeed / 2), 0.02f);
            inp = transform.TransformDirection(inp);
            inp.y = 0;
            transform.position += inp;
        }
        if (Input.GetKey("m"))
        {
            NormalCamera.SetActive(false);
            BirdsEyeCamera.SetActive(true);
        }
        
    }
    IEnumerator ShootTimer()
    {
        // a repeating test to see whether the user is pressing fire
        while (true)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                Manager.GetComponent<PlayerManager>().Bullets(1);
                Bang.Play();
                bullet.Add((GameObject)Instantiate(bulletPrefab, transform.position + transform.TransformDirection(0, 0, 4), transform.rotation));
                bullet[bulletsFired].GetComponent<ShootBullet>().Manager=Manager;
                bulletsFired++;
                
                //man.Bullets(bulletsFired);
                
                yield return new WaitForSeconds(5);
            }
            else
            {
                yield return new WaitForSeconds(0);
            }
        }
    }
    IEnumerator WalkNoise()
    {
        // a repeating test to see whether the user is walking
        while (true)
        {
            if (Input.GetKey(KeyCode.UpArrow)|| Input.GetKey(KeyCode.DownArrow) || Input.GetKey("w")|| Input.GetKey("s"))
            {
                Walk.Play();
                yield return new WaitForSeconds(0.5f);
            }
            else
            {
                yield return new WaitForSeconds(0);
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        GameObject Target = collision.collider.gameObject;
        if (Target.layer == 10)
        {
            Destroy(Target.gameObject);

            Manager.GetComponent<PlayerManager>().kills(1);
        }
        if ( Target.layer == 9)
        {
            Destroy(Target.gameObject);

           
        }
        else
        {
            transform.position -= inp;
            transform.position -= inp;
            transform.position -= inp;
            transform.position -= inp;
            transform.position -= inp;
        }
    }
}
