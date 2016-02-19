namespace GameFramework
{

  internal class RoomPool
  {
    internal RoomPool()
    {
      pool_size_ = 0;
      cur_position_ = 0;
      free_size_ = 0;
    }

    internal bool Init(uint pool_size)
    {
      pool_size_ = pool_size;
      free_size_ = pool_size_;
      room_pool_ = new Room[pool_size];
      for (uint i = 0; i < pool_size; ++i) {
        room_pool_[i] = new Room();
        room_pool_[i].LocalID = i;
        room_pool_[i].IsIdle = true;
      }
      return true;
    }

    internal Room NewRoom()
    {
      if (free_size_ == 0) {
        return null;
      }

      uint ret_index = 0;
      for (int i = 0; i < pool_size_; ++i) {
        if (room_pool_[cur_position_].IsIdle) {
          ret_index = cur_position_;
          room_pool_[cur_position_].IsIdle = false;
          cur_position_ = ++cur_position_ % pool_size_;
          free_size_--;
          return room_pool_[ret_index];
        }
        cur_position_ = ++cur_position_ % pool_size_;
      }
      return null;
    }

    internal bool FreeRoom(uint localid)
    {
      if (localid >= pool_size_) {
        return false;
      }      
      room_pool_[localid].IsIdle = true;
      ++free_size_;
      return true;
    }

    private uint pool_size_;
    private uint cur_position_;
    private uint free_size_;
    private Room[] room_pool_;
  }

}
