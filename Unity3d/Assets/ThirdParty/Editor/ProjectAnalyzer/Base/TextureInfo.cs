using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace ProjectAnalyzer
{

    public class TextureInfo : ResInfo
    {
        public enum SortType
        {
            Name,
            MemorySize,
            PixWidth,
            PixHeigh,
            IsRW,
            Mipmap,
            IsLightmap,
            AnisoLevel,
            Propose
        }
        public class TexturePlatSetting
        {
            public const string PLAT_ANDROID = "Android";
            public const string PLAT_IPHONE = "iPhone";
            public const string PLAT_STANDALONE = "Standalone";

            public bool isSetting;
            public int maxSize;
            public TextureImporterFormat textureFormat;

            public string ToString()
            {
                return isSetting + " " + maxSize + " " + textureFormat.ToString();
            }
        }

        public Texture texture;
        public int width;
        public int height;
        public bool isRW;
        public bool isMipmap;
        public bool isLightmap;
        public int anisoLevel;

        public TexturePlatSetting androidPlatSetting = new TexturePlatSetting();
        public TexturePlatSetting iosPlatSetting = new TexturePlatSetting();
        public TexturePlatSetting standalonePlatSetting = new TexturePlatSetting();

        private TextureImporter ti;
        public TextureInfo(Texture texture, string path)
        {
            //获取问题贴图的数据
            this.name = texture.name;
            this.path = path;
            this.texture = texture;
            AnalyzerTexture();

        }


        public void SetTextureNoMinmap()
        {
            bool flag = false;
            if (this.ti == null)
            {
                return;
            }
            if (this.ti.mipmapEnabled)
            {
                this.ti.mipmapEnabled = false;
                flag = true;

            }           
            if (flag)
            {
                AssetDatabase.ImportAsset(path);
                AnalyzerTexture();
            }
        }
        public void AnalyzerTexture()
        {
            this.width = this.texture.width;
            this.height = this.texture.height;
            this.size = CalculateTextureSizeBytes(this.texture);
            this.ti = AssetImporter.GetAtPath(path) as TextureImporter;
            if (ti != null)
            {
                this.isRW = ti.isReadable;

                androidPlatSetting.isSetting = ti.GetPlatformTextureSettings(TexturePlatSetting.PLAT_ANDROID,
                        out androidPlatSetting.maxSize, out androidPlatSetting.textureFormat);

                iosPlatSetting.isSetting = ti.GetPlatformTextureSettings(TexturePlatSetting.PLAT_IPHONE,
                        out iosPlatSetting.maxSize, out iosPlatSetting.textureFormat);

                standalonePlatSetting.isSetting = ti.GetPlatformTextureSettings(TexturePlatSetting.PLAT_STANDALONE,
                        out standalonePlatSetting.maxSize, out standalonePlatSetting.textureFormat);

                this.isMipmap = ti.mipmapEnabled;
                this.isLightmap = ti.lightmap;
                this.anisoLevel = ti.anisoLevel;
            }

            CheckValid();

        }


        public override string GetRefInfos()
        {
            string info = "";

            info += LanguageCfg.PIX_W + ":" + this.width.ToString() + "\n";
            info += LanguageCfg.PIX_H + ":" + this.width.ToString() + "\n";
            info += LanguageCfg.IsRW + ":" + this.isRW.ToString() + "\n";
            info += LanguageCfg.Mipmap + ":" + this.isMipmap.ToString() + "\n";
            info += LanguageCfg.IsLightmap + ":" + this.isLightmap.ToString() + "\n";
            info += LanguageCfg.AnisoLevel + ":" + this.anisoLevel.ToString() + "\n\n\n";
            return info;
        }
        //检验纹理贴图的数据合法性
        public void CheckValid()
        {
            ResetProposeTip();
            SettingCfg setting = SettingCfg.instance;
            if (setting.textureCheckMemSize)
            {

                if (this.size >= setting.textureCheckMemSizeValue * 1024)
                {
                    AddProposeTip("文件不小啊，大于" + setting.textureCheckMemSizeValue + "KB，还能减小不？？");
                }
            }

            if (setting.textureCheckPix)
            {
                if (this.width > setting.textureCheckPixW || this.height > setting.textureCheckPixH)
                {
                    AddProposeTip("建议纹理像素最大是" + setting.textureCheckPixW + "x" + setting.textureCheckPixH);
                }
            }
            if (setting.textureCheckPix2Pow)
            {
                if (!Powerof2(this.width) || !Powerof2(this.height))
                {
                    AddProposeTip("建议像素大小为2的n次幂(GUI纹理除外) " + this.width + "x" + this.height);
                }
            }

            if (setting.textureCheckPlatSetting)
            {
                if (!androidPlatSetting.isSetting || !iosPlatSetting.isSetting || !standalonePlatSetting.isSetting)
                {
                    AddProposeTip("建议对不同的平台设置不同的参数");
                }
            }
            if (setting.textureCheckIsRW)
            {
                if (this.isRW)
                {
                    AddProposeTip("建议非读写的贴图将这个读写开关关掉");
                }
            }
            if (setting.textureCheckIsLightmap)
            {
                if (this.isLightmap)
                {
                    AddProposeTip("建议检查当前纹理确实是lightmap需要的纹理否？");
                }
            }
        }


        public string IsOverridePlatform()
        {
            return standalonePlatSetting.isSetting
                + " " + androidPlatSetting.isSetting
                + " " + iosPlatSetting.isSetting;
        }

        int CalculateTextureSizeBytes(Texture tTexture)
        {

            int tWidth = tTexture.width;
            int tHeight = tTexture.height;
            if (tTexture is Texture2D)
            {
                Texture2D tTex2D = tTexture as Texture2D;
                int bitsPerPixel = GetBitsPerPixel(tTex2D.format);
                int mipMapCount = tTex2D.mipmapCount;
                int mipLevel = 1;
                int tSize = 0;
                while (mipLevel <= mipMapCount)
                {
                    tSize += tWidth * tHeight * bitsPerPixel / 8;
                    tWidth = tWidth / 2;
                    tHeight = tHeight / 2;
                    mipLevel++;
                }
                return tSize;
            }

            if (tTexture is Cubemap)
            {
                Cubemap tCubemap = tTexture as Cubemap;
                int bitsPerPixel = GetBitsPerPixel(tCubemap.format);
                return tWidth * tHeight * 6 * bitsPerPixel / 8;
            }
            return 0;
        }

        int GetBitsPerPixel(TextureFormat format)
        {
            switch (format)
            {
                case TextureFormat.Alpha8: //	 Alpha-only texture format.
                    return 8;
                case TextureFormat.ARGB4444: //	 A 16 bits/pixel texture format. Texture stores color with an alpha channel.
                    return 16;
                case TextureFormat.RGBA4444: //	 A 16 bits/pixel texture format.
                    return 16;
                case TextureFormat.RGB24:	// A color texture format.
                    return 24;
                case TextureFormat.RGBA32:	//Color with an alpha channel texture format.
                    return 32;
                case TextureFormat.ARGB32:	//Color with an alpha channel texture format.
                    return 32;
                case TextureFormat.RGB565:	//	 A 16 bit color texture format.
                    return 16;
                case TextureFormat.DXT1:	// Compressed color texture format.
                    return 4;
                case TextureFormat.DXT5:	// Compressed color with alpha channel texture format.
                    return 8;
                /*
                case TextureFormat.WiiI4:	// Wii texture format.
                case TextureFormat.WiiI8:	// Wii texture format. Intensity 8 bit.
                case TextureFormat.WiiIA4:	// Wii texture format. Intensity + Alpha 8 bit (4 + 4).
                case TextureFormat.WiiIA8:	// Wii texture format. Intensity + Alpha 16 bit (8 + 8).
                case TextureFormat.WiiRGB565:	// Wii texture format. RGB 16 bit (565).
                case TextureFormat.WiiRGB5A3:	// Wii texture format. RGBA 16 bit (4443).
                case TextureFormat.WiiRGBA8:	// Wii texture format. RGBA 32 bit (8888).
                case TextureFormat.WiiCMPR:	//	 Compressed Wii texture format. 4 bits/texel, ~RGB8A1 (Outline alpha is not currently supported).
                    return 0;  //Not supported yet
                */
                case TextureFormat.PVRTC_RGB2://	 PowerVR (iOS) 2 bits/pixel compressed color texture format.
                    return 2;
                case TextureFormat.PVRTC_RGBA2://	 PowerVR (iOS) 2 bits/pixel compressed with alpha channel texture format
                    return 2;
                case TextureFormat.PVRTC_RGB4://	 PowerVR (iOS) 4 bits/pixel compressed color texture format.
                    return 4;
                case TextureFormat.PVRTC_RGBA4://	 PowerVR (iOS) 4 bits/pixel compressed with alpha channel texture format
                    return 4;
                case TextureFormat.ETC_RGB4://	 ETC (GLES2.0) 4 bits/pixel compressed RGB texture format.
                    return 4;
                case TextureFormat.ATC_RGB4://	 ATC (ATITC) 4 bits/pixel compressed RGB texture format.
                    return 4;
                case TextureFormat.ATC_RGBA8://	 ATC (ATITC) 8 bits/pixel compressed RGB texture format.
                    return 8;
                case TextureFormat.BGRA32://	 Format returned by iPhone camera
                    return 32;
            }
            return 0;
        }

    }

}
