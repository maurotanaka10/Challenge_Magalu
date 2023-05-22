using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LaserBehavior : MonoBehaviour
{
    [SerializeField] private float laserVelocity;
    [SerializeField] private Transform startPoint;

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

        if(collision.gameObject.tag == "Player")
        {
            SceneManager.LoadScene("Level1");
        }
    }
}
