using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DamageHandler : MonoBehaviour
{
    public Action<float, Transform, Vector3,float, float> action;
    public void Take(float hardTime, Transform souceTransform, Vector3 repelVelocity, float repelTransitionTime, float damgeValue)
    {
        action(hardTime, souceTransform, repelVelocity, repelTransitionTime, damgeValue);
    }
}
