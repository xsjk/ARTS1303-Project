using UnityEngine;

namespace Effects
{
    public interface IEffect
    {
        public bool Hidden { get; }
        public string Name { get; }
        public Texture Icon { get; }
        public IEffectResult PerformEffect();
        public bool EffectEnded(float deltaTime);
    }
}