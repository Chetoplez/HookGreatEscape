using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(CircleCollider2D)),RequireComponent(typeof(AudioSource))]
public class Bubble : MonoBehaviour {

    /* Velocity vector */
    private Vector3 velocity=new Vector3(1f,0f,0f);
    public Vector3 Velocity{
        get { return velocity; }
        set{ velocity = value; }
        }

    /* UpSpeed */
    [Range(1,10)]
    public float up_speed = 1f;
    

    /* Speed of the bubble */
    [Range(0,5)]
    public float speed = 1f;
    
    /* Holded or shooted? */
    private bool shooted = false;
    public bool Shooted { get { return shooted; } set { shooted = value; } }
   
	/* Life of the bubble in seconds */
    [Range(1,10)]
    public float bubble_life = 10;
    private float old_life = 10;
    private bool alive = true;

    /* Growing factor of the bubble */
    [Range(1,5)]
    public int growing_factor = 1;

    /* Maximum size of the bubble */
    [Range(2,10)]
    public int max_growing_factor = 10;

    private CircleCollider2D circle;
    /* Where the bubble will be shooted */
    private Vector3 shooting_position= Vector3.zero;
    public Vector3 Shooting_position { get { return shooting_position; } set { shooting_position = value; } }

    private bool encapsuled = false;
    private AI encapsuled_ai = null;
    private float gravityscale_ai = 0f;
    private GameObject bomb = null;
    private bool bomb_encapsuled = false;


    public AudioSource popsound = null;
    private bool can_die=false;

    void Start() { 
        circle=GetComponent<CircleCollider2D>();
        this.shooting_position = this.transform.position;
        old_life = bubble_life;
        popsound= GetComponent<AudioSource>();
    }
	
	void Update () {

        if (shooted)
        {
            bubble_life -= Time.deltaTime;
            if (bubble_life <= 0 && alive)
            {
                alive = false;
                die();
            }

            this.transform.Translate(velocity * speed * Time.deltaTime);
        }
        else
        {
            Vector3 desired_position = Vector3.Lerp(this.transform.position,shooting_position,0.5f);
            this.transform.position = desired_position;
        }
	}

    /* Became bigger....bigger...bigger... */
    public void grow(){
        if (shooted) return;
        this.transform.localScale += new Vector3(1f * growing_factor, 1f*growing_factor,0f)*Time.deltaTime;
        this.circle.radius += (growing_factor/2)*Time.deltaTime;
        if (this.transform.localScale.x >= max_growing_factor)
            die();
    }

    /* ....and POP! */
    public void die() {
        
        if (encapsuled)
        {
            if (bomb_encapsuled)
            {
                if (bomb != null)
                {
                    bomb.transform.parent = null;
                    change_scale_factor(bomb, false);
                }
             }
            else
            {
                encapsuled_ai.gameObject.transform.parent = null;
                change_scale_factor(encapsuled_ai.gameObject,false);
            }
        }
        AudioHandler.Instance.PlayAudio(popsound);
        DestroyObject(this.gameObject);
    }

  

   
    void OnTriggerStay2D(Collider2D other) {
        if (other.gameObject.tag != "Pirate" && other.gameObject.tag != "Bomb" && other.gameObject.tag!= "Floor") return;


        if (other.gameObject.tag == "Floor")
         die();

        
        if (other.gameObject.tag == "Bomb")
        {
            encapsuled = true;
            velocity = new Vector3(0f, 1f * up_speed, 0f);
            bubble_life = old_life;
            bomb_encapsuled = true;
            bomb =parent(other.gameObject);
            bomb.transform.position = this.transform.position;
            bomb.transform.parent = this.transform;
            change_scale_factor(bomb, true);
            return;
        }
        


        if (is_encapsuled(other.gameObject) && !encapsuled)
        {
            encapsuled = true;
            velocity = new Vector3(0f, 1f * up_speed, 0f);
            bubble_life = old_life;

            if (other.gameObject.tag == "Pirate")
            {
                encapsuled_ai = parent(other.gameObject).GetComponent<AI>() ?? null;
                if (encapsuled_ai != null)
                {
                    encapsuled_ai.transform.parent = this.transform;
                    encapsuled_ai.Blocked = true;
                    change_scale_factor(encapsuled_ai.gameObject,true);
                }
            }
    
        }
    }

    /* Set the gravity scale factor of the encapsuled gameobject*/
    private void change_scale_factor(GameObject other, bool set=false) {
        Rigidbody2D other_rigidbody = other.GetComponent<Rigidbody2D>() ?? null;
        if (other_rigidbody == null) return;
        if(set)
            gravityscale_ai = other_rigidbody.gravityScale;
        other_rigidbody.gravityScale = (set)? 0f : gravityscale_ai;
    }

    /* Shortcut */
    private static GameObject parent(GameObject other) { return other.transform.parent.gameObject; }

    /* Check if encapsuled an enemy */
    bool is_encapsuled(GameObject other) {
        var radius_circle = (circle.bounds.size.x) / 2;
        Vector3 center_circle = this.gameObject.transform.position;

        BoxCollider2D other_collider = other.GetComponent<BoxCollider2D>() ?? null;
        if (other_collider == null) return false;
        

        var size = new Vector2(other_collider.size.x * Mathf.Abs(other.transform.localScale.x), other_collider.size.y * Mathf.Abs(other.transform.localScale.y)) / 2;
        var center = new Vector2(other_collider.offset.x * other.transform.localScale.x, other_collider.offset.y * other.transform.localScale.y);

        var top_left = other.transform.position + new Vector3(center.x - size.x , center.y + size.y);
        var top_right = other.transform.position + new Vector3(center.x + size.x, center.y + size.y);
        var bottom_left = other.transform.position + new Vector3(center.x - size.x , center.y - size.y);
        var bottom_right = other.transform.position + new Vector3(center.x + size.x, center.y - size.y);

        /*
         * Debug
        Debug.DrawLine(top_left,top_right,Color.red);
        Debug.DrawLine(top_right,bottom_right,Color.red);
        Debug.DrawLine(bottom_right,bottom_left,Color.red);
        Debug.DrawLine(bottom_left,top_left,Color.red);
        */    

        return ( Mathf.Pow((top_left.x - center_circle.x),2) + Mathf.Pow((top_left.y-center_circle.y),2)< Mathf.Pow( radius_circle,2)
                 &&
                 Mathf.Pow((top_right.x - center_circle.x), 2) + Mathf.Pow((top_right.y - center_circle.y), 2) < Mathf.Pow(radius_circle, 2)
                 &&
                 Mathf.Pow((bottom_right.x - center_circle.x), 2) + Mathf.Pow((bottom_right.y - center_circle.y), 2) < Mathf.Pow(radius_circle, 2)
                 &&
                 Mathf.Pow((bottom_left.x - center_circle.x), 2) + Mathf.Pow((bottom_left.y - center_circle.y), 2) < Mathf.Pow(radius_circle, 2)
               );
    }

   

}
