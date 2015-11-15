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

    public GameObject live_1;
    public GameObject live_2;
    public GameObject live_3;
    public GameObject live_4;
    public GameObject live_5;


    void Start() {
        if (live_text == null)
            Debug.LogError("HUD: not linked the live_number_text object");
        get_initial_life();
    }

    void Update() {
        if (!is_valid()) return;
        if(gc!=null)
         this.lives = gc.Lives;

        if (live_1 != null && live_2 != null && live_3 != null && live_4 != null && live_5 != null)
        {
            live_1.SetActive(lives>=1);
            live_2.SetActive(lives>=2);
            live_3.SetActive(lives>=3);
            live_4.SetActive(lives>=4);
            live_5.SetActive(lives>=5);
        }
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
