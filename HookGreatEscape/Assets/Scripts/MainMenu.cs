using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

    /* Start game */
    public void start_game() {
        Application.LoadLevel(1);
    }

    /* Exit game */
    public void exit_game() {
        Application.Quit();
    }
}
