using UnityEngine;
using System.Collections;

public class FishGenerator : MonoBehaviour {

    /* Every delta_fish second, a fish will be instantiated */
    [Range(1, 10)]
    public int delta_fish = 2;
    /* Illimitates fish?*/
    public bool no_limits = true;
    [Range(1,100)]
    public int max_fish_number = 100;

    private int fish_generated = 0;

    private float fish_counter;

    void Start() {
        fish_counter = delta_fish;
        create_fish();
    }

    
	
	void Update () {
        fish_counter -= Time.deltaTime;
        if(!no_limits)
        {
            if (fish_generated > fish_counter)
            {
                Destroy(this.gameObject);
            }
        }

        if (fish_counter <= 0)
        {
            fish_counter = delta_fish;
            create_fish();
        }

	}

    void create_fish() {
        Instantiate(Resources.Load("Fish"), this.transform.position, this.transform.rotation);
        fish_generated++;
    }

}
