#if !UNITY_2019_4_OR_NEWER
using System;
using UnityEngine.Internal;

namespace UnityEditor
{
    internal static class EditoUtilityCompatibilityHelper
    {
#if !UNITY_2019_4_OR_NEWER
        static class Content
        {
            public static readonly string Cancel = L10n.Tr("Cancel");
            static readonly string k_DialogOptOutForThisMachine = L10n.Tr("Do not show me this message again on this machine.");
            static readonly string k_DialogOptOutForThisSession = L10n.Tr("Do not show me this message again for this session.");
            public static string GetDialogOptOutMessage(DialogOptOutDecisionType dialogOptOutType)
            {
                switch (dialogOptOutType)
                {
                    case DialogOptOutDecisionType.ForThisMachine:
                        return k_DialogOptOutForThisMachine;
                    case DialogOptOutDecisionType.ForThisSession:
                        return k_DialogOptOutForThisSession;
                    default:
                        throw new NotImplementedException(string.Format("The DialogOptOut type named {0} has not been implemented.", dialogOptOutType));
                }
            }
        }
#endif

        public enum DialogOptOutDecisionType
        {
            ForThisMachine,
            ForThisSession,
        }

        public static bool GetDialogOptOutDecision(DialogOptOutDecisionType dialogOptOutDecisionType, string dialogOptOutDecisionStorageKey)
        {
#if UNITY_2019_4_OR_NEWER
            return EditorUtility.GetDialogOptOutDecision((UnityEditor.DialogOptOutDecisionType)dialogOptOutDecisionType, dialogOptOutDecisionStorageKey);
#else
            switch (dialogOptOutDecisionType)
            {
                case DialogOptOutDecisionType.ForThisMachine:
                    return EditorPrefs.GetBool(dialogOptOutDecisionStorageKey, false);
                case DialogOptOutDecisionType.ForThisSession:
                    return SessionState.GetBool(dialogOptOutDecisionStorageKey, false);
                default:
                    throw new NotImplementedException(string.Format("The DialogOptOut type named {0} has not been implemented.", dialogOptOutDecisionType));
            }
#endif
        }

        public static void SetDialogOptOutDecision(DialogOptOutDecisionType dialogOptOutDecisionType, string dialogOptOutDecisionStorageKey, bool optOutDecision)
        {
#if UNITY_2019_4_OR_NEWER
            EditorUtility.SetDialogOptOutDecision((UnityEditor.DialogOptOutDecisionType)dialogOptOutDecisionType, dialogOptOutDecisionStorageKey, optOutDecision);
#else
            switch (dialogOptOutDecisionType)
            {
                case DialogOptOutDecisionType.ForThisMachine:
                    EditorPrefs.SetBool(dialogOptOutDecisionStorageKey, optOutDecision);
                    break;
                case DialogOptOutDecisionType.ForThisSession:
                    SessionState.SetBool(dialogOptOutDecisionStorageKey, optOutDecision);
                    break;
                default:
                    throw new NotImplementedException(string.Format("The DialogOptOut type named {0} has not been implemented.", dialogOptOutDecisionType));
            }
#endif
        }

        public static bool DisplayDialog(string title, string message, string ok, DialogOptOutDecisionType dialogOptOutDecisionType, string dialogOptOutDecisionStorageKey)
        {
            return DisplayDialog(title, message, ok, string.Empty, dialogOptOutDecisionType, dialogOptOutDecisionStorageKey);
        }

        public static bool DisplayDialog(string title, string message, string ok, [DefaultValue("\"\"")] string cancel, DialogOptOutDecisionType dialogOptOutDecisionType, string dialogOptOutDecisionStorageKey)
        {
#if UNITY_2019_4_OR_NEWER
            return EditorUtility.DisplayDialog(title, message, ok, cancel, (UnityEditor.DialogOptOutDecisionType)dialogOptOutDecisionType, dialogOptOutDecisionStorageKey);
#else
            if (GetDialogOptOutDecision(dialogOptOutDecisionType, dialogOptOutDecisionStorageKey))
            {
                return true;
            }
            else
            {
                bool optOutDecision;
                bool dialogDecision = DisplayDialog(title, message, ok, cancel, Content.GetDialogOptOutMessage(dialogOptOutDecisionType), out optOutDecision);
                // Cancel means the user pressed ESC as the Cancel button was grayed out. Don't store the opt-out decision on cancel. Also, only store it if the user opted out since it defaults to opt-in.
                if (dialogDecision && optOutDecision)
                    SetDialogOptOutDecision(dialogOptOutDecisionType, dialogOptOutDecisionStorageKey, optOutDecision);
                return dialogDecision;
            }
#endif
        }

#if !UNITY_2019_4_OR_NEWER
        // TODO: This is an MVP solution. The OptOut option should be a check-box in the dialog. To achieve that, this API will need to move to bindings and get platform specific implementations.
        static bool DisplayDialog(string title, string message, string ok, string cancel, string optOutText, out bool optOutDecision)
        {
            if (string.IsNullOrEmpty(cancel))
            {
                // we can't allow empty cancel buttons in this MVP workaround. Only the two button dialog would be possible to use and it can't differentiate between pressing a cancel button (labeled with OptOut text) and pressing X or ESC.
                cancel = Content.Cancel;
            }
            int result = UnityEditor.EditorUtility.DisplayDialogComplex(title, message, ok, cancel, string.Format("{0} - {1}", ok, optOutText));
            // result 0 -> OK, 1 -> Cancel, 2 -> Ok & opt out
            optOutDecision = result == 2;
            return result != 1;
        }

#endif
    }
}
#endif
