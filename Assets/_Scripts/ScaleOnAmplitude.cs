using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleOnAmplitude : MonoBehaviour
{
    public AudioPeer _audioPeer;

    public float _startScale, _maxScale;
    public bool _useBuffer;
    Material _material;

    public float red, green, blue;

    // Start is called before the first frame update
    void Start()
    {
        _audioPeer = FindObjectOfType<AudioPeer>();
        _material = GetComponent<MeshRenderer>().materials[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (_useBuffer)
        {
            transform.localScale = new Vector3((_audioPeer._AmplitudeBuffer * _maxScale) + _startScale, (_audioPeer._AmplitudeBuffer * _maxScale) + _startScale, (_audioPeer._AmplitudeBuffer * _maxScale) + _startScale);
            Color _color = new Color((red / 255f) * _audioPeer._AmplitudeBuffer, (green / 255f) * _audioPeer._AmplitudeBuffer, (blue / 255f) * _audioPeer._AmplitudeBuffer);
            _material.SetColor("_EmissionColor", _color);
        }
        else
        {
            transform.localScale = new Vector3((_audioPeer._Amplitude * _maxScale) + _startScale, (_audioPeer._Amplitude * _maxScale) + _startScale, (_audioPeer._Amplitude * _maxScale) + _startScale);
            Color _color = new Color((red / 255f) * _audioPeer._Amplitude, (green / 255f) * _audioPeer._Amplitude, (blue / 255f) * _audioPeer._Amplitude);
            _material.SetColor("_EmissionColor", _color);
        }
    }
}
