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
        soundEffects.Add("sword", GameObject.Find("SwordSound").GetComponent<AudioSource>());
        soundEffects.Add("hammer", GameObject.Find("HammerSound").GetComponent<AudioSource>());
        soundEffects.Add("bow", GameObject.Find("BowSound").GetComponent<AudioSource>());
        soundEffects.Add("orcdamage", GameObject.Find("DamageOrcSound").GetComponent<AudioSource>());
        soundEffects.Add("playerdamage", GameObject.Find("DamagePlayerSound").GetComponent<AudioSource>());
        soundEffects.Add("match", GameObject.Find("MatchSound").GetComponent<AudioSource>());
        soundEffects.Add("walk", GameObject.Find("WalkSound").GetComponent<AudioSource>());



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

    public void PlayDelayed(string newName, float newDelay)
    {

        AudioSource tempAudio;
        soundEffects.TryGetValue(newName, out tempAudio);
        tempAudio.PlayDelayed(newDelay);

    }
}
