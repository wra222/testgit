using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Reflection;
using log4net;
using IMES.Entity.Infrastructure.Interface;

namespace IMES.Entity.Infrastructure.Framework
{
    public class RepositoryFactory
    {
        protected static ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
       private static readonly RepositoryFactory _instance = new RepositoryFactory();
        public static RepositoryFactory GetInstance()
        {
            return _instance;
        }

        private RepositoryFactory() { }

        private Dictionary<string, object> _repositories = new Dictionary<string, object>();

        private object syncRoot = new object();

        public TRepository GetRepository<TRepository,TEntity>()
            where TRepository : class, IRepository<TEntity>
            where TEntity : class
        {
             string methodName = MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
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
            catch (Exception e)
            {
                logger.Error(methodName, e);
                throw;
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }
        }

        public TRepository GetRepository<TRepository>()
            where TRepository : class
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
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
            catch (Exception e)
            {
                logger.Error(methodName, e);
                throw;
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }
        }

        private Type GetImplementClass(Type Intf)
        {
            // Go with Vincent's idea to support out of box repository implementation
            string[] paths = ConfigurationManager.AppSettings.Get("RepositoryImplAssembly").ToString().Split(new char[] { ',', ';' });
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
       
    }
}
