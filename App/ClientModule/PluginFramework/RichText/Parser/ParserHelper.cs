using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using RichTextParser;

internal struct ParserValue
{
    internal IRichText TextValue;
    internal IRichTextList TextValues;
    internal HyperTextAttr AttrValue;
    internal HyperTextAttrList AttrValues;
    internal string StringValue;
}