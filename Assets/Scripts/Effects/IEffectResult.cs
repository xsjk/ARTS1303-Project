using System.Collections.Generic;

namespace Effects
{
    public interface IEffectResult
    {
        // Return the delta of the attributes
        public Attributes ApplyAdditionEffect(Attributes attributes);

        // Return the percentage delta of the attributes
        public Attributes ApplyPercentageEffect(Attributes attributes);

        // Return the new attributes with the min and max values applied
        public Attributes ApplyMinMaxEffect(Attributes attributes);
    }

    public class AdditionEffectResult : IEffectResult
    {
        private readonly Attributes _attributes;

        public AdditionEffectResult(Attributes attributes)
        {
            _attributes = attributes;
        }

        public Attributes ApplyAdditionEffect(Attributes attributes)
        {
            return _attributes;
        }

        public Attributes ApplyPercentageEffect(Attributes attributes)
        {
            return Attributes.One;
        }

        public Attributes ApplyMinMaxEffect(Attributes attributes)
        {
            return attributes;
        }
    }

    public class PercentageEffectResult : IEffectResult
    {
        private readonly Attributes _attributes;

        public PercentageEffectResult(Attributes attributes)
        {
            _attributes = attributes;
        }

        public Attributes ApplyAdditionEffect(Attributes attributes)
        {
            return Attributes.Zero;
        }

        public Attributes ApplyPercentageEffect(Attributes attributes)
        {
            return _attributes;
        }

        public Attributes ApplyMinMaxEffect(Attributes attributes)
        {
            return attributes;
        }
    }

    public class MinMaxEffectResult : IEffectResult
    {
        private readonly Attributes _attributes;

        public MinMaxEffectResult(Attributes attributes)
        {
            _attributes = attributes;
        }

        public Attributes ApplyAdditionEffect(Attributes attributes)
        {
            return Attributes.Zero;
        }

        public Attributes ApplyPercentageEffect(Attributes attributes)
        {
            return Attributes.One;
        }

        public Attributes ApplyMinMaxEffect(Attributes attributes)
        {
            return _attributes;
        }
    }

    public class CombinedEffectResult : IEffectResult
    {
        private readonly List<IEffectResult> _effects;

        public CombinedEffectResult(List<IEffectResult> effects)
        {
            _effects = effects;
        }

        public CombinedEffectResult(params IEffectResult[] effects)
        {
            _effects = new List<IEffectResult>(effects);
        }

        public Attributes ApplyAdditionEffect(Attributes attributes)
        {
            Attributes result = Attributes.Zero;
            foreach (IEffectResult effect in _effects)
            {
                var e = effect.ApplyAdditionEffect(attributes);
                result += e;
            }

            return result;
        }

        public Attributes ApplyPercentageEffect(Attributes attributes)
        {
            Attributes result = Attributes.One;
            foreach (IEffectResult effect in _effects)
            {
                var e = effect.ApplyPercentageEffect(attributes);
                result += e;
            }

            return result;
        }

        public Attributes ApplyMinMaxEffect(Attributes attributes)
        {
            Attributes result = attributes;
            foreach (IEffectResult effect in _effects)
            {
                var e = effect.ApplyMinMaxEffect(result);
                result = e;
            }

            return result;
        }
    }
}