using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SweepControllerLight : MonoBehaviour
{

    public LaneController _laneController; // Lane asociated with this sweep
    /*******************************************************************************************************************/


    // Called when the Move Down animation starts
    public void MoveDownStart()
    {

        _laneController.AnimationOn = true;                     // Indicates there's an animation playing to the Game Manager
        gameObject.GetComponent<Animator>().SetTrigger("Down"); // Starts the Move Down animation

    }

    // Called when the Move Down animation has ended
    public void MoveDownEnd()
    {

        gameObject.GetComponent<Animator>().ResetTrigger("Down");                           // Resets the trigger so the animation doesn't repeat

        // If all the Pins has fallen or its the end of second try, starts the Sweep animation, if not, calls the Picker to Move Down
        if (_laneController.NumPinsFallen == _laneController.ListPins.Count || _laneController.CurrentTry >= 2)
            SweepStart();
        else
            _laneController.BowlingPicker.GetComponent<PickerControllerLight>().MoveDownStart(); // Starts Picker's Move Down animation

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

        // Resets the animations variables
        _laneController.StartGame = false;
        _laneController.SweepEnded = false;
        _laneController.AnimationOn = false;
        _laneController.NumPinsFallen = 0;
        _laneController.BowlingPicker.GetComponent<PickerControllerLight>().FirstMove = false;

        // Reset BALL and PINS and re-enables shooting
        StartCoroutine(_laneController.ResetShoot());

    }

    // Called when the Sweep starts
    public void SweepStart()
    {

        gameObject.GetComponent<Animator>().SetTrigger("Sweep"); // Starts the Sweep animation

        // If it's the end of the second try or all the Pins have fallen and it's not the 10th Frame passes to the next frame, if not, passes to the next turn
        if (_laneController.CurrentTry >= 2 || _laneController.NumPinsFallen == _laneController.ListPins.Count)
            _laneController.CurrentTry = 1;
        else
            _laneController.CurrentTry++;

    }

    // Called when the Sweep animation has ended, if its the 10th Frame the Game ends
    public void SweepEnd()
    {

        gameObject.GetComponent<Animator>().ResetTrigger("Sweep");                           // Resets the trigger so the animation doesn't repeat

        _laneController.SweepEnded = true;                                                   // Indicates that the sweep has ended
        _laneController.BowlingPicker.GetComponent<PickerControllerLight>().MoveDownStart(); // Starts Picker's Move Down animation

        // If all the Pins has fallen, starts the Move Down of the Picker with all the new Pins created, if not, starts the Move Down of the Picker with the Pins that survived
        if (_laneController.NumPinsFallen == _laneController.ListPins.Count)
        {

            _laneController.DestroyPins();
            _laneController.CreatePins();
            _laneController.BowlingPicker.GetComponent<PickerControllerLight>().FirstMove = true; // Set to true so the Picker doesn't elevate the Pins again

        }

        // Starts the Move Down animation of the pins that didn't fell
        for (int i = 0; i < _laneController.GetComponent<LaneController>().ListPins.Count; i++)
        {

            Transform aux = _laneController.GetComponent<LaneController>().ListPins[i].transform.GetChild(0); // Gets the child (Pin's transform)

            // Activates de Move Down animation of the Pins that didn't fell
            if (!aux.GetComponent<PinController>().Fallen)
                aux.GetComponent<PinController>().MoveDownStart();

        }

    }

}
