using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class AudioManager : MonoBehaviour
{
    public static AudioManager current;

    public bool UseLevelManagerTracks = false;
    public string[] LoopingTracks;

    private float masterVolume;
    private float musicVolume;
    private float sfxVolume;

    private bool gameHasFocus = true;


    private void Awake()
    {
        if (current != null) Debug.LogWarning("Oops! it looks like there might already be a " + GetType().Name + " in this scene!");
        current = this;

        AkSoundEngine.StopAll();
    }

    private void Start()
    {
        PlayTracks(LoopingTracks);
    }

    private void OnApplicationFocus(bool focus)
    {
        gameHasFocus = focus; //This hopefully will stop all the sounds playing when someone tabs back into the game
    }

    public void PlayTracks(IEnumerable<string> tracks)
    {
        if (IsEditorClone()) return;

        AkSoundEngine.StopAll(gameObject);

        if (tracks.Any())
        {
            foreach (string track in tracks)
            {
                if (string.IsNullOrEmpty(track)) continue;

                print("Playing " + track);
                AkSoundEngine.PostEvent(track, gameObject); //Its better that this calls its own AKSoundEngine event rather than AK_PlayClipOnObject
            }
        }
    }

    public uint AK_PlayClipOnObjectWithEndEventCallback(string eventName, GameObject in_gameObjectID, AkCallbackManager.EventCallback in_pfnCallback, bool onlyWhenGameFocus = true)
    {
        if (IsEditorClone()) return new uint();
        if (onlyWhenGameFocus && gameHasFocus == false) return new uint();

        return AkSoundEngine.PostEvent(eventName, in_gameObjectID, (uint)AkCallbackType.AK_EndOfEvent, in_pfnCallback, in_gameObjectID);
    }


    public void StopAll(GameObject gameObject = null)
    {
        if (IsEditorClone()) return;

        if (gameObject != null)
        {
            AkSoundEngine.StopAll(gameObject);
        }
        else
        {
            AkSoundEngine.StopAll();
        }
    }

    public uint AK_PlayEventAt(string eventName, Vector3 pos, bool onlyWhenGameFocus = true)
    {
        if (IsEditorClone()) return new uint();
        if (onlyWhenGameFocus && gameHasFocus == false) return new uint();

        GameObject tempGO = new GameObject($"TempAudio_{eventName}_"); // create the temp object
        tempGO.transform.position = pos; // set its position
        uint eventID = AkSoundEngine.PostEvent(eventName, tempGO, (uint)AkCallbackType.AK_EndOfEvent, AK_CallbackFunction, tempGO);
        tempGO.name += eventID;
        return eventID;
    }

    //This will follow the object
    public uint AK_PlayClipOnObject(string eventName, GameObject in_gameObjectID, bool onlyWhenGameFocus = true)
    {
        if (IsEditorClone()) return new uint();
        if (onlyWhenGameFocus && gameHasFocus == false) return new uint();

        return AkSoundEngine.PostEvent(eventName, in_gameObjectID);
    }

    public void AK_StopClipsById(IEnumerable<uint> ids)
    {
        if (IsEditorClone()) return;


        foreach (uint id in ids)
        {
            AkSoundEngine.StopPlayingID(id);
        }
    }

    public void AK_StopClipById(uint id)
    {
        if (IsEditorClone()) return;

        AkSoundEngine.StopPlayingID(id);
    }

    public void AK_PlayButtonClickSound()
    {
        //AK_PlayEventAt("Play_MenuClick", transform.position);
        AK_PlayClipOnObject("Play_MenuClick", gameObject, false);
    }


    private void AK_CallbackFunction(object in_cookie, AkCallbackType in_type, object in_info)
    {
        switch (in_type)
        {
            case AkCallbackType.AK_EndOfEvent:
                if (in_cookie is GameObject) Destroy((GameObject)in_cookie);
                break;

            default:
                break;
        }
    }

    public void SetMasterVolume(float value)
    {
        if (IsEditorClone()) return;
        masterVolume = value;
        AkSoundEngine.SetRTPCValue("MasterVolume", masterVolume);
    }

    public void SetMusicVolume(float value)
    {
        if (IsEditorClone()) return;
        musicVolume = value;
        AkSoundEngine.SetRTPCValue("MusicVolume", musicVolume);
    }

    public void SetSFXVolume(float value)
    {
        if (IsEditorClone()) return;
        sfxVolume = value;
        AkSoundEngine.SetRTPCValue("SFXVolume", sfxVolume);
    }



    private void OnDestroy()
    {
        if (IsEditorClone()) return;

        AkSoundEngine.StopAll();
    }

    //This is pretty much just to make sure that in editor clones we don't use any wwise calls at all
    private bool IsEditorClone()
    {
        return false;//In any other circumstance, we want to use it
    }
}