using UnityEngine;
using System.Collections;

/// <summary>
/// Handles everything sound related
/// </summary>
public class SoundModule : MonoBehaviour
{


    public AudioClip popSound;
    private AudioSource source;

    // Use this for initialization
    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Plays the sound specified by a_soundtoplay
    /// </summary>
    /// <param name="a_soundtoplay"></param>
    public void PlaySound(string a_soundtoplay)
    {
        if (a_soundtoplay == "popsound")
            source.PlayOneShot(popSound, 1F);
    }

}
