using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class InputController : MonoBehaviour {

    public PlayerIndex playerControllerIndex;
    private GamePadState state;
    
   
    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        state = GamePad.GetState(playerControllerIndex);
        }


   
    //Dash
    //Prendi, lancia, parata
    public Vector3 getDirection() {
        float horizontal = state.ThumbSticks.Left.X;
        float vertical = state.ThumbSticks.Left.Y;
        return new Vector3(horizontal, 0.0f, vertical);
    }

    public bool isFiring() {
        return state.Buttons.A.Equals(ButtonState.Pressed);
    }

    public bool isDashing()
    {
        return state.Buttons.B.Equals(ButtonState.Pressed);
    }

    public bool isThrowingCrow() {
        return state.Buttons.X.Equals(ButtonState.Pressed);
    }


}
