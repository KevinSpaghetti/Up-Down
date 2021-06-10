using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SongsPlaylistAudioController : MonoBehaviour
{
    public bool paused;
    public bool loopSongList;
    public bool playOnStart;
    public List<AudioClip> songs;
    
    
    private int indexOfPlayingSong = 0;
    private AudioSource _source;
    
    
    // Start is called before the first frame update
    void Start()
    {
        _source = GetComponent<AudioSource>();
        
        if(playOnStart) StartPlaying();
    }

    public void StartPlaying()
    {
        songs = songs.OrderBy(a => Guid.NewGuid()).ToList();
        StartCoroutine(nameof(PlayClipsSequentally));
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
        while (loopSongList)
        {
            for (int i = 0; i < songs.Count; ++i)
            {
                _source.clip = songs[i];
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
    
}
