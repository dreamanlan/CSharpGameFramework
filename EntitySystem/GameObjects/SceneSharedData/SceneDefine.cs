using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameFramework
{
    public enum SceneOperationResultEnum
    {
        Success = 0,
        Cant_Find_Room,
        Cant_Find_User,
        User_Key_Exist,
    }
    public enum SceneTypeEnum
    {
        Unclassified = 0,
        MainUi,
        Battle,
        Story,
        Room,
    }
}
