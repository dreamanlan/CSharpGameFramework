using ScriptableFramework;

namespace ScriptableFramework
{
  public class UserPool
  {
    public UserPool()
    {
      m_PoolSize = 0;
      m_CurPosition = 0;
      m_FreeSize = 0;
    }

    public bool Init(uint pool_size)
    {
      m_PoolSize = pool_size;
      m_FreeSize = m_PoolSize;
      m_DataPool = new User[pool_size];
      for (uint i = 0; i < pool_size; ++i) {
        m_DataPool[i] = new User();
        m_DataPool[i].LocalID = i;
        m_DataPool[i].IsIdle = true;
      }
      m_Lock = new object();
      return true;
    }

    public User NewUser()
    {
      lock (m_Lock) {
        if (m_FreeSize == 0) {
          return null;
        }

        uint ret_index = 0;
        for (int i = 0; i < m_PoolSize; ++i) {
          if (m_DataPool[m_CurPosition].IsIdle) {
            ret_index = m_CurPosition;
            m_DataPool[m_CurPosition].IsIdle = false;
            m_CurPosition = ++m_CurPosition % m_PoolSize;
            m_FreeSize--;
            return m_DataPool[ret_index];
          }
          m_CurPosition = ++m_CurPosition % m_PoolSize;
        }
      }
      return null;
    }

    public bool FreeUser(uint localid)
    {
      lock (m_Lock) {
        if (localid >= m_PoolSize) {
          return false;
        }

        m_DataPool[localid].Reset();
        m_DataPool[localid].IsIdle = true;
        ++m_FreeSize;
      }
      return true;
    }

    public int GetUsedCount()
    {
      return (int)(m_PoolSize - m_FreeSize);
    }

    private uint m_PoolSize;
    private uint m_CurPosition;
    private uint m_FreeSize;
    private User[] m_DataPool;
    private object m_Lock;
  }

} // namespace 
