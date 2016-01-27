using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace ProjectAnalyzer
{
    public enum ActiveType
    {
        Home, Textures, Models, Materials, Particles
    };

    public enum ActiveSubType
    {
        Details, Helps, Settings
    };

    class BaseAnalyzer
    {
        protected int mCurPage = 0 ;
        protected int mTotalPage;
        protected int mPageCnt = 50;

        protected void DrawHelpTips(string helpInfos)
        {
            GUILayout.Label(helpInfos, GUILayout.MaxWidth(1500));
        }

        protected void DrawPageCnt(int totalCnt)
        {
            int mTotalPage = totalCnt / mPageCnt;
            mTotalPage = totalCnt % mPageCnt == 0 ? mTotalPage : mTotalPage + 1;

            if (mCurPage == 0)
            {
                GUILayout.Label( LanguageCfg.PAGE_PRE, GUILayout.Width(100));
            }
            else
            {
                if (GUILayout.Button(LanguageCfg.PAGE_PRE, GUILayout.Width(100)))
                {
                    mCurPage--;
                }
            }

            GUILayout.Label((mCurPage + 1) + " / " + mTotalPage, GUILayout.Width(100));

            if (mTotalPage -1 == mCurPage)
            {

                GUILayout.Label(LanguageCfg.PAGE_NEXT, GUILayout.Width(100));
            }
            else
            {
                if (GUILayout.Button(LanguageCfg.PAGE_NEXT, GUILayout.Width(100)))
                {
                    mCurPage++;
                }
            }
        }

        protected void DrawProposeTips(ResInfo refInfo)
        {
            if (refInfo.proposeTipCount > 0)
            {
                if (GUILayout.Button("建议", GUILayout.Width(100)))
                {
                    EditTools.PingAssetInProject(refInfo.path);
                    EditorUtility.DisplayDialog("Tips", refInfo.GetResInfoDetails(), "OK");
                }

            }
        }
    }
}
