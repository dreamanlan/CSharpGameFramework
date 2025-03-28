# CSharpGameFramework

一个极简的基于unity3d引擎与c#语言的游戏框架/架构（包括客户端与服务器）。本工程使用[ServerPlatform](https://github.com/dreamanlan/ServerPlatform)作为服务端基础通信设施。

现代计算机硬件与系统软件采用分层虚拟机的方式很好的实现了松散耦合并保证功能完备。受此启发，我在想是否也能采用分层虚拟机的样式来实现游戏框架，框架部分就像操作系统内核，游戏具体逻辑就像操作系统的其他部分，在内核提供的机制上运作起来。然后内核是可以与其他部分完全隔离开来的。

想要实现完全隔离，计算机硬件与软件是一个实例，任何时候都可以把硬件上的软件部分完全抹去后重装。从这个角度看，除了逻辑上的松散耦合外，还需要一种部署上的区分来保证隔离，就像硬件一样，这部分提供的是运行环境，与在环境里运行的软件有明显的区分。

硬件提供给软件的是指令集，从这个角度理解虚拟机，我们可以看到支持脚本的软件系统，软件系统与脚本的关系就像硬件与软件的关系一样。系统提供了运行环境，而脚本在环境里运行，二者有明显的区分。我们也可以像重装系统一样，在不改变软件系统的情况下，抹去整个脚本后再重新安装。

就像现代软件系统并不会直接使用机器指令来编写一样，每一级分层虚拟机对下一层必须提供能支持更好的抽象或更贴近使用者习惯的工具语言。我们用C#来实现游戏框架，如果使用lua或类似的脚本语言来实现游戏逻辑，虽然也实现了松散耦合与完全隔离，由于通用脚本与编译类语言提供的是同样的抽象级别，这就像只能用硬件指令编写软件一样，显然不是比较好的方式（而且脚本语言通常较编译语言更难进行模块组织、错误检查与调试以及更低的运行性能）。所以我考虑用DSL作为这一层虚拟机提供给游戏逻辑的工具语言。

DSL是我个人从事软件开发以来逐渐演化出的一种通用元语言语法，也就是说只有基础语法是确定的，具体语法与语义由DSL作为元语言在具体应用时再定义，所以，这个游戏框架也是在尝试形成一种适合游戏逻辑开发的语言（语法稍稍受限于DSL的元语言语法，语义完全由游戏框架实现）。

另外一面，我在想尝试在策划层面提供尽可能少的机制来实现一个游戏，这可能只是一个不切实际的梦。不过当我反复看过我自己主导程序设计与开发的几个游戏项目后，我执着的认为这个方向或许是有希望的。因为我看到太多系统层面的不协调与冲突，以及大量的重复。即便我想像的尽可能少的游戏机制的集合最后证明是过于理想了，这个过程也应该会给程序与策划交叉的领域带来一些思考吧。

不管怎样，一个优秀的软件系统（程序层面）应该有一个优美的体系，一个优秀的游戏系统（策划层面）也应该是这样，就像数学与物理一样，简单、优雅并且广泛适用。

[windows平台用法]

1、buildall.bat是编译client与server端的批处理（调用2与3）

2、buildclient.bat编译client，编译完成会拷贝相关文件到unity3d工程相应目录

3、buildserver.bat编译server，编译完成会拷贝相关文件到server端运行环境目录(App/ServerModule/ServerEnv)

4、RunServer.cmd是启动服务器的批处理

5、StopServer.cmd是关闭服务器的批处理，其实就是kill掉服务器各进程

6、copyroomdlls.bat是单独拷贝room server的exe/dll的批处理，一般用于room server开发时

7、copyserverdlls.bat是单独拷贝各server的exe/dll的批处理，包括lobby、user server、room server与data cache服务器，主要用在开发服务端功能时

8、dslcopy.bat处理dsl与表格文件，并拷贝到server端与unity3d工程的相应目录，一般用于仅修改dsl或表格文件时

9、generate_file_list.bat用于生成unity3d工程的streamingassets目录下的文件的列表文件list.txt，这个文件用于安卓与iOS上记录打包的散文件列表，开发中一般用不着
