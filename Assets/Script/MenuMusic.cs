using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;


public class MenuMusic : SoundManager
{
    public AudioClip[] menuSoundtrack;
    public float pauseBetweenSongsDuration;

    private bool canGetNextSong = false;

    protected override void Awake()
    {
        base.Awake();

        if (audioSource.clip == null)
        {
            audioSource.clip = menuSoundtrack[Random.Range(0, menuSoundtrack.Length)];
            audioSource.Play();
        }
    }

    protected new void Start ()
    {
        base.Start();
    }

    private void Update()
    {
        if (!audioSource.isPlaying && !canGetNextSong)
        {
            canGetNextSong = true;
            StartCoroutine(GetNextSong(pauseBetweenSongsDuration));
        }
    }

    IEnumerator GetNextSong (float delay)
    {
        yield return new WaitForSeconds(delay);

        List<AudioClip> otherTracks = new List<AudioClip>();

        foreach (AudioClip track in menuSoundtrack)
        {
            if (track != audioSource.clip)
                otherTracks.Add(track);
        }

        audioSource.clip = otherTracks[Random.Range(0, otherTracks.Count)];
        audioSource.Play();
        canGetNextSong = false;
    }
}