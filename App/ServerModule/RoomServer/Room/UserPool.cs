using RoomServer;

namespace GameFramework
{
  internal class UserPool
  {
    internal UserPool()
    {
      pool_size_ = 0;
      cur_position_ = 0;
      free_size_ = 0;
    }

    internal bool Init(uint pool_size)
    {
      pool_size_ = pool_size;
      free_size_ = pool_size_;
      data_pool_ = new User[pool_size];
      for (uint i = 0; i < pool_size; ++i) {
        data_pool_[i] = new User();
        data_pool_[i].LocalID = i;
        data_pool_[i].IsIdle = true;
      }
      this_lock_ = new object();
      return true;
    }

    internal User NewUser()
    {
      lock (this_lock_) {
        if (free_size_ == 0) {
          return null;
        }

        uint ret_index = 0;
        for (int i = 0; i < pool_size_; ++i) {
          if (data_pool_[cur_position_].IsIdle) {
            ret_index = cur_position_;
            data_pool_[cur_position_].IsIdle = false;
            cur_position_ = ++cur_position_ % pool_size_;
            free_size_--;
            return data_pool_[ret_index];
          }
          cur_position_ = ++cur_position_ % pool_size_;
        }
      }
      return null;
    }

    internal bool FreeUser(uint localid)
    {
      lock (this_lock_) {
        if (localid >= pool_size_) {
          return false;
        }

        data_pool_[localid].Reset();
        data_pool_[localid].IsIdle = true;
        ++free_size_;
      }
      return true;
    }

    internal int GetUsedCount()
    {
      return (int)(pool_size_ - free_size_);
    }

    private uint pool_size_;
    private uint cur_position_;
    private uint free_size_;
    private User[] data_pool_;
    private object this_lock_;
  }

} // namespace 
