using System;

using System.Windows.Forms;
using System.Workflow.Activities.Rules;
using System.Workflow.Activities.Rules.Design;
using System.Workflow.ComponentModel.Compiler;
using System.Reflection;

namespace IMES.Infrastructure.Utility.RuleSets
{
    /// <summary>
    /// Rule/RuleSet的设计器
    /// </summary>
    public class RuleDesigner
    {
        public static string RemotingUrl { get; set; }
        public static void DesignRule(System.Workflow.ComponentModel.Activity activity, bool UseProvider)
        {
            RuleSet ruleSet = null;
            //MessageBox.Show(activity.UserData[0].ToString());
            //MessageBox.Show(activity.GetType().ToString());
            string ruleName = activity.UserData[0].ToString();

            //load ruleset
            try
            {
                ruleSet = RuleSetManagerFactory.CreateRuleSetManager(RuleDesigner.RemotingUrl).getRuleSetDesign("", "", ruleName);
            }
            catch (Exception e)
            {
                MessageBox.Show("Error deserializing file: " + ruleName + e.Message);
            }

            if (ruleSet == null)
            {
                MessageBox.Show("RuleSet is null");
                ruleSet = new RuleSet(ruleName);
            }

            RuleSetDialog ruleSetDialog = null;
            if (UseProvider)
            {
                TypeProvider provider = new TypeProvider(null);
                AssemblyName[] ass = activity.GetType().Assembly.GetReferencedAssemblies();
                provider.AddAssembly(activity.GetType().Assembly);
                FillWithAssemblies(provider, ass);
                ruleSetDialog = new RuleSetDialog(activity.GetType(), provider, ruleSet);
            }
            else
            {
                ruleSetDialog = new RuleSetDialog(activity, ruleSet);
            }

            var result = ruleSetDialog.ShowDialog();

            // Only update the .rules file if the OK is pressed 
            if (result == DialogResult.OK)
            {
                ruleSet = ruleSetDialog.RuleSet;
                try
                {
                    Console.WriteLine(ruleName);
                    RuleSetManagerFactory.CreateRuleSetManager(RuleDesigner.RemotingUrl).saveRuleSetDesign("", "", ruleName, ruleSet);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }
        }

        private static void FillWithAssemblies(TypeProvider provider, AssemblyName[] assbls)
        {
            if (provider != null && assbls != null && assbls.Length > 0)
            {
                foreach (AssemblyName assbl in assbls)
                {
                    Assembly asb = Assembly.Load(assbl);
                    if (asb != null)
                        provider.AddAssembly(asb);
                }
            }
        }
    }
}
