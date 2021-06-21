using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

//Script that gives many commands every frame to the player script
//used to test if the input system can handle many commands in a short
//period of time and still keep a consistent internal state that allows the
//player car model not to go off the rails ever
public class Commander : MonoBehaviour
{
    public PlayerInputController playerInput;

    public bool giveCommands;

    private Random rng;
    
    void Start()
    {
        rng = new Random();
        StartCoroutine(nameof(GiveCommands));
    }

    IEnumerator GiveCommands()
    {
        for (;;)
        {
            if (giveCommands)
            {
                int rnd1 = rng.Next(0, 1);
                int rnd2 = rng.Next(0, 1);
                int rnd3 = rng.Next(0, 1);

                if(rnd3 == 0) playerInput.OnSwitchSide();
                if(rnd1 == 0) playerInput.OnMoveRight();
                if(rnd2 == 0) playerInput.OnMoveLeft();

                yield return new WaitForEndOfFrame();
            }
            else
            {
                yield return new WaitForSeconds(1.0f);
            }
        }
    }
}
