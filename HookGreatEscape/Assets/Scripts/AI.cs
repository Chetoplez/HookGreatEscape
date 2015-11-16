using UnityEngine;
using System.Collections;
using System;

public class AI : MonoBehaviour
{

    #region Variabili
    public AiState.pirate typePirate;
    private float pause;
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

    private Animator animator = null;

    private Vector2 dirBomb;
    private BoxCollider2D col;
    private float minDistanceChaising = 10;
    private float maxDistanceChaising = 1;
    private float maxDepth = 15; //Dove cercare "Hook"
    private float minDepth = 1f;  //distanza massima per avvicinarsi a un oggetto
    #endregion


    // Use this for initialization
    void Start()
    {
        prevState = currentState =  AiState.pirateState.idle;
        canAttack = canChasing = canThrowing = blocked = died = false;
        hook = null;
        col = gameObject.GetComponent<BoxCollider2D>();
        maxDistanceChaising = col.size.x / 2 + 1;
        minDistanceChaising = maxDistanceChaising / 2;
        choosePirateLife(typePirate);
        animator = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        pause -= Time.deltaTime;
        if (pause <= 0)
        {
            newState = chooseState(currentState);
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
          RaycastHit2D hit = Physics2D.Raycast( new Vector2(transform.position.x,transform.position.y), direction, Mathf.Infinity, layerMask, -Mathf.Infinity, maxDepth);
          Debug.DrawLine(transform.position, new Vector2(direction.x*15,transform.position.y));
        if (hit.collider != null && Mathf.Abs(hit.distance) > minDepth)
        {
            animator.SetInteger("State", 1);
            transform.position = Vector2.MoveTowards(transform.position, hit.transform.position , speed * Time.deltaTime);
        }
        else if (hit.collider != null && Mathf.Abs(hit.distance) < minDepth)
        {
            animator.SetInteger("State",0);
            flip();
            pause = 1.5f;
        }
        else {
            animator.SetInteger("State", 0);
            flip();
            pause = 2;
        }
    }

    public void chasing()
    {
      transform.position = Vector2.MoveTowards(transform.position,chasingTarget, speed * Time.deltaTime);
      animator.SetInteger("State", 1);
    }

    public void die() {
        Destroy(this.gameObject); //controllare
    }

    public void confuse() {
        lifePir();
        if (lifePirate <= 0)
        {
            died = true;
        }
        else blocked = false;
    }

    public void lifePir() {
        lifePirate--;
    }

    public void throwBomb()
    {//Per istanziare il prefabs Bomb
        if (!((dirBomb.x > 0 && transform.position.x > 0) || (dirBomb.x < 0 && transform.position.x < 0)))
        {
            flip();
        }
        animator.SetInteger("State", 1);
        GameObject prefab =(GameObject) Instantiate(Resources.Load<GameObject>("Bomb"));
        Bomb b = prefab.GetComponent<Bomb>();
        b.trap = false;
        b.direction = dirBomb;
        b.piratePosition= transform.position ;
  
    }

    public void changeAnimation() {
        animator.SetInteger("State", 0);
    }

    public void attack()
    {

        animator.SetInteger("State", 2);
        if (hook != null) {
            Player p = hook.GetComponent<Player>() ?? null;
            if (p != null)
            {  if(p.Alive)
                p.hit();
            }
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
            case AiState.pirateState.idle:
                animator.SetInteger("State", 0);
                break;
            case AiState.pirateState.wandering:
                {
                    if(typePirate!=AiState.pirate.verySober)
                    wandering();
                    break;
                }
            case AiState.pirateState.chasing:
                {
                    if (typePirate != AiState.pirate.verySober)
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
                    if (typePirate.Equals(AiState.pirate.verySober)) {
                        pause = 0.5f;
                    }
                    else  pause = 1.5f;
                    break;
                }
            case AiState.pirateState.throwing:
                {

                    throwBomb();
                    pause = 8;
                    break;
                }
            case AiState.pirateState.confuse:
                {
                    pause = 1f;
                    confuse();
                    if (died) die();
                    break;
                    
                }
            default:
                Debug.LogError("There is an error in doSomething, invalid state");
                return;
        }
    }

    public AiState.pirateState chooseState(AiState.pirateState state)
    {
        switch (state)
        {
            case AiState.pirateState.idle:
                {   
                    if (blocked) return AiState.pirateState.blocked;
                    if (canThrowing) return AiState.pirateState.throwing;
                    if (canAttack) return AiState.pirateState.attack;
                    if (canChasing) return AiState.pirateState.chasing;
                    if (typePirate != AiState.pirate.verySober) return AiState.pirateState.wandering;
                    return AiState.pirateState.idle;
                       
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
                {   
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

    void OnCollisionStay2D(Collision2D other) {
        if (other.gameObject.tag == "Pirate") {
            flip();
        }
    }

  

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Hook" && !died && !blocked) {
            if (typePirate.Equals(AiState.pirate.verySober)) {
                canThrowing = true;
                dirBomb = other.transform.position - transform.position;
                dirBomb.y = Mathf.Abs(dirBomb.y);
            }
            else {
                Vector2 direction = (transform.localScale.x >= 0 ?Vector2.right : Vector2.left);
                RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), direction, Mathf.Infinity, layerMask, -Mathf.Infinity, maxDepth);
                Debug.DrawLine(transform.position, new Vector2(direction.x * 15, transform.position.y));
                if (hit.collider != null && hit.transform.gameObject.tag == "Hook") {
                    hook = other.gameObject;
                    if (Mathf.Abs(hit.distance) >= minDistanceChaising && Mathf.Abs(hit.distance) < maxDistanceChaising) {
                        chasingTarget = new Vector2( hit.transform.position.x, transform.position.y);
                        canChasing = true;
                        canAttack = false;
                    } else if(hit.distance < minDistanceChaising && typePirate.Equals(AiState.pirate.sober)) // Controllare se funziona, i pirati drunk non dovrebbero attaccare, in caso togliere l'&&
                    {
                        canChasing = false;
                        canAttack = true;
                    }
                 }else
                {
                    canChasing = canAttack = canThrowing = false;
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



