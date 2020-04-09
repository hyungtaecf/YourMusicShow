using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightOnaudio : MonoBehaviour
{
    public AudioPeer _audioPeer;

    public int _band;
    public float _minIntensity, _maxIntensity;
    Light _light;

    // Start is called before the first frame update
    void Start()
    {
        _audioPeer = FindObjectOfType<AudioPeer>();
        _light = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        _light.intensity = (_audioPeer._audioBandBuffer[_band] * (_maxIntensity - _minIntensity)) + _minIntensity;
    }
}
