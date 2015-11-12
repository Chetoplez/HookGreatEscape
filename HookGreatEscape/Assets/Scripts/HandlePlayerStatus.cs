using UnityEngine;
using System.Collections;

public class HandlePlayerStatus : MonoBehaviour {

    public enum PlayerStatus { 
        IDLE,
        ONGROUND,
        ONAIR,
        DIED,
        SHOOTING,
        JUMPING
    }
    public PlayerStatus state;

    /* Delta time after shooting a new hit */
    [Range(0,10)]
    public int shooting_pause = 1;
    /* Same for jump */
    [Range(0, 10)]
    public int jumping_pause = 1;
    /* TimeStamps */
    private System.DateTime last_jumping_time;
    private System.DateTime last_shooting_time;
    public System.DateTime JumpingTime { set { last_jumping_time = value; last_jumping_time.AddSeconds(jumping_pause); can_jump = false; } }
    /* Can the character move? */
    private bool can_move = true;
    public bool Can_move { get { return can_move; } }
    /* Can the character jump? */
    private bool can_jump = true;
    public bool Can_jump { get { return can_jump; } }
    /* Can the character shoot? */
    private bool can_shoot = true;
    public bool Can_Shoot { get { return can_shoot; } set { can_shoot = value; } }

    void Update() {

        if (!can_jump)
        {
            can_move = false;
            if (System.DateTime.Now > last_jumping_time)
            {
                can_jump = true;
                can_move = true;
            }
        }
    }


    /* Not apply the gravity if we are on ground */
    public bool can_apply_gravity() { return (state==PlayerStatus.JUMPING || state==PlayerStatus.ONAIR);}



}
