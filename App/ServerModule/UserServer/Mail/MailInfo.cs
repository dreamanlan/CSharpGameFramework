using System;
using System.Collections.Generic;
using ScriptableFramework;
using GameFrameworkData;
using GameFrameworkMessage;

namespace ScriptableFramework
{
    public sealed class MailItem
    {
        internal int m_ItemId;
        internal int m_ItemNum;
    }
    public sealed partial class TableMailInfoWrap
    {
        internal List<MailItem> m_Items = new List<MailItem>();
    }
}
