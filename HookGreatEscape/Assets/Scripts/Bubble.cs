using UnityEngine;
using System.Collections;
using System;

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

    


	void Start () {
	
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
        Debug.Log("Bubble: POP!");
        throw new NotImplementedException();
    }

    /* Encapsule an enemy */
    public void encapsule() {
        Debug.Log("Bubble: Enemy encapsulated!");
        throw new NotImplementedException();
    }

}
