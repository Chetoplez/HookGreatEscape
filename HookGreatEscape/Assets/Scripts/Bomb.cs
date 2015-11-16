using UnityEngine;
using System.Collections;

public class Bomb : MonoBehaviour {

    #region Variable
    private float timeToExplode, second, timePassed,asp;
    private int velocity;
    private bool blocked;
    public Vector2 direction; //Destra o sinistra
    public GameObject sprite;
    public GameObject bomb;
    public Vector2 piratePosition;
    private bool die;
    #endregion

    // Use this for initialization
    void Start () {
        timeToExplode = timePassed =  Random.Range(3, 10);
        second = asp = 1;
        changeSprite(timePassed);
        throwBomb();
        transform.position = piratePosition;
        die = false;
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
        this.GetComponent<Rigidbody2D>().AddForce(new Vector2(direction.x*0.5f, direction.y), ForceMode2D.Impulse);
    }

    public void changeSprite(float time) {
        SpriteRenderer sprite_render = sprite.GetComponent<SpriteRenderer>();
        if (sprite_render == null) Debug.LogError("Sprite render is null");
        Sprite newSprite = Resources.Load<Sprite>(time.ToString());
        if (newSprite == null)
            Debug.LogError("Error on loading the sprite.");
        else
            sprite_render.sprite = newSprite;

        if(time == 0)
        {
            bomb.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("11");
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Hook" || other.gameObject.tag == "Pirate") {
            die = true;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (die)
        {
            if (other.gameObject.tag == "Hook")
            {
                Player p = other.gameObject.GetComponent<Player>() ?? null;
                if (p != null) p.hit();
                die = false;
            }
            else if(other.gameObject.tag == "Pirate"){
                AI p = other.gameObject.GetComponent<AI>() ?? null;
                if (p != null) p.Blocked = true;
                die = false;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Hook" || other.gameObject.tag == "Pirate")
        {
            die = false;
        }
    }

}
