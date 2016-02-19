using System;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Reflection;
using System.Threading;

public class Log
{
  public Log()
  {
  }
  public string GetServiceName()
  {
    string name = "Unknown";
    Assembly assembly = Assembly.GetEntryAssembly();
    if (null != assembly) {
      name = assembly.FullName;
    }
    int index = name.IndexOf(',');
    if (index >= 0)
      name = name.Substring(0, index).Trim();
    return name;
  }
  public void LOG(string value, string module_name = "")
  {
    m_ModuleName = module_name;
    switch (m_Type) {
      case 0:
        CacheLog(value);
        break;
      case 1:
        DiskLog(value);
        break;
      default:
        break;
    }
  }
  public virtual void CacheLog(string value)
  {
    lock (m_CacheLock) {
      string buf = value + "(";
      buf += Thread.CurrentThread.ManagedThreadId + ")(";
      buf += DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss-fff") + ")\n";
      byte[] bs = System.Text.Encoding.UTF8.GetBytes(buf);
      if (m_WritePos + bs.Length < m_CacheSize) {
        Buffer.BlockCopy(bs, 0, m_CacheLog, m_WritePos, bs.Length);
        m_WritePos += bs.Length;
      } else {
        //throw (new Exception("LOG Cache Overflow !!!"));
      }
    }
  }
  public virtual void DiskLog(string value)
  {
    lock (m_WriteLock) {
      string name = GenLogName();
      if (!Directory.Exists("./log/")) {
        Directory.CreateDirectory("./log/");
      }
      using (FileStream fs = new FileStream(name, FileMode.Append, FileAccess.Write)) {
        if (fs != null) {
          string buf = value + "(";
          buf += Thread.CurrentThread.ManagedThreadId + ")(";
          buf += DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss-fff") + ")\n";
          byte[] bs = System.Text.Encoding.UTF8.GetBytes(buf);
          fs.Write(bs, 0, bs.Length);
          fs.Close();
        } else {
          throw (new Exception("DiskLog error!"));
        }
      }
    }
  }
  public virtual void FlushLog(byte[] ptr, int size, string module_name = "")
  {
    try {
      if (ptr == null)
        return;
      if (size > 0) {
        string name = GenLogName();
        if (!Directory.Exists("./log/")) {
          Directory.CreateDirectory("./log/");
        }
        using (FileStream fs = new FileStream(name, FileMode.Append, FileAccess.Write)) {
          if (fs != null) {
            fs.Write(ptr, 0, size);
            fs.Close();
          } else {
            throw (new Exception("FlushLog error !!!"));
          }
        }
      }
    } catch (Exception ex) {
      Console.WriteLine("Log::FlushLog exception:{0}\n{1}", ex.Message, ex.StackTrace);
    }
  }
  public virtual string GenLogName(string module_name = "")
  {
    return "";
  }

  public byte[] m_CacheLog = null;
  public int m_WritePos = 0;
  public object m_CacheLock = new object();
  public int m_Type = -1;
  public string m_ModuleName = null;
  public string m_LastModuleName = "";
  public int m_ID = -1;
  public string m_Name = null;
  public int m_CacheSize = 4 * 1024 * 1024;
  public object m_WriteLock = new object();
  public DateTime m_CurDay = DateTime.Today;
  public int m_MaxLogSize = 1024 * 1024 * 1024;
  public int m_LogSer = 0;
  public string m_FileName = null;
}