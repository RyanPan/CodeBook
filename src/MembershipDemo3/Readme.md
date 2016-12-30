Access ODBC 驱动安装
自定义的 MembershipProvider 使用了 Access ODBC 数据源；
.Net 是64位，所以要求 ODBC 也是64位。
当前系统安装了 Office 2007，因此 Access ODBC 驱动是32位，需要安装64位驱动，在 https://www.microsoft.com/zh-cn/download/details.aspx?id=13255 下载安装程序（AccessDatabaseEngine_X64.exe）。
安装时使用 passive 选项 AccessDatabaseEngine_X64.exe /passive，参见 https://social.technet.microsoft.com/Forums/zh-CN/843de28c-d48f-4657-a430-67da8075bad9/32-bit-odbc-driver-for-ms-access-2010-accdb-is-missing-in-win7-64bit?forum=w7itproappcompat。

IIS 配置
自定义的 MembershipProvider 默认无法通过“IIS Manager -> <站点主页> -> .NET 用户”进行管理。
设置 IIS 配置文件“C:\Windows\System32\inetsrv\config\Administration.confg”节点“configuration->system.webServer->management->trustedProviders”属性“allowUntrustedProviders="true"”。
或者，可以将 Provider 打包进强签名 dll，添加到 GAC，然后在 IIS Manager 配置中添加到 trustedProviders 集合。

错误调试
除了将自定义的 Provider 放到单独的 dll 中，在站点中也有一份拷贝
自定义的 RoleProvider 添加到配置文件始终是“无法加载程序集的错误”，去掉类库签名后就好了