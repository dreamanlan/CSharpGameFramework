using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using tutorial;
using SharpSvrAPI;
using SharpSvrAPI.Messenger;

public class TestMessenger2 : SharpSvr<TestMessenger2>
{
  public override void OnInit(string[] parameters)
  {
    Console.WriteLine("TestMessenger2 Initialize");
    messenger_ = new PBMessenger((byte)Channel.Count, SvrAPI);
    messenger_.AddChannel((byte)Channel.A, Assembly.GetExecutingAssembly());
    messenger_.To((byte)Channel.A).Register<Person>((msg, fh, s) =>
        {
          Console.WriteLine("Get message Person:");
          Console.WriteLine("Id   : " + msg.Id);
          Console.WriteLine("Name : " + msg.Name);
          Console.WriteLine("Email: " + msg.Email);
      
          Console.WriteLine("TestMessenger2 Send Message");

          messenger_.To((byte)Channel.A).Send("TestMessenger1",
              Person.CreateBuilder().SetId(2)
                                    .SetName("colin")
                                    .SetEmail("colinisalsome@gmail.com")
                                    .Build());
        });

    Console.WriteLine("TestMessenger2 Online");
  }

  unsafe public override void OnMessage(uint source_handle, uint dest_handle,
                                        uint session, byte msg_type,
                                        byte* data, ushort len)
  {
    messenger_.Dispatch(source_handle, dest_handle, session, msg_type, data, len);
  }

  public override void OnFinalize()
  {
  }

  private PBMessenger messenger_;
}