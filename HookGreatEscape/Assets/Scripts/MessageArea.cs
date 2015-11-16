using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MessageArea : MonoBehaviour {


    private bool messageShow,read;
    public string textArea;
    private GameObject gameController;
    public GameObject HUD;

    // Use this for initialization
    void Start () {
        gameController = GameObject.Find("GameController");
        messageShow = read = false;

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
