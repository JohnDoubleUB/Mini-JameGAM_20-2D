using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    public bool AlwaysKillPlayer;
    public string SoundToPlay;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.WasWithPlayer() && (AlwaysKillPlayer || GameManager.current.CurrentPlayer.IsJumping) && GameManager.current.CurrentPlayer.IsAlive) 
        {
            GameManager.current.CurrentPlayer.Kill();
            if(string.IsNullOrEmpty(SoundToPlay) == false) AudioManager.current.AK_PlayClipOnObject("PlaySpikes", gameObject);
        }
    }
}
