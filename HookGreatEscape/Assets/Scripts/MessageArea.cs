using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MessageArea : MonoBehaviour {


    private bool messageShow,read;
    public string textArea;
    private GameObject gameController;
    private GameObject HUD;

    // Use this for initialization
    void Start () {
        gameController = GameObject.Find("GameController");
        HUD = GameObject.Find("HUD");
        messageShow = read = false;
        if (gameController == null) Debug.LogError("Game Controller is null");
        if (HUD == null) Debug.LogError("HUD is null");
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
