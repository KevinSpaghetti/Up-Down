using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(AudioSource), typeof(InfinitePlaylistIterable))]
public class SongsPlaylistAudioController : MonoBehaviour
{
    [SerializeField]
    private bool paused;
    public bool playOnStart;
    
    private AudioSource _source;
    private InfinitePlaylistIterable playlist;
    
    // Start is called before the first frame update
    void Start()
    {
        _source = GetComponent<AudioSource>();
        playlist = GetComponent<InfinitePlaylistIterable>();
        
        if(playOnStart) StartPlaying();
    }

    public void StartPlaying()
    {
        StartCoroutine(nameof(PlayClipsSequentally));
    }

    public void StopPlaying()
    {
        StopCoroutine(nameof(PlayClipsSequentally));
        _source.Stop();
        paused = false;
    }

    public void Pause()
    {
        paused = true;
    }
    public void Resume()
    {
        paused = false;
    }

    IEnumerator PlayClipsSequentally()
    {
        foreach (var song in playlist)
        {
            _source.clip = song;
            _source.Play();

            while (_source.isPlaying)
            {
                if (paused)
                {
                    _source.Pause();
                    yield return new WaitUntil(() => paused == false);
                    _source.UnPause();
                }
                yield return null;
            }

        }
    }
    
}
