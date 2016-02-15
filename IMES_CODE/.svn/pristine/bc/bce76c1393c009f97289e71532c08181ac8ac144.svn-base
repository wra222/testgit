using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace IMES.Infrastructure.ExpressionScript
{
    /// <summary>
    /// string extension
    /// </summary>
    public static class StringExtension
    {
        private const string GroupPattern = @"\(\?<(?<name>.+?)>.+?\)";
        private const string GroupPatternName = "name";
        /// <summary>
        /// Regex IsMatch
        /// </summary>
        /// <param name="input"></param>
        /// <param name="partern"></param>
        /// <returns></returns>
        public static bool IsMatch(this string input, string partern)
        {
            if (input == null)
                return false;
            return Regex.IsMatch(input, partern);
        }
        /// <summary>
        /// Regex Match Group Name
        /// </summary>
        /// <param name="input"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static DictionaryWithDefault<string, string> MatchGroup(this string input, string pattern)
        {
            DictionaryWithDefault<string, string> ret = new DictionaryWithDefault<string, string>();
            Match matchRegex = Regex.Match(input, pattern);
            if (matchRegex.Success)
            {
                IList<string> groupNameList = new List<string>();
                //string groupNamePattern = @"\(\?<(?<name>.+?)>.+?\)";
                MatchCollection mc = Regex.Matches(pattern, GroupPattern, RegexOptions.Compiled);
                foreach (Match m in mc)
                {
                    Group group = m.Groups[GroupPatternName];
                    if (group.Success)
                    {
                        groupNameList.Add(group.Value);
                    }
                }

                if (groupNameList == null || groupNameList.Count == 0)
                {
                    return ret;
                }
                else
                {
                    if (matchRegex.Groups != null &&
                        matchRegex.Groups.Count > 0)
                    {

                        foreach (string groupName in groupNameList)
                        {

                            Group group = matchRegex.Groups[groupName];
                            if (group != null &&
                                group.Success)
                            {
                                ret[groupName] = group.Value;
                            }
                        }
                        return ret;
                    }
                    else
                    {
                        return ret;
                    }
                }
            }
            else
            {
                return ret;
            }
        }
    }

}
