using System.Collections.Generic;
using System;
using System.Linq;
using System.Reflection;
#if !SLUA_STANDALONE
using UnityEngine;
#endif
using System.IO;

namespace SLua
{

    public class Lua3rdMeta
#if !SLUA_STANDALONE
 : ScriptableObject
#endif
    {
        /// <summary>
        ///Cache class types here those contain 3rd dll attribute.
        /// </summary>
        public List<string> typesWithAttribtues = new List<string>();

        void OnEnable()
        {
#if !SLUA_STANDALONE
            this.hideFlags = HideFlags.NotEditable;
#endif
        }
        private static Lua3rdMeta _instance = null;
        public static Lua3rdMeta Instance
        {
            get
            {
#if !SLUA_STANDALONE
                if (_instance == null) {
                    _instance = Resources.Load<Lua3rdMeta>("lua3rdmeta");
                }
#endif
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }
    }
}
