using System.Collections.Generic;
using System.Linq;

namespace Items
{
    public struct ItemSample
    {
        public IItem Item;
        public float Weight;
        public IItemPoolTreeNode Node;
    }

    public interface IItemPoolTreeNode
    {
        public bool EnableSampleSelf();
        public bool EnableSampleChildren();
        public ItemSample GetItem();
        public void SampleChildren(List<ItemSample> acc);

        public void Sample(List<ItemSample> acc)
        {
            if (EnableSampleSelf())
            {
                acc.Add(GetItem());
            }

            if (EnableSampleChildren())
            {
                SampleChildren(acc);
            }
        }

        public void OnSampledCallback();
        public int SampledCount();
    }

    internal class ItemPoolTreeNodeLeaf : IItemPoolTreeNode
    {
        private readonly IItem _item;

        // Weight is used to determine the probability of this item being selected
        // Must between 0 and 1
        private readonly float _weight;
        private int _spawnCount;
        private readonly bool _isUnique;

        public ItemPoolTreeNodeLeaf(bool isUnique, IItem item, float weight)
        {
            _item = item;
            _weight = weight;
            _isUnique = isUnique;
        }

        public bool EnableSampleSelf()
        {
            return !_isUnique || _spawnCount == 0;
        }

        public bool EnableSampleChildren()
        {
            return false;
        }

        public ItemSample GetItem()
        {
            return new ItemSample
            {
                Item = _item,
                Weight = _weight,
                Node = this
            };
        }

        public void SampleChildren(List<ItemSample> acc)
        {
        }

        public void OnSampledCallback()
        {
            _spawnCount += 1;
        }

        public int SampledCount()
        {
            return _spawnCount;
        }
    }

    internal class ItemPoolTreeNodeBranch : IItemPoolTreeNode
    {
        private readonly List<IItemPoolTreeNode> _children;
        private readonly IItemPoolTreeNode _self;
        private readonly bool _spawnChildrenOnlyAfterSelfIsSpawned;

        public ItemPoolTreeNodeBranch(IItemPoolTreeNode self, List<IItemPoolTreeNode> children,
            bool spawnChildrenOnlyAfterSelfIsSpawned)
        {
            _children = children;
            _self = self;
            _spawnChildrenOnlyAfterSelfIsSpawned = spawnChildrenOnlyAfterSelfIsSpawned;
        }

        public bool EnableSampleSelf()
        {
            return _self?.EnableSampleSelf() ?? false;
        }

        public bool EnableSampleChildren()
        {
            return !_spawnChildrenOnlyAfterSelfIsSpawned || _self.SampledCount() > 0;
        }

        public ItemSample GetItem()
        {
            return _self.GetItem();
        }

        public void SampleChildren(List<ItemSample> acc)
        {
            var localAcc = new List<ItemSample>();
            foreach (var child in _children)
            {
                child.Sample(localAcc);
            }

            acc.AddRange(localAcc.Select(x => new ItemSample
            {
                Item = x.Item,
                Weight = x.Weight / localAcc.Count,
                Node = x.Node
            }));
        }

        public void OnSampledCallback()
        {
        }

        public int SampledCount()
        {
            return _self.SampledCount();
        }
    }

    public class ItemPool
    {
        private readonly IItemPoolTreeNode _root;

        public ItemPool(IItemPoolTreeNode root)
        {
            _root = root;
        }

        public IItem Sample()
        {
            var acc = new List<ItemSample>();
            _root.Sample(acc);
            var total = acc.Aggregate(0f, (a, itemSample) => itemSample.Weight + a);
            var sampleValue = Rng.Rand.NextDouble() * total;
            var sample = acc.First(x =>
            {
                sampleValue -= x.Weight;
                return sampleValue <= 0;
            });
            sample.Node.OnSampledCallback();
            return sample.Item;
        }
    }
}