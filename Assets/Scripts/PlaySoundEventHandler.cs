using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundEventHandler : MonoBehaviour
{
    public bool AllowSound = true;
    public void PlaySound(string eventName) 
    {
        if (string.IsNullOrEmpty(eventName)) return;
        AudioManager.current.AK_PlayClipOnObject(eventName, gameObject);
    }
}
