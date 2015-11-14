using UnityEngine;
using System.Collections;

public class Fish : MonoBehaviour {
   
    /* Life fish in seconds */
    [Range(1,60)]
    public float life = 10;
    [Range(1, 5)]
    public int speed = 2;

    private Vector3 velocity=new Vector3(1.0f,0f,0f);
    
  
	void Update () {
        this.transform.Translate(velocity*speed*Time.deltaTime);
        life -= Time.deltaTime;
        if (life <= 0)
            GameObject.Destroy(this.gameObject);
	}
}
