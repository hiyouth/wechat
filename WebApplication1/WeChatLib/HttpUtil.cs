using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace WeChat
{
    public class HttpUtil
    {
                public const string ErrorStr = "-";

        /// <summary>
        /// 发送Rest请求
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public string SendPost(string url)
        {
            try
            {
                HttpWebRequest req = (HttpWebRequest) WebRequest.Create(url);
                req.Timeout = 1000*60*30;//30分钟
                HttpWebResponse rep = (HttpWebResponse) req.GetResponse();//得到请求结果
                Stream stream = rep.GetResponseStream();
                if (stream!=null)
                {
                    using (var reader=new StreamReader(stream,Encoding.UTF8))
                    {
                        string responseHtml = reader.ReadToEnd();
                        rep.Close();
                        return responseHtml;
                    }
                }
                rep.Close();
                return null;//如果结果流为空，则返回为空
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 发送send请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="dataStr">序列化的数据</param>
        /// <returns></returns>
        public string SendPost(string url,string dataStr)
        {
            try
            {
                HttpWebRequest req = (HttpWebRequest) WebRequest.Create(url);
                req.Method = "POST";
                byte[] data = Encoding.UTF8.GetBytes(dataStr);
                req.CookieContainer=new CookieContainer();
                req.AllowAutoRedirect = true;
                req.ContentType = "text/plain";
                req.ContentLength = data.Length;
                req.Timeout = 1000*60*30;
                Stream stream = req.GetRequestStream();
                stream.Write(data,0,data.Length);//传输POST数据
                stream.Flush();
                stream.Close();

                HttpWebResponse rep = (HttpWebResponse) req.GetResponse();
                Stream resstream = rep.GetResponseStream();
                if (resstream != null)
                {
                    using ( var reader=new  StreamReader(resstream,Encoding.UTF8))
                    {
                        string reponseHtml = reader.ReadToEnd();
                        rep.Close();
                        return reponseHtml;
                    }
                }
                rep.Close();
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 发送Rest请求返回Stream流
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public  Stream SendPostStream(string url)
        {
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.Timeout = 1000 * 60 * 30;//30分钟
                HttpWebResponse rep = (HttpWebResponse)req.GetResponse();//得到请求结果
                Stream stream = rep.GetResponseStream();
                if (stream != null)
                {
                    return stream;
                }
                rep.Close();
                return null;//如果结果流为空，则返回为空
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string SendGet(string uri)
        {
            try
            { 
                WebClient wc = new WebClient();
                Stream st = wc.OpenRead(uri);
                StreamReader sr = new StreamReader(st);
                string res = sr.ReadToEnd();
                sr.Close();
                st.Close();
                return res;
            }
            catch(Exception ex)
            {
                return "error";
            }
        }

    }
    
}