using System.Collections.Generic;
using UnityEngine;

public class UIKeyDisplayer : MonoBehaviour
{
    [SerializeField]
    private GameObject keyUIPrefab;

    private List<GameObject> uiKeys = new List<GameObject>();

    private void Update()
    {
        if (GameManager.current.KeysFound != uiKeys.Count) 
        {
            bool lessKeysPresent = uiKeys.Count < GameManager.current.KeysFound;

            if (lessKeysPresent)
            {
                while (uiKeys.Count < GameManager.current.KeysFound) 
                {
                    uiKeys.Add(Instantiate(keyUIPrefab, transform));
                }
            }
            else 
            {
                int keysToRemove = uiKeys.Count - GameManager.current.KeysFound;

                for (int i = 0; i < keysToRemove; i++) 
                {
                    Destroy(uiKeys[0]);
                    uiKeys.RemoveAt(0);
                }
            }
        }
    }
}
