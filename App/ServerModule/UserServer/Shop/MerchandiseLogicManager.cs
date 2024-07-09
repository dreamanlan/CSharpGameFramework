using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScriptableFramework
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
            //All product logic is initialized here and recorded to the corresponding list.
        }
        private Dictionary<int, IMerchandiseLogic> m_MerchandiseLogics = new Dictionary<int, IMerchandiseLogic>();

        internal static MerchandiseLogicManager Instance
        {
            get { return s_Instance; }
        }
        private static MerchandiseLogicManager s_Instance = new MerchandiseLogicManager();
    }
}
