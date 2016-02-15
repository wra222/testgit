using System;
using System.Configuration;
using System.Workflow.Activities.Rules;
using System.Workflow.ComponentModel.Serialization;
using System.Xml;
using IMES.Route;
using System.IO;

namespace IMES.Infrastructure.Utility.RuleSets
{
    /// <summary>
    /// 以文件为存储形式的RuleSet的管理器
    /// </summary>
    public class RemotingRuleSetManager : IRulesetManager
    {
        private string _remotingUrl = null;
        private IRouteManagement _rm = null;
        public IRouteManagement RouteManager
        {
            get
            {
                try
                {
                    if (_rm != null && _rm.IsActive())
                        return _rm;
                }
                catch (Exception)
                {
                }

                try
                {
                    IRouteManagementService rms =
                        (IRouteManagementService)Activator.GetObject(
                    typeof(IRouteManagementService), _remotingUrl);
                    _rm = rms.RouteManager;
                }
                catch (Exception)
                {
                }

                return _rm;
            }
        }

        public RemotingRuleSetManager(string remotingUrl)
        {
            this._remotingUrl = remotingUrl;
        }

        public RuleSet getRuleSet(string site, string station, string name)
        {
            RuleSet ruleSet = this.getRuleSet(
                this.RouteManager.ReadRuleSet(site, station, name));
            return ruleSet;
        }
        public RuleDefinitions getRuleDef(string site, string station, string name)
        {
            RuleDefinitions ruleDef = this.getRuleDef(
                this.RouteManager.ReadRuleSet(site, station, name));
            return ruleDef;
        }
        public RuleSet getRuleSetDesign(string site, string station, string name)
        {
            RuleSet ruleSet = this.getRuleSet(
                this.RouteManager.ReadRuleSet(site, station, name));
            return ruleSet;
        }
        private RuleSet getRuleSet(string content)
        {
            object objectRuleSet = null;

            // Deserialize the .rules file that contains one RuleSet
            StringReader sr = new StringReader(content);
            using (XmlTextReader reader = new XmlTextReader(sr))
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
        private RuleDefinitions getRuleDef(string content)
        {
            object objectRuleSet = null;

            // Deserialize the .rules file that contains one RuleSet
            StringReader sr = new StringReader(content);
            using (XmlTextReader reader = new XmlTextReader(sr))
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

        public void saveRuleSetDesign(string site, string station, string name, RuleSet ruleSet)
        {
            saveRuleSet(site, station, name, ruleSet);
        }
        public void saveRuleSet(string site, string station, string name, RuleSet ruleSet)
        {
            StringWriter sw = new StringWriter();
            using (XmlTextWriter writer = new XmlTextWriter(sw))
            {
                RuleDefinitions ruleDef = new RuleDefinitions();
                ruleDef.RuleSets.Add(ruleSet);
                WorkflowMarkupSerializer serializer = new WorkflowMarkupSerializer();
                serializer.Serialize(writer, ruleDef);
            }
            string content = sw.ToString();
            RouteManager.WriteRuleSet(site, station, name, content);
        }
        public void saveRuleDef(string site, string station, string name, RuleDefinitions ruleDef)
        {
            StringWriter sw = new StringWriter();
            using (XmlTextWriter writer = new XmlTextWriter(sw))
            {
                WorkflowMarkupSerializer serializer = new WorkflowMarkupSerializer();
                serializer.Serialize(writer, ruleDef);
            }
            string content = sw.ToString();
            RouteManager.WriteRuleSet(site, station, name, content);
        }
    }
}
