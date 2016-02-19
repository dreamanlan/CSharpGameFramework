//todo:node <-> user server
var exec = require('child_process').exec;
var fs = require('fs');
var hash = require("./hash");
var config = require("./config");
var NodeMessage = require("./nodemessage").NodeMessage;
var WebSocketServer = require('ws').Server;
var MsgEnum = require('./LobbyMsgEnum').GameFrameworkMessage.LobbyMessageDefine;

var nodeMessage = new NodeMessage();
var nodeMessageForWrite = new NodeMessage();

var VOICE_IP = core_query_config("voiceip");
var PORT = parseInt(core_query_config("nodejsport"));
var ENABLE_CLIENT_LOG = 0;
var SHOP_MASK = 0xffffffff;
try {
  ENABLE_CLIENT_LOG = parseInt(core_query_config("enableclientlog"));
  if (isNaN(ENABLE_CLIENT_LOG)) {
    ENABLE_CLIENT_LOG = 0;
  }
  console.log("enableclientlog: " + ENABLE_CLIENT_LOG);
} catch (e) {
  ENABLE_CLIENT_LOG = 0;
}
try {
  SHOP_MASK = parseInt(core_query_config("shopmask"));
  if (isNaN(SHOP_MASK)) {
    SHOP_MASK = 0xffffffff;
  }
  console.log("shopmask: " + SHOP_MASK);
} catch (e) {
  SHOP_MASK = 0xffffffff;
}

var cfg = {
  ssl: false,
  port: PORT,
  ssl_key: './nodejs/key.pem',
  ssl_cert: './nodejs/cert.pem'
};

var httpServ = (cfg.ssl) ? require('https') : require('http');
var httpServInst = null;
var processRequest = function (req, res) {

  res.writeHead(200);
  res.end("All glory to WebSockets!\n");
};

if (cfg.ssl) {
  httpServInst = httpServ.createServer({
    key: fs.readFileSync(cfg.ssl_key),
    cert: fs.readFileSync(cfg.ssl_cert)

  }, processRequest);

} else {
  httpServInst = httpServ.createServer(processRequest);
}

var accountList = new hash.HashTable();
var guidList = new hash.HashTable();
var roleEnterList = new hash.HashTable();

var c_LoginMonitorInterval = 5;
var c_MaxLoginCountPerIp = 0;
var loginQueueForIp = new hash.HashTable();

var lockIndex = 0;
var kickedAccounts = [{}, {}];

var registered = false;
var inited = false;
var tick_handle = null;

function isKickedAccount(account) {
  try {
    return kickedAccounts[lockIndex][account];
  } catch (ex) {
    console.log("isKickedAccount exception:" + ex);
  }
}
function addKickedAccount(account) {
  try {
    kickedAccounts[lockIndex][account] = true;
  } catch (ex) {
    console.log("addKickedAccount exception:" + ex);
  }
}

function jsonParse(jsonStr) {
  try {
    var jsonObj = JSON.parse(jsonStr);
    return jsonObj;
  } catch (ex) {
    console.log("jsonParse exception:" + ex);
  }
  return null;
}

function getClientIp(req) {
  return req.headers['x-forwarded-for'] ||
    req.connection.remoteAddress ||
    req.socket.remoteAddress;
}

function getTimeString() {
  var d = new Date();
  var ts = [d.getFullYear(), d.getMonth() + 1, d.getDate(), d.getHours(), d.getMinutes(), d.getSeconds(), d.getMilliseconds()];
  return ts.join("-");
}

function canLogin(socket) {
  if (c_MaxLoginCountPerIp <= 0) {
    return true;
  }
  var ret = false;
  var ip = getClientIp(socket.upgradeReq);
  var curTime = process.uptime();
  var times = loginQueueForIp.getValue(ip);
  if (!times) {
    times = new Array();
    times.push(curTime);
    loginQueueForIp.add(ip, times);
    ret = true;
  } else {
    if (times.length > 0) {
      while (times.length > 0) {
        if (times[0] + c_LoginMonitorInterval < curTime) {
          times.shift();
        } else {
          break;
        }
      }
      times.push(curTime);

      console.log("login count: " + times.length + " from ip: " + ip);

      if (times.length > c_MaxLoginCountPerIp) {
        ret = false;
      } else {
        ret = true;
      }
    } else {
      times.push(curTime);
      ret = true;
    }
  }
  return ret;
}

var serverLogic = {};
serverLogic.init = function () {

  var app = httpServInst.listen(cfg.port);
  var ioServer = new WebSocketServer({ server: app });
  console.log("Server runing at port: " + PORT + ". voice ip:" + VOICE_IP);

  var unlockHandle = setInterval(function () {
    try {
      var unlockIndex = 1 - lockIndex;
      kickedAccounts[unlockIndex] = {};
      lockIndex = unlockIndex;
    } catch (ex) {
      console.log("unlock interval exception:" + ex);
    }
  }, 60000);

  ioServer.on('connection', function (socket) {
    socket.logicLifeCount = 30;
    var timeoutHandle = setInterval(function () {
      try {
        if (socket) {
          --socket.logicLifeCount;
          if (socket.logicLifeCount <= 0) {
            var socketKey = socket.upgradeReq.headers['sec-websocket-key'];
            if (socketKey) {
              var account = socket.logicAccount;
              var guid = socket.logicGuid;
              if (guid) {
                nodeMessageForWrite.initForNodeMessageWithGuid(MsgEnum.Logout, guid);
                core_send_message_by_name("UserSvr", nodeMessageForWrite.toBase64());
                guidList.remove(guid);
              }
              if (account) {
                nodeMessageForWrite.initForNodeMessageWithAccount(MsgEnum.AccountLogout, account);
                core_send_message_by_name("UserSvr", nodeMessageForWrite.toBase64());
                accountList.remove(account);
                roleEnterList.remove(account);
              }

              var ip = getClientIp(socket.upgradeReq);
              console.log("socket " + socketKey + " disconnect for timeout, account:" + account + " guid:" + guid + " ip:" + ip + " time:" + getTimeString());

              socket.logicAccount = null;
              socket.logicGuid = null;
            }
            socket.close();
            clearInterval(timeoutHandle);
          }
        } else {
          clearInterval(timeoutHandle);
        }
      } catch (ex) {
        console.log("socket interval exception:" + ex);
      }
    }, 10000);
    socket.on('message', function (arg) {
      try {
        nodeMessage.initForRead(arg);
        var msgId = nodeMessage.getId();        

        switch (msgId) {
          case MsgEnum.VersionVerify:
            {
              var version = nodeMessage.getUint32();
              console.log("version verify:" + version);
              nodeMessageForWrite.initForWrite(MsgEnum.VersionVerifyResult);
              if ((config.version & 0xffffff00) == (version & 0xffffff00)) {
                nodeMessageForWrite.writeInt(1,1);
              } else {
                nodeMessageForWrite.writeInt(1,0);
              }
              nodeMessageForWrite.writeInt(2,ENABLE_CLIENT_LOG);
              nodeMessageForWrite.writeInt(3,SHOP_MASK);
              nodeMessageForWrite.writeString(4,VOICE_IP);
              socket.send(nodeMessageForWrite.toBase64());
            }
            break;
          case MsgEnum.AccountLogin:
            {
              var account = nodeMessage.getString();
              if (account) {
                if (!isKickedAccount(account)) {
                  if (canLogin(socket)) {
                    var ip = getClientIp(socket.upgradeReq);
                    core_send_message_by_name("UserSvr", arg);
                    console.log("!!post to user server AccountLogin msg: " + arg + " ip:" + ip);
										console.log("========= Account: " + account);
                    accountList.add(account, socket);
                  } else {
                    console.log("login refuse for too many login ! msg:" + arg + " ip:" + ip + " time:" + getTimeString());
                  }
                }
              }
            }
            break;
          case MsgEnum.DirectLogin:
            {
              var account = nodeMessage.getString();
              if (account) {
                if (!isKickedAccount(account)) {
                  if (canLogin(socket)) {
                    var ip = getClientIp(socket.upgradeReq);
                    core_send_message_by_name("UserSvr", arg);
                    console.log("!!post to user server DirectLogin msg: " + arg + " ip:" + ip);
                    accountList.add(account, socket);
                  } else {
                    console.log("login refuse for too many login ! msg:" + arg + " ip:" + ip + " time:" + getTimeString());
                  }
                }
              }
            }
            break;
          case MsgEnum.RoleEnter:
            {
              var account = nodeMessage.getString();
              var socketKey = socket.upgradeReq.headers['sec-websocket-key'];
              if(socket.logicAccount==account){
                core_send_message_by_name("UserSvr", arg);
                console.log("!!post to user server RoleEnter msg: " + arg + " account:" + account);
              }
            }
            break;
          case MsgEnum.AccountLogout:
            {
              var account = nodeMessage.getString();
              if(socket.logicAccount==account){
                core_send_message_by_name("UserSvr", arg);
                console.log("!!post to user server AccountLogout msg: " + arg + " account:" + account);
                
                accountList.remove(account);
                roleEnterList.remove(account);
              }
            }
            break;
          case MsgEnum.Logout:
            {
              var guid = nodeMessage.getUint64();
              if(socket.logicGuid==guid){
                core_send_message_by_name("UserSvr", arg);
                console.log("!!post to user server Logout msg: " + arg + " guid:" + guid);
                guidList.remove(guid);
              }
            }
            break;
          case MsgEnum.GetQueueingCount:
            {
              var socketKey = socket.upgradeReq.headers['sec-websocket-key'];
              socket.logicLifeCount = 6;
              core_send_message_by_name("UserSvr", arg);
              console.log("!!post to user server GetQueueingCount msg: " + arg + " key:" + socketKey);
            }
            break;
          case MsgEnum.UserHeartbeat:
            {
              var socketKey = socket.upgradeReq.headers['sec-websocket-key'];
              var ip = getClientIp(socket.upgradeReq);
              socket.logicLifeCount = 6;
              core_send_message_by_name("UserSvr", arg);
              //console.log("!!post to user server UserHeartbeat msg: " + arg + " key:" + socketKey + " ip:" + ip + " time:" + getTimeString());
            }
            break;
          case MsgEnum.NodeRegister:
            break;
          default:
            var account = nodeMessage.getString();
            var guid = nodeMessage.getUint64();
            var socketKey = socket.upgradeReq.headers['sec-websocket-key'];
            if (!socket.logicGuid && !socket.logicAccount) {
              console.log("msg " + arg + " from unknown socket " + socketKey);
              break;
            } else {
              if (guid && socket.logicGuid && socket.logicGuid != guid) {
                console.log("msg " + arg + " from socket " + socketKey + " (guid " + guid + "), guid is different ! id:" + msgId);
                break;
              }
              if (account && socket.logicAccount && socket.logicAccount != account) {
                console.log("msg " + arg + " from socket " + socketKey + " (account " + account + "), account is different ! id:" + msgId);
                break;
              }
            }
            core_send_message_by_name("UserSvr", arg);
            //console.log("!!post to user server msg: " + arg + " id:" + msgId);
            break;
        }
      } catch (ex) {
        console.log("onmessage exception:" + ex + " msg:" + arg);
      }
    });
    socket.on('close', function () {
      try {
        var socketKey = socket.upgradeReq.headers['sec-websocket-key'];
        if (socketKey) {
          var account = socket.logicAccount;
          var guid = socket.logicGuid;
          if (guid) {
            nodeMessageForWrite.initForNodeMessageWithGuid(MsgEnum.Logout, guid);
            core_send_message_by_name("UserSvr", nodeMessageForWrite.toBase64());
            guidList.remove(guid);
          }
          if (account) {
            nodeMessageForWrite.initForNodeMessageWithAccount(MsgEnum.AccountLogout, account);
            core_send_message_by_name("UserSvr", nodeMessageForWrite.toBase64());
            accountList.remove(account);
            roleEnterList.remove(account);
          }

          console.log("socket " + socketKey + " disconnect, account:" + account + " guid:" + guid);

          socket.logicAccount = null;
          socket.logicGuid = null;
        }
        socket.close();
        clearInterval(timeoutHandle);
      } catch (ex) {
        console.log("onclose exception:" + ex);
      }
    });
    socket.on('error', function () {
      try {
        var socketKey = socket.upgradeReq.headers['sec-websocket-key'];
        if (socketKey) {
          var account = socket.logicAccount;
          var guid = socket.logicGuid;
          if (guid) {
            nodeMessageForWrite.initForNodeMessageWithGuid(MsgEnum.Logout, guid);
            core_send_message_by_name("UserSvr", nodeMessageForWrite.toBase64());
            guidList.remove(guid);
          }
          if (account) {
            nodeMessageForWrite.initForNodeMessageWithAccount(MsgEnum.AccountLogout, account);
            core_send_message_by_name("UserSvr", nodeMessageForWrite.toBase64());
            accountList.remove(account);
            roleEnterList.remove(account);
          }

          console.log("socket " + socketKey + " network error, account:" + account + " guid:" + guid);

          socket.logicAccount = null;
          socket.logicGuid = null;
        }
        socket.close();
        clearInterval(timeoutHandle);
      } catch (ex) {
        console.log("onerror exception:" + ex);
      }
    });
  });
};

global.onCoreMessage = function (handle, session, msg) {
  try {
    //console.log("@@node.js receive core message:" + handle + "," + session + "," + msg);
    if (msg == "QuitNodeJs") {
      try {
        exec("pkill nginx", { timeout: 3000 }, function (error, stdout, stderr) {
          console.log("stdout:" + stdout);
          console.log("stderr:" + stderr);
          console.log("QuitNodeJs, pkill nginx, quit nodejs");
          core_quit_nodejs();
        });
      } catch (ex) {
        console.log("oncoremessage exception:" + ex + " msg:" + msg);
        core_quit_nodejs();
      }
    } else if(msg == "Echo") {
    } else if (msg.indexOf("AddOrUpdate ") == 0) {
      console.log("oncoremessage " + msg);
      var cmdArgs = msg.substring(12).split(" ");
      if (cmdArgs.length == 3) {
        if (parseInt(cmdArgs[0]) == 0 && cmdArgs[1] == "UserSvr") {
          console.log("userserver down, try register again.");
          registered = false;
        }
      }
    } else if (msg.indexOf("EvalScp ") == 0) {
      eval(msg.substring(8));
      console.log("oncoremessage " + msg);
    } else {
      nodeMessage.initForRead(msg);
      var msg_id = nodeMessage.getId();        

      if (msg_id === MsgEnum.AccountLoginResult) {
        var _account = nodeMessage.getString();
        var _socket = accountList.getValue(_account);
        if (_socket) {
          _socket.send(msg);

          var socketKey = _socket.upgradeReq.headers['sec-websocket-key'];
          console.log("Socket2Account:" + socketKey + " -> " + _account);

          _socket.logicAccount = _account;
        }
      } else if (msg_id === MsgEnum.RoleEnterResult) {
        var _account = nodeMessage.getString();
        var _guid = nodeMessage.getUint64();
        var _socket = accountList.getValue(_account);
        if (_socket) {
          _socket.send(msg);

          var socketKey = _socket.upgradeReq.headers['sec-websocket-key'];
          console.log("Socket2Guid:" + socketKey + " -> " + _guid);

          guidList.add(_guid, _socket);
          _socket.logicGuid = _guid;
          roleEnterList.add(_account, 1);
        }
      } else if (msg_id === MsgEnum.UserHeartbeat) {
        var _guid = nodeMessage.getUint64();
        var _socket = guidList.getValue(_guid);
        if (_socket) {
          _socket.send(msg);
        }
      } else if (msg_id === MsgEnum.KickUser) {
        var _guid = nodeMessage.getUint64();
        var _socket = guidList.getValue(_guid);
        if (_socket) {
          _socket.close();

          console.log("kick user, guid:" + _guid);

          var account = _socket.logicAccount;
          var guid = _socket.logicGuid;
          if (guid) {
            guidList.remove(guid);
          }
          if (account) {
            addKickedAccount(account);
            accountList.remove(account);
            roleEnterList.remove(account);
          }

          _socket.logicGuid = null;
          _socket.logicAccount = null;
        }
      } else if (msg_id === MsgEnum.NodeRegisterResult) {
        var _IsOk = nodeMessage.getUint32();
        if (_IsOk != 0) {
          registered = true;
          console.log("register node ok.");
          if (!inited) {
            inited = true;
            serverLogic.init();
          }
        }
      } else {
        var _guid = nodeMessage.getUint64();
        if (_guid) {
          var _socket = guidList.getValue(_guid);
          if (_socket) {
            try {
              _socket.send(msg);
            } catch (ex) {
              console.log("_socket.send exception:" + ex);
            }
          }
        } else {
          var _account = nodeMessage.getString();
          if (_account) {
            var _socket = accountList.getValue(_account);
            if (_socket) {
              _socket.send(msg);
            }
          }
        }
      }
    }
  } catch (ex) {
    console.log("oncoremessage exception:" + ex + " msg:" + msg);
  }
}

var mainLogic = {};
mainLogic.Start = function () {
  tick_handle = setInterval(function () {
    try {
      if (registered === false) {
        var name = core_service_name();
        nodeMessageForWrite.initForNodeRegister(MsgEnum.NodeRegister, name);
        var str = nodeMessageForWrite.toBase64();
        core_send_message_by_name("UserSvr", str);
        console.log("!!post to userserver msg: " + str);
      } else {
        var name = core_service_name();
        core_send_command_by_name(name, "Echo");
        console.log("!!post Echo " + name);
      }
    } catch (ex) {
      console.log("ontick exception:" + ex);
    }
  }, 8000);
}

mainLogic.Start();
