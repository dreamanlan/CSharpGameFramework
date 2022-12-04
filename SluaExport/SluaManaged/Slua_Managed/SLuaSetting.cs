﻿// The MIT License (MIT)

// Copyright 2015 Siney/Pangweiwei siney@yeah.net
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using System.Collections;
using UnityEngine;

namespace SLua
{
    public enum EOL
    {
        Native,
        CRLF,
        CR,
        LF,
    }

    public class SLuaSetting : ScriptableObject
    {
        public EOL eol = EOL.Native;
        public bool exportExtensionMethod = true;
        public string UnityEngineGeneratePath = "../SluaExport/LuaObject/";
        public bool IsDebug = false;
        public bool RecordObjectStackTrace = false;

        public static SLuaSetting Instance
        {
            get {
                if (_instance == null) {
                    _instance = Resources.Load<SLuaSetting>("setting");
                }
                return _instance;
            }
            set {
                _instance = value;
            }
        }
        public static bool IsEditor
        {
            get { return _isEditor; }
            set { _isEditor = value; }
        }
        public static bool IsPlaying
        {
            get { return _isPlaying; }
            set { _isPlaying = value; }
        }

        private static SLuaSetting _instance = null;
        private static bool _isEditor = false;
        private static bool _isPlaying = true;
    }
}
