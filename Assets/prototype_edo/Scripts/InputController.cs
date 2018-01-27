using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class InputController : MonoBehaviour {

    private bool hasToFire = false;
    public PlayerIndex playerControllerIndex;
    private GamePadState state;
    
    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
    }


   
    //Dash
    //Prendi, lancia, parata
    public Vector3 getDirection() {
        float horizontal = state.ThumbSticks.Left.X;
        float vertical = state.ThumbSticks.Left.Y;
        return new Vector3(horizontal, vertical, 0.0f);
    }

    public bool isFiring() {
        state = GamePad.GetState(playerControllerIndex);
        return state.Buttons.A.Equals(ButtonState.Pressed);
     
    }

    public bool isDashing()
    {
        state = GamePad.GetState(playerControllerIndex);
        return state.Buttons.X.Equals(ButtonState.Pressed);

    }


}
