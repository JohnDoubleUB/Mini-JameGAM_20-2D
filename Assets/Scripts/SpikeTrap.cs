using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    public bool AlwaysKillPlayer;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.WasWithPlayer() && (AlwaysKillPlayer || GameManager.current.CurrentPlayer.IsJumping)) 
        {
            GameManager.current.CurrentPlayer.Kill();
        }
    }
}
