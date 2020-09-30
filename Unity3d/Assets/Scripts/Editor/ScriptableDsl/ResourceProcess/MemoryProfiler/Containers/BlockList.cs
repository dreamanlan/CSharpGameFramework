using System;
using System.Collections;
using System.Collections.Generic;

namespace Unity.MemoryProfilerForExtension.Editor.Containers
{
    internal class BlockList<T>
    {
        const int k_InitialBlockSlotsInList = 4;
        List<T[]> m_Blocks;
        int m_BlockSize;

        public long Count { get; private set; }

        int m_Capacity;

        public int Capacity
        {
            get { return m_Capacity; }
            set
            {
                int blocks = ComputeBlockCount(value, m_BlockSize);

                if (blocks > m_Blocks.Count)
                {
                    Grow(blocks - m_Blocks.Count);
                }
                else if(blocks < m_Blocks.Count)
                {
                    Shrink(m_Blocks.Count - blocks);
                }
            }
        }


        public BlockList(int blockSize) : this(blockSize, k_InitialBlockSlotsInList)
        {

        }

        static int ComputeBlockCount(long elementCount, int blockSize)
        {
            return (int)(elementCount / blockSize) + (elementCount % blockSize != 0 ? 1 : 0);
        }

        public BlockList(int blockSize, long initialCapacity)
        {
            m_BlockSize = blockSize;

            int preAllocatedBlockCount = ComputeBlockCount(initialCapacity, m_BlockSize);
            m_Capacity = preAllocatedBlockCount * m_BlockSize;
            Count = 0;
            m_Blocks = new List<T[]>(preAllocatedBlockCount > k_InitialBlockSlotsInList ? preAllocatedBlockCount : k_InitialBlockSlotsInList);

            for (int i = 0; i < preAllocatedBlockCount; ++i)
            {
                m_Blocks.Add(new T[m_BlockSize]);
            }
        }

        void Grow(int blocks)
        {
            for (int i = 0; i < blocks; ++i)
            {
                m_Blocks.Add(new T[m_BlockSize]);
            }
            m_Capacity += blocks * m_BlockSize;
        }

        void Shrink(int blocks)
        {
            for (int i = 0; i < blocks; ++i)
            {
                m_Blocks.RemoveAt(m_Blocks.Count);
            }

            m_Capacity -= blocks * m_BlockSize;
            if (Count > m_Capacity)
                Count = m_Capacity;
        }

        public void Add(T val)
        {
            var count = Count + 1;
            if (count > m_Capacity)
            {
                Grow(1);
            }
            this[Count] = val;
            Count = count;
        }

        //TODO: optimize to use purely unsafe code
        public T this[long idx]
        {
            get
            {
                return m_Blocks[(int)(idx / m_BlockSize)][idx % m_BlockSize];

            }
            set
            {
                m_Blocks[(int)(idx / m_BlockSize)][idx % m_BlockSize] = value;
            }
        }

        public void Clear()
        {
            //TODO: optimize as memset 0x0
            for (int i = 0; i < m_Blocks.Count; ++i)
            {
                var block = m_Blocks[i];
                Array.Clear(block, 0, block.Length);
            }
            Count = 0;
        }
    }

}
