using UnityEngine;

public class AudioHandler : MonoBehaviour
{
   
    public int number;
    private static AudioHandler instance;
    private AudioSource ac;

    public static AudioHandler Instance
    {
        get { return instance ?? (instance = new GameObject("AudioHandler").AddComponent<AudioHandler>()); }
    }

    public void PlayAudio(AudioSource src)
    {
        ac = src;
        Debug.Log("AudioSource" + ac.name);
        ac.Play();
    }
}