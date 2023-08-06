using System.Collections;
using System.Collections.Generic;
using UIManagerLibrary.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public int step = 0;

    private void Update()
    {
        if (Input.GetButtonDown("Interact")) 
        {
            if (step == 0)
            {
                UIManager.current.SetActiveContexts(false, "Start");
                UIManager.current.SetActiveContexts(true, "Controls");
                step = 1;
            }
            else 
            {
                SceneManager.LoadScene("SampleScene");
            }
        }

        if (Input.GetButtonDown("Cancel")) 
        {
            Application.Quit();
        }
    }
}
