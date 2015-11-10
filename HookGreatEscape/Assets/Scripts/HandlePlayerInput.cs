using UnityEngine;
using System.Collections;

public class HandlePlayerInput : MonoBehaviour {

    /* Must link the player or not work something! */
    public GameObject player;

	void Start () {
	
	}
	
	
	void Update () {
	
	}



    #region Methods

    private void move_player() { }
    private void jump() { }
    private void pause() { }

    #endregion


    /* If the player is not null, we cannot afford any action */
    bool is_valid() { 
        return player!=null;
    }

}
