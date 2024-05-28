namespace Libraries
{
    class DisjointSet
    {
        private readonly int[] _parent;

        public DisjointSet(int n)
        {
            _parent = new int[n];
            for (int i = 0; i < n; i++)
            {
                _parent[i] = i;
            }
        }

        public int Find(int i)
        {
            if (i != _parent[i])
            {
                _parent[i] = Find(_parent[i]);
            }

            return _parent[i];
        }

        public void Union(int i, int j)
        {
            _parent[Find(i)] = Find(j);
        }
    }
}