using UnityEngine;
using System.Collections;

public class AI : MonoBehaviour
{

    #region Variabili
    public AiState.pirate typePirate;
    
    public float pause; //Quanto restare in pausa da quello stato

    private AiState.pirateState prevState, currentState, newState;
    private bool canAttack, canChasing, canThrowing, attacked,died,inAir;
    private Vector2 prevPosition;
    private Vector2 chaisingTarget = Vector2.zero;
    public LayerMask layerMask;
    public float speed = 1;


    public float minDistanceChaising = 1;
    public float maxDistanceChaising = 150;
    private float maxDepth = 100; //Dove cercare "Hook"
    private float minDepth = 1;  //distanza massima per avvicinarsi a un oggetto
    #endregion

    // Use this for initialization
    void Start()
    {
        prevState = currentState = newState =  AiState.pirateState.idle;
        canAttack = canChasing = canThrowing = attacked = died = false;
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
        else if (hit.collider != null && Mathf.Abs(hit.distance) <  minDepth) {
            flip();
            pause = 1.5f;
            }
        //else 
        //{
        //    float value = (transform.localScale.x >= 0 ? (transform.position.x + transform.position.x *2) : transform.position.x - transform.position.x*2);
        //    pause = 1.5f;
        //    transform.position = Vector2.MoveTowards(transform.position, new Vector2(value, transform.position.y), speed * Time.deltaTime);
        //    flip();
        // // transform.Translate((( new Vector3(value, transform.position.y,transform.position.z) )- transform.position)*Time.deltaTime );    
        //}
    }

    public void chasing()
    {
       transform.Translate(chaisingTarget * Time.deltaTime);
        //Cambio Sprite
    }

    public void die() {
        Destroy(this.gameObject); //controllare
    }

    public void confuse() { }

    public void throwBomb()
    {
    }

    public void attack()
    {
        //CambioSprite
    }

    public void showMessage()
    { // Riceve in ingresso un HUD e una posizione per scrivere il mess
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
                   // pause = 0.5f;
                    wandering();
                    break;
                }
            case AiState.pirateState.chasing:
                {
                   // pause = 1;
                    chasing();
                    break;
                }
            case AiState.pirateState.attack:
                {
                    if (typePirate.Equals(AiState.pirate.verySober)) throwBomb();
                    else attack();
                    break;
                }
            case AiState.pirateState.blocked:
                {
                   // pause = 1.5f;
                    break;
                }
            case AiState.pirateState.throwing:
                {
                    throwBomb();
                    break;
                }
            case AiState.pirateState.confuse:
                {
                    if (died) die();
                    else confuse();
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
                {   if (attacked) return AiState.pirateState.blocked;
                    if (canThrowing) return AiState.pirateState.throwing;
                    if (canAttack) return AiState.pirateState.attack;
                    if (canChasing) return AiState.pirateState.chasing;
                    return AiState.pirateState.wandering;
                }
            case AiState.pirateState.wandering:
                {
                    if (attacked) return AiState.pirateState.blocked;
                    if (canAttack) return AiState.pirateState.attack;
                    if (canChasing) return AiState.pirateState.chasing;
                    return AiState.pirateState.wandering;
                }
            case AiState.pirateState.chasing:
                {
                    if (attacked) return AiState.pirateState.blocked;
                    if (canAttack) return AiState.pirateState.attack; 
                    if (canChasing) return AiState.pirateState.chasing;
                    return AiState.pirateState.wandering;
                }
            case AiState.pirateState.attack:
                {
                    if (attacked) return AiState.pirateState.blocked;
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
                    if (attacked) return AiState.pirateState.blocked;
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

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Hook")
        {

        }
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
                if (hit.collider != null && hit.rigidbody.tag  == "Hook") {
                    if (Mathf.Abs(hit.distance) >= minDistanceChaising && Mathf.Abs(hit.distance) < maxDistanceChaising) {
                        chaisingTarget = new Vector2(transform.position.x + hit.transform.position.x + 5, transform.position.y);
                        canChasing = true;
                        canAttack = false;
                    } else if(hit.distance < minDistanceChaising){
                        canChasing = false;
                        canAttack = true;
                    }
                } else {
                    canChasing = canAttack = false;
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



