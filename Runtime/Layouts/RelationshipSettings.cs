namespace UIFlow.Runtime.Layouts
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using UnityEngine;

    [Serializable]
    public sealed class RelationshipSettings
    {
        [SerializeField]
        private RelationshipColumn[] m_Matrix = new RelationshipColumn[LayoutTable.MaxLayoutCount];
        
        private bool[,] m_MatrixCache;
        
        public bool[,] Matrix
        {
            get
            {
                if (m_MatrixCache == null || m_MatrixCache.Length == 0)
                    m_MatrixCache = CreateMatrix();

                return m_MatrixCache;
            }
            set => UpdateFromMatrix(value);
        }

        public UILayoutMask GetMask(int index)
        {
            var mask = new UILayoutMask(0);

            for (var i = 0; i < LayoutTable.MaxLayoutCount; i++)
            {
                if(Matrix[index, i])
                    mask |= 1 << i;
            }
            
            return mask;
        }

        public bool Get(int a, int b)
        {
            return Matrix[a, b];
        }

        public void Set(int a, int b, bool value)
        {
            m_MatrixCache[a, b] = value;
            m_Matrix[b].m_Row[a] = value;
        }
        
        public void Clear()
        {
            if (m_MatrixCache == null)
                return;
            
            for (var x = 0; x < LayoutTable.MaxLayoutCount; x++)
            {
                for (var y = 0; y < LayoutTable.MaxLayoutCount; y++)
                {
                    m_MatrixCache[x, y] = false;
                }
            }

            UpdateFromMatrix(m_MatrixCache);
        }

        private bool[,] CreateMatrix()
        {
            var matrix = new bool[LayoutTable.MaxLayoutCount, LayoutTable.MaxLayoutCount];
            for (var y = 0; y < LayoutTable.MaxLayoutCount; y++)
            {
                if(m_Matrix[y] == null)
                    m_Matrix[y] = new RelationshipColumn();
            
                var row = m_Matrix[y].m_Row;
                for (var x = 0; x < LayoutTable.MaxLayoutCount; x++)
                {
                    matrix[x, y] = row[x];
                }
            }
            
            return matrix;
        }

        private void UpdateFromMatrix([NotNull]bool[,] matrix)
        {
            for (var y = 0; y < LayoutTable.MaxLayoutCount; y++)
            {
                for (var x = 0; x < LayoutTable.MaxLayoutCount; x++)
                {
                    var value = matrix[x, y];
                    m_Matrix[y].m_Row[x] = value;
                }
            }
            
            m_MatrixCache = matrix;
        }
        
        [Serializable]
        private sealed class RelationshipColumn
        {
            public bool[] m_Row = new bool[LayoutTable.MaxLayoutCount];
        }
    }
}