using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour {

    private bool pause_activated = false;
    public bool Pause { set { pause_activated = value; } }
    [Range(1,3)]
    public int level = 1;
    private Canvas my_canvas;
    private HandlePlayerInput hpi = null;
    private GameController game_controller = null;

    void Start() {
        get_player_input();
        my_canvas=GetComponentInChildren<Canvas>();
        my_canvas.gameObject.SetActive(false);
    }

    void get_player_input() { 
        GameObject gc = GameObject.Find("GameController") ?? null;
        if (gc != null)
        {
            hpi = gc.GetComponent<HandlePlayerInput>() ?? null;
            game_controller = gc.GetComponent<GameController>() ?? null;
            if (hpi != null)
                hpi.Pause_menu = this.gameObject;
        }
    }

	
	void Update () {
        my_canvas.gameObject.SetActive(pause_activated);
	}

    public void restart() { 
        Application.LoadLevel(level); 
        pause_activated = false;
        game_controller.Lives = 5;
        hpi.Alive = true; 
        hpi.Pause = false;
        Time.timeScale = 1; 
    }
    public void pause() { Time.timeScale = 0; pause_activated = true; hpi.Pause = true; }
    public void resume() { Time.timeScale = 1; pause_activated = false; hpi.Pause = false; }
    public void backtomain() { Application.LoadLevel(0); hpi.Pause = false; hpi.Alive = true;  }
}
