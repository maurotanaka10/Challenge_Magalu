using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class ColorOrderBehavior : MonoBehaviour
{
    [Header("Button Variables")] public bool canPressButton;
    [SerializeField] private Transform buttonPosition;
    [SerializeField] private LayerMask playerMask;

    [Header("Obstacles")] [SerializeField] private GameObject[] obstacleColor;
    private Color newColor;
    [SerializeField] private Color redColor;
    [SerializeField] private Color blueColor;
    [SerializeField] private Color StartColor;
    public int obstacle1Index, obstacle2Index, obstacle3Index, obstacle4Index;

    private bool _challengeWasStart;

    private void Awake()
    {
        ObstacleManager.OnUsingItemHandler += ShowChallenge;
        _challengeWasStart = false;
    }

    private void ShowChallenge(bool context)
    {
        if (!_challengeWasStart)
        {
            StartCoroutine(StartChallenge());
            _challengeWasStart = true;
        }
        else if (_challengeWasStart)
        {
            StartCoroutine(ShowChallengeAgain());
        }
    }

    void Update()
    {
        canPressButton = Physics.CheckSphere(buttonPosition.position, 3f, playerMask);
    }

    IEnumerator ShowChallengeAgain()
    {
        yield return new WaitForSeconds(0.3f);
        if (obstacle1Index == 1)
        {
            obstacleColor[0].GetComponent<Renderer>().material.color = redColor;
        }
        else if(obstacle1Index == 2)
        {
            obstacleColor[0].GetComponent<Renderer>().material.color = blueColor;
        }
        yield return new WaitForSeconds(0.3f);
        if (obstacle2Index == 1)
        {
            obstacleColor[1].GetComponent<Renderer>().material.color = redColor;
        }
        else if(obstacle2Index == 2)
        {
            obstacleColor[1].GetComponent<Renderer>().material.color = blueColor;
        }
        yield return new WaitForSeconds(0.3f);
        if (obstacle3Index == 1)
        {
            obstacleColor[2].GetComponent<Renderer>().material.color = redColor;
        }
        else if(obstacle3Index == 2)
        {
            obstacleColor[2].GetComponent<Renderer>().material.color = blueColor;
        }
        yield return new WaitForSeconds(0.3f);
        if (obstacle4Index == 1)
        {
            obstacleColor[3].GetComponent<Renderer>().material.color = redColor;
        }
        else if(obstacle4Index == 2)
        {
            obstacleColor[3].GetComponent<Renderer>().material.color = blueColor;
        }
        yield return new WaitForSeconds(0.5f);
        obstacleColor[0].GetComponent<Renderer>().material.color = StartColor;
        obstacleColor[1].GetComponent<Renderer>().material.color = StartColor;
        obstacleColor[2].GetComponent<Renderer>().material.color = StartColor;
        obstacleColor[3].GetComponent<Renderer>().material.color = StartColor;
    }

    IEnumerator StartChallenge()
    {
        yield return new WaitForSeconds(0.3f);
        newColor = Random.value < 0.5f ? redColor : blueColor;
        if (newColor == redColor)
        {
            obstacle1Index = 1;
        }
        else
        {
            obstacle1Index = 2;
        }

        obstacleColor[0].GetComponent<Renderer>().material.color = newColor;

        yield return new WaitForSeconds(0.3f);
        newColor = Random.value < 0.5f ? redColor : blueColor;
        if (newColor == redColor)
        {
            obstacle2Index = 1;
        }
        else
        {
            obstacle2Index = 2;
        }

        obstacleColor[1].GetComponent<Renderer>().material.color = newColor;

        yield return new WaitForSeconds(0.3f);
        newColor = Random.value < 0.5f ? redColor : blueColor;
        if (newColor == redColor)
        {
            obstacle3Index = 1;
        }
        else
        {
            obstacle3Index = 2;
        }

        obstacleColor[2].GetComponent<Renderer>().material.color = newColor;

        yield return new WaitForSeconds(0.3f);
        newColor = Random.value < 0.5f ? redColor : blueColor;
        if (newColor == redColor)
        {
            obstacle4Index = 1;
        }
        else
        {
            obstacle4Index = 2;
        }

        obstacleColor[3].GetComponent<Renderer>().material.color = newColor;

        yield return new WaitForSeconds(0.5f);
        obstacleColor[0].GetComponent<Renderer>().material.color = StartColor;
        obstacleColor[1].GetComponent<Renderer>().material.color = StartColor;
        obstacleColor[2].GetComponent<Renderer>().material.color = StartColor;
        obstacleColor[3].GetComponent<Renderer>().material.color = StartColor;
    }

    private void OnDisable()
    {
        ObstacleManager.OnUsingItemHandler -= ShowChallenge;
    }
}