using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideNearPlayer : MonoBehaviour
{
    public float HideDist = 10f;
    public MeshRenderer MR;
    void Update()
    {
        int ff = 0;
        Vector3 j = (transform.forward);
        Ray l = new Ray(transform.position, j);
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, HideDist);

        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            if (target.gameObject.layer == 11)
            {
                MR.enabled=false;
                ff = 1;
            }
        }
        if (ff == 0)
        {
            MR.enabled = true;
        }
    }
}
