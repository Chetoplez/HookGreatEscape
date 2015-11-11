using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(Collider2D)),RequireComponent(typeof(Rigidbody2D)),RequireComponent(typeof(HandlePlayerStatus)),RequireComponent(typeof(SpriteRenderer))]
public class Player : MonoBehaviour
{

    #region Variables

    /* Player Speed */
    [Range(0,10)]
    public int speed_factor=1;
    
    /* Are we going forward or backward?*/
    private bool forward=true;
    public bool Forward { set { forward = value; } }
    
    /* Jump speed */
    [Range(0,10)]
    public int jump_speed = 1;

    /* Velocity direction */
    private Vector3 velocity=Vector3.zero;
    public Vector3 Velocity { get { return velocity; } set { velocity = value; } }

    /* this is the gravity */
    [Range(0,10)]
    public int gravity_factor=1;


    /* This is the target that help with the shoot direction */
    public GameObject target = null;
    private HandlePlayerStatus player_status = null;

    #endregion

    #region StateMachine

    void Start () 
    {
	    player_status= GetComponent<HandlePlayerStatus>();
	}
	
	
	void Update () 
    {
        if (player_status.can_apply_gravity())
            apply_gravity();
        if (player_status.state == HandlePlayerStatus.PlayerStatus.JUMPING)
        {
            move_vertically();
        }
        
    }

    #endregion

    #region Methods

    /* Apply gravity to our character */
    void apply_gravity() {
        this.velocity.y -= gravity_factor;
    }

    /* Add force to our character */
    public void add_force(Vector2 force)
    {
       this.velocity += new Vector3(force.x,force.y,0);
    }


    /* Move by a fixed quantity */
    public void move(bool horizontal=true){
        if (horizontal)
        {
            velocity.x = 1;
            velocity.x = (this.forward) ? 1 : -1;
        }
        this.transform.Translate(velocity * speed_factor * Time.deltaTime);
    }

    /* Move vertically */
    public void move_vertically() {
        move(false);
    }

    /* Jump! */
    public void jump() {
        velocity.y = 1;
        Vector2 my_jump = new Vector2(0,jump_speed*Time.deltaTime);
        add_force(my_jump);
        player_status.JumpingTime = System.DateTime.Now ;
        player_status.state = HandlePlayerStatus.PlayerStatus.JUMPING;
        move(false);
    }

    /* Shoot a bubble! */
    public void shoot() {
        throw new NotImplementedException();
    }

    /* So long */
    public void die() {
        throw new NotImplementedException();
    }

    /* Move the target */
    public void move_target() {
        throw new NotImplementedException();
    }

    #endregion

}
