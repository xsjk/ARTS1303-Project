using System.Collections.Generic;

namespace Effects
{
    public interface IEffectResult
    {
        // Return the delta of the attributes
        public Attributes ApplyAdditionEffect();

        // Return the percentage delta of the attributes
        public Attributes ApplyPercentageEffect();

        // Return the min attribute and the max attribute
        public (Attributes, Attributes) ApplyMinMaxEffect();
    }

    public class AdditionEffectResult : IEffectResult
    {
        private readonly Attributes _attributes;

        public AdditionEffectResult(Attributes attributes)
        {
            _attributes = attributes;
        }

        public Attributes ApplyAdditionEffect()
        {
            return _attributes;
        }

        public Attributes ApplyPercentageEffect()
        {
            return Attributes.One;
        }

        public (Attributes, Attributes) ApplyMinMaxEffect()
        {
            return (Attributes.NegativeInfinity, Attributes.PositiveInfinity);
        }
    }

    public class PercentageEffectResult : IEffectResult
    {
        private readonly Attributes _attributes;

        public PercentageEffectResult(Attributes attributes)
        {
            _attributes = attributes;
        }

        public Attributes ApplyAdditionEffect()
        {
            return Attributes.Zero;
        }

        public Attributes ApplyPercentageEffect()
        {
            return _attributes;
        }

        public (Attributes, Attributes) ApplyMinMaxEffect()
        {
            return (Attributes.NegativeInfinity, Attributes.PositiveInfinity);
        }
    }

    public class MinMaxEffectResult : IEffectResult
    {
        private readonly Attributes _min, _max;

        public MinMaxEffectResult(Attributes min, Attributes max)
        {
            _min = min;
            _max = max;
        }

        public Attributes ApplyAdditionEffect()
        {
            return Attributes.Zero;
        }

        public Attributes ApplyPercentageEffect()
        {
            return Attributes.One;
        }

        public (Attributes, Attributes) ApplyMinMaxEffect()
        {
            return (_min, _max);
        }
    }

    public class CombinedEffectResult : IEffectResult
    {
        private readonly Attributes _additionResult, _percentageResult, _minResult, _maxResult;

        public CombinedEffectResult(List<IEffectResult> effects)
        {
            _additionResult = Attributes.Zero;
            _percentageResult = Attributes.One;
            _minResult = Attributes.NegativeInfinity;
            _maxResult = Attributes.PositiveInfinity;

            foreach (var effect in effects)
            {
                var er = effect.ApplyAdditionEffect();
                _additionResult += er;
            }

            foreach (var effect in effects)
            {
                var er = effect.ApplyPercentageEffect();
                _percentageResult *= er;
            }

            foreach (var effect in effects)
            {
                var (min, max) = effect.ApplyMinMaxEffect();
                _minResult = Attributes.ApplyMax(_minResult, min);
                _maxResult = Attributes.ApplyMin(_maxResult, max);
            }
        }

        public Attributes ApplyAdditionEffect()
        {
            return _additionResult;
        }

        public Attributes ApplyPercentageEffect()
        {
            return _percentageResult;
        }

        public (Attributes, Attributes) ApplyMinMaxEffect()
        {
            return (_minResult, _maxResult);
        }
    }
}