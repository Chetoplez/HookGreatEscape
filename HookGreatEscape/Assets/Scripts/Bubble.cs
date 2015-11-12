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

   
    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag != "Pirate") return;
        
    }

    /* Check if encapsuled an enemy */
    bool is_encapsuled(GameObject other) {
        
        return false;
    }

}
