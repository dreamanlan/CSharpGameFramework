using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ScriptableFramework
{
    public abstract class AbstractScriptBehavior : MonoBehaviour
    {
        public bool ResourceEnabled
        {
            get { return m_ResourceEnabled; }
            set 
            { 
                m_ResourceEnabled = value;
                OnResourceEnabled(value);
            }
        }

        protected abstract void OnResourceEnabled(bool _enabled);

        private bool m_ResourceEnabled = false;
    }
}
