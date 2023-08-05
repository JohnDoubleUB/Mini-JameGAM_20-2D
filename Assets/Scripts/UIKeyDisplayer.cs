using System.Collections.Generic;
using UnityEngine;

public class UIKeyDisplayer : MonoBehaviour
{
    [SerializeField]
    private GameObject keyUIPrefab;

    private List<GameObject> uiKeys = new List<GameObject>();

    [Range(0, 2)]
    public int KeyVariable = 0;

    private void Update()
    {
        int keyCount = GetKeyCount();

        if (keyCount != uiKeys.Count) 
        {
            bool lessKeysPresent = uiKeys.Count < keyCount;

            if (lessKeysPresent)
            {
                while (uiKeys.Count < keyCount) 
                {
                    uiKeys.Add(Instantiate(keyUIPrefab, transform));
                }
            }
            else 
            {
                int keysToRemove = uiKeys.Count - keyCount;

                for (int i = 0; i < keysToRemove; i++) 
                {
                    Destroy(uiKeys[0]);
                    uiKeys.RemoveAt(0);
                }
            }
        }
    }

    private int GetKeyCount() 
    {
        switch (KeyVariable) 
        {
            case 0:
                return GameManager.current.KeysFound;
            case 1:
                return GameManager.current.KeysPlaced;
            default:
                return GameManager.current.KeysNeeded;
        }
    }
}
