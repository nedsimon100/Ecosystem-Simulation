using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VeggieDie : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        GameObject Target = collision.collider.gameObject;
        if (Target.layer == 4)
        {
            Destroy(this.gameObject);
        }
    }
}
