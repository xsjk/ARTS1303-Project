using UnityEngine;
using Effects;

[CreateAssetMenu(fileName = "Hit Effect Configuration", menuName = "Config/Hit Effect")]
public class HitEffect : ScriptableObject
{
    public SpawnConfig spawn;
    public AudioClip sound;
}
