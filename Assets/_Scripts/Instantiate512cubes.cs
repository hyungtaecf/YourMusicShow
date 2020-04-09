using System.Collections;
using UnityEngine;

public class Instantiate512cubes : MonoBehaviour
{
    /*
    public GameObject _sampleCubePrefab;

    private const int numberOfSamples = 512;
    private const float goesRound = (float)numberOfSamples/ 360f;
    private float distance = 100f;
    public float _maxScale;

    GameObject[] _sampleCube = new GameObject[numberOfSamples];



    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < numberOfSamples; i++)
        {
            GameObject _instanceSampleCube = (GameObject)Instantiate(_sampleCubePrefab);
            _instanceSampleCube.transform.position = this.transform.position;
            _instanceSampleCube.transform.parent = this.transform;
            _instanceSampleCube.name = "SampleCube" + i;
            this.transform.eulerAngles = new Vector3(0, -goesRound * i, 0); //negative, so it goes in the right direction
            _instanceSampleCube.transform.position = Vector3.forward * distance;
            _sampleCube[i] = _instanceSampleCube;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < numberOfSamples; i++)
        {
            if(_sampleCube != null)
            {
                _sampleCube[i].transform.localScale = new Vector3(10, 
                    (AudioPeer._samples[i] * _maxScale) + 2, 10);
            }
        }
    }
    */
}
