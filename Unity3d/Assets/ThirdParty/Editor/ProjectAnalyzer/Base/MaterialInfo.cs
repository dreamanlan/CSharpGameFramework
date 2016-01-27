using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ProjectAnalyzer
{

    public class MaterialInfo : ResInfo
    {
        public enum SortType
        {
            Name,
            ShaderName,
            Propose
        }


        public string shaderName;
        public MaterialInfo(Material material, string path)
        {
            this.name = material.name;
            this.path = path;
            this.shaderName = material.shader.name;


            if (!this.shaderName.Contains("Mobile") && !this.shaderName.Contains("Unlit"))
            {
                AddProposeTip("建议移动设备上使用mobile或者unlit的shader。");
            }
        }
    }
   

}
