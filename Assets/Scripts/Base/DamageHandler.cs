using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Effects;

public class DamageHandler : MonoBehaviour
{
    public Action<float, Transform, Vector3,float, IEffectResult> action;
    public void Take(float hardTime, Transform souceTransform, Vector3 repelVelocity, float repelTransitionTime, IEffectResult effectResult)
    {
        action(hardTime, souceTransform, repelVelocity, repelTransitionTime, effectResult);
    }
}
