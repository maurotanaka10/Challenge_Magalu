using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallGroundBehavior : MonoBehaviour
{
    private Rigidbody rigidBody;
    private MeshRenderer _meshRenderer;

    [Header("Indentifier Ground")]
    [SerializeField] private bool isRedGround;
    [SerializeField] private bool isBlueGround;

    [Header("Obstacle Order")]
    [SerializeField] private bool isFirstObstacle;
    [SerializeField] private bool isSecondObstacle;
    [SerializeField] private bool isThirdObstacle;
    [SerializeField] private bool isFourthObstacle;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isRedGround)
        {
            if (isFirstObstacle)
            {
                if (other.CompareTag("Player"))
                {
                    if (GameObject.Find("Button").GetComponent<ColorOrderBehavior>().obstacle1Index != 1)
                    {
                        rigidBody.useGravity = true;
                        _meshRenderer.enabled = false;
                    }
                }
            }
            if (isSecondObstacle)
            {
                if (other.CompareTag("Player"))
                {
                    if (GameObject.Find("Button").GetComponent<ColorOrderBehavior>().obstacle2Index != 1)
                    {
                        rigidBody.useGravity = true;
                        _meshRenderer.enabled = false;
                    }
                }
            }
            if (isThirdObstacle)
            {
                if (other.CompareTag("Player"))
                {
                    if (GameObject.Find("Button").GetComponent<ColorOrderBehavior>().obstacle3Index != 1)
                    {
                        rigidBody.useGravity = true;
                        _meshRenderer.enabled = false;
                    }
                }
            }
            if (isFourthObstacle)
            {
                if (other.CompareTag("Player"))
                {
                    if (GameObject.Find("Button").GetComponent<ColorOrderBehavior>().obstacle4Index != 1)
                    {
                        rigidBody.useGravity = true;
                        _meshRenderer.enabled = false;
                    }
                }
            }
        }

        if (isBlueGround)
        {
            if (isFirstObstacle)
            {
                if (other.CompareTag("Player"))
                {
                    if (GameObject.Find("Button").GetComponent<ColorOrderBehavior>().obstacle1Index != 2)
                    {
                        rigidBody.useGravity = true;
                        _meshRenderer.enabled = false;
                    }
                }
            }
            if (isSecondObstacle)
            {
                if (other.CompareTag("Player"))
                {
                    if (GameObject.Find("Button").GetComponent<ColorOrderBehavior>().obstacle2Index != 2)
                    {
                        rigidBody.useGravity = true;
                        _meshRenderer.enabled = false;
                    }
                }
            }
            if (isThirdObstacle)
            {
                if (other.CompareTag("Player"))
                {
                    if (GameObject.Find("Button").GetComponent<ColorOrderBehavior>().obstacle3Index != 2)
                    {
                        rigidBody.useGravity = true;
                        _meshRenderer.enabled = false;
                    }
                }
            }
            if (isFourthObstacle)
            {
                if (other.CompareTag("Player"))
                {
                    if (GameObject.Find("Button").GetComponent<ColorOrderBehavior>().obstacle4Index != 2)
                    {
                        rigidBody.useGravity = true;
                        _meshRenderer.enabled = false;
                    }
                }
            }
        }

    }
    
    
}
