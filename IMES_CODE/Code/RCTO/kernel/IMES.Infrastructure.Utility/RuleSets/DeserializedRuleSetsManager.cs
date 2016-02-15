using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Practices.EnterpriseLibrary.Caching;
using System.Workflow.Activities.Rules;
using System.Configuration;
using System.IO;
//using log4net;
using System.Threading;

namespace IMES.Infrastructure.Utility.RuleSets
{
    /// <summary>
    /// Rule Set反序列化管理器
    /// </summary>
    public class DeserializedRuleSetsManager
    {
        private CacheManager _cache = CacheFactory.GetCacheManager("DeserializedRuleSetsManager");

        private object _syncObj = new object();

        private FileSystemWatcher rulesfileWatcher = null;

        private string snTmplRuleFilePath = null;

        private IRulesetManager irm = null;

        #region "Singleton"
        private static DeserializedRuleSetsManager _instance = null;

        /// <summary>
        /// Singleton Method
        /// </summary>
        public static DeserializedRuleSetsManager getInstance
        {
            get 
            {
                if (_instance == null)
                    _instance = new DeserializedRuleSetsManager();
                return _instance;
            }
        }

        private DeserializedRuleSetsManager()
        {
            snTmplRuleFilePath = ConfigurationManager.AppSettings["RulePath"].ToString();
            irm = RuleSetManagerFactory.CreateFileRuleSetManager();
            rulesfileWatcher = new FileSystemWatcher();
            rulesfileWatcher.BeginInit();
            rulesfileWatcher.EnableRaisingEvents = true;
            rulesfileWatcher.Filter = "*.rules";
            rulesfileWatcher.NotifyFilter = System.IO.NotifyFilters.LastWrite;
            rulesfileWatcher.Path = snTmplRuleFilePath;
            rulesfileWatcher.Changed += new FileSystemEventHandler(ruleFileWatcher_Changed);
            rulesfileWatcher.EndInit();
        }
        #endregion

        /// <summary>
        /// 预加载所有的Rule Set到主存.
        /// </summary>
        public void LoadAll()
        {
            string[] fileNames = System.IO.Directory.GetFiles(snTmplRuleFilePath);
            lock (_syncObj)
            {
                _cache.Flush();
                if (fileNames != null && fileNames.Length > 0)
                {
                    foreach (string fileName in fileNames)
                    {
                        string name = System.IO.Path.GetFileNameWithoutExtension(fileName);
                        RuleDefinitions defs = loadFile(name);
                        AddOneFileCache(name, defs);
                    }
                }
            }
        }

        private RuleDefinitions loadFile(string fileName)
        {
            RuleDefinitions ruleDefs = null;
            int attempTimes = 3;
            while (ruleDefs == null && attempTimes > 0)
            {
                try
                {
                    Thread.Sleep((4 - attempTimes) * (4 - attempTimes) * 100);
                    ruleDefs = irm.getRuleDef("", "", fileName);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(String.Format("The loading of rule definition with the file of '{0}'.rules failes! [Error: ({1}) ] ", fileName, ex.ToString()));
                }
                finally
                {
                    attempTimes = attempTimes - 1;
                }
            }

            if (ruleDefs != null && ruleDefs.RuleSets != null && ruleDefs.RuleSets.Count > 0)
                return ruleDefs;
            else
                return null;
        }

        private void clearOneFileCache(string fileName)
        {
            if (_cache.Contains(fileName))
                _cache.Remove(fileName);
        }

        private void AddOneFileCache(string fileName, RuleDefinitions ruleDefs)
        {
            if (ruleDefs == null || fileName == null || fileName.Trim().Equals(String.Empty))
                return;

            if (_cache.Contains(fileName))
                _cache.Remove(fileName);
            _cache.Add(fileName, ruleDefs);
        }

        /// <summary>
        /// 获得一个Rule Set
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="ruleSetName"></param>
        /// <returns></returns>
        public RuleSet getRuleSet(string fileName, string ruleSetName)
        {
            RuleSet codeDom_obj = null;
            lock (_syncObj)
            {
                if (_cache.Contains(fileName))
                {
                    RuleDefinitions ruleDefs = (RuleDefinitions)_cache[fileName];
                    if (ruleDefs != null && ruleDefs.RuleSets != null && ruleDefs.RuleSets.Contains(ruleSetName))
                        codeDom_obj = ruleDefs.RuleSets[ruleSetName];
                }
                else
                {
                    RuleDefinitions defs = loadFile(fileName);
                    AddOneFileCache(fileName, defs);
                    if (defs != null && defs.RuleSets != null && defs.RuleSets.Contains(ruleSetName))
                        codeDom_obj = defs.RuleSets[ruleSetName];
                }
            }
            return codeDom_obj;
        }

        private void ruleFileWatcher_Changed(object sender, System.IO.FileSystemEventArgs e)
        {
            string fileFullName = e.Name;
            string fileName = System.IO.Path.GetFileNameWithoutExtension(fileFullName);
            lock (_syncObj)
            {
                RuleDefinitions defs = loadFile(fileName);
                clearOneFileCache(fileName);
                AddOneFileCache(fileName, defs);
            }
        }
    }
}
