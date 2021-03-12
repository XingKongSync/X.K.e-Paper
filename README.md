# X.K.e-Paper
XingKong e-Paper
基于树莓派和微雪电子墨水屏实现，可以显示时间、天气、温度以及系统状态，支持编写APP扩展功能。<br/>
功能介绍：[http://www.bilibili.com/video/av9109903/](http://www.bilibili.com/video/av9109903/)<br/>

## 关键组件 
| 名称 | 说明 |
|-|-|
AppLoader | 程序主入口，提供了用于加载 App 的基础环境，启动后会自动加载 SystemUI
SystemUI | 一个特殊的 App，用于显示程序主界面
RemoteController | 可以广播并发现局域网中的 X.K.e-paper 并支持向其发送按键消息
XingKongForm | 提供了一系列 UI 交互控件，封装了底层的绘制操作
XingKongFormGenerator | 一个帮助 App 开发者用可视化的方式来生成界面的工具
AppBase | App 的基类
RemoteSerialPort | 串口服务器，用于接收网络上发来的 TCP 数据，并转发到本机指定的串口

## 目录结构
程序的目录结构示例如下：
```
└── X.K.e-Paper
    ├── AppBase.dll
    ├── AppForm.json
    ├── AppLoader.exe
    ├── AppLoader.exe.config
    ├── Apps
    │   ├── PhotoLibrary
    │   │   ├── test_image.jpg
    │   │   ├── MainForm.json
    │   ├── PhotoLibrary.dll
    │   ├── RemotePlayer
    │   │   ├── album.png
    │   │   └── RemotePlayerForm.json
    │   └── RemotePlayer.dll
    ├── asciiLogo.txt
    ├── Config.json
    ├── MainForm.json
    ├── Newtonsoft.Json.dll
    ├── Newtonsoft.Json.xml
    ├── ShutdownForm.json
    ├── StartingForm.json
    ├── SystemUI
    │   ├── APP.BMP
    │   └── 省略其他资源图片
    ├── SystemUI.dll
    ├── XingKongForm.dll
    └── XingKongUtils.dll
```
App 应当放入 Apps 文件夹中，dll 在 Apps 目录下，该 App 所要使用的资源放在 App 同名目录中

## 配置文件
程序的配置保存在 Config.json 中，示例如下：
```Json
{
	"PortName": "/dev/ttyUSB0",
	"BackupPortName": "/dev/ttyUSB1"
}
```
`PortName` 是程序连接屏幕所使用的串口名称，如果在 Windows 环境中则类似于 `COM1`

有些情况下，由于某些原因，串口号可能会变化（比如硬件性能不稳定导致断开后又自动连上），此时可以通过指定 `BackupPortName` 来设置一个备用串口名称，当主串口无法打开时，会自动切换到备用串口

## 远程调试
AppLoader 启动时可以远程连接另一台主机的 e-paper，这样方便开发和调试，具体步骤如下：
1. 在连接了 e-paper 的计算机中启动 RemoteSerialPort，命令行参数中传入串口名称
1. 在开发机上启动 AppLoader，命令行中传入 `-ip xxx.xxx.xxx.xxx`，其中 `xxx.xxx.xxx.xxx` 请替换为上一步中计算机中的 IP 地址

## 用户交互
目前程序仅支持模拟按键式的交互方式。在 XingKongForm 中提供的 Keyboard 对象会在本机监听 UDP 9849 端口，并接收长度为 4 个 byte 的消息，这 4 bytes 会被转换为 .Net 中的 `System.Windows.Forms.Keys` 枚举

App 开发者可以通过绑定 `KeyPressed` 事件来处理按键消息

注意：当 App 挂起时，需要开发者手动取消绑定 `KeyPressed` 事件

## 界面生成
1. 在 `XingKongFormGenerator` 中新建一个 `Form`，并继承于 `XingKongFormBase`
1. 借助 WinForm 的图形化界面设计器，使用 Label、Button、ListBox、PictureBox、Panel 来构建你的界面
1. 在 `Program` 中将 `Form` 替换为你的 `Form`<br>
```C#
[STAThread]
static void Main(string[] args)
{
    Args = args;
    Application.EnableVisualStyles();
    Application.SetCompatibleTextRenderingDefault(false);
    Application.Run(new YourForm());
}
```
1. 在你的 Form 的 Load 事件中执行如下代码<br>
```C#
private void YourForm_Load(object sender, EventArgs e)
{
    PortName = "COM4";

    XingKongWindow window = GetXingKongWindow();
    LocalShow();//在 e-paper 上预览效果，支持远程调试，方法类似于 AppLoader
    ShowCode();
}
```
1. 运行 `XingKongFormGenerator` 后会生成界面的 json 代码，保存此 json 并在你的 App 中进行加载
