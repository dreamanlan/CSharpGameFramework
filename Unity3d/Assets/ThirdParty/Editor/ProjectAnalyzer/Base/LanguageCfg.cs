using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ProjectAnalyzer
{
    #region 文字语言类
    public class LanguageCfg
    {
        public const string BALABALALBALA = "";
        public const string OPTIMIZE = "优化";

        public const string PAGE_PRE = "上一页";
        public const string PAGE_NEXT = "下一页";

        public const string REFRESH = "刷新";
        public const string REFRESH_TIP = "Welcome，使用前请点击上方刷新按钮";
        public const string PROJECT_LIST = "工程资源列表如下:";
        public const string PROJECT_TEXTURES_LIST = "纹理贴图个数：  {0}";
        public const string TEXTURE_SETTING_NO_MINMAP = "设置没有MinMap";
        public const string PROJECT_MODELS_LIST = "模型FBX个数 ：  {0}";
        public const string PROJECT_AUDIOS_LIST = "音效资源个数：  {0} ";
        public const string PROJECT_MATERIALS_LIST = "材质球个数  ：  {0}";
        public const string PROJECT_PARTICLE_LIST = "粒子特效个数  ：  {0}";
        public const string HOME = "总览";
        public const string TEXTURES = "纹理贴图";
        public const string MODELS = "模型";
        public const string MATERIALS = "材质球";
        public const string PARTICLES = "特效";
        public const string PARTICLES_REFRESH = "刷新特效";
        public const string DETAILS = "详情";
        public const string HELPS = "帮助";
        public const string SETTINGS = "设置";
        public const string PROPOSE = " 建议";
        public const string HELP_TEXTURE = "1.Texture的大小尽量做成2的幂 \n\n" +
            "2.设置合理的maxSize\n\n" +
            "3.覆盖不同平台Texture的参数\n\n" +
            "4.Texture大小尽量小于1024 * 1024. 建议尽量使用最小尺寸的贴图\n\n" +
            "5.如果贴图的alpha通道是不用的建议使用RGB24位代替RGB32位\n\n";
        public const string NAME = "NAME";
        public const string MemorySize = "内存大小";
        public const string PIX_W = "宽";
        public const string PIX_H = "高";
        public const string IsRW = "可读写";
        public const string OverridePlat = "平台设置";
        public const string Mipmap = "MipMap";
        public const string IsLightmap = "Lightmap";
        public const string AnisoLevel = "AnisoLevel";


        public const string Scale = "Scale";
        public const string MeshCompress = "Mesh压缩";
        public const string AnimCompress = "Anim压缩";
        public const string Collider = "碰撞器";
        public const string NormalMode = "模型法线";
        public const string TangentMode = "模型切线";
        public const string BakeIK = "BakeIK";
        public const string FileSize = "文件大小";
        public const string SkinnedMeshCnt = "蒙皮数";
        public const string MeshFilterCnt = "MeshFilter数";
        public const string AnimCnt = "动画数";
        public const string VertexCnt = "顶点数";
        public const string TriangleCnt = "三角数";
        public const string BoneCnt = "骨骼数";
        public const string SettingModelDefault = "一键设置";
        public const string SettingMeshCompress = "设置模型压缩";
        public const string SettingAnimCompress = "设置动画压缩";
        public const string SettingWriteClose = "设置读写关";
        
        public const string HELP_MODEL =
            "1.unity和其它三维建模软件间的单位差异-它将缩放整个模型，建议设置为1\n\n" +
            "2.网格和动画的压缩意味着占用更少的空间，但是会有损游戏的质量\n\n" +
            "3.注意碰撞体的碰撞层，不必要的碰撞检测一定要舍去\n\n" +
            "4.法线使用于灯光。如果你不在你的网格里使用实时照明，你也并不需要存储法线\n\n" +
            "5.切线使用于法线贴图，如果不使用法线贴图，也许并不需要在网格里存储这些切线\n\n" +
            "6.注意是否有多余的动画脚本，模型自动导入到U3D会有动画脚本，大量的话会严重影响消耗CPU计算\n\n" +
            "7.尽量减少每个动画使用的骨骼\n\n" +
            "8.最好把你人物的三角面数量控制在1500以下";

        public const string ShaderName = "ShaderName";


        public const string HELP_AUDIO =
          "1.压缩声音是通过从编辑器导入设置选择compressed选项，在音频数据将很小，但在播放时会消耗CPU周期来解码。最适用于中等长度音效与音乐\n\n" +
          "2.是否为3D的音效。一般背景的音效不会设置为3D\n\n" +
          "3.Compression kpbs一般128就可以了。128Kbps=磁带、手机立体声MP3播放器最佳设定值、低档MP3播放器最佳设定值\n\n";
        
        public const string PARTICLE_SYS_CNT = "粒子系统数";
        public const string PARTICLE_SYS_PARENT_IS_PARTICLE = "最外层是粒子";
        public const string PARTICLE_SYS_MAX_DURATION = "最大停留时间";
        public const string PARTICLE_SYS_MAX_PARTICLES = "最大粒子数";
        public const string PARTICLE_SYS_MIN_PARTICLES = "最大粒子最小值";
        public const string PARTICLE_SYS_MIN_COLIDER = "粒子碰撞器";
        public const string PARTICLE_SYS_CURRENT_CNT = "当前例子数";

        public const string HELP_PARTICLE =
          "1.屏幕同时存在的最大粒子数建议小于200个\n\n" +
          "2.每个粒子发射器发射的最大粒子数建议小于50个\n\n" +
          "3.粒子的问题建议去掉alpha通道\n\n" +
          "4.不要使用粒子碰撞\n\n";
    }
    #endregion




}
