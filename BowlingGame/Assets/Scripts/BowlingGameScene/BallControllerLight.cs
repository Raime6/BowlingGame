using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallControllerLight : MonoBehaviour
{
    public GlobalVariables _globalVariables;    // globalVariables
    public LaneController _laneController;      // laneController script

    public Rigidbody _rb;                       // Ball's Rigidbody
    public float Force;                         // Impulse Force
    public float PinCollisionForce;             // Force used when the ball collides with a Pin
    public List<float> PinsHorizontalPosition; // List of horizontal positions where the Pins are placed
    /*******************************************************************************************************************/

    // Builder
    void Awake()
    {

        _globalVariables = GameObject.Find("GlobalVariables").GetComponent<GlobalVariables>();

        _rb = GetComponent<Rigidbody>(); // Rigidbody's ball
        PinCollisionForce = 5;

    }

    private void Update()
    {

        // Shooting force
        if (_globalVariables.EffectShoot)
            Force = 0.4f;
        else
            Force = 5;

    }

    public IEnumerator CpuShoot(int difficulty)
    {

        bool fails = false;
        float percentageFailure;
        float percentageStrike;
        int randPosition;
        float randPositionFloat;
        int randTypeShoot;
        float randEffect;
        int randStrike;

        // Depending on the difficulty of the CPU, the percentage of failure it's higher
        if (difficulty == 1)      // Easy CPU
        {

            percentageFailure = 2.5f;
            percentageStrike = 1;

        }
        else if (difficulty == 2) // Normal CPU
        {

            percentageFailure = 5;
            percentageStrike = 3;

        }
        else if (difficulty == 3) // Hard CPU
        {

            percentageFailure = 7.5f;
            percentageStrike = 5;

        }
        else                      // Very Hard CPU
        {

            percentageFailure = 10;
            percentageStrike = 8;

        }

        // If User has enabled Effect Shoot, there's a chance, if it's disabled, there is no chance and always will shoot without effect
        if (_globalVariables.EffectShoot)
        {

            randTypeShoot = Random.Range(0, 3); // Selects randomly if it's going to shoot with effect or not | 0 - no | 1 & 2 - yes
            if (randTypeShoot != 0)
                randTypeShoot = 1;

        }
        else
            randTypeShoot = 0;

        // If at least one Pin has fallen
        if (_laneController.ListPins.Count != 10)
        {

            // Sets a random number between 0 and 10 as a probability that will vary depending in the CPU difficulty
            float randPercentage = Random.Range(0, 11);

            if (randPercentage > percentageFailure)
                fails = true;

        }

        if (_laneController.ListPins.Count != 10 && !fails) // Not failure
        {

            // Selects randomly the horizontal position
            randPosition = Random.Range(0, _laneController.ListPins.Count);
            _rb.transform.position += new Vector3(0, 0, Mathf.Abs(_rb.transform.position.z - _laneController.ListPins[randPosition].transform.position.z));

            if (randTypeShoot == 1) // Effect shoot
            {

                randEffect = Random.Range(-0.8f, 0.81f); // Random Bezier's Central Point
                // Sets the value only if the effect doesn't pass the Lane's limits
                if ((_rb.transform.position.z + randEffect) > (_laneController.GetComponent<LaneController>().BallInitialPosition.z - 0.42f) && (_rb.transform.position.z + randEffect) < (_laneController.GetComponent<LaneController>().BallInitialPosition.z + 0.42f))
                    gameObject.transform.GetChild(1).transform.position = new Vector3(0, 0, _rb.transform.position.z + randEffect);

            }

        }
        else // Normal shoot
        {

            // Depending on the difficulty of the CPU, has higher possibilities of doing a Strike
            float randPercentageStrike = Random.Range(0, 11);

            if (randPercentageStrike < percentageStrike && _laneController.ListPins.Count == 10) // Strike
            {

                randStrike = Random.Range(0, 2); // As there is two coords to do a strike, determines which one
                if (randStrike == 0)
                    _rb.transform.position += new Vector3(0, 0, -0.064f); // Strike's Z coords
                else
                    _rb.transform.position += new Vector3(0, 0, 0.053f); // Strike's Z coords

            }
            else                                          // Normal shoot
            {

                // Selects randomly the horizontal position
                randPositionFloat = Random.Range(-0.42f, 0.43f);
                _rb.transform.position += new Vector3(0, 0, randPositionFloat);

                if (randTypeShoot == 1) // Effect shoot
                {

                    randEffect = Random.Range(-0.8f, 0.81f); // Random Bezier's Central Point
                    // Sets the value only if the effect doesn't pass the Lane's limits
                    if ((_rb.transform.position.z + randEffect) > (_laneController.GetComponent<LaneController>().BallInitialPosition.z - 0.42f) && (_laneController.transform.position.z + randEffect) < (_laneController.GetComponent<LaneController>().BallInitialPosition.z + 0.42f))
                        gameObject.transform.GetChild(1).transform.position = new Vector3(0, 0, _rb.transform.position.z + randEffect);


                }

            }

        }

        yield return new WaitForSeconds(2);

        _rb.useGravity = true;                                                                    // Enables gravity

        if (_globalVariables.EffectShoot)
            StartCoroutine(gameObject.transform.GetChild(0).GetComponent<BallBezierCurve>().Move(transform, Force));
        else
            _rb.AddRelativeForce(new Vector3(-transform.position.x * Force, 0, 0), ForceMode.Impulse); // Shoot the ball

    }

    /* When the Ball enters on collision with a Pin, adds a little force to make it visually more realistic */
    private void OnCollisionEnter(Collision collision)
    {

        if (collision.transform.name.Contains("Pin"))
            collision.transform.GetComponent<Rigidbody>().AddForce(new Vector3(PinCollisionForce, PinCollisionForce, PinCollisionForce), ForceMode.Force);

    }
}
