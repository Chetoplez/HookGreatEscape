using UnityEngine;
using System.Collections;
using System;

public class HandlePlayerInput : MonoBehaviour {

    /* Must link the player or something might not work ! */
    public GameObject player;
    /* Must link the player status */
    private HandlePlayerStatus player_status;
    /* This is the target that helps player to shoot */
    private GameObject target;

    private const float maximum_y_target= 0.2f;
    private const float minimum_y_target= -0.2f;

    /* If holding a bubble this is true */
    private bool bubble_holding=false;
    private bool can_create_bubble = true;

    /* Personal Menu of the player */
    private GameObject pause_menu = null;
    public GameObject Pause_menu { set { pause_menu = value; } }
    private bool pause = false;
    public bool Pause { set { pause = value; } }

	void Update () {
        if (player != null)
        {

            if (pause_menu != null)
            { 
               PauseMenu pm= pause_menu.GetComponent<PauseMenu>();
               if (pause_input())
               {
                   if (!pause)
                   {
                       pause = true;
                       pm.pause();
                   }
                   else
                   {
                       pause = false;
                       pm.resume();
                   }
               }
            }

        if (pause) return;
        Player p = player.GetComponent<Player>();
        
        if (left_input())
        {
            p.Is_Facing_right = false;
            move_player(true);
        }
        if (right_input())
        {
            p.Is_Facing_right = true;
            move_player(false);  
        }
        if (jump_input())
            jump();
        if (Input.GetAxis("Mouse Y")>0)
            move_target(true);
        
        if (Input.GetAxis("Mouse Y") < 0)
            move_target(false);
        
       
            if (Input.GetMouseButton(0))
            {
                if (can_create_bubble)
                    create_bubble();
                else
                    if(bubble_holding)
                        grow_bubble();
            }

            if (Input.GetMouseButtonUp(0))
            {
                
                if (bubble_holding)
                      shoot_bubble();
            }
        }

	}



    #region Methods


    bool left_input() { return (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A));}
    bool right_input() { return (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)); }
    bool jump_input() { return (Input.GetKeyDown(KeyCode.Space)); }
    bool pause_input() { return (Input.GetKeyDown(KeyCode.Escape)); }

    private void move_player(bool left=false) { 
        if (!is_valid()) return;
        Player p = player.GetComponent<Player>();
        p.Forward = (left)? false:true;
        p.move(true,left);
    }
    private void jump() { if (!is_valid()) return; Player p = player.GetComponent<Player>(); p.jump(); }
    

    /* Move the target */
    private void move_target(bool up=true) {
        if (!is_valid()) return;
        if (target == null)
            target = GameObject.Find("Target") ?? null;
        if (target == null) return;
        if (up)
        {
            if (target.transform.position.y - player.transform.position.y < maximum_y_target)
            {
                target.transform.Translate(new Vector3(0, 1, 0)*Time.deltaTime, Space.World);
            }
        }
        else
        {
            if (target.transform.position.y - player.transform.position.y > minimum_y_target)
            {
                target.transform.Translate(new Vector3(0,- 1, 0)*Time.deltaTime, Space.World);
            }
        
        }


    }

    /* Create bubble */
    private void create_bubble() {
        check_player_presence();
        if (bubble_holding) return;
        bubble_holding = true;
        can_create_bubble = false;
        Player p = player.GetComponent<Player>();
        p.create_bubble();
    }

    /* Grow bubble */
    private void grow_bubble() {
        check_player_presence();
        if (!bubble_holding) return;
        Player p = player.GetComponent<Player>();
        p.grow_bubble();
    }

    /* Shoot the bubble */
    private void shoot_bubble() {
        check_player_presence();
        can_create_bubble=true;
        bubble_holding = false;
        Player p = player.GetComponent<Player>();
        p.shoot();
    }

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
