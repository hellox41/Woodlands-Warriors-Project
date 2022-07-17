using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlindSpotsSound : MonoBehaviour
{
    public string soundName;

    [SerializeField] float soundRadius;

    public bool isLeft;

    public AudioClip soundClip;

    AudioSource meowtiToolAudio;

    public GameObject blindSpotsSpawn;
    // Start is called before the first frame update
    void Start()
    {
        meowtiToolAudio = GameObject.Find("Meow-ti Tool").GetComponent<AudioSource>();
        blindSpotsSpawn = transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlaySound()
    {
        meowtiToolAudio.clip = soundClip;
        meowtiToolAudio.Play();
        Debug.Log("Playing " + soundName);
    }

    public void StopSound()
    {
        meowtiToolAudio.Stop();
        Debug.Log("Stopping " + soundName);
    }

}
