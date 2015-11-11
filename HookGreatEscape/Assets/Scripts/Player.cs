using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(Collider2D)),RequireComponent(typeof(Rigidbody2D)),RequireComponent(typeof(HandlePlayerStatus)),RequireComponent(typeof(SpriteRenderer))]
public class Player : MonoBehaviour
{

    #region Variables

    /* Life of the player */
    private int lives = 5;

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
    private float max_velocity_y = 5f;


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

    /* This is the target that help with the shoot direction */
    public GameObject target = null;
    private HandlePlayerStatus player_status = null;

    #endregion

    #region StateMachine

    void Start () 
    {
	    player_status= GetComponent<HandlePlayerStatus>();
        collider= GetComponent<Collider2D>();

        /* To fix... depends on the collider that we have */
        BoxCollider2D box_collider = collider as BoxCollider2D;
        this.box_collider = box_collider;
        var collider_width = box_collider.size.x * Mathf.Abs(transform.localScale.x) - (2 * skin_width);
        _horizontal_distance_between_rays = collider_width / (total_vertical_rays - 1);

        var collider_heigth = box_collider.size.y * Mathf.Abs(transform.localScale.y) - (2 * skin_width);
        _vertical_distance_between_rays = collider_heigth / (total_horizontal_rays - 1);
        
	}
	
	
	void Update () 
    {
        if (player_status.can_apply_gravity())
        {
            apply_gravity();
            Debug.Log("Can apply gravity...");
        }
        calculate_ray_origins();
        check_if_onground();
        if (player_status.state == HandlePlayerStatus.PlayerStatus.JUMPING || player_status.state==HandlePlayerStatus.PlayerStatus.ONAIR)
        {
            move_vertically();
        }
        Debug.Log("Mystate=" + player_status.state);
    }

    #endregion

    #region Methods

    /* Apply gravity to our character */
    /* TODO:: gravity is too strong!!! */
    void apply_gravity() {
        if (this.velocity.y < (-max_velocity_y))
            this.velocity.y = -max_velocity_y;
        else
            this.velocity.y -=  (gravity_factor);
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


    /* Check if landed */
    public void check_if_onground() {
        var is_going_up = velocity.y > 0;
        var ray_distance = Mathf.Abs(velocity.y) + skin_width;
        var ray_direction = is_going_up ? Vector2.up : -Vector2.up;
        var ray_origin = is_going_up ? _raycast_top_left : _raycast_bottom_left;

        //ray_origin.x += velocity.x;
        ray_distance = skin_width * 2;
        bool collision_detected = false;
        for (int i = 0; i < total_vertical_rays; i++)
        {
            var ray_vector = new Vector2(ray_origin.x + (i * _horizontal_distance_between_rays), ray_origin.y);
            Debug.DrawRay(ray_vector, ray_direction * ray_distance, Color.green);

            var raycasthit = Physics2D.Raycast(ray_vector, ray_direction, ray_distance, platform_mask);
            if (!raycasthit) continue;
            if (raycasthit.collider.gameObject.name == "Player") continue;

            collision_detected = true;

            
            player_status.state = (is_going_up) ? HandlePlayerStatus.PlayerStatus.ONAIR : HandlePlayerStatus.PlayerStatus.ONGROUND;
           

            if (player_status.state == HandlePlayerStatus.PlayerStatus.ONGROUND)
                velocity.y = 0;

            /* Error case */
            if (ray_distance < (skin_width + 0.0001f)) break;

        }

        if (!collision_detected)
        {
            player_status.state = HandlePlayerStatus.PlayerStatus.ONAIR;
            Debug.Log("Nothing touched");
        }
    }

    /* Calculate the origin of the raycast */
    private void calculate_ray_origins()
    {
        var size = new Vector2(box_collider.size.x * Mathf.Abs(transform.localScale.x), box_collider.size.y * Mathf.Abs(transform.localScale.y)) / 2;
        var center = new Vector2(box_collider.offset.x * transform.localScale.x, box_collider.offset.y * transform.localScale.y);

        _raycast_top_left = transform.position + new Vector3(center.x - size.x + skin_width, center.y - size.y - skin_width);
        _raycast_bottom_right = transform.position + new Vector3(center.x + size.x - skin_width, center.y - size.y + skin_width);
        _raycast_bottom_left = transform.position + new Vector3(center.x - size.x + skin_width, center.y - size.y + skin_width);
        _raycast_top_right = transform.position + new Vector3(center.x + size.x - skin_width, center.y - size.y - skin_width);
    }

    /* Shoot a bubble! */
    public void shoot() {
        throw new NotImplementedException();
    }

    /* Hitted by a pirate or by a bomb */
    public void hit(int damage=1) {
        this.lives--;
        if (lives == 0)
            die();
        GameController gc = GameObject.Find("GameController").GetComponent<GameController>() ?? null;
        if(gc!=null)
            gc.Lives = this.lives;
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
