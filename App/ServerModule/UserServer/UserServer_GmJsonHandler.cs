using System;
using System.Collections.Generic;
using System.Text;
using CSharpCenterClient;
using ScriptableFramework;
using GameFrameworkMessage;

namespace ScriptableFramework
{
    internal partial class UserServer
    {
        /// <summary>
        /// Note that message processing from node needs to be distributed to the user thread of DataProcess for processing!
        /// Note that GM tool messages and client GM messages should not be mixed, and the implementation codes must be separated (there is a mark in the following code, the client's GM message processing is in the front, the GM tool's is in the back, and there is a separation area in the middle)! ! !
        /// </summary>
        private void InstallGmJsonHandlers()
        {
            //Client GM message
            if (UserServerConfig.WorldIdNum > 0) {
                JsonGmMessageDispatcher.Init(UserServerConfig.WorldId1);
                //GM tool news
            }
        }

        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        // Message processing that requires GM permissions
        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        // The GM tool message processing is finished, do not add logical messages after this,
        // put it in the ordinary message processing file! ! !
        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    }
}
