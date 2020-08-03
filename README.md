## 基于Asp.Net Core 3.1开发的Api后台框架，方便快速搭建新项目的框架。
1. #### 项目名称重命名工具在QuickAdmin项目下，名称为`ProjectRenameExecutable.zip`，解压运行即可。最新版本的工具可以从这里获取 [重命名工具](https://github.com/stwhh/ProjectRename "ProjectRename")。
2. #### 也可以运行项目里面的`gen_tpl.ps1`或者`gen_tpl.sh`脚本或者自己在命令行输入`dotnet new -i .`制作项目模版，后面只需要用 `dotnet new qadmin -n newProjectName`即可基于该模版生成新的项目。对模版不太熟悉的可参考 [链接](https://www.cnblogs.com/deepthought/p/11373537.html)

>切记：***不管是用重命命名工具还是模版生成新的项目，`appsettings.json`里面的数据库连接字符串里面的`Initial Catalog`库名也会被重命名，记得跟实际项目的DB保持一致。***
