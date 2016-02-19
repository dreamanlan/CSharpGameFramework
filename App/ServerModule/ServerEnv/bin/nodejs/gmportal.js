var exec = require('child_process').exec;
var fs = require('fs');
var querystring = require('querystring');
var crypto = require('crypto');
var hash = require("./hash");
var config = require("./config");
var WebSocketServer = require('ws').Server;
var MsgEnum = require('./DashFireGmMsgEnum').DashFireMessage.GmMessageDefine;

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

var gmAccountTable = new hash.HashTable();
var ipListTable = new hash.HashTable();

var accountList = new hash.HashTable();
var sessionList = new hash.HashTable();
var nextHttpSessionId = 1;

var c_LoginMonitorInterval = 5;
var c_MaxLoginCountPerIp = 0;
var loginQueueForIp = new hash.HashTable();

var lockIndex = 0;
var kickedAccounts = [{}, {}];

var httpServ = (cfg.ssl) ? require('https') : require('http');
var httpServInst = null;
var processRequest = function (req, res) {  
  try {
    var chunks = [];
    var length = 0;
    var rows = null;
    req.on('data', function(data){
      try {
        chunks.push(data);
        length += data.length;
      } catch (ex) {
        console.log("processRequest req.on 'data' exception:" + ex);
      }
    }); 
    req.on('end', function(){
      try {
        var rows = new Buffer(length);
        var len = 0;
        for (var i = 0, il = chunks.length; i < il; i++) {
          chunks[i].copy(rows, len);
          len += chunks[i].length;
        }
        var ip = trimString(getClientIp(req));
        var args = querystring.parse(rows.toString());
        var key = ip+"_"+nextHttpSessionId++;
        res.httpSessionKey = key;
        args.httpSessionKey = key;
        sessionList.add(key, res);
        
        var jsonStr = JSON.stringify(args);
        console.log(jsonStr);
        
        if(ipListTable.containsKey(ip)) {
          var wid = args["m_WorldId"];
          core_send_message_by_name("Lobby"+wid, MsgEnum.MsgForWeb+"|"+jsonStr);
        } else {
          console.log("illegal ip: "+ip);
          /*
          ipListTable.visit(function(ip,val){
            console.log(""+ip+" -> "+val);
          });
          */
        }
      } catch (ex) {
        console.log("processRequest req.on 'end' exception:" + ex);
      }
    });
    res.on('close', function(){
    	try {
	    	var key = res.httpSessionKey;
	    	if(key) {
	    		sessionList.remove(key);
	    	}
	    	res.end(JSON.stringify({status:0}));
	    	console.log("onclose key:" + key);
      } catch (ex) {
        console.log("processRequest res.on 'close' exception:" + ex);
      }
    });
  } catch (ex) {
    console.log("processRequest exception:" + ex);
  }
};

if (cfg.ssl) {
  httpServInst = httpServ.createServer({
    key: fs.readFileSync(cfg.ssl_key),
    cert: fs.readFileSync(cfg.ssl_cert)

  }, processRequest);

} else {
  httpServInst = httpServ.createServer(processRequest);
}

function loadIpListTable() {
  try{
    var content = fs.readFileSync("nodejs/iplist.txt",{encoding:"ascii"});
    var lines = content.split("\n");
    for(var i in lines){
      var line = trimString(lines[i]);
      if(line && line.length>0){
        ipListTable.add(line,true);
        console.log("legal ip: "+line);
      }
    }
  } catch(ex) {
    console.log("loadIpListTable exception:" + ex);
  }
}

function loadGmAccountTable() {
  try{
    var content = fs.readFileSync("nodejs/GmAccount.txt",{encoding:"ascii"});
    var lines = content.split("\n");
    for(var i in lines){
      var line = lines[i];
      var fields = line.split(",");
      if(fields.length==2){
        var key = trimString(fields[0]);
        var val = trimString(fields[1]);
        gmAccountTable.add(key,val);
        
        console.log("gm:"+key+","+val);
      }
    }
  } catch(ex) {
    console.log("loadGmAccountTable exception:" + ex);
  }
}

function saveGmAccountTable() {
  try{
    var lines=new Array();
    gmAccountTable.visit(function(key,val){
      lines.push(key+","+val);
    });
    var content = lines.join("\n");
    fs.writeFileSync("nodejs/GmAccount.txt",content,0,{encoding:"ascii"});
  } catch(ex) {
    console.log("saveGmAccountTable exception:" + ex);
  }
}

function modifyPassword(account, oldpwd, newpwd) {
  try{
    var oldPwd=md5sum(oldpwd);
    var newPwd=md5sum(newpwd);
    var pwd = gmAccountTable.getValue(account);
    if(null!=pwd && (oldPwd==pwd || pwd.length==0)){
      gmAccountTable.add(account,newPwd);
      saveGmAccountTable();
      return 1;
    } else {
      return 0;
    }
  } catch(ex) {
    console.log("modifyPassword exception:" + ex);
    return 0;
  }
}

function md5sum(content){
  try{
    var hash = crypto.createHash('md5');
    hash.update(content);
    return hash.digest('hex');
  } catch(ex) {
    console.log("md5sum exception:" + ex);
    return "";    
  }
}

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

function trimString(str) {
  try{
    return str.replace(/(^\s*)|(\s*$)/g,"");
  } catch (ex) {
    return str;
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
  console.log("Server runing at port: " + PORT + ".");
  
  loadIpListTable();
  loadGmAccountTable();
  saveGmAccountTable();
  //modifyPassword("dreaman","","123456");

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
              if (account) {
                accountList.remove(account);                
              }

              var ip = getClientIp(socket.upgradeReq);
              console.log("socket " + socketKey + " disconnect for timeout, account:" + account + " ip:" + ip + " time:" + getTimeString());

              socket.logicAccount = null;
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
        var ix = arg.indexOf('|');
        if (ix >= 0) {
          var ix2 = arg.indexOf('|', ix + 1);
          var msgId = parseInt(arg.substr(0, ix));
          var msgBody;
          var target = null;
          var argWithoutTarget = arg;
          if (ix2 > 0) {
            msgBody = arg.substr(ix + 1, ix2 - ix - 1);
            var ix3 = arg.indexOf('|', ix2 + 1);
            if(ix3 > 0) {
              target = arg.substr(ix3 + 1);
              argWithoutTarget = arg.substr(0, ix3);
            }
          } else {
            msgBody = arg.substr(ix + 1);
          }

          switch (msgId) {
            case MsgEnum.AccountLogin:
              {
                var jsonObj = jsonParse(msgBody);
                if (null != jsonObj) {
                  var account = jsonObj.m_Account;
                  var password = md5sum(jsonObj.m_Password);
                  if (account) {
                    if (!isKickedAccount(account)) {
                      if (canLogin(socket)) {
                        var ip = getClientIp(socket.upgradeReq);
                        var pwd = gmAccountTable.getValue(account);
                        if(null!=pwd && (pwd==password || pwd.length==0)){
                          socket.logicAccount = account;
                          accountList.add(account, socket);
                          socket.send(MsgEnum.AccountLoginResult+"|{\"m_Result\":1}");
                          console.log("login success, account:" + account + " ip:" + ip + " time:" + getTimeString());
                        } else {
                          socket.send(MsgEnum.AccountLoginResult+"|{\"m_Result\":0}");
                          console.log("login failed, account:" + account + " ip:" + ip + " time:" + getTimeString());
                        }
                      } else {
                        console.log("login refuse for too many login ! account:" + account + " ip:" + ip + " time:" + getTimeString());
                      }
                    }
                  }
                }
              }
              break;
            case MsgEnum.AccountLogout:
              {
                var ip = getClientIp(socket.upgradeReq);
                var jsonObj = jsonParse(msgBody);
                if (null != jsonObj && jsonObj.m_Account==socket.logicAccount) {
                  try {                    
                    accountList.remove(jsonObj.m_Account); 
                    console.log("account "+ jsonObj.m_Account + " logout. ip:" + ip + " time:" + getTimeString());
                  } catch(ex) {
                    console.log("account logout exception:" + ex + " msg:" + arg);                    
                  }
                }
              }
              break;
            case MsgEnum.ModifyPassword:
              {
                var ip = getClientIp(socket.upgradeReq);
                var jsonObj = jsonParse(msgBody);
                if (null != jsonObj && jsonObj.m_Account==socket.logicAccount) {
                  try {
                    var account = jsonObj.m_Account;
                    var oldPwd = jsonObj.m_Password;
                    var newPwd = jsonObj.m_NewPassword;
                    if(modifyPassword(account,oldPwd,newPwd)==1){
                      socket.send(MsgEnum.ModifyPasswordResult+"|{\"m_Result\":1}");
                      console.log("modify password success, account:" + account + " ip:" + ip + " time:" + getTimeString());
                    } else {
                      socket.send(MsgEnum.ModifyPasswordResult+"|{\"m_Result\":0}");
                      console.log("modify password failed, account:" + account + " ip:" + ip + " time:" + getTimeString());
                    }
                  } catch(ex) {
                    console.log("modify password exception:" + ex + " msg:" + arg);                    
                  }
                }
              }
              break;
            case MsgEnum.UserHeartbeat:
              {
                var socketKey = socket.upgradeReq.headers['sec-websocket-key'];
                var ip = getClientIp(socket.upgradeReq);
                socket.logicLifeCount = 6;
                //echo
                socket.send(arg);
              }
              break;
            case MsgEnum.MsgForWeb:
              {
                var ip = getClientIp(socket.upgradeReq);
                //error!!!
                console.log("illegal message:" + arg + " ip:" + ip + " time:" + getTimeString());
              }
              break;
            default:
              var jsonObj = jsonParse(msgBody);
              if (null != jsonObj) {
                var socketKey = socket.upgradeReq.headers['sec-websocket-key'];
                if (!socket.logicAccount) {
                  console.log("msg " + arg + " from unknown socket " + socketKey);
                  break;
                } else {
                  var account = socket.logicAccount;
                  if (jsonObj["m_Account"] && jsonObj["m_Account"] != account) {
                    console.log("msg " + arg + " from socket " + socketKey + " (account " + account + "), account is different !");
                    break;
                  }
                }
                if(target){
                  core_send_message_by_name(target, argWithoutTarget);
                  console.log("post to " + target + " msg: " + argWithoutTarget);
                } else {
                  console.log("unknown msg: " + arg);
                }
              } else {
                break;
              }
              break;
          }
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
          if (account) {
            accountList.remove(account);            
          }

          console.log("socket " + socketKey + " disconnect, account:" + account);

          socket.logicAccount = null;
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
          if (account) {
            accountList.remove(account);            
          }

          console.log("socket " + socketKey + " network error, account:" + account);

          socket.logicAccount = null;
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
    console.log("@@node.js receive core message:" + handle + "," + session + "," + msg);
    var target = core_query_service_name(handle);
    
    var ix = msg.indexOf('|');
    if (ix >= 0) {
      var ix2 = msg.indexOf('|', ix + 1);
      var msg_id = parseInt(msg.substr(0, ix));
      var msg_tmp;
      if (ix2 >= 0)
        msg_tmp = msg.substr(ix + 1, ix2 - ix - 1);
      else
        msg_tmp = msg.substr(ix + 1);
      var msg_body = jsonParse(msg_tmp);
      if (null == msg_body) {
        return;
      }
      
      if(msg_id == MsgEnum.MsgForWeb){
      	var key = msg_body.httpSessionKey;
      	if(key) {
      		var res = sessionList.getValue(key);
      		if(res) {
	      		res.end(msg_tmp);
	      		
	      		sessionList.remove(key);
	      	} else {
	      		console.log("MsgForWeb can't find response object, " + msg);
	      	}
      	}
      } else {
	      var _account = msg_body["m_Account"];
	      if (_account) {
	        var _socket = accountList.getValue(_account);
	        if (_socket) {          
	          _socket.send(msg+"|"+target);
	        }
	      }
			}
    } else if (msg == "QuitNodeJs") {
      saveGmAccountTable();
      core_quit_nodejs();
    } else if (msg.indexOf("EvalScp ") == 0) {
      eval(msg.substring(8));
      console.log("oncoremessage " + msg);
    }
  } catch (ex) {
    console.log("oncoremessage exception:" + ex + " msg:" + msg);
  }
}

var tick_handle = null;
var mainLogic = {};
mainLogic.Start = function () {
  tick_handle = setInterval(function () {
    try {
      var name = core_service_name();
      core_send_command_by_name(name, "Echo");
      console.log("!!post Echo " + name);
    } catch (ex) {
      console.log("ontick exception:" + ex);
    }
  }, 8000);
}

mainLogic.Start();

serverLogic.init();
