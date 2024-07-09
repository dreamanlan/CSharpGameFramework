namespace ScriptableFramework
{

    public class RoomPool
    {
        public RoomPool()
        {
            m_PoolSize = 0;
            m_CurPosition = 0;
            m_FreeSize = 0;
        }

        public bool Init(uint pool_size)
        {
            m_PoolSize = pool_size;
            m_FreeSize = m_PoolSize;
            m_RoomPool = new Room[pool_size];
            for (uint i = 0; i < pool_size; ++i) {
                m_RoomPool[i] = new Room();
                m_RoomPool[i].LocalID = i;
                m_RoomPool[i].IsIdle = true;
            }
            return true;
        }

        public Room NewRoom()
        {
            if (m_FreeSize == 0) {
                return null;
            }

            uint ret_index = 0;
            for (int i = 0; i < m_PoolSize; ++i) {
                if (m_RoomPool[m_CurPosition].IsIdle) {
                    ret_index = m_CurPosition;
                    m_RoomPool[m_CurPosition].IsIdle = false;
                    m_CurPosition = ++m_CurPosition % m_PoolSize;
                    m_FreeSize--;
                    return m_RoomPool[ret_index];
                }
                m_CurPosition = ++m_CurPosition % m_PoolSize;
            }
            return null;
        }

        public bool FreeRoom(uint localid)
        {
            if (localid >= m_PoolSize) {
                return false;
            }
            m_RoomPool[localid].IsIdle = true;
            ++m_FreeSize;
            return true;
        }

        public Room GetRoomByLocalId(int localId)
        {
            Room room = null;
            if (localId >= 0 && localId < m_PoolSize) {
                room = m_RoomPool[localId];
            }
            return room;
        }

        private uint m_PoolSize;
        private uint m_CurPosition;
        private uint m_FreeSize;
        private Room[] m_RoomPool;
    }

}
