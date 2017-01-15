using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Story;
using StorySystem;

namespace GameFramework.Story
{
    internal class UiStoryEventHandler : MonoBehaviour
    {
        internal void OnInputHandler(string tag, string val)
        {
            ArrayList args = new ArrayList();
            args.Add(tag);
            args.Add(val);
            BuildInputArgs(args);
            GfxStorySystem.Instance.SendMessage(string.Format("{0}:on_input", WindowName), args.ToArray());
        }
        internal void OnSliderHandler(string tag, float val)
        {
            ArrayList args = new ArrayList();
            args.Add(tag);
            args.Add(val);
            BuildInputArgs(args);
            GfxStorySystem.Instance.SendMessage(string.Format("{0}:on_slider", WindowName), args.ToArray());
        }
        internal void OnDropdownHandler(string tag, int val)
        {
            ArrayList args = new ArrayList();
            args.Add(tag);
            args.Add(val);
            BuildInputArgs(args);
            GfxStorySystem.Instance.SendMessage(string.Format("{0}:on_dropdown", WindowName), args.ToArray());
        }
        internal void OnToggleHandler(string tag, bool val)
        {
            ArrayList args = new ArrayList();
            args.Add(tag);
            args.Add(val ? 1 : 0);
            BuildInputArgs(args);
            GfxStorySystem.Instance.SendMessage(string.Format("{0}:on_toggle", WindowName), args.ToArray());
        }
        internal void OnClickHandler(string tag)
        {
            ArrayList args = new ArrayList();
            args.Add(tag);
            BuildInputArgs(args);
            GfxStorySystem.Instance.SendMessage(string.Format("{0}:on_click", WindowName), args.ToArray());
        }

        private void BuildInputArgs(ArrayList args)
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
                    args.Add(strVals);
                }
            }
            if (null != InputToggles) {
                int boolCt = InputToggles.Count;
                boolVals = new int[boolCt];
                for (int i = 0; i < boolCt; ++i) {
                    boolVals[i] = InputToggles[i].isOn ? 1 : 0;
                }
                if (boolCt > 0) {
                    args.Add(boolVals);
                }
            }
            if (null != InputSliders) {
                int floatCt = InputSliders.Count;
                floatVals = new float[floatCt];
                for (int i = 0; i < floatCt; ++i) {
                    floatVals[i] = InputSliders[i].value;
                }
                if (floatCt > 0) {
                    args.Add(floatVals);
                }
            }
            if (null != InputDropdowns) {
                int dropdownCt = InputDropdowns.Count;
                dropdownVals = new int[dropdownCt];
                for (int i = 0; i < dropdownCt; ++i) {
                    dropdownVals[i] = InputDropdowns[i].value;
                }
                if (dropdownCt > 0) {
                    args.Add(dropdownVals);
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
