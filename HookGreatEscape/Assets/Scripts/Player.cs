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


    #endregion

    #region StateMachine

    void Start () 
    {
	
	}
	
	
	void Update () 
    {

    }

    #endregion

    #region Methods

    /* Move by a fixed quantity */
    public void move(){
        velocity.x = 1;
        velocity.x = (this.forward) ? 1 : -1;
        this.transform.Translate(velocity*speed_factor*Time.deltaTime);
    }

    /* Jump! */
    public void jump() {
        throw new NotImplementedException();
    }

    /* Shoot a bubble! */
    public void shoot() {
        throw new NotImplementedException();
    }

    /* So long */
    public void die() { 
    
    }

    /* Move the target */
    public void move_target() { 
    
    }

    #endregion

}
