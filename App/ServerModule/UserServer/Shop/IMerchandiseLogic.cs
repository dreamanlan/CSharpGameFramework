using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameFramework
{
  internal enum MerchandiseLogicId : int
  {
    Invalid = 0,
    MaxNum,
  }
  internal interface IMerchandiseLogic
  {
    bool CanBuy(UserInfo user, int merchandiseId);
    void Buy(UserInfo user, int merchandiseId);
  }
}
