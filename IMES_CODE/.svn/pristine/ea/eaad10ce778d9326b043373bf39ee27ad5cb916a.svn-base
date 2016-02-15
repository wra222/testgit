using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using IMES.Infrastructure.FisObjectBase;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.ComponentModel;

namespace IMES.FisObject.PAK.Carton
{
    public class CartonQCLog : FisObjectBase, IAggregateRoot
    {
        public CartonQCLog() { this._tracker.MarkAsAdded(this); }

        #region . Essential Fields .
        private int _id = 0;
        private string _cartonSN = "";
        private string _model = "";
        private string _line = "";
        private string _type = "";
        private CartonQCStatus _status = CartonQCStatus.NonePAQC;
        private string _remark = "";  
        private string _editor = "";
        private DateTime _cdt = DateTime.MinValue;
       

        public int ID
        {
            get
            {
                return this._id;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._id = value;
            }
        }

        public string CartonSN
        {
            get
            {
                return this._cartonSN;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._cartonSN = value.Trim();
            }
        }
       

        public string Model
        {
            get
            {
                return this._model;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._model = value.Trim();
            }
        }

        public string Line
        {
            get
            {
                return this._line;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._line = value.Trim();
            }
        }

        public string Type
        {
            get
            {
                return this._type;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._type = value.Trim();
            }
        }


        //[TypeConverter(typeof(CastCartonQCStatus))]
        public CartonQCStatus Status
        {
            get
            {
                return this._status;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._status = value;
            }
        }

        public string Remark
        {
            get
            {
                return this._remark;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._remark = value.Trim();
            }
        }

        /// <summary>
        /// Editor
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
                this._editor = value.Trim();
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
       
        #endregion
        #region IFisObject Members
        public override object Key
        {
            get { return _id; }
        }

        #endregion
    }

    [TypeConverter(typeof(CastCartonQCStatus))]
    public enum CartonQCStatus
    {
        NonePAQC=1,
        PAQC=8,
        PAQC_Fail=9,
        PAQC_Pass=65
    }

    public class CastCartonQCStatus : TypeConverter 
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
                    case "1":
                        return CartonQCStatus.NonePAQC;
                    case "8":
                        return CartonQCStatus.PAQC;
                    case "9":
                        return CartonQCStatus.PAQC_Fail;
                    case "A":
                        return CartonQCStatus.PAQC_Pass;
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

        //public override object ConvertTo(object value, Type destinationType)
        //{
        //    if (destinationType != typeof(string) && value.GetType() != typeof(CartonQCStatus))
        //    {
        //        CartonQCStatus status = (CartonQCStatus)value;


        //        switch (status)
        //        {
        //            case CartonQCStatus.NonePAQC:
        //                return "1";
        //            case CartonQCStatus.PAQC:
        //                return "8";
        //            case CartonQCStatus.PAQC_Fail:
        //                return "9";
        //            case CartonQCStatus.PAQC_Pass:
        //                return "A";
        //            default:
        //                throw new InvalidCastException();
        //        }
        //    }
        //    else
        //    {
        //        return base.ConvertTo( value, destinationType);
        //    }
        //}
        public override object ConvertTo(ITypeDescriptorContext context, 
                                                             System.Globalization.CultureInfo  culture, 
                                                             object value, 
                                                             Type destinationType)
        {
            if (destinationType == typeof(string) && value.GetType() == typeof(CartonQCStatus))
            {  
                CartonQCStatus status = (CartonQCStatus)value ;


                 switch (status)
                {
                    case  CartonQCStatus.NonePAQC:
                        return "1";
                    case  CartonQCStatus.PAQC:
                        return "8";
                    case  CartonQCStatus.PAQC_Fail:
                        return "9";
                    case  CartonQCStatus.PAQC_Pass:
                        return "A";
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
