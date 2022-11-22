## 基于 Asp.Net Core 6 开发的 Api 后台框架，方便快速搭建新项目的框架。

1. 项目名称重命名工具在 QuickAdmin 项目下，名称为`ProjectRenameExecutable.zip`，解压运行即可。最新版本的工具可以从这里获取 [重命名工具](https://github.com/stwhh/ProjectRename 'ProjectRename')。
2. 也可以在项目根目录(gen_tpl.sh 脚本所在的目录)运行`.\gen_tpl.ps1`或者`.\gen_tpl.sh`脚本或者在命令行输入`dotnet new -i ./QuickAdmin`制作项目模版，后面只需要用 `dotnet new qadmin -n newProjectName`即可基于该模版生成新的项目。对模版不太熟悉的可参考 [链接](https://www.cnblogs.com/deepthought/p/11373537.html)

   > 切记：*不管是用重命命名工具还是模版生成新的项目，`appsettings.json`里面的数据库连接字符串里面的`Initial Catalog`库名也会被重命名，记得跟实际项目的 DB 保持一致。*

3. 这里的 Dockerfile 是依据 linux 环境的的。更多 docker 信息，建议参考[MSDN 文档](https://docs.microsoft.com/zh-cn/aspnet/core/host-and-deploy/docker/building-net-docker-images?view=aspnetcore-3.1) 和[Github Demo](https://github.com/dotnet/dotnet-docker/tree/master/samples/aspnetapp)

   ```
   docker 构建镜像
   1. 转到Dockerfile文件所在目录，运行 docker build . -t qadmin 创建镜像(名称是qadmin,tag没指定，默认latest)
   2. 根据镜像创建并运行容器 docker run -d --name quick_admin_container -p 5301:80 qadmin (注意，因为Dockerfile里面只暴露了80和443端口，
   所以这里映射的docker端口也只能是这俩个，如果映射其它端口，应该还需要手动开放docker的其它端口)
   ```

4. Code First 更新数据库：设置 xxx.DAO 项目为启动项目，打开 nuget 包管理器控制台，输入`update-database`生成数据库，生成的数据库连接配置在`QuickAdminDbContextDesignFactory`这个文件里，可自己修改。
