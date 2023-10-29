using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootBullet : MonoBehaviour
{
    public float shootSpeed = 20f;
    private float speed;
    public float minspeed = 10f;
    public float speedDiv = 5f;
    private Vector3 inp;
    public GameObject Manager;
    private void Awake()
    {   
        speed = shootSpeed;
        StartCoroutine(lifeSpan());

    }
    IEnumerator lifeSpan()
    {
        while (true)
        {
            if (speed <= minspeed)
            {
                Destroy(this.gameObject);
            }

            speed = speed - (speed /( speedDiv*speedDiv));
            yield return new WaitForSeconds(1);
        }
    }
    private void FixedUpdate()
    {


        moveBullet();

        
    }
    public void moveBullet()
    {
        inp = Vector3.Lerp(Vector3.zero, new Vector3(0, 0, speed), 0.02f);
        inp = transform.TransformDirection(inp);
        inp.y = 0;
        transform.position += inp;
    }
    private void OnCollisionEnter(Collision collision)
    {
        GameObject Target = collision.collider.gameObject;
        if (Target.layer == 10 || Target.layer == 12)
        {
            Destroy(Target.gameObject);
            //man.kills(1);
            Manager.GetComponent<PlayerManager>().kills(1);

        }
        speed = speed - speed / speedDiv;
       // Destroy(this.gameObject);
    }
}
