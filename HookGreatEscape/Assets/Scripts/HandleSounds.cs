using UnityEngine;

public class AudioHandler : MonoBehaviour
{

    private static AudioHandler instance;
    private AudioSource ac;

    public static AudioHandler Instance
    {
        get { return instance ?? (instance = new GameObject("AudioHandler").AddComponent<AudioHandler>()); }
    }

    public void PlayAudio(AudioSource src)
    {
        ac = src;
        ac.Play();
    }
}