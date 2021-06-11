using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class InfinitePlaylistIterable : MonoBehaviour, IEnumerable<AudioClip>
{
    
    public List<AudioClip> songs;

    private Random rng;
    public void Start()
    {
        rng = new Random();
    }

    //Returns an infinite amount of random chosen songs from the songs list
    public IEnumerator<AudioClip> GetEnumerator()
    {
        for (;;)
        {
            int songIndex = rng.Next(0, songs.Count);
            yield return songs[songIndex];
        }
    }
    
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
