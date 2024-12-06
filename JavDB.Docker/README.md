# JavDB.Docker  
JavDB.Docker 支持容器化部署，可以根据需要自行编译镜像。  

## 镜像编译  
示例：  
1、通过文件管理器或SSH上传编译后的项目文件到 /home/javdb.docker 目录。  
2、执行命令：cd /home/javdb.docker  
3、执行命令：docker build -t karevin/javdb.docker:1.0.1 .  
4、使用命令行或Docker管理器启动镜像。  
5、需要映射的目录：/app/config【用于存储配置文件】、/moive【需要检测的目标文件夹】、/movie2.......  