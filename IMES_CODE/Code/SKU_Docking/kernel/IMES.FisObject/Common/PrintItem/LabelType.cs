// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 对应LabelType表
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2010-02-01   Yuan XiaoWei                 create
// Known issues:

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.FisObjectBase;

namespace IMES.FisObject.Common.PrintItem
{
    /// <summary>
    /// 对应LabelType表
    /// </summary>
    public class LabelType : FisObjectBase, IAggregateRoot
    {
        private static ILabelTypeRepository _lblTypeRepository = null;
        private static ILabelTypeRepository LblTypeRepository
        {
            get
            {
                if (_lblTypeRepository == null)
                    _lblTypeRepository = RepositoryFactory.GetInstance().GetRepository<ILabelTypeRepository, LabelType>();
                return _lblTypeRepository;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public LabelType()
        {
            this._tracker.MarkAsAdded(this);
        }

        #region . Essential Fields .

        private string _lblType = null;
        private string _description = null;
        private int _printMode;
        private int _ruleMode;
        private string _editor = null;
        private DateTime _cdt;
        private DateTime _udt;

        /// <summary>
        /// 
        /// </summary>
        public string LblType
        {
            get
            {
                return this._lblType;
            }
            set
            {
                _tracker.MarkAsModified(this);
                this._lblType = value;
            }
        }

        /// <summary>
        /// LabelType描述
        /// </summary>
        public string Description
        {
            get
            {
                return this._description;
            }
            set
            {
                _tracker.MarkAsModified(this);
                this._description = value;
            }
        }

        /// <summary>
        /// PrintMode
        /// </summary>
        public int PrintMode
        {
            get
            {
                return _printMode;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                _printMode = value;
            }
        }

        /// <summary>
        /// RuleMode
        /// </summary>
        public int RuleMode
        {
            get
            {
                return _ruleMode;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                _ruleMode = value;
            }
        }

        /// <summary>
        /// Editor
        /// </summary>
        public string Editor 
        {
            get
            {
                return _editor;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                _editor = value;
            }
        }

        /// <summary>
        /// Cdt
        /// </summary>
        public DateTime Cdt 
        {
            get
            {
                return _cdt;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                _cdt = value;
            } 
        }

        /// <summary>
        /// Udt
        /// </summary>
        public DateTime Udt 
        {
            get
            {
                return _udt;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                _udt = value;
            }
        }

        #endregion

        #region . Sub-Instances .

        private IList<PrintTemplate> _templates = null;
        private object _syncObj_templates = new object();

        /// <summary>
        /// 此Label可用的模板
        /// </summary>
        public IList<PrintTemplate> Templates
        {
            get
            {
                lock (_syncObj_templates)
                {
                    if (_templates == null)
                    {
                        LblTypeRepository.FillTemplates(this);
                    }
                    if (_templates != null)
                    {
                        return new ReadOnlyCollection<PrintTemplate>(this._templates);
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }

        #endregion

        #region Overrides of FisObjectBase

        /// <summary>
        /// 对象标示key, 在同类型FisObject范围内唯一
        /// </summary>
        public override object Key
        {
            get { return _lblType; }
        }

        #endregion


    }





}