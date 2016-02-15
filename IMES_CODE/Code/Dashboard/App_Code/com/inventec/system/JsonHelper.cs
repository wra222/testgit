using System;
using System.Data;
using System.Configuration;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Text;

/// <summary>
/// Summary description for JsonHelper
/// </summary>
/// 

namespace com.inventec.system
{
    public static class JsonHelper<T>
    {
        static JsonHelper()
        { }

        /// <summary>  
        /// 序列化为JSON对象  
        /// </summary>  
        /// <param name="obj"></param>  
        /// <returns></returns>  
        public static string WriteObject(T obj)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
            MemoryStream mstream = new MemoryStream();
            serializer.WriteObject(mstream, obj);
            byte[] Bytes = new byte[mstream.Length];
            mstream.Position = 0;
            mstream.Read(Bytes, 0, (int)mstream.Length);
            return Encoding.UTF8.GetString(Bytes);
        }

        /// <summary>  
        /// JSON对象反序列化  
        /// </summary>  
        /// <param name="data"></param>  
        /// <returns></returns>  
        public static T ReadObject(string data)
        {
            MemoryStream mstream = new MemoryStream(Encoding.UTF8.GetBytes(data));
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
            return (T)serializer.ReadObject(mstream);
        }
    }

}
/*List<ParamInfo> paramList = new List<ParamInfo>();

ParamInfo paramInfo1 = new ParamInfo();
paramInfo1.ParamName = "@Dn";

List<String> values = new List<String>();
values.Add("A3DN0M1020000001");
paramInfo1.Values = values;

paramList.Add(paramInfo1);


string strTags = JsonHelper<List<ParamInfo>>.WriteObject(paramList);
*/