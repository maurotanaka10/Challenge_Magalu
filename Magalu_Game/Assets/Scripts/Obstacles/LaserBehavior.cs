using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBehavior : MonoBehaviour
{
    [SerializeField] private float laserVelocity;

    void Update()
    {
        transform.Translate(Vector3.right.normalized * laserVelocity * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "ReboundWall")
        {
            laserVelocity *= -1;
        }
    }
}
