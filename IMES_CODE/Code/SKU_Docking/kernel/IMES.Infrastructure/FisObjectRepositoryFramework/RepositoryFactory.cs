using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Configuration;
using IMES.Infrastructure.FisObjectBase;
using IMES.Infrastructure.UnitOfWork;

namespace IMES.Infrastructure.FisObjectRepositoryFramework
{
    ///<summary>
    /// Repository工厂
    ///</summary>
    public class RepositoryFactory
    {
        #region . Singleton .
        private static readonly RepositoryFactory _instance = new RepositoryFactory();

        /// <summary>
        /// 获取RepositoryFactory实例
        /// </summary>
        /// <returns></returns>
        public static RepositoryFactory GetInstance()
        {
            return _instance;
        }
        private RepositoryFactory() {  }
        #endregion

        /// <summary>
        /// key: FisObjectType.Name, Value: Repository Object
        /// </summary>
        private  Dictionary<string, object> _repositories = new Dictionary<string, object>();

        private  object syncRoot = new object(); 

        /// <summary>
        /// 获取/创建Repository对象, 
        /// </summary>
        public  TRepository GetRepository<TRepository, TFisObject>()
            where TRepository : class, IRepository<TFisObject>
            where TFisObject : IAggregateRoot
        {
            string interfaceShortName = typeof(TRepository).Name;

            TRepository repository = default(TRepository);

            lock (syncRoot)
            {
                if (!_repositories.ContainsKey(interfaceShortName))
                {
                    Type repositoryType = null;
                    repositoryType = GetImplementClass(typeof(TRepository));
                    repository = Activator.CreateInstance(repositoryType) as TRepository;
                    _repositories.Add(interfaceShortName, repository);
                }
                else
                {
                    repository = (TRepository)_repositories[interfaceShortName];
                }
            }

            return repository;
        }

        /// <summary>
        /// 获取Repository对象
        /// </summary>
        /// <typeparam name="TRepository">Reporsitory具体类型</typeparam>
        /// <returns>Repository对象</returns>
        public  TRepository GetRepository<TRepository>()
            where TRepository : class
        {
            string interfaceShortName = typeof(TRepository).Name;

            TRepository repository = default(TRepository);

            lock (syncRoot)
            {
                if (!_repositories.ContainsKey(interfaceShortName))
                {
                    Type repositoryType = null;
                    repositoryType = GetImplementClass(typeof(TRepository));
                    repository = Activator.CreateInstance(repositoryType) as TRepository;
                    _repositories.Add(interfaceShortName, repository);
                }
                else
                {
                    repository = (TRepository)_repositories[interfaceShortName];
                }
            }

            return repository;
        }

        private Type GetImplementClass(Type Intf)
        {
            // Go with Vincent's idea to support out of box repository implementation
            string[] paths = ConfigurationManager.AppSettings.Get("RepositoryImplAssembly").ToString().Split(new char[] {',',';'});
            foreach (string path in paths)
            {
                Type[] tps = Assembly.Load(path).GetTypes();
                foreach (Type tp in tps)
                {
                    if (tp.GetInterface(Intf.Name) != null)
                        return tp;
                }
            }
            return null;
        }

        /// <summary>
        /// this method should only be used in ruleset,  for ruleset does not support template.
        /// </summary>
        /// <param name="repositoryType"></param>
        /// <returns></returns>
        public object GetRepoistory(Type repositoryType)
        {
            return _repositories[repositoryType.Name];
        }

        /// <summary>
        /// this method should only be used in ruleset,  for ruleset does not support template.
        /// </summary>
        /// <param name="repositoryType"></param>
        /// <returns></returns>
        public object GetRepoistory(string repositoryTypeName)
        {
            return _repositories[repositoryTypeName];
        }
    }
}
