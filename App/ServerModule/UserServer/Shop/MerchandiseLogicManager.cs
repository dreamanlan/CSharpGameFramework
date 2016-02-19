using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameFramework
{
  internal class MerchandiseLogicManager
  {
    internal IMerchandiseLogic GetMerchandisLogic(int id)
    {
      IMerchandiseLogic logic = null;
      m_MerchandiseLogics.TryGetValue(id, out logic);
      return logic;
    }
    private MerchandiseLogicManager()
    {
      //这里初始化所有的商品逻辑，并记录到对应的列表
    }
    private Dictionary<int, IMerchandiseLogic> m_MerchandiseLogics = new Dictionary<int, IMerchandiseLogic>();

    internal static MerchandiseLogicManager Instance
    {
      get { return s_Instance; }
    }
    private static MerchandiseLogicManager s_Instance = new MerchandiseLogicManager();
  }
}
