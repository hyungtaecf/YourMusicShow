using System.Collections;
using UnityEngine;

[RequireComponent (typeof (AudioSource))]
public class AudioPeer : MonoBehaviour
{
    AudioSource _audioSource;

    private const int numberOfBands = 8;
    private const int numberOfSamples = 512;
    private const float bufferDecreaseFactorStart = 0.005f;
    private const float bufferDecreaseFactor = 1.2f;
    private const float freqBandMultiplier = 10f;

    private float[] _samplesLeft = new float[numberOfSamples];
    private float[] _samplesRight = new float[numberOfSamples];

    //audio8
    private float[] _freqBand = new float[numberOfBands];
    private float[] _bandBuffer = new float[numberOfBands];
    private float[] _bufferDecrease = new float[numberOfBands];
    private float[] _freqBandHighest = new float[numberOfBands];

    //audio64 -> Depois do tutorial melhor organizar de maneira uniforme
    private float[] _freqBand64 = new float[64];
    private float[] _bandBuffer64 = new float[64];
    private float[] _bufferDecrease64 = new float[64];
    private float[] _freqBandHighest64 = new float[64];
    [HideInInspector]
    public float[] _audioBand64, _audioBandBuffer64;

    [HideInInspector]
    public float[] _audioBand, _audioBandBuffer;

    [HideInInspector]
    public float _Amplitude, _AmplitudeBuffer;

    private float _AmplitudeHighest;

    public float _audioProfile;
    
    public enum _channel {Stereo, Left, Right}
    public _channel channel = new _channel();

    // Start is called before the first frame update
    void Start()
    {
        _audioBand = new float[numberOfBands];
        _audioBandBuffer = new float[numberOfBands];

        _audioBand64 = new float[64];
        _audioBandBuffer64 = new float[64];

        _audioSource = GetComponent<AudioSource>();
        AudioProfile(_audioProfile);
    }

    // Update is called once per frame
    void Update()
    {
        GetSpectrumAudioSource();
        Make8FrequencyBands();
        Make64FrequencyBands();
        CreateAudioBands();
        Create64AudioBands();
        GetAmplitude();
        BandBuffer();
        BandBuffer64();
    }

    void AudioProfile(float audioProfile)
    {
        for (int i = 0; i < numberOfBands; i++)
        {
            _freqBandHighest[i] = audioProfile;
        }
    }

    void GetSpectrumAudioSource()
    {
        _audioSource.GetSpectrumData(_samplesLeft, 0, FFTWindow.Blackman);
        _audioSource.GetSpectrumData(_samplesRight, 1, FFTWindow.Blackman);
    }

    void Make8FrequencyBands()
    {
        /*
            22050 heartz / 512 = 43 hertz per sample

            20 - 60 hertz
            60 - 250 hertz
            250 - 500 hertz
            500 - 2000 hertz
            2000 - 4000 hertz
            4000 - 6000 hertz
            6000 - 20000 hertz
            
            0: 2   = 86hertz
            1: 4   = 172hz     -> 87hz-258hz
            2: 8   = 344hz     -> 259hz-602hz
            3: 16  = 688hz     -> 603hz-1290hz
            4: 32  = 1376hz    -> 1291hz-2666hz
            5: 64  = 2752hz    -> 2667hz-5418hz
            6: 128 = 5504hz    -> 5419hz-10922hz 
            7: 256 = 11008hz   -> 10923hz-21930hz
            total sum: 510 -> samples
        */

        int count = 0;

        for (int i = 0; i < numberOfBands; i++)
        {
            float average = 0;

            int sampleCount = (int)Mathf.Pow(2, i) * 2; //making the table written before

            if (i == (numberOfBands - 1))
            {
                sampleCount += 2; // because the samples are 512, but the calculation sum is 510 (should be 512 because GetSpectrumData needs a number of samples that is multiple of 2)
            }
            for (int j = 0; j < sampleCount; j++)
            {
                switch(channel)
                {
                    case _channel.Left:
                        average += _samplesLeft[count] * (count + 1);
                        break;
                    case _channel.Right:
                        average += _samplesRight[count] * (count + 1);
                        break;
                    default:
                    case _channel.Stereo:
                        average += ((_samplesLeft[count] + _samplesRight[count])/2) * (count + 1); //In the tutorial they didn't diveded by 2, but I find it better for stereo
                        break;
                }
                count++;
            }

            average /= count;

            _freqBand[i] = average * freqBandMultiplier; // just because it is a too small number


        }

    }

    void Make64FrequencyBands()
    {
        /*
            0-15  = 1 sample    =  16
            16-31 = 2 samples   =  32
            32-39 = 4 samples   =  32
            40-47 = 6 samples   =  48      * 6 samples, not 8, else we cannot get the right sum
            48-55 = 16 samples  = 128
            56-63 = 32 samples  = 256 +
                                  ---
                                  512
        */

        int count = 0;
        int sampleCount = 1;
        int power = 0;

        for (int i = 0; i < 64; i++)
        {
            float average = 0;

            if (i == 16 || i == 32 || i == 40 || i == 48 || i == 56)
            {
                power++;
                sampleCount = (int)Mathf.Pow(2, power);
                if(power == 3)
                {
                    sampleCount -= 2; //Because if you see the table it is 6, not 8
                }
            }
            for (int j = 0; j < sampleCount; j++)
            {
                switch (channel)
                {
                    case _channel.Left:
                        average += _samplesLeft[count] * (count + 1);
                        break;
                    case _channel.Right:
                        average += _samplesRight[count] * (count + 1);
                        break;
                    default:
                    case _channel.Stereo:
                        average += ((_samplesLeft[count] + _samplesRight[count]) / 2) * (count + 1); //In the tutorial they didn't diveded by 2, but I find it better for stereo
                        break;
                }
                count++;
            }

            average /= count;

            _freqBand64[i] = average * 80; // They didn't explain why this time is 80


        }

    }

    void CreateAudioBands()
    {
        for (int i = 0; i < numberOfBands; i++)
        {
            if (_freqBand[i] > _freqBandHighest[i])
            {
                _freqBandHighest[i] = _freqBand[i];
            }
            if (_freqBandHighest[i] != 0)
            {
                _audioBand[i] = (_freqBand[i] / _freqBandHighest[i]);
                _audioBandBuffer[i] = (_bandBuffer[i] / _freqBandHighest[i]);
            }
        }
    }

    void Create64AudioBands()
    {
        for (int i = 0; i < 64; i++)
        {
            if (_freqBand64[i] > _freqBandHighest64[i])
            {
                _freqBandHighest64[i] = _freqBand64[i];
            }
            if (_freqBandHighest64[i] != 0)
            {
                _audioBand64[i] = (_freqBand64[i] / _freqBandHighest64[i]);
                _audioBandBuffer64[i] = (_bandBuffer64[i] / _freqBandHighest64[i]);
            }
        }
    }

    void GetAmplitude()
    {
        float _CurrentAmplitude = 0;
        float _CurrentAmplitudeBuffer = 0;
        for(int i=0; i<numberOfBands; i++)
        {
            _CurrentAmplitude += _audioBand[i];
            _CurrentAmplitudeBuffer += _audioBandBuffer[i];
        }
        if(_CurrentAmplitude > _AmplitudeHighest)
        {
            _AmplitudeHighest = _CurrentAmplitude;
        }
        _Amplitude = _CurrentAmplitude / _AmplitudeHighest;
        _AmplitudeBuffer = _CurrentAmplitudeBuffer / _AmplitudeHighest;
    }

    void BandBuffer()
    {
        for (int g = 0; g < numberOfBands; ++g)
        {
            if (_freqBand[g] > _bandBuffer[g])
            {
                _bandBuffer[g] = _freqBand[g];
                _bufferDecrease[g] = bufferDecreaseFactorStart;
            }
            else
            {
                if (_bandBuffer[g] > bufferDecreaseFactorStart)
                {
                    _bandBuffer[g] -= _bufferDecrease[g];
                }
                _bufferDecrease[g] *= bufferDecreaseFactor;
            }
        }
    }

    void BandBuffer64()
    {
        for (int g = 0; g < 64; ++g)
        {
            if (_freqBand64[g] > _bandBuffer64[g])
            {
                _bandBuffer64[g] = _freqBand64[g];
                _bufferDecrease64[g] = bufferDecreaseFactorStart;
            }
            else
            {
                if (_bandBuffer64[g] > bufferDecreaseFactorStart)
                {
                    _bandBuffer64[g] -= _bufferDecrease64[g];
                }
                _bufferDecrease64[g] *= bufferDecreaseFactor;
            }
        }
    }


}
