using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Effects
{
    public class EntityEffect : MonoBehaviour
    {
        private Attributes _baseAttributes;

        public Attributes BaseAttributes
        {
            get => _baseAttributes;
            set
            {
                _baseAttributes = value;
                ComputeAttributes();
            }
        }

        public Attributes ComputedAttributes { get; private set; }
        private Dictionary<string, IEffect> _effects;

        public void AddEffect(IEffect effect)
        {
            if (!_effects.TryAdd(effect.Name, effect))
            {
                _effects[effect.Name] = effect;
                return;
            }

            ComputeAttributes();
        }

        public void Update()
        {
            var newEffects = _effects.Values.Select(e => (e, e.EffectEnded(Time.deltaTime)))
                .Where(t => !t.Item2)
                .Select(t => t.Item1)
                .ToDictionary(e => e.Name);
            if (_effects.Count != newEffects.Count)
            {
                ComputeAttributes();
            }

            _effects = newEffects;
        }

        private void ComputeAttributes()
        {
            var effectResults = _effects.Values.Select(e => e.PerformEffect()).ToList();
            var composedEffectResult = new CombinedEffectResult(effectResults);
            ComputedAttributes = composedEffectResult.ApplyAdditionEffect(_baseAttributes);
        }
    }
}