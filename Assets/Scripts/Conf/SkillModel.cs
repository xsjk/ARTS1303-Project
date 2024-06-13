using UnityEngine;
using System;

[Serializable]
public class SkillModel
{
    public SkillConfig skill;
    public KeyCode KeyCode;
    public float CD;
    public bool Ready { get => Time.time - _lastReleaseTime >= CD; }
    private float _lastReleaseTime = 0;

    public void OnRelease()
    {
        _lastReleaseTime = Time.time;
    }
}
