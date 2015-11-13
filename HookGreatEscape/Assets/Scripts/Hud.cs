using UnityEngine;
using System.Collections;

public class Hud : MonoBehaviour {
    /* Life of the player */
    private int lives = 0;
    public int Lives { get { return lives; } set { lives = value; } }

    public UnityEngine.UI.Text live_text;

    void Start() {
        if (live_text == null)
            Debug.LogError("HUD: not linked the live_number_text object");
    }

    void Update() {
        if (!is_valid()) return;
        live_text.text = lives.ToString();
    }

    
    bool is_valid() { return live_text != null; }
}
