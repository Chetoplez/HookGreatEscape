using UnityEngine;
using System.Collections;

public class EffectArea : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public bool isPlayer(GameObject p) {
        return p.tag == "Player";
    }
}
