using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class AudioManager : MonoBehaviour
{
    public static AudioManager current;

    public bool UseLevelManagerTracks = false;
    public string[] LoopingTracks;
    private Dictionary<string, uint> activeTracks = new Dictionary<string, uint>();

    private float masterVolume;
    private float musicVolume;
    private float sfxVolume;

    private bool gameHasFocus = true;


    private void Awake()
    {
        if (current != null) Debug.LogWarning("Oops! it looks like there might already be a " + GetType().Name + " in this scene!");
        current = this;
        try
        {
            AkSoundEngine.StopAll();
        }
        catch { }
    }

    private void Start()
    {
        PlayTracks(LoopingTracks);
    }

    private void OnApplicationFocus(bool focus)
    {
        gameHasFocus = focus; //This hopefully will stop all the sounds playing when someone tabs back into the game
    }

    public bool StopTrack(string eventName)
    {
        if (activeTracks.TryGetValue(eventName, out uint value) == false) return false;
        try
        {
            AkSoundEngine.StopPlayingID(value, 300, AkCurveInterpolation.AkCurveInterpolation_Linear);
        }
        catch
        {
            return false;
        }


        activeTracks.Remove(eventName);

        return true;
    }

    public bool PlayTrack(string eventName)
    {
        if (activeTracks.ContainsKey(eventName)) return false;
        try
        {
            activeTracks.Add(eventName, AkSoundEngine.PostEvent(eventName, gameObject));
        }
        catch
        {
            return false;
        }

        return true;
    }

    public bool PlayTracks(IEnumerable<string> tracks)
    {
        if (IsEditorClone()) return false;

        try
        {
            AkSoundEngine.StopAll(gameObject);
        }
        catch
        {
            return false;
        }

        if (tracks.Any())
        {
            try
            {
                foreach (string track in tracks)
                {
                    if (string.IsNullOrEmpty(track)) continue;

                    print("Playing " + track);
                    activeTracks.Add(track, AkSoundEngine.PostEvent(track, gameObject)); //Its better that this calls its own AKSoundEngine event rather than AK_PlayClipOnObject
                }
            }
            catch
            {
                return false;
            }
        }

        return true;
    }

    public uint? AK_PlayClipOnObjectWithEndEventCallback(string eventName, GameObject in_gameObjectID, AkCallbackManager.EventCallback in_pfnCallback, bool onlyWhenGameFocus = true)
    {
        if (IsEditorClone()) return new uint();
        if (onlyWhenGameFocus && gameHasFocus == false) return new uint();

        try
        {
            uint id = AkSoundEngine.PostEvent(eventName, in_gameObjectID, (uint)AkCallbackType.AK_EndOfEvent, in_pfnCallback, in_gameObjectID);
            return id;
        }
        catch
        {
            return null;
        }
    }


    public bool StopAll(GameObject gameObject = null)
    {
        if (IsEditorClone()) return false;
        try
        {
            if (gameObject != null)
            {
                AkSoundEngine.StopAll(gameObject);
            }
            else
            {
                AkSoundEngine.StopAll();
            }
        }
        catch 
        {
            return false;
        }

        return true;
    }

    public uint? AK_PlayEventAt(string eventName, Vector3 pos, bool onlyWhenGameFocus = true)
    {
        if (IsEditorClone()) return null;
        if (onlyWhenGameFocus && gameHasFocus == false) return null;

        GameObject tempGO = new GameObject($"TempAudio_{eventName}_"); // create the temp object
        tempGO.transform.position = pos; // set its position

        try
        {
            uint eventID = AkSoundEngine.PostEvent(eventName, tempGO, (uint)AkCallbackType.AK_EndOfEvent, AK_CallbackFunction, tempGO);
            tempGO.name += eventID;
            return eventID;
        }
        catch 
        {
            return null;
        }
    }

    //This will follow the object
    public uint? AK_PlayClipOnObject(string eventName, GameObject in_gameObjectID, bool onlyWhenGameFocus = true)
    {
        if (IsEditorClone()) return null;
        if (onlyWhenGameFocus && gameHasFocus == false) return null;

        try
        {
            uint id = AkSoundEngine.PostEvent(eventName, in_gameObjectID);
            return id;
        }
        catch 
        {
            return null;
        }
    }

    public bool AK_StopClipsById(IEnumerable<uint> ids)
    {
        if (IsEditorClone()) return false;

        try
        {
            foreach (uint id in ids)
            {
                AkSoundEngine.StopPlayingID(id);
            }

            return true;
        }
        catch 
        {
            return false;
        }
    }

    public bool AK_StopClipById(uint id)
    {
        if (IsEditorClone()) return false;

        try
        {
            AkSoundEngine.StopPlayingID(id);
        } catch 
        {
            return false;
        }

        return true;
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

    private void OnDestroy()
    {
        if (IsEditorClone()) return;
        try
        {
            AkSoundEngine.StopAll();
        }
        catch { }
    }

    //This is pretty much just to make sure that in editor clones we don't use any wwise calls at all
    private bool IsEditorClone()
    {
        return false;//In any other circumstance, we want to use it
    }
}