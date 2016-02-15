using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using IMES.FisObject.Common.FisBOM;
using IMES.Infrastructure.FisObjectBase;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.FisObject.Common.Part
{
    /// <summary>
    /// 匹配规则接口
    /// </summary>
    public interface IMatchRule
    {
        /// <summary>
        /// 尝试将指定匹配目标与指定part在当前匹配规则下进行匹配
        /// </summary>
        /// <param name="target">匹配目标</param>
        /// <param name="part">指定part</param>
        /// <param name="containCheckBit">匹配目标是否带校验位</param>
        /// <returns>匹配结果</returns>
        bool Match(string target, object part, out bool containCheckBit);
    }

    ///<summary>
    /// 一般匹配规则，定义各种匹配规则的公有实现，一个匹配规则由多个匹配元素组成
    ///</summary>
    public class CommonMatchRule : IMatchRule
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public CommonMatchRule(IList<PartMatchRuleElement> matchRules)
        {
            MatchRules = matchRules;
        }

        ///<summary>
        /// 匹配规则的组成元素
        ///</summary>
        public IList<PartMatchRuleElement> MatchRules { get; private set; }

        /// <summary>
        /// 尝试将指定匹配目标与指定part在当前匹配规则下进行匹配
        /// </summary>
        /// <param name="target">匹配目标</param>
        /// <param name="part">指定part</param>
        /// <param name="containCheckBit">匹配目标是否带校验位</param>
        /// <returns>匹配结果</returns>
        public bool Match(string target, object part, out bool containCheckBit)
        {
            containCheckBit = false;
            foreach (var item in MatchRules)
            {
                if (item.Match(target, part))
                {
                    containCheckBit = item.ContainCheckBit;
                    return true;
                }
            }
            return false;
        }
    }

    /// <summary>
    /// 匹配规则元素
    /// </summary>
    public struct PartMatchRuleElement
    {
        /// <summary>
        /// Id
        /// </summary>
        public int ID;

        /// <summary>
        /// 所属的PartCheck
        /// </summary>
        public int PartCheckID;

        /// <summary>
        /// 正则表达式匹配规则
        /// </summary>
        public string RegExp;

        /// <summary>
        /// 使用Pn与匹配目标关系定义的匹配规则
        /// </summary>
        public string PnExp;

        /// <summary>
        /// 使用Part属性与匹配目标之间关心定义的匹配规则
        /// </summary>
        public string PartPropertyExp;

        /// <summary>
        /// 该匹配规则是否假定匹配目标带校验位
        /// </summary>
        public bool ContainCheckBit;

        /// <summary>
        /// 尝试将匹配目标与指定Part在当前匹配规则元素下进行匹配
        /// </summary>
        /// <param name="target">匹配目标</param>
        /// <param name="part">指定Part</param>
        /// <returns>匹配结果</returns>
        public bool Match(string target, object part)
        {
            var partObj = (IBOMPart) part;
            if (Regex.IsMatch(target, RegExp) && PartPropertyMatchPattern.IsMatch(target, partObj, PnExp) && PartPropertyMatchPattern.IsMatch(target, partObj, PartPropertyExp))
            {
                return true; 
            }
            return false;
        }
    }

    /// <summary>
    /// PartPropertyExp与PnExp的匹配方法实现
    /// </summary>
    public class PartPropertyMatchPattern
    {
        /// <summary>
        /// PartPropertyExp与PnExp的匹配方法实现
        /// e.g.
        ///  PN(1,8)=CODE(2,9);PN(10,10)=CODE(15,15)
        ///  PN=CODE
        ///  PN=CODE(1,6)
        /// e.g. 
        /// GCode(1,8)=CODE(2,9);
        /// GCode(10,10)=CODE(15,15);
        /// GCode=CODE
        /// </summary>
        /// <param name="input">匹配目标</param>
        /// <param name="pattern">匹配规则表达式</param>
        /// <returns>匹配结果</returns>
        public static bool IsMatch(string input, IPart part, string pattern)
        {
            if (string.IsNullOrEmpty(pattern))
            {
                return true;
            }

            string[] patternSections = pattern.Split(new char[] {';'}, StringSplitOptions.RemoveEmptyEntries);
            foreach (var section in patternSections)
            {
                string[] pair = section.Split(new char[] {'='});
                if ((pair.Length != 2) || string.IsNullOrEmpty(pair[0]) || string.IsNullOrEmpty(pair[1]))
                {
                    string msg = string.Format("Invalid match rule : {0}", pattern);
                    var e = new Exception(msg);
                    throw e;
                }

                var leftDef = pair[0];
                var rightDef = pair[1];
                
                string[] leftItem = leftDef.Split(new char[] {'(', ',', ')'}, StringSplitOptions.RemoveEmptyEntries);
                string[] rightItem = rightDef.Split(new char[] {'(', ',', ')'}, StringSplitOptions.RemoveEmptyEntries);
                
                string leftValue = string.Empty;
                string rightValue = string.Empty;
                
                int begin;
                int end;

                if ((leftItem.Length == 1))
                {
                    leftValue = part.GetProperty(leftItem[0]);
                }
                else if ((leftItem.Length == 3) 
                    && int.TryParse(leftItem[1], out begin) 
                    && int.TryParse(leftItem[2], out end)
                    && begin <= end)
                {
                    string property = part.GetProperty(leftItem[0]);
                    if (property == null || property.Length < end)
                    {
                        return false;
                    }
                    leftValue = property.Substring(begin - 1, end - begin + 1);
                }
                else
                {
                    string msg = string.Format("Invalid match rule : {0}", pattern);
                    var e = new Exception(msg);
                    throw e;
                }

                if ((rightItem.Length == 1) && rightItem[0].Equals("CODE"))
                {
                    rightValue = input;
                }
                else if ((rightItem.Length == 3)
                    && rightItem[0].Equals("CODE")
                    && int.TryParse(rightItem[1], out begin)
                    && int.TryParse(rightItem[2], out end)
                    && begin <= end
                    )
                {
                    if (input.Length < end)
                    {
                        return false;
                    }
                    rightValue = input.Substring(begin - 1, end - begin + 1);
                }
                else
                {
                    string msg = string.Format("Invalid match rule : {0}", pattern);
                    var e = new Exception(msg);
                    throw e;
                }

                if (leftValue.Equals(rightValue) == false)
                {
                    return false;
                }
            }
            return true;
        }
    }

    /// <summary>
    /// 系统中可用的通用PartCheck类型
    /// </summary>
    public class PartCheck : FisObjectBase, IAggregateRoot
    {
        private static IPartRepository _prtRepository = null;
        private static IPartRepository PrtRepository
        {
            get
            {
                if (_prtRepository == null)
                    _prtRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
                return _prtRepository;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public PartCheck(int id, string customer, string partType, /*int mode,*/ string valueType, int needSave, int needCheck, IMatchRule matchRule)
        {
            _id = id;
            _customer = customer;
            _partType = partType;
            //_mode = mode;
            _valueType = valueType;
            _needSave = needSave;
            _needCheck = needCheck;
            _matchRule = matchRule;

            this._tracker.MarkAsAdded(this);
        }

        #region . Essential Fields .

        private int _id;
        private string _customer;
        private string _partType;
        //private int _mode;
        private string _valueType;
        private int _needSave;
        private int _needCheck;
        private string _editor;
        private DateTime _cdt;
        private DateTime _udt;

        /// <summary>
        /// Id
        /// </summary>
        public int ID
        {
            get
            {
                return _id;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                _id = value;
            }
        }

        /// <summary>
        /// Customer
        /// </summary>
        public string Customer
        {
            get
            {
                return _customer;
            }
            //private set; 
        }

        /// <summary>
        /// PartType
        /// </summary>
        public string PartType 
        {
            get
            {
                return _partType;
            }
            //private set; 
        }
        //public int Mode 
        //{
        //    get
        //    {
        //        return _mode;
        //    }
        //    //private set; 
        //}

        /// <summary>
        /// PartCheck对应的Part属性类型
        /// </summary>
        public string ValueType
        {
            get
            {
                return _valueType;
            }
            //private set; 
        }

        /// <summary>
        /// 是否需要保存绑定数据
        /// </summary>
        public int NeedSave 
        {
            get
            {
                return _needSave;
            }
            //private set; 
        }

        /// <summary>
        /// 是否需要进行PartCheck
        /// </summary>
        public int NeedCheck 
        {
            get
            {
                return _needCheck;
            }
            //private set; 
        }

        #endregion

        /// <summary>
        /// 数据维护人员工号
        /// </summary>
        public string Editor
        {
            get
            {
                return this._editor;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._editor = value;
            }
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime Cdt
        {
            get
            {
                return this._cdt;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._cdt = value;
            }
        }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime Udt
        {
            get
            {
                return this._udt;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._udt = value;
            }
        }

        #region . Sub-Instances .

        private  IMatchRule _matchRule = null;

        /// <summary>
        /// 匹配规则
        /// </summary>
        public IMatchRule MatchRule 
        {
            get
            {
                if (_matchRule == null)
                {
                    PrtRepository.FillMatchRule(this);
                }
                return _matchRule;
            }
        }

        #endregion

        /// <summary>
        /// 对象标示key, 在同类型FisObject范围内唯一
        /// </summary>
        public override object Key
        {
            get 
            {
                return _id;
               //return string.Format("{0},{1}", _customer, _partType); 
            }
        }
    }

    //public struct PartMatchRule
    //{
    //    public string RegExp;
    //    public string PnExp;
    //}
}
