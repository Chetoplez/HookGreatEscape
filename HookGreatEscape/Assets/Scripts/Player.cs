using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(AudioSource)),RequireComponent(typeof(Animator)),RequireComponent(typeof(Collider2D)),RequireComponent(typeof(Rigidbody2D)),RequireComponent(typeof(HandlePlayerStatus)),RequireComponent(typeof(SpriteRenderer))]
public class Player : MonoBehaviour
{

    #region Variables

    /* Life of the player */
    private int lives = 5;
    [Range(2,5)]
    public int max_lives = 5;
    private bool alive = true;
    public bool Alive { get { return alive; } }

    /* Player Speed */
    [Range(0,10)]
    public int speed_factor=1;
    
    /* Are we going forward or backward?*/
    private bool forward=true;
    public bool Forward { set { forward = value; } }
    
    /* Jump speed */
    [Range(10,100)]
    public int jump_speed = 1;

    /* Velocity direction */
    private Vector3 velocity=Vector3.zero;
    public Vector3 Velocity { get { return velocity; } set { velocity = value; } }
    private float max_velocity_y = 5f;
    private bool is_facing_right = true;
    public bool Is_Facing_right { set { is_facing_right = value; } }
    private bool is_grounded = false;
    public bool Is_Grounded { get { return is_grounded; } }

    /* this is the gravity */
    [Range(0,10)]
    public int gravity_factor=1;

    /* Param used for threshold with the collider size for creating raycast */
    private const float skin_width = 0.1f;
    /* Must cast a ray in order to detect collisions. */
    private const int total_horizontal_rays = 8;
    private const int total_vertical_rays = 4;
    private Collider2D collider;
    private BoxCollider2D box_collider;
    private Vector3 _raycast_top_left;
    private Vector3 _raycast_top_right;
    private Vector3 _raycast_bottom_left;
    private Vector3 _raycast_bottom_right;
    private float _vertical_distance_between_rays;
    private float _horizontal_distance_between_rays;
    public LayerMask platform_mask;
    private bool shooting = false;

    /* This is the target that help with the shoot direction */
    public GameObject target = null;
    private HandlePlayerStatus player_status = null;
    /* Starting point of a bubble */
    public GameObject pistol = null;
    /* This is the bubble that will be created  */
    private GameObject bubble = null;
    private Animator animator = null;
    public AudioSource gun = null;
    
    #endregion

    #region StateMachine

    void Start () 
    {
        
	    player_status= GetComponent<HandlePlayerStatus>();
        collider= GetComponent<Collider2D>();
        if (target == null || pistol == null)
            Debug.Log("Target or Pistol null!!");


        /* To fix... depends on the collider that we have */
        BoxCollider2D box_collider = collider as BoxCollider2D;
        this.box_collider = box_collider;
        var collider_width = box_collider.size.x * Mathf.Abs(transform.localScale.x) - (2 * skin_width);
        _horizontal_distance_between_rays = collider_width / (total_vertical_rays - 1);

        var collider_heigth = box_collider.size.y * Mathf.Abs(transform.localScale.y) - (2 * skin_width);
        _vertical_distance_between_rays = collider_heigth / (total_horizontal_rays - 1);

        animator=GetComponent<Animator>();
        gun=GetComponent<AudioSource>();

        GameObject game_controller = GameObject.Find("GameController") ?? null;
        if (game_controller != null)
        { 
            HandlePlayerInput hpi= game_controller.GetComponent<HandlePlayerInput>();
            hpi.player = this.gameObject;
        }

	}

    /* Used for know if we can jump again */
    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.transform.gameObject.tag == "Floor")
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (contact.normal.y > 0.1f)
                {
                    is_grounded = true;
                    break;
                }
            }
        }
    }

    void OnCollisionStay2D(Collision2D collision) {
        if (collision.transform.gameObject.tag == "Floor")
            is_grounded = true;
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.gameObject.tag == "Floor")
            is_grounded = false;
    }




    #endregion

    #region Methods

    public void stop_shooting_anim() {
        this.animator.SetBool("Shooting",false);
    }


    /* Move by a fixed quantity */
    public void move(bool horizontal=true,bool left=false){
        Rigidbody2D rigidbody = this.GetComponent<Rigidbody2D>();
        is_facing_right = this.transform.localScale.x > 0;
        if (horizontal)
        {
            if ((is_facing_right && left) || (!left && !is_facing_right))
                flip();
            rigidbody.AddForce(new Vector2((this.forward) ? this.speed_factor : -speed_factor, 0f));
        }
        else
            rigidbody.AddForce(new Vector2(0f, jump_speed * 10));
    }

    private void flip() {
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    /* Move vertically */
    public void move_vertically() {
        move(false);
    }

    /* Jump! */
    public void jump() {
        move_vertically();
    }

    /*  Create a bubble */
    public void create_bubble() {
        this.bubble=Instantiate(Resources.Load("Bubble"), this.pistol.transform.position, this.transform.rotation) as GameObject;
    }
    
    /* Grow the bubble */
    public void grow_bubble() {
        if (bubble == null) return;
        Bubble bb= bubble.GetComponent<Bubble>();
        bb.grow();
        bb.Shooting_position = this.pistol.transform.position;
    }

    /* Shoot a bubble! */
    public void shoot() {
        if (bubble == null) return;
        animator.SetBool("Shooting", true);
        Bubble bb = bubble.GetComponent<Bubble>();
        bb.Velocity = target.transform.position - bubble.transform.position;
        bb.Shooted = true;
        AudioHandler.Instance.PlayAudio(gun);
    }

    


    /* Hitted by a pirate or by a bomb */
    public void hit(int damage=1) {
        this.lives--;
        alive = (lives > 0);
        Debug.Log("Lives=" + this.lives);
        if (lives <= 0)
            die();
        GameController gc = GameObject.Find("GameController").GetComponent<GameController>() ?? null;
        if(gc!=null)
            gc.Lives = this.lives;
    }

    /* Gain life */
    public void gain_life() {
        this.lives++;
        if (this.lives > max_lives)
            this.lives = max_lives;
        GameController gc = GameObject.Find("GameController").GetComponent<GameController>() ?? null;
        if (gc != null)
            gc.Lives = this.lives;
    }

    /* So long */
    public void die() {
        HandlePlayerInput hpi = GameObject.Find("GameController").GetComponent<HandlePlayerInput>() ?? null;
        hpi.Alive = false;
    }

    
    #endregion

}
