using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScriptableFramework.DataCache
{
    //Data storage Save operation return result type
    public enum DSSaveResult
    {
        UnknownError = -1,  //unknown error
        Success = 0,        //success
        PrepError = 1,      //Prefix error: error before message is sent
        PostError = 2,      //Post error: DS Save operation error
        TimeoutError = 3,   //time out
    }
    //Data storage Load operation return result type
    public enum DSLoadResult
    {
        UnknownError = -1,  //unknown error
        Success = 0,        //success
        NotFound = 1,       //The queried data does not exist in the database
        PrepError = 2,      //Prefix error: error before message is sent
        PostError = 3,      //Post error: DS Load operation error
        TimeoutError = 4,   //time out
        Undone = 5,         //Data transfer not completed
    }
}
