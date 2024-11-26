using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PickerController : MonoBehaviour
{

    public GameController _gameController; // gameController script
    public bool FirstMove;                 // If true, the first Move Down/Up has been activated
    /*******************************************************************************************************************/

    // Builder
    void Awake()
    {

        FirstMove = true;

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
        MoveUpStart();                                            // Starts the Move Up animation

        // The interaction with the pins only happens in the first Move Down/Up
        if(!FirstMove)
        {

            FirstMove = true; // Indicates that the first Move Down/Up has been activated

            for (int i = 0; i < _gameController.GetComponent<GameController>().ListPins.Count; i++)
            {

                Transform aux = _gameController.GetComponent<GameController>().ListPins[i].transform.GetChild(0); // Gets the child (Pin's transform)

                // Starts the Move Up animation of the pins that didn't fell
                if (!aux.GetComponent<PinController>().Fallen)
                {

                    aux.GetComponent<Animator>().enabled = true;      // Enables the Pin's animator
                    aux.GetComponent<Rigidbody>().isKinematic = true;
                    aux.GetComponent<PinController>().MoveUpStart();

                }

            }

        }

    }

    // Called when the Move Up starts
    public void MoveUpStart()
    {

        gameObject.GetComponent<Animator>().SetTrigger("Up"); // Starts the Move Up animation

    }

    // Called when the Move Up animation has ended
    public void MoveUpEnd()
    {

        gameObject.GetComponent<Animator>().ResetTrigger("Up");                    // Resets the trigger so the animation doesn't repeat

        // If it's the start of the game some aniamtions are skipped, if the sweep has ended chooses a flow, if not chooses the other
        if (_gameController.StartGame)
            _gameController.SweeperVagon.GetComponent<SweepController>().MoveUpEnd();   // Calls the Sweeper's MoveUpEnd to reset the animation's variables a play normally
        else if (_gameController.SweepEnded)
            _gameController.SweeperVagon.GetComponent<SweepController>().MoveUpStart(); // Starts Sweeper's Move Up animation
        else
            _gameController.SweeperVagon.GetComponent<SweepController>().SweepStart();  // Starts Sweeper's Sweep animation

    }

}
