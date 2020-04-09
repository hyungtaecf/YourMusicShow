using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParamCube : MonoBehaviour
{
    public AudioPeer _audioPeer;

    public int _band;
    public float _startScale, _scaleMultiplier;
    public bool _useBuffer;
    Material _material;

    //rgb
    public static float red = 92;
    public static float green = 200;
    public static float blue = 255;

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
            transform.localScale = new Vector3(transform.localScale.x, (_audioPeer._audioBandBuffer[_band] * _scaleMultiplier) + _startScale, transform.localScale.z);
            Color _color = new Color(_audioPeer._audioBandBuffer[_band]*(red/255f), _audioPeer._audioBandBuffer[_band]*(green/255f), _audioPeer._audioBandBuffer[_band] * (blue / 255f));
            _material.SetColor("_EmissionColor", _color);
        }
        else {
            transform.localScale = new Vector3(transform.localScale.x, (_audioPeer._audioBand[_band] * _scaleMultiplier) + _startScale, transform.localScale.z);
            Color _color = new Color(_audioPeer._audioBand[_band], _audioPeer._audioBand[_band], _audioPeer._audioBand[_band]);
            _material.SetColor("_EmissionColor", _color);
        }
    }
}
