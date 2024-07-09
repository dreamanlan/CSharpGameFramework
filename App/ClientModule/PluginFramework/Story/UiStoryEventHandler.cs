using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using ScriptableFramework;
using ScriptableFramework.Story;
using DotnetStoryScript;

namespace ScriptableFramework.Story
{
    internal class UiStoryEventHandler : MonoBehaviour
    {
        internal void OnInputHandler(string tag, string val)
        {
            BoxedValueList args = GfxStorySystem.Instance.NewBoxedValueList();
            args.Add(tag);
            args.Add(val);
            BuildInputArgs(args);
            GfxStorySystem.Instance.SendMessage(string.Format("{0}:on_input", WindowName), args);
        }
        internal void OnSliderHandler(string tag, float val)
        {
            BoxedValueList args = GfxStorySystem.Instance.NewBoxedValueList();
            args.Add(tag);
            args.Add(val);
            BuildInputArgs(args);
            GfxStorySystem.Instance.SendMessage(string.Format("{0}:on_slider", WindowName), args);
        }
        internal void OnDropdownHandler(string tag, int val)
        {
            BoxedValueList args = GfxStorySystem.Instance.NewBoxedValueList();
            args.Add(tag);
            args.Add(val);
            BuildInputArgs(args);
            GfxStorySystem.Instance.SendMessage(string.Format("{0}:on_dropdown", WindowName), args);
        }
        internal void OnToggleHandler(string tag, bool val)
        {
            BoxedValueList args = GfxStorySystem.Instance.NewBoxedValueList();
            args.Add(tag);
            args.Add(val ? 1 : 0);
            BuildInputArgs(args);
            GfxStorySystem.Instance.SendMessage(string.Format("{0}:on_toggle", WindowName), args);
        }
        internal void OnClickHandler(string tag)
        {
            BoxedValueList args = GfxStorySystem.Instance.NewBoxedValueList();
            args.Add(tag);
            BuildInputArgs(args);
            GfxStorySystem.Instance.SendMessage(string.Format("{0}:on_click", WindowName), args);
        }

        private void BuildInputArgs(BoxedValueList args)
        {
            string[] strVals = null;
            int[] boolVals = null;
            float[] floatVals = null;
            int[] dropdownVals = null;
            if (null != InputLabels) {
                int strCt = InputLabels.Count;
                strVals = new string[strCt];
                for (int i = 0; i < strCt; ++i) {
                    strVals[i] = InputLabels[i].text;
                }
                if (strCt > 0) {
                    args.Add(BoxedValue.FromObject(strVals));
                }
            }
            if (null != InputToggles) {
                int boolCt = InputToggles.Count;
                boolVals = new int[boolCt];
                for (int i = 0; i < boolCt; ++i) {
                    boolVals[i] = InputToggles[i].isOn ? 1 : 0;
                }
                if (boolCt > 0) {
                    args.Add(BoxedValue.FromObject(boolVals));
                }
            }
            if (null != InputSliders) {
                int floatCt = InputSliders.Count;
                floatVals = new float[floatCt];
                for (int i = 0; i < floatCt; ++i) {
                    floatVals[i] = InputSliders[i].value;
                }
                if (floatCt > 0) {
                    args.Add(BoxedValue.FromObject(floatVals));
                }
            }
            if (null != InputDropdowns) {
                int dropdownCt = InputDropdowns.Count;
                dropdownVals = new int[dropdownCt];
                for (int i = 0; i < dropdownCt; ++i) {
                    dropdownVals[i] = InputDropdowns[i].value;
                }
                if (dropdownCt > 0) {
                    args.Add(BoxedValue.FromObject(dropdownVals));
                }
            }
        }

        internal string WindowName = string.Empty;
        internal List<InputField> InputLabels = new List<InputField>();
        internal List<Toggle> InputToggles = new List<Toggle>();
        internal List<Slider> InputSliders = new List<Slider>();
        internal List<Dropdown> InputDropdowns = new List<Dropdown>();
    }
}
