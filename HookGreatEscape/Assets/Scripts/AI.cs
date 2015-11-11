using UnityEngine;
using System.Collections;

public class AI : MonoBehaviour
{



    public AiState.pirate typePirate;
    public int minDistanceChaising=0, maxDistanceChaising = 150;

    private AiState.pirateState prevState, currentState, newState;
    private bool canAttack, canChasing, canThrowing;
    private float speed = 1.0f;

    private float maxDepth = 150; //Dove cercare "Hook"
    private float minDepth = 0.5f;
    public float MaxDepth{
        get { return maxDepth; }
        set { maxDepth = value;  }
        }

    // Use this for initialization
    void Start()
    {
        prevState = currentState = newState = AiState.pirateState.idle;
        canAttack = canChasing = canThrowing = false;
    }

    // Update is called once per frame
    void Update()
    {
        newState = chooseState(currentState, typePirate);
        changeState(newState);
        doSomething();
        Debug.Log("State is" + currentState);
    }

    public void wandering()
    {
        Vector2 direction = (transform.localScale.x >= 0 ? Vector2.right : Vector2.left);
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x,transform.position.y), direction, Mathf.Infinity, Physics2D.DefaultRaycastLayers, -Mathf.Infinity, maxDepth);
        Debug.DrawLine(transform.position, direction * maxDistanceChaising);
        if (hit.collider == null) return;
        Debug.Log("Ho colliso:" + hit.rigidbody.tag);
        Debug.Log("Distance in wandering is: " + hit.distance);
        if (Mathf.Abs(hit.distance) > minDepth) {
           transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x + hit.transform.position.x * 0.02f, transform.position.y), speed);
        } else
        {
            transform.localScale = new Vector2(transform.localScale.x * (-1), transform.localScale.y );
        }

    }

    public void chasing()
    {
    }


    public void throwBomb()
    {
    }

    public void attack()
    {
    }

    public void showMessage()
    { // Riceve in ingresso un HUD e una posizione per scrivere il mess
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
                    if (typePirate.Equals(AiState.pirate.verySober)) throwBomb();
                    else attack();
                    break;
                }
            case AiState.pirateState.blocked:
                {
                    break;
                }
            case AiState.pirateState.throwing:
                {
                    throwBomb();
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
                {
                    if (canThrowing) return AiState.pirateState.throwing;
                    if (canAttack) return AiState.pirateState.attack;
                    if (canChasing) return AiState.pirateState.chasing;
                    return AiState.pirateState.wandering;
                }
            case AiState.pirateState.wandering:
                {
                    if (canAttack) return AiState.pirateState.attack;
                    if (canChasing) return AiState.pirateState.chasing;
                    return AiState.pirateState.idle;
                }
            case AiState.pirateState.chasing:
                {
                    if (canAttack) return AiState.pirateState.attack;
                    return AiState.pirateState.wandering;
                }
            case AiState.pirateState.attack:
                {
                    if (canChasing) return AiState.pirateState.chasing;
                    return AiState.pirateState.idle;
                }
            case AiState.pirateState.blocked:
                {   // Bisogna capire se è morto o meno
                    break;
                }
            case AiState.pirateState.throwing:
                {
                    return AiState.pirateState.idle;
                }
            default:
                Debug.LogError("There is an error in chooseState, invalid state");
                break;
        }

        return AiState.pirateState.idle;
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
                RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, Mathf.Infinity, Physics2D.DefaultRaycastLayers, -Mathf.Infinity, maxDepth);
                Debug.DrawLine(transform.position, direction*maxDistanceChaising);
                if (hit.rigidbody.tag == "Hook") {
                    Debug.Log("Raycast, find Hook. Distance is:" + hit.distance);
                    if (hit.distance >= minDistanceChaising && hit.distance < maxDistanceChaising) {
                        canChasing = true;
                        canAttack = false;
                    } else if(hit.distance < minDistanceChaising){
                        canAttack = true;
                        canChasing = false;
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



