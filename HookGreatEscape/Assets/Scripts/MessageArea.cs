using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MessageArea : MonoBehaviour {


    private bool messageShow;
    public Text t;
    public GameObject button;
    public string textArea;
    public GameObject gameController;

    // Use this for initialization
    void Start () {
        messageShow = false;
        t.enabled = false;
        button.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        if (messageShow) {
            showMessage();
        }
	}

    public void showMessage()
    {
        t.enabled = true;
        t.text = textArea;
        button.SetActive(true);
    }

    public void disableMessage() {
        Time.timeScale = 1;
        t.enabled = messageShow = false;
        button.SetActive(false);
        HandlePlayerInput hpi = gameController.GetComponent<HandlePlayerInput>() ?? null;
        if (hpi != null)
        {
            hpi.Pause =false;
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
