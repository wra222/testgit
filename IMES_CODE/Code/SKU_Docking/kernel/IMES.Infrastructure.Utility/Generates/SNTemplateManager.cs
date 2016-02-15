using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Configuration;

namespace IMES.Infrastructure.Utility.Generates
{
    /// <summary>
    /// 序列号生成管理类
    /// </summary>
    public class SNTemplateManager
    {
        private FileSystemWatcher rulesfileWatcher = null;
        static SNTemplateManager(){}

        /// <summary>
        /// SN Generate模版緩存
        /// </summary>
        /// <remarks></remarks>
        private IDictionary<string, IList<IConverter>> _keyAndArray = new Dictionary<string, IList<IConverter>>();

        #region "Singleton"

        private static SNTemplateManager instance = null;
        public static SNTemplateManager GetInstance()
        {
            if (instance == null)
                instance = new SNTemplateManager(ConfigurationManager.AppSettings.Get("RulePath").ToString());
            return instance;
        }

        private SNTemplateManager(string snTmplRuleFilePath)
        {
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
        /// Rules緩存賍位
        /// </summary>
        /// <remarks></remarks>
        private IDictionary<string, bool> _snRuleFileDirtyBits = new Dictionary<string, bool>();

        private object _sycRoot = new object();

        //'Public Property KeyAndArray() As IDictionary(Of String, IList(Of IConverter))
        //'    Get
        //'        Return Me._keyAndArray
        //'    End Get
        //'    Set(ByVal value As IDictionary(Of String, IList(Of IConverter)))
        //'        Me._keyAndArray = value
        //'    End Set
        //'End Property

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="array"></param>
        public void Add(string key, IConverter[] array)
        {
            if (array == null || array.Length < 1 )
                throw new FisException("GEN025", new string[] {});

            lock (_sycRoot)
            {
                IList<IConverter> stem = null;
                try
                {
                    stem = this._keyAndArray[key];
                    stem = Convert(array); //Update
                }
                catch (KeyNotFoundException ex)
                {
                    stem = Convert(array);
                    this._keyAndArray.Add(key, stem);  //Insert
                }
            }
        }

        public IConverter[] Gets(string key)
        {
            lock (_sycRoot)
            {
                List<IConverter> stem = null;
                try
                {
                    stem = (List<IConverter>)this._keyAndArray[key];
                }
                catch (KeyNotFoundException ex)
                { }

                if (stem == null)
                {
                    List<String> param = new List<String>();
                    param.Add(key);
                    throw new FisException("GEN026", param);
                }
                else
                    return Convert(stem);
            }
        }

        public void SetDirtyBit(string snGenType, bool bValue)
        {
            lock (_sycRoot)
            {
                if (!_snRuleFileDirtyBits.ContainsKey(snGenType))
                    _snRuleFileDirtyBits.Add(snGenType, true); //Initialize the dirty bit.
                else
                    _snRuleFileDirtyBits[snGenType] = bValue;
            }
        }

        public bool GetDirtyBit(string snGenType)
        {
            lock (_sycRoot)
            {
                if (!_snRuleFileDirtyBits.ContainsKey(snGenType))
                {
                    this._snRuleFileDirtyBits.Add(snGenType, true); //Initialize the dirty bit.
                    return true;
                }
                else
                {
                    return _snRuleFileDirtyBits[snGenType];
                }
            }
        }

        private List<IConverter> Convert(IConverter[] array)
        {
            List<IConverter> item = new List<IConverter>();
            foreach (IConverter cvt in array)
                item.Add(cvt);
            return item;
        }

        private IConverter[] Convert(List<IConverter> array)
        {
            return array.ToArray();
        }


        private void ruleFileWatcher_Changed(object sender, System.IO.FileSystemEventArgs e)
        {
            string fileName = e.Name;
            string snGenType = System.IO.Path.GetFileNameWithoutExtension(fileName);
            this.SetDirtyBit(snGenType, true);
        }
    }
}
