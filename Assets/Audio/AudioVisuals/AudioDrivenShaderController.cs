using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AudioDrivenShaderController : MonoBehaviour
{
    public GameObject audioFrequencyRepresentationPrefab;
    public int nOfFrequencies;
    public float distanceBetween;

    private List<GameObject> audioFrequencyRepresentations;

    public AudioSource _audioSource; 


    void Start()
    {

        audioFrequencyRepresentations = new List<GameObject>();

        for (int i = 0; i < nOfFrequencies; i++){
            GameObject rep = Instantiate(audioFrequencyRepresentationPrefab, transform) as GameObject;
            rep.transform.SetParent(transform, false);
            rep.transform.localPosition = new Vector3(0, 0, i * distanceBetween);
            audioFrequencyRepresentations.Add(rep);
        }

    }

    void Update(){

        if(_audioSource.isPlaying){
            float[] spectrum = new float[64];
            _audioSource.GetSpectrumData(spectrum, 0, FFTWindow.Rectangular);
        }
        
    }

    public void OnDrawGizmos()
    {
        if(_audioSource.isPlaying)
        {
            const int n_samples = 64;
            float[] spectrum = new float[n_samples];
            _audioSource.GetSpectrumData(spectrum, 0, FFTWindow.Rectangular);
            
            Gizmos.color = Color.blue;
            float avg = spectrum.Sum() / n_samples;
            for (int i = 0; i < n_samples - 1; i++)
            {
                Vector3 startPoint = new Vector3(0, avg * 20, i * distanceBetween);
                Vector3 endPoint = new Vector3(0, avg * 20, i+1 * distanceBetween);
                Gizmos.DrawSphere(transform.position + startPoint, 0.2f);
                Gizmos.DrawLine(transform.position + startPoint, transform.position + endPoint);
            }
        }
    }
}
