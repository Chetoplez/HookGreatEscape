using UnityEngine;
using System.Collections;

public class WinArea : MonoBehaviour {

    private bool win, canWin;

    void Start() {
        win = canWin =  false;
    }

    void Update()
    {
        if (canWin && Input.GetMouseButtonDown(0))
        {
            win = true;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Hook")
        {
            canWin= true;
        }
    }

    void OnTriggerStay2D(Collider2D other) {
        if (other.gameObject.tag == "Hook" && win) {
            GameController.change_level();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Hook")
        {
            canWin = false;
        }
    }
}
