using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public static class ExtensionMethods
{
    public static bool WasWithPlayer(this Collision2D collision) 
    {
        return collision.gameObject.tag == "Player";
    }

    public static bool WasWithPlayer(this Collider2D collision)
    {
        return collision.gameObject.tag == "Player";
    }

}

