using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using IMES.Infrastructure.UnitOfWork;
//using System.Data;
//using System.Data.SqlClient;

namespace IMES.Infrastructure.Util
{
    public class InvokeBody
    {
        #region
        //public string ConnectionStr;
        //public CommandType CmdType;
        //public string CmdText;
        //public SqlParameter[] Params;
        #endregion

        public InvokeBody(object repository, MethodInfo method, params object[] args)
        {
            _repository = repository;
            _method = method;
            _args = args;
        }

        public InvokeBody(object repository, Action method)
        {
            _repository = repository;
            _action = method;           
            _isAction = true;
        }
        //public InvokeBody(object repository, MethodInfo method, object expectRetVal, params object[] args)
        //    : this(repository, method, args)
        //{
        //    _expectRetVal = expectRetVal;
        //}

        private object _repository;
        private MethodInfo _method;
        private object[] _args;
        private object _retVal;
        private object _expectRetVal = true;
        private InvokeBody _dependencyIvkbdy = null;
        private Action _action;
        private bool _isAction = false; 

        public object ReturnedValue
        {
            get { return _retVal; }
        }

        public object ExpectRetVal
        {
            set { _expectRetVal = value; }
        }

        public bool IsExpected
        {
            get { return _expectRetVal != null && _retVal != null && _expectRetVal.Equals(_retVal); }
        }

        public InvokeBody DependencyIvkbdy
        {
            set { _dependencyIvkbdy = value; }
        }

        public static void ExecuteOne(InvokeBody ivkBdy)
        {
            try
            {
                if (ivkBdy._dependencyIvkbdy == null || (ivkBdy._dependencyIvkbdy != null && ivkBdy._dependencyIvkbdy.IsExpected))
                {
                    if (ivkBdy._isAction)
                    {
                        ivkBdy._action();
                    }
                    else
                    {
                        ivkBdy._retVal = ivkBdy._method.Invoke(ivkBdy._repository, ivkBdy._args);
                    }
                }
            }
            catch(Exception ex)
            {
                throw ex.InnerException;
            }
        }

        public static InvokeBody AddOneInvokeBody(object repository, IUnitOfWork uow, Action actionMethod)
        {
            if (actionMethod != null && 
                uow!=null)
            {
                InvokeBody ivkBdy = new InvokeBody(repository, actionMethod);
                uow.RegisterDeferMethods(ivkBdy);
                return ivkBdy;
            }

            return null;
        }

        public static InvokeBody AddOneInvokeGenericBody(object repository, IUnitOfWork uow, MethodBase method, Type[] genericTypes, params object[] args)
        {
            if (uow != null)
            {
                string methodName = method.Name;
                string postFix = "Defered";
                MethodInfo mi = null;
                MethodInfo genericMethod = repository.GetType().GetMethod(methodName.Substring(0, methodName.Length - postFix.Length));
                mi = genericMethod.MakeGenericMethod(genericTypes);
                InvokeBody ivkBdy = new InvokeBody(repository, mi, args);
                uow.RegisterDeferMethods(ivkBdy);
                return ivkBdy;
            }
            return null;
        }

        public static InvokeBody AddOneInvokeBody(object repository, IUnitOfWork uow, MethodBase method, params object[] args)
        {
            return AddOneInvokeBody_Inner(repository, uow, method, 1, args);
        }

        public static InvokeBody AddOneInvokeBody(object repository, IUnitOfWork uow, int iBeginParam, MethodBase method, params object[] args)
        {
            return AddOneInvokeBody_Inner(repository, uow, method, iBeginParam, args);
        }

        private static InvokeBody AddOneInvokeBody_Inner(object repository, IUnitOfWork uow, MethodBase method, int iBeginParam, params object[] args)
        {
            if (uow != null)
            {
                string methodName = method.Name;

                string postFix = "Defered";
                MethodInfo mi = null;
                ParameterInfo[] parameters = method.GetParameters();
                if (parameters != null && parameters.Length > 0)
                {
                    List<Type> paramTypes = new List<Type>();
                    for (int i = iBeginParam; i < parameters.Length; i++)
                    {
                        paramTypes.Add(parameters[i].ParameterType);
                    }
                    mi = repository.GetType().GetMethod(methodName.Substring(0, methodName.Length - postFix.Length), paramTypes.ToArray());
                }
                else
                {
                    mi = repository.GetType().GetMethod(methodName.Substring(0, methodName.Length - postFix.Length));
                }
                InvokeBody ivkBdy = new InvokeBody(repository, mi, args);
                uow.RegisterDeferMethods(ivkBdy);
                return ivkBdy;
            }
            return null;
        }

        //2013-01-09 vincent add : Get actual method name 
        public string ToString()
        {
            if (_isAction)
            {
                return _action.Method.Name;
            }
            else
            {
                return _method.Name;
            }
        }
    }
}
