using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorOrderBehavior : MonoBehaviour
{
    [Header("Button Variables")]
    public bool canPressButton;
    [SerializeField] private Transform buttonPosition;
    [SerializeField] private LayerMask playerMask;

    [Header("Obstacles")]
    [SerializeField] private GameObject[] obstacleColor;
    private Color newColor;
    [SerializeField] private Color redColor;
    [SerializeField] private Color blueColor;
    [SerializeField] private Color StartColor;
    public int obstacle1Index, obstacle2Index, obstacle3Index, obstacle4Index;
    
    void Update()
    {
        canPressButton = Physics.CheckSphere(buttonPosition.position, 3f, playerMask);
    }

    public void ShowChallenge()
    {
        StartCoroutine(StartChallenge());
    }

    IEnumerator StartChallenge()
    {
        yield return new WaitForSeconds(0.3f);
        newColor = Random.value < 0.5f ? redColor : blueColor;
        if(newColor == redColor)
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
}
