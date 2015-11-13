using UnityEngine;
using System.Collections;
using System;

public class AI : MonoBehaviour
{

    #region Variabili
    public AiState.pirate typePirate;
    public float pause;
    private AiState.pirateState prevState, currentState, newState;
    private bool canAttack, canChasing, canThrowing, died,inAir,blocked;

    public bool Blocked {
        get { return blocked; }
        set { blocked = value; }
    }

    private Vector2 chasingTarget = Vector2.zero;
    public LayerMask layerMask;
    public float speed = 1;
    private GameObject hook;
    private int lifePirate;
    public float minDistanceChaising = 1;
    public float maxDistanceChaising = 150;
    private float maxDepth = 100; //Dove cercare "Hook"
    private float minDepth = 1;  //distanza massima per avvicinarsi a un oggetto

    private Vector2 prevPosition;
    #endregion


    // Use this for initialization
    void Start()
    {
        prevState = currentState = newState =  AiState.pirateState.idle;
        canAttack = canChasing = canThrowing = blocked = died = false;
        hook = null;
        choosePirateLife(typePirate);
    }

    // Update is called once per frame
    void Update()
    {
        pause -= Time.deltaTime;
        if (pause <= 0)
        {
            newState = chooseState(currentState, typePirate);
            changeState(newState);
            doSomething();
            Debug.Log("Stato: " + currentState);
        }
    }

    public void choosePirateLife(AiState.pirate pirate) {
        switch (pirate) {
            case AiState.pirate.drunk: lifePirate = 1;
                break;
            case AiState.pirate.sober: lifePirate = 2;
                break;
            case AiState.pirate.verySober: lifePirate = 3;
                break;
            default: Debug.LogError("An error in AI choosePirateLifeState");
                break;
        }
    }

    public void wandering()
    {
        Vector2 direction = (transform.localScale.x >= 0 ? Vector2.right : Vector2.left);
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x,transform.position.y), direction, Mathf.Infinity, layerMask, -Mathf.Infinity, maxDepth);
        Debug.DrawLine(transform.position, direction * maxDistanceChaising);
        if (hit.collider != null && Mathf.Abs(hit.distance) > minDepth)
        {
            transform.position = Vector2.MoveTowards(transform.position, hit.transform.position, speed * Time.deltaTime);
        }
        else if (hit.collider != null && Mathf.Abs(hit.distance) < minDepth)
        {
            flip();
            pause = 1.5f;
        }
        else {
            flip();
            pause = 1.5f;
        }
        //Fare se mai non si trova un collide
    }

    public void chasing()
    {
        
        transform.position = Vector2.MoveTowards(transform.position,chasingTarget, speed * Time.deltaTime);
        //Cambio Sprite
    }

    public void die() {
        Destroy(this.gameObject); //controllare
    }

    public void confuse() {
        lifePirate -= 1;
        if (lifePirate == 0) {
            died = true;
        }
    }

    public void throwBomb()
    {//Per istanziare il prefabs Bomb
        GameObject prefab =(GameObject) Instantiate(Resources.Load<GameObject>("Bomb"));
    }

    public void attack()
    {
        if (hook != null) {
            Debug.Log("Ho il game object Hook");
            Player p = hook.GetComponent<Player>() ?? null;
            if (p != null) p.hit();
            //CambioSprite
        }
    }

    public void showMessage()
    { // Riceve in ingresso un HUD e una posizione per scrivere il mess
        throw new NotImplementedException();
    }

    public void flip() {
        transform.localScale = new Vector2(transform.localScale.x * (-1), transform.localScale.y);
    }

    public void changeState(AiState.pirateState newState)
    {
        prevState = currentState;
        currentState = newState;
    }

    public void doSomething()
    {
        switch (currentState)
        {
            case AiState.pirateState.idle: break;
            case AiState.pirateState.wandering:
                {
                    wandering();
                    break;
                }
            case AiState.pirateState.chasing:
                {
                    chasing();
                    break;
                }
            case AiState.pirateState.attack:
                {
                    pause = 1.5f;
                    if (typePirate.Equals(AiState.pirate.verySober)) throwBomb();
                    else attack();
                    break;
                }
            case AiState.pirateState.blocked:
                {
                    pause = 2;
                    break;
                }
            case AiState.pirateState.throwing:
                {
                    throwBomb();
                    break;
                }
            case AiState.pirateState.confuse:
                {
                    pause = 1;
                    confuse();
                    if (died) die();
                    break;
                    
                }
            default:
                Debug.LogError("There is an error in doSomething, invalid state");
                return;
        }
    }

    public AiState.pirateState chooseState(AiState.pirateState state, AiState.pirate p)
    {
        switch (state)
        {
            case AiState.pirateState.idle:
                {   if (blocked) return AiState.pirateState.blocked;
                    if (canThrowing) return AiState.pirateState.throwing;
                    if (canAttack) return AiState.pirateState.attack;
                    if (canChasing) return AiState.pirateState.chasing;
                    return AiState.pirateState.wandering;
                }
            case AiState.pirateState.wandering:
                {
                    if (blocked) return AiState.pirateState.blocked;
                    if (canAttack) return AiState.pirateState.attack;
                    if (canChasing) return AiState.pirateState.chasing;
                    return AiState.pirateState.wandering;
                }
            case AiState.pirateState.chasing:
                {
                    if (blocked) return AiState.pirateState.blocked;
                    if (canAttack) return AiState.pirateState.attack; 
                    if (canChasing) return AiState.pirateState.chasing;
                    return AiState.pirateState.wandering;
                }
            case AiState.pirateState.attack:
                {
                    if (blocked) return AiState.pirateState.blocked;
                    if (canAttack) return AiState.pirateState.attack;
                    if (canChasing) return AiState.pirateState.chasing;
                    return AiState.pirateState.idle;
                }
            case AiState.pirateState.blocked:
                {   // Bisogna capire se è morto o meno
                    return AiState.pirateState.confuse;
                }
            case AiState.pirateState.throwing:
                {
                    if (blocked) return AiState.pirateState.blocked;
                    return AiState.pirateState.idle;
                }
            case AiState.pirateState.confuse:
                {
                    return AiState.pirateState.idle;
                }
            default:
                Debug.LogError("There is an error in chooseState, invalid state");
                return AiState.pirateState.idle;
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        //if (other.gameObject.tag == "Hook")
        //{
        //    if (typePirate.Equals(AiState.pirate.verySober))
        //    {
        //        canThrowing = true;
        //    }
        //    else
        //    {
        //        Vector2 direction = (transform.localScale.x >= 0 ? Vector2.right : Vector2.left);
        //        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, Mathf.Infinity, layerMask, -Mathf.Infinity, maxDepth);
        //        Debug.DrawLine(transform.position, direction * maxDistanceChaising);
        //        if (hit.collider == null) return;
                
        //        if (hit.collider != null && hit.transform.gameObject.tag == "Hook")
        //        {
        //            hook = other.gameObject;
        //            if (Mathf.Abs(hit.distance) >= minDistanceChaising && Mathf.Abs(hit.distance) < maxDistanceChaising)
        //            {
        //                chasingTarget = new Vector2(transform.position.x + hit.transform.position.x + 5, transform.position.y);
        //                canChasing = true;
        //                canAttack = false;
        //            }
        //            else if (hit.distance < minDistanceChaising)
        //            {
        //                canChasing = false;
        //                canAttack = true;
        //            }
        //        }
        //        else
        //        {
        //            canChasing = canAttack = false;
        //        }
        //    }
        //}

    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Hook") {
            if (typePirate.Equals(AiState.pirate.verySober)) {
                canThrowing = true;
            }
            else {
                Vector2 direction = (transform.localScale.x >= 0 ? Vector2.right : Vector2.left);
                RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, Mathf.Infinity, layerMask, -Mathf.Infinity, maxDepth);
                Debug.DrawLine(transform.position, direction*maxDistanceChaising);
                if (hit.collider == null) return;
                if (hit.collider != null && hit.transform.gameObject.tag == "Hook") {
                    hook = other.gameObject;
                    if (Mathf.Abs(hit.distance) >= minDistanceChaising && Mathf.Abs(hit.distance) < maxDistanceChaising) {
                        chasingTarget = new Vector2(transform.position.x + hit.transform.position.x + 5, transform.position.y);
                        canChasing = true;
                        canAttack = false;
                    } else if(hit.distance < minDistanceChaising){
                        canChasing = false;
                        canAttack = true;
                    }
                 }else
                {
                    canChasing = canAttack = false;
                    Debug.Log("Sei in OntriggerStay tutto a false");
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Hook")
        {
            canAttack = canChasing = canThrowing = false;
        }
    }
    

}



