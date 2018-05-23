using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardTower : Tower
{
    // Use this for initialization
	void Start ()
	{
	    Health = 20.0f;
	    Damage = 10.0f;
	    Range = 30.0f;
	    FireRate = 1.0f;
	    Size = new Vector2(1, 1);
	    Cost = 0;
	    Aerial = false;
	    Effect = (int)Effects.ENone;
    }

    void Update()
    {
        ShootInterval();
    }
}
