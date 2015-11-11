using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

    /* Lives between levels */
    private int lives = 5;
    public int Lives { get { return lives; } set { lives = value; } }
    /* Deaths*/
    private int deaths = 0;
    public int Deaths { get { return deaths; } set { deaths = value; } }
}

