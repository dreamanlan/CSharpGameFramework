using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Text;
using System.Net;
using System.Net.Security;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

[Serializable]
public class ResourceKeys
{
    public List<string> Keys = new List<string>();
}

[Serializable]
public class ResourceParams
{
    public string Resource
    {
        get {
            return m_Resource;
        }
        set {
            m_Resource = value;
        }
    }
    public int Count
    {
        get {
            return m_Count;
        }
    }
    public List<string> Keys
    {
        get { return m_Keys; }
    }
    public string GetKey(int index)
    {
        string ret = string.Empty;
        if (index >= 0 && index < m_Count) {
            ret = m_Keys[index];
        }
        return ret;
    }
    public string GetValue(int index)
    {
        string obj = null;
        if (index >= 0 && index < m_Count) {
            obj = m_Vals[index];
        }
        return obj;
    }
    public void Clear()
    {
        m_Count = 0;
        m_Keys.Clear();
        m_Vals.Clear();
    }
    public bool Contains(string key)
    {
        bool ret = false;
        if (null != m_Keys) {
            ret = m_Keys.Contains(key);
        }
        return ret;
    }
    public void AddOrUpdate(string key, string val)
    {
        int ix = m_Keys.IndexOf(key);
        if (ix >= 0) {
            m_Vals[ix] = val;
        } else {
            m_Keys.Add(key);
            m_Vals.Add(val);
            ++m_Count;
        }
    }
    public void Remove(string key)
    {
        int ix = m_Keys.IndexOf(key);
        if (ix >= 0) {
            m_Keys.RemoveAt(ix);
            m_Vals.RemoveAt(ix);
            --m_Count;
        }
    }
    public void Merge(ResourceParams other)
    {
        for (int i = 0; i < other.Count; ++i) {
            var key = other.GetKey(i);
            var val = other.GetValue(i);
            AddOrUpdate(key, val);
        }
    }
    public bool TryGetValue(string key, out string val)
    {
        bool ret = false;
        int ix = m_Keys.IndexOf(key);
        if (ix >= 0) {
            val = m_Vals[ix];
            ret = true;
        } else {
            val = null;
        }
        return ret;
    }
    public ResourceParams() : this(8)
    {
    }
    private ResourceParams(int capacity)
    {
        m_Count = 0;
        m_Keys = new List<string>(capacity);
        m_Vals = new List<string>(capacity);
    }

    [SerializeField]
    private string m_Resource = null;
    [SerializeField]
    private int m_Count = 0;
    [SerializeField]
    private List<string> m_Keys = null;
    [SerializeField]
    private List<string> m_Vals = null;
}

[Serializable]
public class ResourceParamsDB
{
    public int Count
    {
        get {
            return m_Count;
        }
    }
    public List<string> ResourceKeys
    {
        get { return m_Keys; }
    }
    public string GetResourceKey(int index)
    {
        string ret = string.Empty;
        if (index >= 0 && index < m_Count) {
            ret = m_Keys[index];
        }
        return ret;
    }
    public ResourceParams GetResourceParams(int index)
    {
        ResourceParams obj = null;
        if (index >= 0 && index < m_Count) {
            obj = m_Vals[index];
        }
        return obj;
    }
    public void Upgrade()
    {
        var data = this;
        //之前以assetPath作为Key保存设置，坤总说服务器KEY最长只有100字节，因此改用guid作KEY，并将assetPath作为数据保存
        for (int i = 0; i < data.Count; ++i) {
            var paramsData = data.GetResourceParams(i);
            if (string.IsNullOrEmpty(paramsData.Resource)) {
                string assetPath = data.GetResourceKey(i);
                string guid = AssetDatabase.AssetPathToGUID(assetPath);
                if (!string.IsNullOrEmpty(guid)) {
                    paramsData.Resource = assetPath;
                    data.UpdateResourceKey(i, guid);
                }
                else {
                    //可能有一些不存在的资源保存在旧格式数据里，此时找不到guid，为了方便删除，保留原来的assetPath作key
                    //并补全新版本数据
                    paramsData.Resource = assetPath;
                }
            }
        }
    }
    public void Clear()
    {
        m_Count = 0;
        m_Keys.Clear();
        m_Vals.Clear();
    }
    public bool Contains(string resKey)
    {
        bool ret = false;
        if (null != m_Keys) {
            ret = m_Keys.Contains(resKey);
        }
        return ret;
    }
    public void AddOrMerge(string resKey, ResourceParams resParams)
    {
        int ix = m_Keys.IndexOf(resKey);
        if (ix >= 0) {
            m_Vals[ix].Merge(resParams);
        } else {
            m_Keys.Add(resKey);
            m_Vals.Add(resParams);
            ++m_Count;
        }
    }
    public void Remove(string resKey)
    {
        int ix = m_Keys.IndexOf(resKey);
        if (ix >= 0) {
            m_Keys.RemoveAt(ix);
            m_Vals.RemoveAt(ix);
            --m_Count;
        }
    }
    public void Merge(ResourceParamsDB other)
    {
        for (int i = 0; i < other.Count; ++i) {
            var key = other.GetResourceKey(i);
            var val = other.GetResourceParams(i);
            AddOrMerge(key, val);
        }
    }
    public bool TryGetResourceParams(string res, out ResourceParams val)
    {
        bool ret = false;
        int ix = m_Keys.IndexOf(res);
        if (ix >= 0) {
            val = m_Vals[ix];
            ret = true;
        } else {
            val = null;
        }
        return ret;
    }
    private void UpdateResourceKey(int index, string resKey)
    {
        if (index >= 0 && index < m_Count) {
            m_Keys[index] = resKey;
        }
    }
    public ResourceParamsDB() : this(8)
    {
    }
    private ResourceParamsDB(int capacity)
    {
        m_Count = 0;
        m_Keys = new List<string>(capacity);
        m_Vals = new List<ResourceParams>(capacity);
    }

    [SerializeField]
    private int m_Count = 0;
    [SerializeField]
    private List<string> m_Keys = null;
    [SerializeField]
    private List<ResourceParams> m_Vals = null;
}

[Serializable]
public class ResourceWebResponse
{
    public int ret = -1;
    public string msg = null;
}

public class ResourceEditGetPoster
{
    public static bool Fetch(string dbKey, ResourceParamsDB db)
    {
        bool ret = false;
        string keysJson;
        if (ResourceEditGetPoster.HttpGet(dbKey, out keysJson)) {
            try {
                ret = true;
                var keys = JsonUtility.FromJson<ResourceKeys>(keysJson);
                int ct = 0;
                int totalCt = keys.Keys.Count;
                foreach (var key in keys.Keys) {
                    string paramsJson;
                    if (ResourceEditGetPoster.HttpGet(key, out paramsJson)) {
                        try {
                            var ps = JsonUtility.FromJson<ResourceParams>(paramsJson);
                            db.AddOrMerge(key, ps);
                        }
                        catch (Exception ex) {
                            Debug.LogErrorFormat("{0}\n{1}", ex.Message, ex.StackTrace);
                            ret = false;
                        }
                    }
                    ++ct;
                    if (EditorUtility.DisplayCancelableProgressBar("Fetch", string.Format("{0}/{1}", ct, totalCt), ct * 1.0f / totalCt))
                        break;
                }
                if (ret) {
                    Debug.Log("Fetch ok.");
                }
            }
            catch (Exception ex) {
                Debug.LogErrorFormat("{0}\n{1}", ex.Message, ex.StackTrace);
            }
            finally {
                EditorUtility.ClearProgressBar();
            }
        }
        return ret;
    }
    public static bool Commit(string dbKey, ResourceParamsDB db)
    {
        bool ret = false;
        ResourceKeys keys = new ResourceKeys { Keys = db.ResourceKeys };
        var keysJson = JsonUtility.ToJson(keys, true);
        string r;
        if (ResourceEditGetPoster.HttpPost(dbKey, keysJson, out r)) {
            try {
                ret = true;
                int ct = 0;
                int totalCt = keys.Keys.Count;
                foreach (var key in keys.Keys) {
                    ResourceParams ps;
                    if (db.TryGetResourceParams(key, out ps)) {
                        var paramsJson = JsonUtility.ToJson(ps, true);
                        if (!ResourceEditGetPoster.HttpPost(key, paramsJson, out r)) {
                            ret = false;
                        }
                    }
                    ++ct;
                    if (EditorUtility.DisplayCancelableProgressBar("Commit", string.Format("{0}/{1}", ct, totalCt), ct * 1.0f / totalCt))
                        break;
                }
                if (ret) {
                    Debug.Log("Commit ok.");
                }
            }
            finally {
                EditorUtility.ClearProgressBar();
            }
        }
        return ret;
    }
    public static bool HttpGet(string key, out string txt)
    {
        bool ret = false;
        txt = string.Empty;
        string url = string.Format("{0}{1}", s_GetUrl, WWW.EscapeURL(key));
        var r = GetWebRequest(url, Encoding.UTF8);
        if (!string.IsNullOrEmpty(r)) {
            var res = JsonUtility.FromJson<ResourceWebResponse>(r);
            if (null != res && res.ret == 0) {
                txt = WWW.UnEscapeURL(res.msg);
                ret = true;
            }
            else {
                Debug.LogErrorFormat("{0}", r);
            }
        }
        return ret;
    }
    public static bool HttpPost(string key, string val, out string txt)
    {
        bool ret = false;
        txt = string.Empty;
        string url = s_PostUrl;
        string data = string.Format("set={0}&data={1}", WWW.EscapeURL(key), WWW.EscapeURL(val));
        var r = PostWebRequest(url, data, Encoding.UTF8);
        if (!string.IsNullOrEmpty(r)) {
            var res = JsonUtility.FromJson<ResourceWebResponse>(r);
            if (null != res && res.ret == 0) {
                txt = WWW.UnEscapeURL(res.msg);
                ret = true;
            }
            else {
                Debug.LogErrorFormat("{0}", r);
            }
        }
        return ret;
    }

    private static string GetWebRequest(string url, Encoding dataEncode)
    {
        string html = string.Empty;
        try {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse()) {
                using (Stream stream = response.GetResponseStream()) {
                    using (StreamReader reader = new StreamReader(stream)) {
                        html = reader.ReadToEnd();
                    }
                }
            }
        }
        catch (Exception ex) {
            Debug.LogErrorFormat("error:{0}\n{1}", ex.Message, ex.StackTrace);
        }
        return html;
    }
    private static string PostWebRequest(string url, string data, Encoding dataEncode)
    {
        string html = string.Empty;
        try {
            byte[] dataBytes = dataEncode.GetBytes(data);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            request.ContentLength = dataBytes.Length;
            request.ContentType = "application/x-www-form-urlencoded";
            request.Method = "POST";

            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);

            using (Stream requestBody = request.GetRequestStream()) {
                requestBody.Write(dataBytes, 0, dataBytes.Length);
            }

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse()) {
                using (Stream stream = response.GetResponseStream()) {
                    using (StreamReader reader = new StreamReader(stream)) {
                        html = reader.ReadToEnd();
                    }
                }
            }
        }
        catch (Exception ex) {
            Debug.LogErrorFormat("error:{0}\n{1}", ex.Message, ex.StackTrace);
        }
        return html;
    }
    private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
    {
        return true;// Always accept
    }

    private static string s_GetUrl = "https://www.google.com";
    private static string s_PostUrl = "https://www.google.com";
}
