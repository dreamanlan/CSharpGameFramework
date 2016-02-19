using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace GameFramework
{
  internal static class Md5Utilify
  {
    internal static string CalcMd5(string msg)
    {
      MD5 md5Hasher = MD5.Create();
      byte[] data = md5Hasher.ComputeHash(Encoding.UTF8.GetBytes(msg));
      StringBuilder sBuilder = new StringBuilder();
      for (int i = 0; i < data.Length; i++)
      {
          sBuilder.Append(data[i].ToString("x2"));
      }
      return sBuilder.ToString();
    }
  }
}
