using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class LightMovementBehavior : MonoBehaviour
{
    [SerializeField] private Vector3 _startPosition;
    [SerializeField] private Vector3 _maxPosition;
    [SerializeField] private float _lightVelocity;

    private void Update()
    {
        transform.Translate(Vector3.forward * (_lightVelocity * Time.deltaTime));

        if (transform.position.z >= _maxPosition.z)
        {
            transform.position = _startPosition;
        }
    }
}
