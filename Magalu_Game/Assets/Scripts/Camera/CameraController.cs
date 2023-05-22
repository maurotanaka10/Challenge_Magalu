using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public bool inLabyrinth = false;
    public bool outLabyrinth = false;

    public CinemachineVirtualCamera cameraPlayer;

    void Update()
    {
        StatesCamera();
    }

    void StatesCamera()
    {
        if (inLabyrinth)
        {
            cameraPlayer.transform.rotation = Quaternion.Euler(85f, 0.9f, 0f);
            GameObject.Find("EnterLabCollider").GetComponent<BoxCollider>().isTrigger = true;
        }
        else if (outLabyrinth)
        {
            cameraPlayer.transform.rotation = Quaternion.Euler(21f, 0.9f, 0f);
        }
    }
}
