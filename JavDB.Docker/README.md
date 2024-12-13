# JavDB.Docker  
JavDB.Docker 支持容器化部署，可以根据需要自行编译镜像。  

## 镜像编译  
举例：  
1、要刮削的影片存放目录是 "/volume2/video" 和 "/volume2/movie"；  
2、配置文件存放路径是"/volume1/docker/javdb"；  
3、刮削时间间隔是15秒；  
4、要刮削的文件格式类型是*.mp4、*.avi、*.mkv、*.rmvb；  
操作步骤：  
1、通过文件管理器或SSH上传编译后的项目文件到 /home/javdb.docker 目录。  
2、执行命令：cd /home/javdb.docker  
3、执行命令：docker build -t karevin/javdb.docker:1.0.1 .  
4、在"/volume1/docker/javdb"上传fileEx.txt文件，文件内容如下：  
*.mp4  
*.avi  
*.mkv  
*.rmvb  
5、在"/volume1/docker/javdb"上传listen.txt文件，文件内容如下：  
/movie1  
/movie2  
6、目录映射，将物理机路径"/volume1/docker/javdb"映射到Docker容器路径"/app/config"；将物理机路径"/volume2/video"映射到Docker容器路径"/movie1"；将物理机路径"/volume2/movie"映射到Docker容器路径"/movie2"。  
7、添加环境变量"INTERVAL"，值为15000。  
8、使用命令行或Docker管理器启动镜像。  
## 命令行示例：  
docker run -d --name javdb.docker -e INTERVAL="15000" -v /volume1/docker/javdb:/app/config -v /volume2/video:/movie1 -v /volume2/movie:/movie2 karevin/javdb.docker:1.0.1  