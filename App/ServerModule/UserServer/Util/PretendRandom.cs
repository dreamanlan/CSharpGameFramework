using System;
using System.Collections.Generic;

namespace GameFramework
{
  internal class PretendRandom<T> where T : IComparable
  {
    internal class Item
    {
      internal T Id;
      internal int OrigWeight;
      internal int CurWeight;
    }

    internal List<Item> Items
    {
      get { return items_; }
      set { items_ = value; }
    }

    internal void AddPart(T id, int count, int orig_count = -1)
    {
      Item item = new Item();
      item.Id = id;
      item.CurWeight = count;
      if (orig_count > 0) {
        item.OrigWeight = orig_count;
      } else {
        item.OrigWeight = count;
      }
      items_.Add(item);
    }

    internal void SetPartOrigCount(T id, int orig_count)
    {
      for (int i = 0; i < items_.Count; i++) {
        if (items_[i].Id.CompareTo(id) == 0) {
          items_[i].OrigWeight = orig_count;
          return;
        }
      }
    }

    internal void Clear()
    {
      items_.Clear();
    }

    internal int GetLeftCount()
    {
      int left = 0;
      for (int i = 0; i < items_.Count; i++) {
        left += items_[i].CurWeight;
      }
      return left;
    }

    internal T GetRandom()
    {
      if (items_.Count == 0) {
        return default(T);
      }
      int left_count = GetLeftCount();
      if (left_count == 0) {
        Reset();
        left_count = GetLeftCount();
      }
      Item random_item = GetRandomItem(left_count);
      if (random_item != null) {
        return random_item.Id;
      }
      return default(T);
    }

    private Item GetRandomItem(int max_index)
    {
      int random_index = random_.Next(0, max_index);
      int cur_index = 0;
      for (int i = 0; i < items_.Count; i++) {
        if (items_[i].CurWeight > 0) {
          if (cur_index <= random_index && random_index < (cur_index + items_[i].CurWeight)) {
            //Console.Write(string.Format("[b={0},r={1},e={2},i={3},m={4}]", cur_index, random_index, cur_index + items_[i].CurCount, i, max_index));
            --items_[i].CurWeight;
            return items_[i];
          }
          cur_index += items_[i].CurWeight;
        }
      }
      return null;
    }

    private void Reset()
    {
      for (int i = 0; i < items_.Count; ++i) {
        items_[i].CurWeight = items_[i].OrigWeight;
      }
    }

    private Random random_ = new Random();
    private List<Item> items_ = new List<Item>();
  }
}
