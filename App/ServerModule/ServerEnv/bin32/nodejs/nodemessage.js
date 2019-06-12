exports.NodeMessage = function () {
  var g_Buffer = null;
  var g_Id = 0;
  var g_HeaderSize = 0;
  var g_Cursor = 0;
  
  //nodejs接收到的nodemessaeg只会出现如下类型数据，且每种类型至多出现一次
  var g_String = null;
  var g_Uint64 = 0;
  var g_Uint32 = 0;
  
  var c_Uint32PlusOne = 0x100000000;
  var c_MaxWriteBuffer = 4096;
  
  function readVarInt(){
    var val = 0;
    var lf = 0;
    var haveLeft = false;
    for(var i=0;i<4 && g_Cursor<g_HeaderSize+4;++i){
      var v = g_Buffer.readUInt8(g_Cursor++);
      if((v & 0x80)==0x80){
        val+=((v & 0x7f)<<lf);
        lf+=7;
        haveLeft = true;
      }else{
        val+=(v<<lf);
        lf+=7;
        haveLeft = false;
        break;
      }
    }
    if(haveLeft){
      lf=0;
      var valHigh = 0;  
      for(var i=0;i<4 && g_Cursor<g_HeaderSize+4;++i){
        var v = g_Buffer.readUInt8(g_Cursor++);
        if((v & 0x80)==0x80){
          valHigh+=((v & 0x7f)<<lf);
          lf+=7;
        }else{
          valHigh+=(v<<lf);
          lf+=7;
          break;
        }
      }
      //console.log("read64 "+val+" "+valHigh);
      return Math.abs(val) + Math.abs(valHigh)*c_Uint32PlusOne;
    } else {
      //console.log("read32 "+val);
      return Math.abs(val);
    }
  }
  
  function writeVarInt(arg){
    var val=arg;
    if(val>=c_Uint32PlusOne){
      var valHigh = Math.floor(val/c_Uint32PlusOne);
      val = (val % c_Uint32PlusOne);
      var mask = 0x7f;
      var unmask = 0xffffff00;
      var lf=7;
      for(var i=0;i<4;++i){      
        g_Buffer.writeUInt8((val & mask) | 0x80, g_Cursor++);
        val>>>=lf;
      }
      for(var i=0;i<4;++i){
        if((valHigh & unmask)!=0){
          g_Buffer.writeUInt8((valHigh & mask) | 0x80, g_Cursor++);
          valHigh>>>=lf;
        }else{
          g_Buffer.writeUInt8(valHigh & mask, g_Cursor++);
          break;
        }
      }
    }else{
      var mask = 0x7f;
      var unmask = 0xffffff00;
      var lf=7;
      for(var i=0;i<4;++i){
        if((val & unmask)!=0){
          g_Buffer.writeUInt8((val & mask) | 0x80, g_Cursor++);
          val>>>=lf;
        }else{
          g_Buffer.writeUInt8(val & mask, g_Cursor++);
          break;
        }
      }
    }
  }
  
  function read(){
    var v = readVarInt();
    var type = (v & 0x07);
    var ix = (v >>> 3);
    if(type==0){
      var val = readVarInt();
      if(val>=c_Uint32PlusOne){
        g_Uint64 = val;
      }else{
        g_Uint32 = val;
        g_Uint64 = val;
      }
    }else if(type==2){
      var len = readVarInt();
      g_String = g_Buffer.toString('utf8',g_Cursor,g_Cursor+len);
      g_Cursor+=len;
    }
  }
  
  /**
    node消息格式：
    2字节id
    2字节node头长度
    node消息proto-buf编码
    proto数据部分proto-buf编码（nodejs不读取此部分）
  */  
  this.initForRead=function(msgstr){
    try{
      g_Buffer = Buffer.from(msgstr,'base64');
      g_Cursor = 0;
      g_Id = g_Buffer.readUInt16LE(g_Cursor);
      g_Cursor += 2;
      g_HeaderSize = g_Buffer.readUInt16LE(g_Cursor);
      g_Cursor += 2;
      
      g_String = null;
      g_Uint64 = 0;
      g_Uint32 = 0;
      
      while(g_Cursor<g_HeaderSize+4){
        read();
      }
    }catch(ex){
      console.log(""+ex);
    }
  }
  
  this.getId=function(){
    return g_Id;
  }
  
  this.getString=function(){
    return g_String;
  }
  
  this.getUint64=function(){
    return g_Uint64;
  }
  
  this.getUint32=function(){
    return g_Uint32;
  }
  
  
  this.initForWrite=function(id){
    g_Buffer = Buffer.alloc(c_MaxWriteBuffer);
    g_Cursor = 0;
    g_Id = id;
    g_Buffer.writeUInt16LE(id,g_Cursor);
    g_Cursor+=2;
    g_Buffer.writeUInt16LE(0,g_Cursor);
    g_Cursor+=2;
    
    g_String = null;
    g_Uint64 = 0;
    g_Uint32 = 0;
  }
  
  this.writeString=function(index, arg){
    var len=arg.length;
    writeVarInt((index<<3)+2);
    writeVarInt(len);
    g_Buffer.write(arg,g_Cursor,len,'utf8');
    g_Cursor+=len;
  }
  
  this.writeInt=function(index, arg){
    writeVarInt(index<<3);
    writeVarInt(arg);
  }
  
  this.initForNodeMessageWithAccount=function(id,account){    
    g_Buffer = Buffer.alloc(c_MaxWriteBuffer);
    g_Cursor = 0;
    g_Id = id;
    g_Buffer.writeUInt16LE(id,g_Cursor);
    g_Cursor+=2;
    g_Buffer.writeUInt16LE(0,g_Cursor);
    g_Cursor+=2;
    
    this.writeString(1,account);
    var headerSize = g_Cursor-4;
    g_Buffer.writeUInt16LE(headerSize,2);
    
    g_String = null;
    g_Uint64 = 0;
    g_Uint32 = 0;
  }
  
  this.initForNodeMessageWithGuid=function(id,guid){    
    g_Buffer = Buffer.alloc(c_MaxWriteBuffer);
    g_Cursor = 0;
    g_Id = id;
    g_Buffer.writeUInt16LE(id,g_Cursor);
    g_Cursor+=2;
    g_Buffer.writeUInt16LE(0,g_Cursor);
    g_Cursor+=2;
    
    this.writeInt(1,guid);
    var headerSize = g_Cursor-4;
    g_Buffer.writeUInt16LE(headerSize,2);
    
    g_String = null;
    g_Uint64 = 0;
    g_Uint32 = 0;
  }
  
  this.initForNodeRegister=function(id,nodeName){    
    g_Buffer = Buffer.alloc(c_MaxWriteBuffer);
    g_Cursor = 0;
    g_Id = id;
    g_Buffer.writeUInt16LE(id,g_Cursor);
    g_Cursor+=2;
    g_Buffer.writeUInt16LE(0,g_Cursor);
    g_Cursor+=2;
    
    this.writeString(1,nodeName);
    var headerSize = g_Cursor-4;
    g_Buffer.writeUInt16LE(headerSize,2);
    
    g_String = null;
    g_Uint64 = 0;
    g_Uint32 = 0;
  }
  
  this.toBase64=function(){
    return g_Buffer.toString('base64',0,g_Cursor);
  }
}