# .NET Core/.NET Framework的静态分析命令行工具
&emsp;在汲取到动态分析的一些问题，为了解决我个人的工作需求，我还是决定自己简单开发一个简单的.net静态分析工具。但是这个工具不再具备UI界面，更多的我想以命令行的形式进行开发。

## 框架与依赖
+ .net core 6
+ AsmResolver.DotNet

## 命令行
|子命令 | 意义|
|------|----|
|ps|打印进行信息|
|process|以附加进程的方式进行静态分析dll|
|path| 以本地文件路径的方式进行静态分析dll|

### ps
|参数|意义|
|---|----|
|--iis(可选)|打印w3wp的进程(即IIS特定的web进程)|
|--name(可选)|打印进程名(默认只打印pid/命令行)|

```
staticAnalysts.exe ps --iis
# 用于打印IIS特定进程
```

### process
|参数|意义|
|---|---|
|--pid(必选)|附加进程的pid|
|--shell(可选) | 可交互式分析|

```
staticAnalysts.exe process --pid <pid>
# 附件进程，然后分析进程加载的dll

staticAnalysts.exe process --pid <pid> --shell
# 此时会进入一个伪终端的命令行: 支持extend / interface / quit / help
extend <className> :用于输出目标进程程序集中哪些类集成了className
interface <interface> :用于输出目标进程程序集中哪些类实现了interface接口
quit 退出程序
```

### path
|参数|意义|
|---|---|
|--path(必选)|分析目录的地址|

```
staticAnalysts.exe path --path "C:\\"
# 不附加进程，直接分析目录下所有的dll


```

### 可选参数
|参数|意义|
|---|---|
|--pass-system|不分析System相关的dll(默认开启, ps子命令不受影响)|
|--single-assembly|不分析重名的dll文件(默认开启, ps子命令不受影响)|
|--output-type|输出类型(csv/json/console)|
|--output-dir|输出目录(当前工作目录的output文件夹)|