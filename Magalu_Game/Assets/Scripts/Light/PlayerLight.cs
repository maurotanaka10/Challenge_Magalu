using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLight : MonoBehaviour
{
    [SerializeField] private Transform _player;

    private void Update()
    {
        var position = _player.transform.position;
        transform.position = new Vector3(position.x, transform.position.y, position.z);
    }
}
