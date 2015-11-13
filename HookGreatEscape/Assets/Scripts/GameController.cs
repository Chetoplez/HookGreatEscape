using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

    /* Lives between levels */
    private int lives = 5;
    public int Lives { get { return lives; } set { lives = value; change_life_indicator(); } }
    /* Deaths*/
    private int deaths = 0;
    public int Deaths { get { return deaths; } set { deaths = value; } }

    private string hud_handler_name = "HudHandler";
    private GameObject hud_handler;

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    /* Called when lives change */
    public void change_life_indicator() {
        if (hud_handler == null)
            hud_handler = GameObject.Find(hud_handler_name) ?? null;
        if (hud_handler != null)
        { 
            Hud  hud= hud_handler.GetComponent<Hud>();
            hud.Lives = this.lives;
        }
    }


    /* Load level. Must call it from WinArea */
    public static void change_level(int next_level=0) {
        Application.LoadLevel(next_level);
    }
}

