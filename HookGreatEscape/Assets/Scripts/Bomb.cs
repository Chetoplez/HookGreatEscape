using UnityEngine;
using System.Collections;

public class Bomb : MonoBehaviour {

    #region Variable
    private float timeToExplode, second, timePassed,asp = 0;
    private int velocity;
    private bool explode;
    public Vector2 direction; //Destra o sinistra
    public GameObject sprite;
    public GameObject bomb;
    public Vector2 piratePosition = Vector2.zero;
    private bool die;
    public bool trap = true;
    public ArrayList listCollider = new ArrayList(); 
    #endregion

    // Use this for initialization
    void Start () {
        timeToExplode = timePassed = Random.Range(3, 10);
        second = asp = 1;
        changeSprite(timePassed);
        die = explode = false;
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
            explode = true;
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

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!listCollider.Contains(other))
            listCollider.Add(other);

        if (other.gameObject.tag == "Hook")
        {
            die = true;
        }
     }


        void OnTriggerStay2D(Collider2D other)
    {
        if (die && explode)
        {
            Debug.Log("Posso morire");
            foreach (Collider2D col in listCollider)
            {
                Debug.Log("Col.tag is: " + col.gameObject.tag);
                if (col.gameObject.tag == "Hook")
                {
                    Debug.Log("Sono in hook");
                    Player p = col.gameObject.GetComponent<Player>() ?? null;
                    if (p != null) p.hit();
                    else Debug.Log("p è null");
                    die = false;
                }
                else if (col.gameObject.tag == "Pirate")
                {
                    Debug.Log("Sono in Pirate");
                    AI p = col.gameObject.GetComponent<AI>() ?? null;
                    if (p != null) p.Blocked = true;
                    die = false;
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(listCollider.Contains(other))
        listCollider.Remove(other);
        if (other.gameObject.tag == "Hook")
        {
            die = false;
        }
    }

}
