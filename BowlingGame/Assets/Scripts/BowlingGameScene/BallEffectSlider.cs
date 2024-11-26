using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallEffectSlider : MonoBehaviour
{

    [SerializeField] Slider EffectSlider;           // Ball's effect slider
    public Transform PlayerBallCenterPoint; // Ball's Bezier center point that determines the ball's trayectory

    // Builder
    private void Awake()
    {

        EffectSlider = this.GetComponent<Slider>();

    }

    public void SetBallEffect()
    {
        
        PlayerBallCenterPoint.position = new Vector3(PlayerBallCenterPoint.position.x, PlayerBallCenterPoint.position.y, EffectSlider.value);

    }

    public void SetValue(float value)
    {

        EffectSlider.value = value;

    }

}
