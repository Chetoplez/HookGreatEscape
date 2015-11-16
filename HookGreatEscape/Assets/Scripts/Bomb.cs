using UnityEngine;
using System.Collections;

public class Bomb : MonoBehaviour {

    #region Variable
    private float timeToExplode, second, timePassed,asp = 0;
    private int velocity;
    private bool blocked;
    public Vector2 direction; //Destra o sinistra
    public GameObject sprite;
    public GameObject bomb;
    public Vector2 piratePosition = Vector2.zero;
    private bool die;
    public bool trap = true;
    #endregion

    // Use this for initialization
    void Start () {
        timeToExplode = timePassed = Random.Range(3, 10);
        second = asp = 1;
        changeSprite(timePassed);
        die = false;
        if (!trap)
        {
            transform.position = piratePosition;
            throwBomb();
        }
        else
        {
            this.gameObject.SetActive(false);
        }
       
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
        die = true;
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



    void OnTriggerStay2D(Collider2D other)
    {
        if (die)
        {

            Debug.Log("Sono esplosa ora possono morire");
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
        else Debug.Log("La bomba non è esplosa sono salva");
    }
    
}
