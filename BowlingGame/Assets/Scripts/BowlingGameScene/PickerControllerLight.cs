using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickerControllerLight : MonoBehaviour
{

    public LaneController _laneController; // LaneController script
    public bool FirstMove;                 // If true, the first Move Down/Up has been activated
    /*******************************************************************************************************************/


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
        if (!FirstMove)
        {

            FirstMove = true; // Indicates that the first Move Down/Up has been activated

            for (int i = 0; i < _laneController.GetComponent<LaneController>().ListPins.Count; i++)
            {

                Transform aux = _laneController.GetComponent<LaneController>().ListPins[i].transform.GetChild(0); // Gets the child (Pin's transform)

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
        if (_laneController.StartGame)
            _laneController.SweeperVagon.GetComponent<SweepControllerLight>().MoveUpEnd();   // Calls the Sweeper's MoveUpEnd to reset the animation's variables a play normally
        else if (_laneController.SweepEnded)
            _laneController.SweeperVagon.GetComponent<SweepControllerLight>().MoveUpStart(); // Starts Sweeper's Move Up animation
        else
            _laneController.SweeperVagon.GetComponent<SweepControllerLight>().SweepStart();  // Starts Sweeper's Sweep animation

    }

}
