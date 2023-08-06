using System.Collections;
using System.Collections.Generic;
using UIManagerLibrary.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ThanksForPlaying : MonoBehaviour
{
    // Start is called before the first frame updat
    private void Update()
    {
        if (Input.GetButtonDown("Interact"))
        {

            SceneManager.LoadScene("MainMenu");

        }

        if (Input.GetButtonDown("Cancel"))
        {
            Application.Quit();
        }
    }
}
