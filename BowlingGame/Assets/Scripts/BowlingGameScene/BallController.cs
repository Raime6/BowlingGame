using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BallController : MonoBehaviour
{

    public GlobalVariables _globalVariables;    // GlobalVariables's script
    public GameController _gameController;      // gameController script

    public Rigidbody _rb;                       // Ball's Rigidbody
    public float Force;                         // Impulse Force
    public float PinCollisionForce;             // Force used when the ball collides with a Pin
    private List<float> PinsHorizontalPosition; // List of horizontal positions where the Pins are placed
    public GameObject BallLine;                 // Ball's LineRenderer
    /*******************************************************************************************************************/

    // Builder
    void Awake()
    {

        _globalVariables = GameObject.Find("GlobalVariables").GetComponent<GlobalVariables>();

        _rb = GetComponent<Rigidbody>(); // Rigidbody's ball

        PinCollisionForce = 5;
        PinsHorizontalPosition = new List<float>() { 0, 0.15f, -0.15f, 0.3f, 0, -0.3f, 0.45f, 0.15f, -0.15f, -0.45f };

    }

    // Update is called once per frame
    void Update()
    {

        // Shooting force
        if (_globalVariables.EffectShoot)
            Force = 0.4f;
        else
            Force = 5;

        ShootBall();
        HorizontalDirection();
        ChangeRotation();

    }

    /* Controls the shoot of the ball */
    public void ShootBall()
    {

        if (Input.GetButtonDown("Jump") && _gameController.ShootEnable)
        {

            _gameController.ShootEnable = false;                                                      // Disable shooting
            _rb.useGravity = true;                                                                    // Enables gravity

            // Type of shoot (Depends if the User has selected the Effect Shoot setting)
            if (_globalVariables.EffectShoot)
                StartCoroutine(gameObject.transform.GetChild(0).GetComponent<BallBezierCurve>().Move(transform, Force));
            else
                _rb.AddRelativeForce(new Vector3(-transform.position.x * Force, 0, 0), ForceMode.Impulse); // Shoot the ball

            BallLine.SetActive(false); // Hides the LineRenderer

        }

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
        if (_gameController.ListPins.Count != 10)
        {

            // Sets a random number between 0 and 10 as a probability that will vary depending in the CPU difficulty
            float randPercentage = Random.Range(0, 11);

            if (randPercentage > percentageFailure)
                fails = true;

        }

        if (_gameController.ListPins.Count != 10 && !fails) // Not failure
        {

            // Selects randomly the horizontal position
            randPosition = Random.Range(0, _gameController.ListPins.Count);
            _rb.transform.position += new Vector3(0, 0, _gameController.ListPins[randPosition].transform.position.z);

            if (randTypeShoot == 1) // Effect shoot
            {

                randEffect = Random.Range(-0.8f, 0.81f); // Random Bezier's Central Point
                // Sets the value only if the effect doesn't pass the Lane's limits
                if ((_rb.transform.position.z + randEffect) > - 0.42f && (_rb.transform.position.z + randEffect) < 0.42f)
                    gameObject.transform.GetChild(1).transform.position = new Vector3(0, 0, randEffect);

            }

        }
        else // Normal shoot
        {

            // Depending on the difficulty of the CPU, has higher possibilities of doing a Strike
            float randPercentageStrike = Random.Range(0, 11);

            if (randPercentageStrike < percentageStrike && _gameController.ListPins.Count == 10) // Strike
            {
                randStrike = Random.Range(0, 2); // As there is two coords to do a strike, determines which one
                if(randStrike == 0)
                    _rb.transform.position += new Vector3(0, 0, -0.063f); // Strike's Z coords
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
                    if ((_rb.transform.position.z + randEffect) > -0.42f && (_rb.transform.position.z + randEffect) < 0.42f)
                        gameObject.transform.GetChild(1).transform.position = new Vector3(0, 0, randEffect);

                }

            }

        }

        yield return new WaitForSeconds(2);

        _rb.useGravity = true;                                                                    // Enables gravity
                                                                                                  // Type of shoot (Depends if the User has selected the Effect Shoot setting)
        if (_globalVariables.EffectShoot)
            StartCoroutine(gameObject.transform.GetChild(0).GetComponent<BallBezierCurve>().Move(transform, Force));
        else
            _rb.AddRelativeForce(new Vector3(-transform.position.x * Force, 0, 0), ForceMode.Impulse); // Shoot the ball

        BallLine.SetActive(false); // Hides the LineRenderer

    }

    /* Controls de horizontal direction set to the ball before shooting the ball */
    public void HorizontalDirection()
    {

        // If Player hasn't shoot and is between limits
        if (_gameController.ShootEnable && _rb.transform.position.z <= 0.42f && _rb.transform.position.z >= -0.42f)
        {

            // Move horizontally
            if (Input.GetButton("Horizontal"))
            {

                if (Input.GetAxisRaw("Horizontal") > 0) // Move to the right
                    _rb.transform.position += new Vector3(0, 0, 0.001f);
                else                                    // Move to the left
                    _rb.transform.position += new Vector3(0, 0, -0.001f);

            }

        }
        else // If its off limits, restores position
        {

            if(_rb.transform.position.z > 0.42f)
                _rb.transform.position = new Vector3(_rb.transform.position.x, _rb.transform.position.y, 0.42f);
            if(_rb.transform.position.z < -0.42f)
                _rb.transform.position = new Vector3(_rb.transform.position.x, _rb.transform.position.y, -0.42f);

        }

    }

    /* Controls de horizontal direction set to the ball before shooting the ball */
    public void ChangeRotation()
    {

        // If Player hasn't shoot and is between limits
        if (_gameController.ShootEnable)
        {

            // Move Y angle
            if (Input.GetKey(KeyCode.E))       // Move to the right
                _rb.transform.eulerAngles += new Vector3(0, 0.025f, 0);
            if (Input.GetKey(KeyCode.Q))  // Move to the left
                _rb.transform.eulerAngles += new Vector3(0, -0.025f, 0);

        }
        else // If its off limits, restores position
        {

            //if (_rb.transform.eulerAngles.y > 5)
            //    _rb.transform.eulerAngles = new Vector3(_rb.transform.eulerAngles.x, 5, _rb.transform.eulerAngles.z);
            //if (_rb.transform.eulerAngles.y < -5)
            //    _rb.transform.eulerAngles = new Vector3(_rb.transform.eulerAngles.x, 5, _rb.transform.eulerAngles.z);

        }

    }

    /* When the Ball enters on collision with a Pin, adds a little force to make it visually more realistic */
    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.transform.name.Contains("Pin"))
            collision.transform.GetComponent<Rigidbody>().AddForce(new Vector3(PinCollisionForce, PinCollisionForce, PinCollisionForce), ForceMode.Force);

    }

}
