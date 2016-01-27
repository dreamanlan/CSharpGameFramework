using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ProjectAnalyzer
{
    #region 资源基类信息
    public class ResInfo
    {
        public bool isSelected;
        public int instanceID;
        public string name;
        public string path;
        public long size;
        public long fileSize = -1;

        public int proposeTipCount = 0;
        public string proposeTips = "";

        public string GetSize()
        {
            return FormatBytes(size);
        }
        public string GetAbsolutePath()
        {
            string tmpPath = Application.dataPath;
            tmpPath = tmpPath.Replace('\\', '/');
            tmpPath = tmpPath.Substring(0, tmpPath.Length - 6);
            return tmpPath + path;
        }

        public String GetFileLenth()
        {
            if (fileSize == -1)
            {
                FileInfo file = new FileInfo(GetAbsolutePath());
                fileSize = file.Length;
            }
            return FormatBytes(fileSize);
        }

        protected void ResetProposeTip()
        {
            proposeTipCount = 0;
            proposeTips = "";
        }
        protected void AddProposeTip(string tip)
        {
            proposeTipCount++;
            string format = "{0}. {1} \n";
            proposeTips += string.Format(format, proposeTipCount, tip);
        }

        protected bool Powerof2(int n)
        {
            return ((n & (n - 1)) == 0);
        }


        public string FormatBytes(long size)
        {
            if (size < 1024)
            {
                return size + " byte";
            }
            else if (size < (1024 * 1024))
            {
                return ((float)size / 1024).ToString("0.0") + " KB";
            }
            else
            {
                return ((float)size / (1024 * 1024)).ToString("0.0") + " M";
            }
        }


        virtual public string GetRefInfos()
        {
            return "";
        }
        public string GetResInfoDetails()
        {
            return GetRefInfos() + proposeTips;
        }
    }


    #endregion


}
