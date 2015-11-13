using UnityEngine;
using System.Collections;

public class LifeArea : MonoBehaviour {

    private bool takeLife,canTake;

    void Start() {
        takeLife = canTake = false;
    }

    void Update()
    {
        if (canTake && Input.GetMouseButtonDown(0)) {
            takeLife = true; 
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Hook") {
            canTake = true;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Hook" && takeLife) {
            Player p = other.gameObject.GetComponent<Player>() ?? null;
            if (p != null) {
                p.gain_life();
            }
            Destroy(this.gameObject);
        }

    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Hook")
        {
            canTake = false;
        }
    }

}
