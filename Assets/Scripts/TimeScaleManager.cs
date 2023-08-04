using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScaleManager : MonoBehaviour
{
    public static TimeScaleManager current;

    public float TransitionSpeed = 20;

    private float targetSpeed = 1;
    private float prevSpeed = 1;

    private void Awake()
    {
        if (current != null) Debug.LogWarning("Oops! it looks like there might already be a " + GetType().Name + " in this scene!");
        current = this;
    }

    private void Update()
    {
        if (Time.timeScale == targetSpeed) return;
        if (targetSpeed < prevSpeed)
        {
            Time.timeScale = Mathf.Max(Time.timeScale - (Time.unscaledDeltaTime * TransitionSpeed), targetSpeed);
        }
        else 
        {
            Time.timeScale = Mathf.Min(Time.timeScale + (Time.unscaledDeltaTime * TransitionSpeed), targetSpeed);
        }
    }

    public void TransitionToTimeScale(float targetSpeed) 
    {
        this.targetSpeed = targetSpeed;
        prevSpeed = Time.timeScale;
    }
}
