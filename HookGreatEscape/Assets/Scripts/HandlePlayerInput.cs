using UnityEngine;
using System.Collections;
using System;

public class HandlePlayerInput : MonoBehaviour {

    /* Must link the player or something might not work ! */
    public GameObject player;
    /* Must link the player status */
    private HandlePlayerStatus player_status;


	void Start () {
	    
	}
	
	
	void Update () {
        
        if (left_input())
            move_player(true);
        if (right_input())
            move_player(false);
        if (jump_input())
            jump();

	}



    #region Methods


    bool left_input() { return (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A));}
    bool right_input() { return (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)); }
    bool jump_input() { return (Input.GetKeyDown(KeyCode.Space)); }

    private void move_player(bool left=false) { 
        if (!is_valid()) return;
        Player p = player.GetComponent<Player>();
        p.Forward = (left)? false:true;
        p.move();
    }

    private void jump() { if (!is_valid()) return; throw new NotImplementedException(); }
    private void pause() { if (!is_valid()) return; throw new NotImplementedException(); }

    /* Search the player in the tree (not always will be, such as the main menu */
    void check_player_presence() {
        player = GameObject.Find("Player") ?? null;
        if(player!=null)
            player_status = player.GetComponent<HandlePlayerStatus>();
    }
    /* If the player is not null, we cannot afford any action */
    bool is_valid() { check_player_presence(); return (player != null && player_status != null); }
    /* Ask to player status if we can move */
    bool can_move() { return player_status.Can_move; }

    #endregion


    

}
