using System;
using System.Collections.Generic;
using System.Text;

namespace GameFramework
{
    public static class TableConfigUtility
    {
        public static int GenStoryDlgItemId(int dlgId, int index)
        {
            return dlgId * 100 + index;
        }
        public static int GenLevelMonstersId(int sceneId, int campId)
        {
            return sceneId * 10 + campId;
        }
    }
}
