using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RichTextParser
{
    public sealed class IRichTextList : List<IRichText>
    {
        public IRichTextList() { }
        public IRichTextList(IEnumerable<IRichText> coll) : base(coll) { }
    }
    public sealed class HyperTextAttrList : List<HyperTextAttr>
    {
        public HyperTextAttrList() { }
        public HyperTextAttrList(IEnumerable<HyperTextAttr> coll) : base(coll) { }
    }
    public enum RichTextType
    {
        Normal = 0,
        Hyper,
    }
    public interface IRichText
    {
        RichTextType Type { get; }
    }
    public sealed class NormalText : IRichText
    {
        public RichTextType Type { get { return RichTextType.Normal; } }

        public string Text {
            get { return m_Text; }
            set { m_Text = value; }
        }

        private string m_Text = string.Empty;
    }
    public sealed class HyperTextAttr
    {
        public string Key {
            get { return m_Key; }
            set { m_Key = value; }
        }
        public string Value {
            get { return m_Value; }
            set { m_Value = value; }
        }

        private string m_Key = string.Empty;
        private string m_Value = string.Empty;
    }
    public sealed class HyperText : IRichText
    {
        public RichTextType Type { get { return RichTextType.Hyper; } }

        public HyperTextAttrList Attrs {
            get { return m_Attrs; }
        }
        public IRichTextList Texts {
            get { return m_Texts; }
        }

        private HyperTextAttrList m_Attrs = new HyperTextAttrList();
        private IRichTextList m_Texts = new IRichTextList();
    }
    public sealed class RichTextParser
    {
        public IRichTextList Texts
        {
            get { return m_Texts; }
        }
        public void Parse(string txt)
        {
            var val = m_Parser.Parse(txt);
            m_Texts = val.TextValues;
        }

        private IRichTextList m_Texts = null;
        private Parser m_Parser = new Parser();
    }
}
