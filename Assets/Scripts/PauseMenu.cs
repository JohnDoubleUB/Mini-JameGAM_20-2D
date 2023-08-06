using System.Collections;
using System.Collections.Generic;
using UIManagerLibrary.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private bool enablePause;

    private void Update()
    {
        if (Input.GetButtonDown("Cancel")) 
        {
            SetPause(!enablePause);
        }
    }

    private void SetPause(bool pause) 
    {
        UIManager.current.SetActiveContexts(pause, "Pause");
        Cursor.visible = pause;
        enablePause = pause;
        GameManager.current.IsPaused = pause;
    }

    public void UnPause() 
    {
        SetPause(false);
    }

    public void ReturnToMainMenu() 
    {
        SceneManager.LoadScene("MainMenu");
    }
}
