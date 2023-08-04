using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [SerializeField]
    private Text TimerText;

    // Update is called once per frame
    void Update()
    {
        if (TimerText != null) 
        {
            TimerText.text = Mathf.FloorToInt(Mathf.Max(GameManager.current.CurrentGameTimer, 0)).ToString();
        }
    }
}
