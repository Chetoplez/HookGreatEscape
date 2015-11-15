using UnityEngine;
using System.Collections;

public class Bomb : MonoBehaviour {

    #region Variable
    private float timeToExplode, second, timePassed,asp;
    private int velocity;
    private bool blocked;
    public float angle;
    public Vector2 direction; //Destra o sinistra
    public GameObject sprite;
    public Vector2 piratePosition;
    #endregion

    // Use this for initialization
    void Start () {
        angle = Random.Range(45, 135);
        timeToExplode = timePassed =  Random.Range(3, 10);
        second = asp = 1;
        changeSprite(timePassed);
        throwBomb();
        transform.position = piratePosition;
    }
	
	// Update is called once per frame
	void Update () {
        second -= Time.deltaTime;
        if (second <= 0 && timePassed >=1)
        {
            timePassed -= 1;
            changeSprite(timePassed);
            second = 1;
        }
        timeToExplode -= Time.deltaTime;
        if (timeToExplode <= 0) {
            Explode();
        }

	}

    public void Explode(){
        asp -= Time.deltaTime;
        if (asp <= 0)
        Destroy(this.gameObject);
        else changeSprite(0);
    }



    public void throwBomb() {
       //Vector2 rot = new Vector2(direction.x * angle - direction.y * angle, direction.y * angle + direction.x * angle);
       //this.GetComponent<Rigidbody2D>().AddForce(rot, ForceMode2D.Force);
        this.GetComponent<Rigidbody2D>().AddForce(direction, ForceMode2D.Impulse);
        Debug.Log("Direction is:" + direction);
        Debug.Log("transform bomb is: " + transform.position);
        
       // Debug.Break();
    }

    public void changeSprite(float time) {
        SpriteRenderer sprite_render = sprite.GetComponent<SpriteRenderer>();
        if (sprite_render == null) Debug.LogError("Sprite render is null");
        Sprite newSprite = Resources.Load<Sprite>(time.ToString());
        if (newSprite == null)
            Debug.LogError("Error on loading the sprite.");
        else
            sprite_render.sprite = newSprite;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (timeToExplode <= 0)
        {
            if (other.gameObject.tag == "Hook")
            {
                Player p = other.gameObject.GetComponent<Player>() ?? null;
                if (p != null) p.hit();
            }
            else if(other.gameObject.tag == "Pirate"){
                AI p = other.gameObject.GetComponent<AI>() ?? null;
                if (p != null) p.Blocked = true;
            }
        }
    }

}
