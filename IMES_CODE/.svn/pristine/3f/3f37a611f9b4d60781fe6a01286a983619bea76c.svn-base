using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectBase;

namespace IMES.FisObject.Common.QTime
{
    public enum QTimeCategoryEnum
    {
        Max=0,
        Min
    }

    public enum QTimeStationStatusEnum
    {
        Fail=0,
        Pass=1
    }

    public class QTime : FisObjectBase, IAggregateRoot
    {
        public QTime()
        {
            this._tracker.MarkAsAdded(this);
        }

        #region . Essential Fields .
        private string _Line;
        private string _Station;
        private string _Family = "";
        private QTimeCategoryEnum _Category= QTimeCategoryEnum.Max;
        private int _TimeOut=0;
        private int _StopTime=0;
        private string _DefectCode="";
        private string _HoldStation = "";
        private QTimeStationStatusEnum _HoldStatus = QTimeStationStatusEnum.Pass;
        private string _ExceptStation = ""; 
        private string _Editor;
        private DateTime _Cdt= DateTime.MinValue;
        private DateTime _Udt=DateTime.MinValue;
       // private DateTime _Now = DateTime.MinValue;
     

        /// <summary>
        /// Line
        /// </summary>
        public string Line
        {
            get
            {
                return this._Line;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._Line = value;
            }
        }


        /// <summary>
        /// Station
        /// </summary>
        public string Station
        {
            get
            {
                return this._Station;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._Station = value;
            }
        }

        /// <summary>
        /// Family:可填入: 空白
        ///           Family
        ///           Model
        ///         ProductID 
        /// </summary>
        public string Family
        {
            get
            {
                return this._Family;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._Family = value;
            }
        }

        /// <summary>
        ///Category
        /// </summary>
        public QTimeCategoryEnum Category
        {
            get
            {
                return this._Category;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._Category = value;
            }
        }

        /// <summary>
        ///_TimeOut
        /// </summary>
        public int TimeOut
        {
            get
            {
                return this._TimeOut;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._TimeOut = value;
            }
        }

        /// <summary>
        ///_StopTime
        /// </summary>
        public int StopTime
        {
            get
            {
                return this._StopTime;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._StopTime = value;
            }
        }

         /// <summary>
        ///_DefectCode
         /// </summary>
        public string DefectCode
         {
             get
             {
                 return this._DefectCode;
             }
             set
             {
                 this._tracker.MarkAsModified(this);
                 this._DefectCode = value;
             }
         }

         /// <summary>
        ///_HoldStation
         /// </summary>
        public string HoldStation
         {
             get
             {
                 return this._HoldStation;
             }
             set
             {
                 this._tracker.MarkAsModified(this);
                 this._HoldStation = value;
             }
         }


         /// <summary>
        ///  HoldStatus
         /// </summary>
        public QTimeStationStatusEnum HoldStatus
         {
             get
             {
                 return this._HoldStatus;
             }
             set
             {
                 this._tracker.MarkAsModified(this);
                 this._HoldStatus = value;
             }
         }
       
        /// <summary>
        ///   ExceptStation 
         /// </summary>
        public string ExceptStation 
         {
             get
             {
                 return this._ExceptStation;
             }
             set
             {
                 this._tracker.MarkAsModified(this);
                 this._ExceptStation = value;
             }
         }

        /// <summary>
        ///Editor
        /// </summary>
        public string Editor
        {
            get
            {
                return this._Editor;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._Editor = value;
            }
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime Cdt
        {
            get
            {
                return this._Cdt;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._Cdt = value;
            }
        }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime Udt
        {
            get
            {
                return this._Udt;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._Udt = value;
            }
        }

         /// <summary>
        /// 更新时间
        /// </summary>
        //public DateTime Now
        //{
        //    get
        //    {
        //        return this._Now;
        //    }
        //    set
        //    {
        //        this._Now = value;
        //    }
        //}
        
        #endregion

        #region IFisObject Members
        /// <summary>
        /// 对象标示key, 在同类型FisObject范围内唯一
        /// </summary>
        public override object Key
        {
            get { return new string[] {this._Line, this._Station, this.Family}; }
        }

        #endregion
    }
}
