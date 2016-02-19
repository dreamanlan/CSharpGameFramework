using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using tutorial;
using SharpSvrAPI;
using SharpSvrAPI.Messenger;

public class TestMessenger1 : SharpSvr<TestMessenger1>
{
  public override void OnInit(string[] parameters)
  {
    Console.WriteLine("TestMessenger1 Initialize");
    messenger_ = new PBMessenger((byte)Channel.Count, SvrAPI);
    messenger_.AddChannel((byte)Channel.A, Assembly.GetExecutingAssembly());
    messenger_.To((byte)Channel.A).Register<Person>((msg, fh, s) =>
        {
          Console.WriteLine("Get message Person:");
          Console.WriteLine("Id   : " + msg.Id);
          Console.WriteLine("Name : " + msg.Name);
          Console.WriteLine("Email: " + msg.Email);
        });

    Console.WriteLine("TestMessenger1 Online");
    Console.WriteLine("TestMessenger1 Send Message");

    messenger_.To((byte)Channel.A).Send("TestMessenger2",
        Person.CreateBuilder().SetId(1)
                              .SetName("llisper")
                              .SetEmail("llisperzhang@gmail.com")
                              .Build());
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
