using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Hud : MonoBehaviour {
    /* Life of the player */
    private int lives = 0;
    public int Lives { get { return lives; } set { lives = value; } }

    public UnityEngine.UI.Text live_text;
    private GameController gc = null;

    public GameObject message;
    public GameObject message_button_exit;
    public GameObject img_jimmy;
    private bool message_show = false;
    public bool Message_show { get { return message_show; } set { message_show = value; } }

    void Start() {
        if (live_text == null)
            Debug.LogError("HUD: not linked the live_number_text object");
        get_initial_life();
    }

    void Update() {
        if (!is_valid()) return;
        if(gc!=null)
         this.lives = gc.Lives;
        live_text.text = lives.ToString();
        if (message != null)
            message.SetActive(message_show);
        if (message_button_exit)
            message_button_exit.SetActive(message_show);
        if (img_jimmy)
            img_jimmy.SetActive(message_show);
    }


    void get_initial_life() {
        gc = GameObject.Find("GameController").GetComponent<GameController>() ?? null;
        if (gc != null)
        {
            this.lives = gc.Lives;
        }
    }

    bool is_valid() { return live_text != null; }

    public void showMessage(string text)
    {
        message.GetComponent<Text>().text = text;
        message_show = true;
    }

    public void disableMessage()
    {
        message_show = false;
        Time.timeScale = 1;
        HandlePlayerInput hpi =gc.GetComponent<HandlePlayerInput>() ?? null;
        if (hpi != null)
        {
            hpi.Pause = false;
        }
    }
}
