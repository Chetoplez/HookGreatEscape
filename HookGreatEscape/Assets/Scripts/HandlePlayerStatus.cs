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
    /* Can the character move? */
    private bool can_move = true;
    public bool Can_move { get { return can_move; } }
    
    void Update() { 
    }


    /* Not apply the gravity if we are on ground */
    public bool can_apply_gravity() { return (state==PlayerStatus.JUMPING || state==PlayerStatus.ONAIR);}

}
