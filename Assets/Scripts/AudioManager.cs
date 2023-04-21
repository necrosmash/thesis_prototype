using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    Dictionary<string, AudioSource> soundEffects;

    // Start is called before the first frame update
    void Start()
    {

        soundEffects = new Dictionary<string, AudioSource>();

        soundEffects.Add("explosion", GameObject.Find("ExplosionSound").GetComponent<AudioSource>());
        soundEffects.Add("shatter", GameObject.Find("ShatterSound").GetComponent<AudioSource>());
        soundEffects.Add("voice0", GameObject.Find("VoiceSound0").GetComponent<AudioSource>());
        soundEffects.Add("voice1", GameObject.Find("VoiceSound1").GetComponent<AudioSource>());
        soundEffects.Add("voice2", GameObject.Find("VoiceSound2").GetComponent<AudioSource>());
        soundEffects.Add("voiceplayermale", GameObject.Find("VoicePlayerMale").GetComponent<AudioSource>());
        soundEffects.Add("voiceplayerfemale", GameObject.Find("VoicePlayerFemale").GetComponent<AudioSource>());




    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Play(string newName)
    {

        AudioSource tempAudio;
        soundEffects.TryGetValue(newName, out tempAudio);
        tempAudio.Play();

    }
}
