using System;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;

public class NormLog : Log
{
  public NormLog(string name, int cachesize)
  {
    m_Name = name;
    m_CacheSize = cachesize;
    m_CacheLog = new byte[m_CacheSize];
  }
  public override string GenLogName(string module_name)
  {
    m_ModuleName = module_name;
    if (m_ModuleName.Length <= 0 || m_Name == null || m_Type < 0)
      return "";
    bool needRename = false;
    if (m_LastModuleName != m_ModuleName) {
      needRename = true;
      m_LastModuleName = m_ModuleName;
    }
    if (m_CurDay != DateTime.Today) {
      m_CurDay = DateTime.Today;
      m_WritePos = 0;
      needRename = true;
    }
    if (m_WritePos >= m_MaxLogSize) {
      m_LogSer++;
      m_WritePos = 0;
      needRename = true;
    }
    if (null == m_FileName || needRename) {
      string tm = DateTime.Today.ToString("yyyy-MM-dd");
      string name = "./log/cylog/";
      if (m_LogSer > 0)
        name += m_ModuleName + ".log." + tm + "-" + m_LogSer;
      else
        name += m_ModuleName + ".log." + tm;
      m_FileName = name;
    }
    return m_FileName;
  }
  public override void CacheLog(string value)
  {
    lock (m_CacheLock) {
      char separator = Convert.ToChar(0x01);
      string buf = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + separator + value + "\n";
      byte[] bs = System.Text.Encoding.UTF8.GetBytes(buf);
      if (m_WritePos + bs.Length < m_CacheSize) {
        Buffer.BlockCopy(bs, 0, m_CacheLog, m_WritePos, bs.Length);
        m_WritePos += bs.Length;
      } else {
        //throw (new Exception("LOG Cache Overflow !!!"));
      }
    }
  }
  public override void DiskLog(string value)
  {
    lock (m_WriteLock) {
      string name = GenLogName(m_ModuleName);
      if (!Directory.Exists("./log/cylog/")) {
        Directory.CreateDirectory("./log/cylog/");
      }
      char separator = Convert.ToChar(0x01);
      using (FileStream fs = new FileStream(name, FileMode.Append, FileAccess.Write)) {
        if (fs != null) {
          string buf = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + separator + value + "\n";
          byte[] bs = System.Text.Encoding.UTF8.GetBytes(buf);
          fs.Write(bs, 0, bs.Length);
          fs.Close();
        } else {
          Console.WriteLine("DiskLog error!");
        }
      }
    }
  }
  public override void FlushLog(byte[] ptr, int size, string module_name)
  {
    try {
      if (ptr == null)
        return;
      if (size > 0) {
        string name = GenLogName(module_name);
        if (!Directory.Exists("./log/cylog/")) {
          Directory.CreateDirectory("./log/cylog/");
        }
        using (FileStream fs = new FileStream(name, FileMode.Append, FileAccess.Write)) {
          if (fs != null) {
            fs.Write(ptr, 0, size);
            fs.Close();
          } else {
            Console.WriteLine("FlushLog error !!!");
          }
        }
      }
    } catch(Exception ex) {
      Console.WriteLine("NormLog::FlushLog exception:{0}\n{1}", ex.Message, ex.StackTrace);
    }
  }
}

