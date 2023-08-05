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
        return collision.gameObject.CompareTag("Player");
    }

    public static bool WasWithPlayer(this Collider2D collision)
    {
        return collision.gameObject.CompareTag("Player");
    }

    public static bool WasWithPlatform(this Collision2D collision)
    {
        return collision.gameObject.CompareTag("Platform");
    }

    public static bool WasWithPlatform(this Collider2D collision)
    {
        return collision.gameObject.CompareTag("Platform");
    }


    public static void StopAllEntities(this ResetableEntity[] resetableEntities)
    {
        foreach (ResetableEntity entity in resetableEntities)
        {
            entity.EntityStop();
        }
    }

    public static void StartAllEntities(this ResetableEntity[] resetableEntities)
    {
        foreach (ResetableEntity entity in resetableEntities)
        {
            entity.EntityStart();
        }
    }

    public static void ResetAllEntities(this ResetableEntity[] resetableEntities)
    {
        foreach (ResetableEntity entity in resetableEntities)
        {
            entity.EntityReset();
        }
    }

    public static void ResetKeysExcludingIds(this KeyPickup[] resetableEntities, IEnumerable<string> excludedIds)
    {
        foreach (KeyPickup entity in resetableEntities)
        {
            if (excludedIds.Contains(entity.UniqueID)) continue;
            entity.EntityReset();
        }
    }

}

