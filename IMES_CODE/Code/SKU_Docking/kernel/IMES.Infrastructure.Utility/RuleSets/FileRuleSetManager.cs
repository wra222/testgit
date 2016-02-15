using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Workflow.Activities.Rules;
using System.Workflow.ComponentModel.Serialization;
using System.Configuration;
using System.Xml;

namespace IMES.Infrastructure.Utility.RuleSets
{
    /// <summary>
    /// 以文件为存储形式的RuleSet的管理器
    /// </summary>
    public class FileRuleSetManager : IRulesetManager
    {
        private string _designFilePath = "c:";

        public FileRuleSetManager()
        {

        }
            
        public RuleSet getRuleSet(string site, string station, string name)
        {
            string filepath = ConfigurationManager.AppSettings.Get("RulePath").ToString();

            string fileName = string.Format(filepath + "\\{0}.rules", name);
            RuleSet ruleSet = this.getRuleSet(fileName);
            return ruleSet;
        }
        public RuleDefinitions getRuleDef(string site, string station, string name)
        {
            string filepath = ConfigurationManager.AppSettings.Get("RulePath").ToString();

            string fileName = string.Format(filepath + "\\{0}.rules", name);
            RuleDefinitions ruleDef = this.getRuleDef(fileName);
            return ruleDef;
        }
        public RuleSet getRuleSetDesign(string site, string station, string name)
        {
            string filepath = _designFilePath;

            string fileName = String.Format(filepath + "\\{0}.rules", name);
            RuleSet ruleSet = this.getRuleSet(fileName);
            return ruleSet;
        }
        private RuleSet getRuleSet(string filename)
        {
            object objectRuleSet = null;

            // Deserialize the .rules file that contains one RuleSet
            using (XmlTextReader reader = new XmlTextReader(filename))
            {
                WorkflowMarkupSerializer serializer = new WorkflowMarkupSerializer();
                objectRuleSet = serializer.Deserialize(reader);
            }

            RuleSet ruleSet = null;
            if (objectRuleSet != null && objectRuleSet is RuleSet)
            {
                ruleSet = (RuleSet)objectRuleSet;
            }
            RuleDefinitions ruleDef = null;
            if (objectRuleSet != null && objectRuleSet is RuleDefinitions)
            {
                ruleDef = (RuleDefinitions)objectRuleSet;
            }
            if (ruleDef == null && ruleSet == null && objectRuleSet != null)
            {
                // Throw an exception if file does not contain any RuleSet
                throw new InvalidOperationException("RuleSet is null, objectRuleSet.Type: " + objectRuleSet.GetType().FullName);
            }
            if (ruleDef.RuleSets != null && ruleDef.RuleSets.Count > 0)
            {
                ruleSet = ruleDef.RuleSets[0];
            }
            return ruleSet;
        }
        private RuleDefinitions getRuleDef(string filename)
        {
            object objectRuleSet = null;

            // Deserialize the .rules file that contains one RuleSet
            using (XmlTextReader reader = new XmlTextReader(filename))
            {
                WorkflowMarkupSerializer serializer = new WorkflowMarkupSerializer();
                objectRuleSet = serializer.Deserialize(reader);
            }

            RuleDefinitions ruleDef = null;
            if (objectRuleSet != null && objectRuleSet is RuleDefinitions)
            {
                ruleDef = (RuleDefinitions)objectRuleSet;
            }
            if (ruleDef == null && objectRuleSet != null)
            {
                // Throw an exception if file does not contain any RuleSet
                throw new InvalidOperationException("RuleSets is null, objectRuleSets.Type: " + objectRuleSet.GetType().FullName);
            }
            return ruleDef;
        }

        public void saveRuleSet(string site, string station, string name, RuleSet ruleSet)
        {
            string filepath = ConfigurationManager.AppSettings.Get("RulePath").ToString();

            string fileName = string.Format(filepath + "\\{0}.rules", name);
            this.saveRuleSet(fileName, ruleSet);
        }
        public void saveRuleDef(string site, string station, string name, RuleDefinitions ruleDef)
        {
            string filepath = ConfigurationManager.AppSettings.Get("RulePath").ToString();

            string fileName = String.Format(filepath + "\\{0}.rules", name);
            this.saveRuleDef(site, station, name, ruleDef);
        }
        public void saveRuleSetDesign(string site, string station, string name, RuleSet ruleSet)
        {
            string filepath = _designFilePath;

            string fileName = string.Format(filepath + "\\{0}.rules", name);
            this.saveRuleSet(fileName, ruleSet);
        }
        private void saveRuleSet(string filename, RuleSet ruleSet)
        {
            using (XmlTextWriter writer = new XmlTextWriter(filename, null))
            {
                RuleDefinitions ruleDef = new RuleDefinitions();
                ruleDef.RuleSets.Add(ruleSet);
                WorkflowMarkupSerializer serializer = new WorkflowMarkupSerializer();
                serializer.Serialize(writer, ruleDef);
            }
        }
        private void saveRuleDef(string filename, RuleDefinitions ruleDef)
        {
            using (XmlTextWriter writer = new XmlTextWriter(filename, null))
            {
                WorkflowMarkupSerializer serializer = new WorkflowMarkupSerializer();
                serializer.Serialize(writer, ruleDef);
            }
        }
    }
}
