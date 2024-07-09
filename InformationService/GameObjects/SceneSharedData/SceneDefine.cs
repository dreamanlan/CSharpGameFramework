using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScriptableFramework
{
    public enum SceneOperationResultEnum
    {
        Success = 0,
        Cant_Find_Room,
        Cant_Find_User,
        User_Key_Exist,
        Not_Field_Room,
    }
    public enum SceneTypeEnum
    {
        Unclassified = 0,
        MainUi,
        Story,
        Activity,
        Battle,
    }
}
