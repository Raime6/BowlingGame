using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
//using UnityEditor.Animations;
using UnityEngine;

public class PinController : MonoBehaviour
{

    public GameController _gameController; // gameController script
    public LaneController _laneController; // laneController script
    private Rigidbody _rb;                 // Pin's rigidbody
    public Animator _animator;             // Pin's animator

    public bool Fallen;                    // Controls if the pin has fallen
    private bool FallenCheck;              // Controls that in the "IsFallen" method, once check the falling it cannot be executed again
    private int FallingAngle;              // Angle that determines if the pin is going to fall
    /*******************************************************************************************************************/

    // Builder
    void Awake()
    {

        _rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        Fallen = false;
        FallenCheck = false;
        FallingAngle = 300;

    }

    // Update is called once per frame
    void Update()
    {

        if(!FallenCheck)
            IsFallen();

    }

    /* Checks if the Pin has fallen comparing it's rotation and position (position in case the Pin has been clearly hit but stays in the same angle (rarely happens)) */
    private void IsFallen()
    {

        if(transform.eulerAngles.x >= FallingAngle || transform.position.x >= -0.5f)
        {

            FallenCheck = true;
            Fallen = true;

            // Checks if the Pins is asociated with the Player's lane (uses Game Controller) or another lane (uses Lane Controller)
            if(_gameController != null)
                _gameController.NumPinsFallen++;
            if(_laneController != null)
                _laneController.NumPinsFallen++;

        }

    }

    // Called when the Move Down animation starts
    public void MoveDownStart()
    {

        gameObject.GetComponent<Animator>().SetTrigger("Down"); // Starts the Move Down animation

    }

    // Called when the Move Down animation has ended
    public void MoveDownEnd()
    {

        gameObject.GetComponent<Animator>().ResetTrigger("Down"); // Resets the trigger so the animation doesn't repeat
        _rb.isKinematic = false;                                  // Reactivates the Pin's rigidbody so it's affected by the physics again
        _animator.enabled = false;                                // Disables the Pin's animator so it's physics work correctly

    }

    // Called when the Move Up starts
    public void MoveUpStart()
    {

        gameObject.GetComponent<Animator>().SetTrigger("Up"); // Starts the Move Up animation

    }

    // Called when the Move Up animation has ended
    public void MoveUpEnd()
    {

        gameObject.GetComponent<Animator>().ResetTrigger("Up"); // Resets the trigger so the animation doesn't repeat

    }

}
