using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class InputController : MonoBehaviour {

    public bool useKeyboard = false;
    public PlayerIndex playerControllerIndex;
    private GamePadState state;
    private float horizontal;
    private float vertical;
    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (!useKeyboard){
            state = GamePad.GetState(playerControllerIndex);
        }
    }


   
    //Dash
    //Prendi, lancia, parata
    public Vector3 getDirection() {
        if (!useKeyboard)
        {
            horizontal = state.ThumbSticks.Left.X;
            vertical = state.ThumbSticks.Left.Y;
            
        }
        else {
            horizontal = Input.GetAxis("Horizontal_Keyboard");
            vertical = Input.GetAxis("Vertical_Keyboard");
        }
        return new Vector3(horizontal, 0.0f, vertical);
    }

    public bool isFiring() {
        if (!useKeyboard) return state.Buttons.RightShoulder.Equals(ButtonState.Pressed);
        return Input.GetButton("Fire_Keyboard");
    }

    public bool isDashing()
    {
        if (!useKeyboard) return state.Buttons.X.Equals(ButtonState.Pressed);
        return Input.GetButton("Dash_Keyboard");
    }

    public bool isParrying() {
        if (!useKeyboard) return state.Buttons.LeftShoulder.Equals(ButtonState.Pressed);
        return Input.GetButton("Crow_Keyboard");
    }

    public bool isPause()
    {
        if (!useKeyboard)
            return state.Buttons.Start.Equals(ButtonState.Pressed);
        else
            return Input.GetButton("Submit");
    }
}
