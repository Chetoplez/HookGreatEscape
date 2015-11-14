using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MessageArea : MonoBehaviour {


    private bool messageShow;
    public string textArea;
    public GameObject gameController;
    public GameObject HUD;

    // Use this for initialization
    void Start () {
        messageShow = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (messageShow) {
            HUD.GetComponentInChildren<Hud>().showMessage(textArea);
            messageShow = false;
        }
	}

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Hook") {
            messageShow = true;
            Time.timeScale = 0;
            HandlePlayerInput hpi = gameController.GetComponent<HandlePlayerInput>() ?? null;
            if (hpi != null) {
                hpi.Pause = true;
            }

        }  
    }
}
