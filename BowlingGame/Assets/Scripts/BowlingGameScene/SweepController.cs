using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class SweepController : MonoBehaviour
{

    public GlobalVariables _globalVariables; // GlobalVariables's script
    public GameController _gameController;   // gameController script
    /*******************************************************************************************************************/

    private void Awake()
    {
        
        _globalVariables = GameObject.Find("GlobalVariables").GetComponent<GlobalVariables>();

    }

    // Called when the Move Down animation starts
    public void MoveDownStart()
    {

        _gameController.AnimationOn = true;                     // Indicates there's an animation playing to the Game Manager
        gameObject.GetComponent<Animator>().SetTrigger("Down"); // Starts the Move Down animation

    }

    // Called when the Move Down animation has ended
    public void MoveDownEnd()
    {

        gameObject.GetComponent<Animator>().ResetTrigger("Down");                           // Resets the trigger so the animation doesn't repeat

        // If all the Pins has fallen or its the end of second try, starts the Sweep animation, if not, calls the Picker to Move Down
        if (_gameController.NumPinsFallen == _gameController.ListPins.Count || _gameController.CurrentTry >= 2)
            SweepStart();
        else
            _gameController.BowlingPicker.GetComponent<PickerController>().MoveDownStart(); // Starts Picker's Move Down animation

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
        _gameController.StartGame = false;
        _gameController.SweepEnded = false;
        _gameController.AnimationOn = false;
        _gameController.NumPinsFallen = 0;
        _gameController.BowlingPicker.GetComponent<PickerController>().FirstMove = false;

        // Reset BALL and PINS and re-enables shooting
        _gameController.ResetShoot();

    }

    // Called when the Sweep starts
    public void SweepStart()
    {

        gameObject.GetComponent<Animator>().SetTrigger("Sweep"); // Starts the Sweep animation

        // Updates the Player's points
        _gameController.PlayerTotalPointsList[_gameController.Turn - 1] += _gameController.NumPinsFallen;
        
        if(_gameController.Turn == 1)
            _gameController.Player1PointsList[_gameController.CurrentFrame - 1] += _gameController.NumPinsFallen;
        else if (_gameController.Turn == 2)
            _gameController.Player2PointsList[_gameController.CurrentFrame - 1] += _gameController.NumPinsFallen;
        else if (_gameController.Turn == 3)
            _gameController.Player3PointsList[_gameController.CurrentFrame - 1] += _gameController.NumPinsFallen;
        else
            _gameController.Player4PointsList[_gameController.CurrentFrame - 1] += _gameController.NumPinsFallen;

        // Updates the Player's points with the bonuses of the strikes/spares unless it's the last try of the 10th Frame
        if (!(_gameController.CurrentFrame == 10 && _gameController.CurrentTry > 2))
            _gameController.SetBonusPointsInSheet();

        // Updates the Texts in the UI
        if ((_gameController.CurrentTry == 1 || _gameController.CurrentFrame == 10) && _gameController.NumPinsFallen == 10)      // Case it's a Strike
        {

            _gameController.SetPointsInSheet("X");

            if (_gameController.CurrentFrame != 10) // All cases except it's the 10th Frame
            {

                _gameController.PlayerStrikeSpareList.Add(_gameController.Turn);        // Add the current Player to control it's points later
                _gameController.FrameStrikeSpareList.Add(_gameController.CurrentFrame); // Add the current Frame to control it's points later
                _gameController.StrikeSpareList.Add(2);                                 // Indicates that the current Frame is a strike

            }
            else
                _gameController.StrikeInLastFrame = true;                               // Activates the variable to indicate that the game another try up to 3

        }
        else if (_gameController.CurrentTry == 2 && _gameController.NumPinsFallen == _gameController.ListPins.Count) // Case it's a Spare
        {

            _gameController.SetPointsInSheet("/");
            _gameController.PlayerStrikeSpareList.Add(_gameController.Turn);        // Add the current Player to control it's points later
            _gameController.FrameStrikeSpareList.Add(_gameController.CurrentFrame); // Add the current Frame to control it's points later
            _gameController.StrikeSpareList.Add(1);                                 // Indicates that the current Frame is a spare

        }
        else if (_gameController.NumPinsFallen == 0)                                                                 // Case no Pins has fallen
            _gameController.SetPointsInSheet("-");
        else                                                                                                         // Normal case
            _gameController.SetPointsInSheet("" + _gameController.NumPinsFallen);

        // If it's the end of the second try or all the Pins have fallen and it's not the 10th Frame passes to the next frame, if not, passes to the next turn
        if ((_gameController.CurrentFrame != 10 || (_gameController.CurrentFrame == 10 && (_gameController.CurrentTry >= 3 || !_gameController.StrikeInLastFrame))) && (_gameController.CurrentTry >= 2 || _gameController.NumPinsFallen == _gameController.ListPins.Count))
        {

            if (_gameController.Turn < _globalVariables.NumPlayers)
            {

                _gameController.Turn++;
                _gameController.CurrentTry = 1;

            }
            else
            {

                _gameController.Turn = 1;
                _gameController.CurrentTry = 1;
                _gameController.CurrentFrame++;

            }

        }
        else
            _gameController.CurrentTry++;

    }

    // Called when the Sweep animation has ended, if its the 10th Frame the Game ends
    public void SweepEnd()
    {

        int highScoreIndex = 0, highScore = 0;

        gameObject.GetComponent<Animator>().ResetTrigger("Sweep");                      // Resets the trigger so the animation doesn't repeat

        if (_gameController.CurrentFrame > 10) // Case the 10th Frame has ended
        {

            Time.timeScale = 0; // Stops the game

            // Determines the higher score in the game
            highScore = _gameController.PlayerTotalPointsList.Max();

            // Determines it's index in the list
            for(int i = 0; i < _gameController.PlayerTotalPointsList.Count; i++)
            {

                if (_gameController.PlayerTotalPointsList[i] == highScore)
                    highScoreIndex = i;

            }

            /* Shows the Player's Final Score in the UI */
            _gameController.WinnerPlayerText.text = "¡" + _globalVariables.PlayersNamesList[highScoreIndex] + " wins!";
            _gameController.FinalScore.text = highScore + " points";
            _gameController.FinalScorePanel.gameObject.SetActive(true);

        }
        else
        {

            _gameController.SweepEnded = true;                                              // Indicates that the sweep has ended
            _gameController.BowlingPicker.GetComponent<PickerController>().MoveDownStart(); // Starts Picker's Move Down animation

            // If all the Pins has fallen, starts the Move Down of the Picker with all the new Pins created, if not, starts the Move Down of the Picker with the Pins that survived
            if (_gameController.NumPinsFallen == _gameController.ListPins.Count)
            {

                _gameController.DestroyPins();
                _gameController.CreatePins();
                _gameController.BowlingPicker.GetComponent<PickerController>().FirstMove = true; // Set to true so the Picker doesn't elevate the Pins again

            }

            // Starts the Move Down animation of the pins that didn't fell
            for (int i = 0; i < _gameController.GetComponent<GameController>().ListPins.Count; i++)
            {

                Transform aux = _gameController.GetComponent<GameController>().ListPins[i].transform.GetChild(0); // Gets the child (Pin's transform)

                // Activates de Move Down animation of the Pins that didn't fell
                if (!aux.GetComponent<PinController>().Fallen)
                    aux.GetComponent<PinController>().MoveDownStart();

            }

        }

    }

}
