using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkLight : MonoBehaviour
{
    private Light _light;

    [SerializeField] private float _minIntensity;
    [SerializeField] private float _maxIntensity;

    private void Awake()
    {
        _light = GetComponent<Light>();

        InvokeRepeating(nameof(MakeTheLightBlink), 1.0f, 1.0f);
    }

    private void MakeTheLightBlink()
    {
        StartCoroutine(BlinkLightTimer());
        _light.intensity = _maxIntensity;
    }

    IEnumerator BlinkLightTimer()
    {
        yield return new WaitForSeconds(0.5f);
        _light.intensity = _minIntensity;
    }
}
