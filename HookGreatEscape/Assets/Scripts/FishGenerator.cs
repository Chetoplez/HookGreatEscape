﻿using UnityEngine;
using System.Collections;

public class FishGenerator : MonoBehaviour {


    public enum FishType { 
        BLUE,
        RED,
        NOT_FISH
    }

    /* Every delta_fish second, a fish will be instantiated */
    [Range(1, 10)]
    public int delta_fish = 2;
    /* Illimitates fish?*/
    public bool no_limits = true;
    [Range(1,100)]
    public int max_fish_number = 100;
    public FishType fish_type = FishType.BLUE;
    

    private int fish_generated = 0;

    private float fish_counter;
    /* Used for calculate the range between the creation of the fish */
    private int random_range = 5;

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
            System.Random rand = new System.Random();
            int val =(int) ( rand.Next(1,random_range) );
            fish_counter = delta_fish + val;
            create_fish();
        }

	}

    void create_fish() {
        if(fish_type==FishType.RED || fish_type==FishType.BLUE)
            Instantiate(Resources.Load((fish_type==FishType.BLUE)?"Fish":"RedFish"), this.transform.position, this.transform.rotation);
        if(fish_type==FishType.NOT_FISH)
            Instantiate(Resources.Load("Seagull"), this.transform.position, this.transform.rotation);
        fish_generated++;
    
    }

}
