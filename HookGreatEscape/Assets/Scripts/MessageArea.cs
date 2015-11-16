using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MessageArea : MonoBehaviour {


    private bool messageShow,read;
    private string textArea;
    [Range(1, 5)]
    public int text;
    public GameObject gameController;
    public GameObject HUD;

    public void chooseText() {
        switch (text) {
            case 1: textArea = "Aiuta Hook a scappare! ASDW per muoverti, barra Spaziatrice per saltare!";
                break;
            case 2: textArea = "Le Bottiglie di rum ti aumentano la vita! Clicca per raccoglierla!";
                break;
            case 3: textArea = "Per sconfiggere un pirata racchiudilo dentro la bolla! Tasto LMB per sparare,più tieni premuto più la bolla cresce! Attento che non esploda!";
                    break;
            case 4:textArea = "Puoi mirare facendo su e giù con il mouse!";
                break;
            case 5: textArea = "";
                break;

}
    }

    // Use this for initialization
    void Start () {
        messageShow = read = false;
        chooseText();
	}
	
	// Update is called once per frame
	void Update () {
        if (messageShow && !read) {
            HUD.GetComponentInChildren<Hud>().showMessage(textArea);
            messageShow = false;
            read = true;
        }
	}

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Hook" && !read) {
            messageShow = true;
            Time.timeScale = 0;
            HandlePlayerInput hpi = gameController.GetComponent<HandlePlayerInput>() ?? null;
            if (hpi != null) {
                hpi.Pause = true;
            }

        }  
    }
}
