using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameFramework.DataCache
{
    //数据存储Save操作返回结果类型
    public enum DSSaveResult
    {
        UnknownError = -1,  //未知错误  
        Success = 0,        //成功
        PrepError = 1,      //前置错误：消息发送之前错误
        PostError = 2,      //后置错误：DS Save操作错误
        TimeoutError = 3,   //超时错误
    }
    //数据存储Load操作返回结果类型
    public enum DSLoadResult
    {
        UnknownError = -1,  //未知错误  
        Success = 0,        //成功    
        NotFound = 1,       //数据库中不存在查询的数据
        PrepError = 2,      //前置错误：消息发送之前错误
        PostError = 3,      //后置错误：DS Load操作错误
        TimeoutError = 4,   //超时错误
        Undone = 5,         //数据传输未完成
    }
}
