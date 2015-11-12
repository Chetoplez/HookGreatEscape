using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(CircleCollider2D))]
public class Bubble : MonoBehaviour {

    /* Velocity vector */
    private Vector3 velocity=new Vector3(1f,0f,0f);
    public Vector3 Velocity{
        get { return velocity; }
        set{ velocity = value; }
        }

    /* Speed of the bubble */
    [Range(0,5)]
    public float speed = 1f;
    
    /* Holded or shooted? */
    private bool shooted = false;
    public bool Shooted { get { return shooted; } set { shooted = value; } }




	/* Life of the bubble in seconds */
    [Range(1,10)]
    public float bubble_life = 10;
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

    void Start() { 
        circle=GetComponent<CircleCollider2D>();
        this.shooting_position = this.transform.position;
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
            this.transform.position = shooting_position;
        
	}

    /* Became bigger....bigger...bigger... */
    public void grow(){
        if (shooted) return;
        this.transform.localScale += new Vector3(1f * growing_factor, 1f*growing_factor,0f)*Time.deltaTime;
        this.circle.radius += (growing_factor/2)*Time.deltaTime;
        Debug.Log("Radius change...=" + this.circle.radius);
        Debug.Log("Bounds size change..." + circle.bounds.size/2);
        if (this.transform.localScale.x >= max_growing_factor)
            die();
    }

    /* ....and POP! */
    public void die() {
        DestroyObject(this.gameObject);
    }

    /* Encapsule an enemy */
    public void encapsule() {
        Debug.Log("Bubble: Enemy encapsulated!");
        throw new NotImplementedException();
    }

   
    void OnTriggerStay2D(Collider2D other) {
        if (other.gameObject.tag != "Pirate") return;
        if (is_encapsuled(other.gameObject))
        {
            /* TODO:: assign new value to bubble life, block the enemy and make him a child of the bubble */
            Debug.Log("Inside the circle!!");
            Debug.Break();
        }
    }

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
        Debug.DrawLine(top_left,top_right,Color.red);
        Debug.DrawLine(bottom_left,top_left,Color.red);
        Debug.DrawLine(top_right,bottom_right,Color.red);
        Debug.DrawLine(bottom_left,bottom_right,Color.red); 
        */

        if((Vector3.Distance(top_left,center_circle)<=radius_circle) )
        Debug.Log("Top left inside" );
        if ((Math.Pow((top_right.x - center_circle.x), 2) + Math.Pow((top_right.y - center_circle.y), 2) < Math.Pow(radius_circle, 2)))
        Debug.Log("Top right inside");
        if ((Math.Pow((bottom_right.x - center_circle.x), 2) + Math.Pow((bottom_right.y - center_circle.y), 2) < Math.Pow(radius_circle, 2)))
        Debug.Log("Bottom right inside " );
        if ((Math.Pow((bottom_left.x - center_circle.x), 2) + Math.Pow((bottom_left.y - center_circle.y), 2) < Math.Pow(radius_circle, 2)))
        Debug.Log("Bottom left inside " );

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
