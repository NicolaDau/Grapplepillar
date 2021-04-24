using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;

    [Header("Player Sounds")]
    public AudioClip pickupCollected;
    public AudioClip hit;
    public AudioClip grappled;
    public AudioClip leaveGrapple;

    public static AudioManager Instance = null;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        audioSource = gameObject.GetComponent<AudioSource>();
    }

    private void OnLevelWasLoaded(int level)
    {
        
    }

    public void PlayAudio(AudioClip audio)
    {
        audioSource.pitch = (1);
        audioSource.PlayOneShot(audio);
    }

    public void PlayAudio(AudioClip audio, float pitch)
    {
        audioSource.pitch = (pitch);
        audioSource.PlayOneShot(audio);
    }
    public void PlayAudio(AudioClip audio, float minPitch, float maxPitch)
    {
        audioSource.pitch = (Random.Range(minPitch, maxPitch));
        audioSource.PlayOneShot(audio);
    }
    public void PlayAudio(AudioClip[] audio, float minPitch, float maxPitch)
    {
        audioSource.pitch = (Random.Range(minPitch, maxPitch));
        audioSource.PlayOneShot(audio[Random.Range(0, audio.Length)], 0.4f);
    }
}
