using UnityEngine;
using System.Collections;

public class AreaTrap : MonoBehaviour {

    public GameObject trap;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Hook")
        {
            trap.SetActive(true);
        }
    }

}
