using JavDB.Film;
using JavDB.Services.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml;

namespace JavDB.Services
{
    internal class HTTPServices
    {
        private HttpListener HTTPRequest;
        private string root;
        private string index;
        private string h404 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "html", "404.html");
        private MIME m_MIME;
        Grappler grappler = new Grappler(File.OpenText("JavDB.Film.json").ReadToEnd());
        public HTTPServices(string root, string index)
        {
            HTTPRequest = new HttpListener();
            //this.root = AppDomain.CurrentDomain.BaseDirectory + root;
            this.root = root;
            this.index = index;
            m_MIME = new MIME();
        }
        public bool Satrt(string url)
        {
            bool isStart = false;
            try
            {
                Program.Debug("正在启动服务。");
                HTTPRequest.Prefixes.Add(url);
                HTTPRequest.Start();
                Program.Debug("服务已启动。");
                Thread ThrednHttpPostRequest = new Thread(new ThreadStart(HTTPRequestHandle));
                ThrednHttpPostRequest.IsBackground = true;
                ThrednHttpPostRequest.Start();
                isStart = true;
            }
            catch (Exception e1)
            {
                isStart = false;
            }
            return isStart;
        }
        public void Stop()
        {
            if (HTTPRequest.IsListening)
                HTTPRequest.Stop();
            Program.Debug("正在关闭服务。");
        }

        private void HTTPRequestHandle()
        {
            while (HTTPRequest.IsListening)
            {
                HttpListenerContext requestContext;
                try
                {
                    requestContext = HTTPRequest.GetContext();
                }
                catch
                {
                    continue;
                }
                Thread threadsub = new Thread(new ParameterizedThreadStart((requestcontext) =>
                {
                    try
                    {
                        HttpListenerContext? request = requestcontext as HttpListenerContext;
                        if (request == null) return;
                        object result;
                        string output = "";
                        Program.Debug($"Method:{request.Request.HttpMethod},RemoteHost:{request.Request.RemoteEndPoint},Url:{request.Request.RawUrl}");
                        try
                        {
                            if (request.Request.HttpMethod == "POST")
                            {
                                //获取Post过来的数据
                                Stream stream = request.Request.InputStream;
                                StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                                string body = reader.ReadToEnd();
                                Program.Debug($"Request:{body}");
                                var json = JsonDocument.Parse(body);
                                string? uid = json.RootElement.GetProperty("uid").GetString();
                                if (string.IsNullOrEmpty(uid)) throw new Exception(nameof(uid));
                                bool basic = false;
                                try
                                {
                                    basic = json.RootElement.GetProperty("basic").GetBoolean();
                                }
                                catch { };
                                result = new { code = "0", message = "响应成功", data = grappler.Grab(uid, basic) };
                                output = JsonSerializer.Serialize(result, Grappler.SerializerOptions);
                                request.Response.StatusCode = (int)HttpStatusCode.OK;
                                request.Response.ContentType = "application/Json";
                                byte[] buffer = Encoding.UTF8.GetBytes(output);
                                request.Response.ContentLength64 = buffer.Length;
                                request.Response.OutputStream.Write(buffer, 0, buffer.Length);
                                Program.Debug($"Response:{output}");
                            }
                            else if (request.Request.HttpMethod == "GET")
                            {
                                string name = root + request.Request.RawUrl.Replace('/', Path.DirectorySeparatorChar);

                                if (!File.Exists(name))
                                {
                                    if (!Directory.Exists(name))
                                    {
                                        throw new FileNotFoundException("404 FileNotFound");
                                    }
                                    if (name.EndsWith("/"))
                                        name += $"{index}";
                                    else
                                        name += Path.Combine(name, $"{index}");
                                }
                                if (!File.Exists(name))
                                {
                                    throw new FileNotFoundException("404 FileNotFound");
                                }
                                request.Response.StatusCode = (int)HttpStatusCode.OK;
                                string ex = Path.GetExtension(name);
                                request.Response.ContentType = m_MIME.GetValue(ex);
                                using (FileStream fileStream = new FileStream(name, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                                {
                                    request.Response.ContentLength64 = fileStream.Length;
                                    fileStream.CopyTo(request.Response.OutputStream);
                                }
                                Program.Debug("Response:OK");
                            }
                            else
                            {
                                throw new NotSupportedException($"NotSupportedException:{request.Request.HttpMethod}");
                            }
                        }
                        catch (FileNotFoundException e1)
                        {
                            if (File.Exists(h404))
                            {
                                request.Response.StatusCode = (int)HttpStatusCode.OK;
                                request.Response.ContentType = "text/html";
                                using (FileStream fileStream = new FileStream(h404, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                                {
                                    request.Response.ContentLength64 = fileStream.Length;
                                    fileStream.CopyTo(request.Response.OutputStream);
                                }
                            }
                            else
                                request.Response.StatusCode = (int)HttpStatusCode.NotFound;
                        }
                        catch (Exception e1)
                        {
                            result = new { code = "-1", message = e1.Message };
                            output = JsonSerializer.Serialize(result, Grappler.SerializerOptions);
                            request.Response.StatusCode = (int)HttpStatusCode.OK;
                            byte[] buffer = Encoding.UTF8.GetBytes(output);
                            request.Response.ContentLength64 = buffer.Length;
                            request.Response.OutputStream.Write(buffer, 0, buffer.Length);
                            request.Response.ContentType = "application/json";
                        }
                        finally
                        {
                            //request.Response.Headers.Add(".Access-Control-Allow-Private-Network", "true");
                            //request.Response.Headers.Add(".Access-Control-Allow-Origin", "*");
                            //request.Response.Headers.Add(".Access-Control-Allow-Methods", "*");
                            //request.Response.Headers.Add(".Access-Control-Allow-Headers", "*");
                            //request.Response.Headers.Add(".Access-Control-Request-Headers", "*");
                            requestContext.Response.ContentEncoding = Encoding.UTF8;
                            request.Response.OutputStream.Close();
                        }
                    }
                    catch (Exception e1)
                    {
                    }
                }));
                threadsub.Start(requestContext);
            }
        }

    }
}
