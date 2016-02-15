using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using IMES.Infrastructure.FisObjectBase;
using System.ComponentModel;
using IMES.Infrastructure.Util;


namespace IMES.FisObject.Common.Line
{
    /// <summary>
    /// 線速設置
    /// </summary>
    public class LineSpeed : FisObjectBase, IAggregateRoot
    {
        public LineSpeed()
        {
            this._tracker.MarkAsAdded(this);
        }

        public LineSpeed(string station, string aliasLine, string editor)
        {
            this._station = station;
            this._aliasLine = aliasLine;
            this._editor = editor;            

            this._tracker.MarkAsAdded(this);
        }

        private string _station="";
        private string _aliasLine="";
        private int _limitSpeed = 0;
        private YesNoEnum _isCheckPass = YesNoEnum.N;
        //-- = equal, > 大於, < 小於, <=, >= , !=
        private SpeedExpressionEnum _limitSpeedExpression =SpeedExpressionEnum.GreaterEqual;
        private YesNoEnum _isHoldStation = YesNoEnum.N;
        private string _editor="";
        private DateTime _udt=DateTime.Now;

        /// <summary>
        /// Line Id
        /// </summary>
        public string Station
        {
            get
            {
                return _station;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                if (Parent != null)
                {
                    Parent.Tracker.MarkAsModified(Parent.SubTrackerName);
                }
                _station = value;
            }
        }

        /// <summary>
        /// AliasLine
        /// </summary>
        
        public string AliasLine
        {
            get
            {
                return _aliasLine;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                _aliasLine = value;
            }
        }

        /// <summary>
        /// LimeSpeed
        /// </summary>
        public int LimitSpeed
        {
            get
            {
                return _limitSpeed;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                if (Parent != null)
                {
                    Parent.Tracker.MarkAsModified(Parent.SubTrackerName);
                }
                _limitSpeed = value;
            }
        }


        /// <summary>
        /// PassLimitSpeed
        /// </summary>
        public YesNoEnum IsCheckPass
        {
            get
            {
                return _isCheckPass;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                if (Parent != null)
                {
                    Parent.Tracker.MarkAsModified(Parent.SubTrackerName);
                }
                _isCheckPass = value;
            }
        }
       

       //[TypeConverter(typeof(CastSpeedExpression))]
        public SpeedExpressionEnum LimitSpeedExpression
        {
            get
            {
                return _limitSpeedExpression;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                if (Parent != null)
                {
                    Parent.Tracker.MarkAsModified(Parent.SubTrackerName);
                }
                _limitSpeedExpression = value;
            }
        }

        /// <summary>
        /// IsHoldStation
        /// </summary>
        public YesNoEnum IsHoldStation
        {
            get
            {
                return _isHoldStation;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                if (Parent != null)
                {
                    Parent.Tracker.MarkAsModified(Parent.SubTrackerName);
                }
                _isHoldStation = value;
            }
        }
        /// <summary>
        /// 峎誘埜
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
                if (Parent != null)
                {
                    Parent.Tracker.MarkAsModified(Parent.SubTrackerName);
                }
                _editor = value;
            }
        }



        /// <summary>
        /// 載陔奀潔
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

        /// <summary>
        /// Parent object
        /// </summary>
        public Line Parent
        {
            get;
            set;
        }

        #region Overrides of FisObjectBase

        /// <summary>
        /// 勤砓梓尨key, 婓肮濬倰FisObject毓峓囀峔珨
        /// </summary>
        public override object Key
        {
            get { return _station + "," + _aliasLine; }
        }

        #endregion
    }

    [TypeConverter(typeof(CastSpeedExpression))]
    public enum SpeedExpressionEnum
    {
        Equal = 1,
        Greater,
        Less,
        GreaterEqual,
        LessEqual,
        NotEqual
    }

    public enum YesNoEnum
    {
        Y=0,
        N
    }

    public class CastSpeedExpression : TypeConverter
    {
        public override object ConvertFrom(ITypeDescriptorContext context,
                                                                 System.Globalization.CultureInfo culture,
                                                                 object value)
        {
            if (value.GetType() != typeof(string))
            {
                throw new InvalidCastException();
            }

            string s = value as string;
            
            switch (s.Trim())
            {
                case "=":
                    return SpeedExpressionEnum.Equal;
                case ">":
                    return SpeedExpressionEnum.Greater;
                case "<":
                    return SpeedExpressionEnum.Less;
                case "!=":
                    return SpeedExpressionEnum.NotEqual;
                case ">=":
                     return SpeedExpressionEnum.GreaterEqual;
                case "<=":
                     return SpeedExpressionEnum.LessEqual;
                default:
                    throw new InvalidCastException();
            }

        }
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
                return true;

            return false;

        }

        
        public override object ConvertTo(ITypeDescriptorContext context,
                                                             System.Globalization.CultureInfo culture,
                                                             object value,
                                                             Type destinationType)
        {
            if (destinationType == typeof(string) && value.GetType() == typeof(SpeedExpressionEnum))
            {
                SpeedExpressionEnum expression = (SpeedExpressionEnum)value;


                switch (expression)
                {
                    case SpeedExpressionEnum.Equal:
                        return "=";
                   case SpeedExpressionEnum.Greater:
                        return ">";
                    case SpeedExpressionEnum.Less:
                        return "<";
                    case SpeedExpressionEnum.GreaterEqual:
                        return ">=";
                    case SpeedExpressionEnum.LessEqual:
                        return "<=";
                    case SpeedExpressionEnum.NotEqual:
                        return "!=";
                    default:
                        throw new InvalidCastException();
                }
            }
            else
            {
                throw new InvalidCastException();
            }
        }

    } 
}
