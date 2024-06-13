using UnityEngine;
using System;

[Serializable]
public class SkillModel
{
    public SkillConfig skill;
    public KeyCode KeyCode;
    public float CoolDownTime;
    public float curTime = 0;

    public bool canRelease { get; private set; } = true;
    public void Update()
    {
        if (canRelease == false && curTime > 0)
        {
            curTime -= Time.deltaTime;
            if(curTime <= 0)
            {
                curTime = 0;
                canRelease = true;
            }
        }
    }

    public void OnRelease()
    {
        canRelease = false;
        curTime = CoolDownTime;
    }



}
