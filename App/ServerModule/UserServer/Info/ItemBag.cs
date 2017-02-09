using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFrameworkData;

namespace GameFramework
{
    public class ItemBag
    {
        public List<ItemInfo> ItemInfos
        {
            get { return m_ItemData; }
        }
        public int ItemCount
        {
            get
            {
                return m_ItemData.Count;
            }
        }
        public List<ItemInfo> DeletedItemInfos
        {
            get { return m_DeletedItemData; }
        }
        public bool AddItemData(int itemId, int num)
        {
            if (null != m_ItemData) {
                int ct = m_ItemData.Count;
                for (int i = 0; i < ct; i++) {
                    if (null != m_ItemData[i] && itemId == m_ItemData[i].ItemId) {
                        m_ItemData[i].ItemNum += num;
                        return true;
                    }
                }
                if (ct < c_MaxItemNum) {
                    ItemInfo info = new ItemInfo();
                    info.ItemGuid = UserServer.Instance.GlobalProcessThread.GenerateItemGuid();
                    info.ItemId = itemId;
                    info.ItemNum = num;
                    m_ItemData.Add(info);
                    return true;
                }
            }
            return false;
        }
        public void DelItemData(int itemId)
        {
            if (null != m_ItemData) {
                for (int i = m_ItemData.Count - 1; i >= 0; i--) {
                    if (m_ItemData[i].ItemId == itemId) {
                        m_ItemData[i].Deleted = true;
                        m_DeletedItemData.Add(m_ItemData[i]);
                        m_ItemData.RemoveAt(i);
                        break;
                    }
                }
            }
        }
        public void DelItemData(ulong itemGuid)
        {
            if (null != m_ItemData) {
                for (int i = m_ItemData.Count - 1; i >= 0; i--) {
                    if (m_ItemData[i].ItemGuid == itemGuid) {
                        m_ItemData[i].Deleted = true;
                        m_DeletedItemData.Add(m_ItemData[i]);
                        m_ItemData.RemoveAt(i);
                        break;
                    }
                }
            }
        }
        public void ReduceItemData(int itemId, int num)
        {
            if (null != m_ItemData) {
                for (int i = m_ItemData.Count - 1; i >= 0; i--) {
                    if (m_ItemData[i].ItemId == itemId) {
                        if (m_ItemData[i].ItemNum > num) {
                            m_ItemData[i].ItemNum -= num;
                            break;
                        } else {
                            num -= m_ItemData[i].ItemNum;
                            m_DeletedItemData.Add(m_ItemData[i]);
                            m_ItemData.RemoveAt(i);
                        }
                    }
                }
            }
        }
        public ItemInfo GetItemData(ulong guid)
        {
            for (int i = 0; i < m_ItemData.Count; i++) {
                if (m_ItemData[i] != null && m_ItemData[i].ItemGuid == guid) {
                    return m_ItemData[i];
                }
            }
            return null;
        }
        public ItemInfo GetItemData(int itemId)
        {
            ItemInfo itemInfo = m_ItemData.Find(delegate(ItemInfo p) { return (p.ItemId == itemId); });
            return itemInfo;
        }
        public int GetItemNum(int item_id)
        {
            int nItemCount = 0;
            if (null == m_ItemData)
                return nItemCount;
            for (int i = 0; i < m_ItemData.Count; ++i) {
                if (m_ItemData[i].ItemId == item_id) {
                    nItemCount += m_ItemData[i].ItemNum;
                }
            }
            return nItemCount;
        }
        public int GetFreeCount()
        {
            int nItemCount = 0;
            if (null != m_ItemData) {
                nItemCount = m_ItemData.Count;
            }
            return c_MaxItemNum - nItemCount;
        }
        public void ResetItemData()
        {
            m_ItemData.Clear();
        }
        public void Reset()
        {
            ResetItemData();
        }

        public const int c_MaxItemNum = 128;

        private List<ItemInfo> m_ItemData = new List<ItemInfo>();
        private List<ItemInfo> m_DeletedItemData = new List<ItemInfo>();
    }
}
