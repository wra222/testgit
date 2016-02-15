using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Reflection;
using System.Configuration;
using System.Reflection.Emit;

namespace IMES.Infrastructure
{
    public sealed class DynamicFunc
    {
        public static Func<object, object[], object> Wrap(MethodInfo method)
        {
            var dm = new DynamicMethod(method.Name, typeof(object), new Type[] {
                           typeof(object), typeof(object[])
                          }, method.DeclaringType, true);
            var il = dm.GetILGenerator();

            if (!method.IsStatic)
            {
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Unbox_Any, method.DeclaringType);
            }
            var parameters = method.GetParameters();
            for (int i = 0; i < parameters.Length; i++)
            {
                il.Emit(OpCodes.Ldarg_1);
                il.Emit(OpCodes.Ldc_I4, i);
                il.Emit(OpCodes.Ldelem_Ref);
                il.Emit(OpCodes.Unbox_Any, parameters[i].ParameterType);
            }
            il.EmitCall(method.IsStatic || method.DeclaringType.IsValueType ?
                OpCodes.Call : OpCodes.Callvirt, method, null);
            if (method.ReturnType == null || method.ReturnType == typeof(void))
            {
                il.Emit(OpCodes.Ldnull);
            }
            else if (method.ReturnType.IsValueType)
            {
                il.Emit(OpCodes.Box, method.ReturnType);
            }
            il.Emit(OpCodes.Ret);
            return (Func<object, object[], object>)dm.CreateDelegate(typeof(Func<object, object[], object>));
        }

        public static Func<T, object[], TReturn> TemplateWrap<T, TReturn>(MethodInfo method) where T : class
        {

            var dm = new DynamicMethod(method.Name,
                typeof(TReturn),
                new Type[] { typeof(T), typeof(object[]) },
                method.DeclaringType,
                true);
            var il = dm.GetILGenerator();

            if (!method.IsStatic)
            {
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Unbox_Any, method.DeclaringType);
            }
            var parameters = method.GetParameters();
            for (int i = 0; i < parameters.Length; i++)
            {
                il.Emit(OpCodes.Ldarg_1);
                il.Emit(OpCodes.Ldc_I4, i);
                il.Emit(OpCodes.Ldelem_Ref);
                il.Emit(OpCodes.Unbox_Any, parameters[i].ParameterType);
            }
            il.EmitCall(method.IsStatic || method.DeclaringType.IsValueType ?
                OpCodes.Call : OpCodes.Call, method, null);
            if (method.ReturnType == null || method.ReturnType == typeof(void))
            {
                il.Emit(OpCodes.Ldnull);
            }
            else if (method.ReturnType.IsValueType)
            {
                il.Emit(OpCodes.Box, method.ReturnType);
            }
            il.Emit(OpCodes.Ret);
            return (Func<T, object[], TReturn>)dm.CreateDelegate(typeof(Func<T, object[], TReturn>));
        }
    }

    /// <summary>
    /// 系统自定义异常类
    /// </summary>
    [Serializable]
    public class FisException : Exception
    {
        private static object _miscRepository = null;
        private static MethodInfo _mthInfo = null;
        private static Func<object, object[], object> _getFisErrorCodeFunc = null;

        private static string GetFisErrorCode(string errCode, string lang)
        {
            if (null == _miscRepository || null == _getFisErrorCodeFunc)
            {
                Type miscRTp = null;

                // Go with Vincent's idea to support out of box repository implementation
                //string path = ConfigurationManager.AppSettings.Get("RepositoryImplAssembly").ToString();
                string[] paths = ConfigurationManager.AppSettings.Get("RepositoryImplAssembly").ToString().Split(new char[] {',',';'});
                foreach (string path in paths)
                {
                    Type[] tps = Assembly.Load(path).GetTypes();
                    foreach (Type tp in tps)
                    {
                        if (tp.GetInterface("IMiscRepository") != null)
                        {
                            miscRTp = tp;
                            break;
                        }
                    }
                    if (miscRTp != null)
                    {
                        break;
                    }
                }
                _miscRepository = Activator.CreateInstance(miscRTp);
                _mthInfo = miscRTp.GetMethod("GetFisErrorCode");
                _getFisErrorCodeFunc = DynamicFunc.Wrap(_mthInfo);
            }
            try
            {
                //return (string)_mthInfo.Invoke(_miscRepository, new object[] { errCode, lang });
                return (string)_getFisErrorCodeFunc(_miscRepository, new object[] { errCode, lang });
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        private string errcode = string.Empty;
        private string errmsg = string.Empty;

        private bool isStopWF = true;

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("code", this.errcode);
            info.AddValue("msg", this.errmsg);
            info.AddValue("isStop", this.isStopWF);
        }

        protected FisException(SerializationInfo info, StreamingContext context)
        {
            this.errcode = info.GetString("code");
            this.errmsg = info.GetString("msg");
            this.isStopWF = info.GetBoolean("isStop");
        }

        public FisException()
        {

        }

        public FisException(string Msg)
        {
            this.errmsg = Msg;
        }

        public FisException(string Code, string[] param)
            : this(Code, new List<String>(param))
        {

        }

        public FisException(string Code, List<string> param)
        {
            this.errcode = Code;
            this.errmsg = string.Format("[{0}]",Code);

            string dalstr = ConfigurationManager.AppSettings["ERROR_LAN"].ToString();
            string[] errLan = null;

            if (dalstr == null || dalstr.Trim() == "")
            {
                this.errmsg = "FE: Error language is not set in DB (Err Code: " + Code + ")";
            }
            else
            {
                errLan = dalstr.Split(',');
                int i, j;
                for (i = 0; i <= errLan.Length - 1; i++)
                {
                    dalstr = GetFisErrorCode(Code, errLan[i]);
                    if (dalstr == null)
                    {
                        dalstr = "Error description not found in DB (Err Code: " + Code + ")";
                    }
                    if (param != null)
                    {
                        for (j = 0; j <= param.Count - 1; j++)
                            dalstr = dalstr.Replace("%" + (j + 1).ToString(), param[j]);
                    }
                    errmsg = errmsg + dalstr + "\r\n";
                }
            }            
        }

        public FisException(string Code, bool stopWF, string[] param)
            : this(Code, new List<String>(param))
        {
            this.isStopWF = stopWF;
        }

        public FisException(string Code, bool stopWF, List<string> param)
            : this(Code, param)
        {
            this.isStopWF = stopWF;
        }

        //public void logErr()//(ByVal pdline As String, ByVal editor As String, ByVal uutsn As String, ByVal ip As String, ByVal station As String)
        //{
        //    //    Dim commonDAL As ICommonDAL = DataAccess.CreateICommonDAL
        //    //    Dim fli As New FisLogInfo
        //    //    fli.ClientIP = ip
        //    //    fli.ErrorCode = errcode
        //    //    fli.ErrorMsg = errmsg
        //    //    fli.OperatorField = editor
        //    //    fli.PDLine = pdline
        //    //    fli.Uutsn = uutsn
        //    //    fli.Wc = station
        //    //    commonDAL.insertFisLog(fli)
        //}

        public string mErrcode
        {
            get { return this.errcode; }
            set { this.errcode = value; }
        }

        public bool stopWF
        {
            get { return this.isStopWF; }
            set { this.isStopWF = value; }
        }

        public string mErrmsg
        {
            get { return this.errmsg; }
            set { this.errmsg = value; }
        }

        public override string ToString()
        {
            return this.errmsg + "\r\n" + base.ToString();
        }
    }
}
