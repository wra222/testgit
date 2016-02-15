using System;
namespace IMES.FisObject.Common.Model
{
    ///<summary>
    /// 抽样率设定
    ///</summary>
    public class QCRatio
    {
        /// <summary>
        /// 抽样率设定对象构造函数
        /// </summary>
        public QCRatio(/*int id, */string family, int qcRatio, int eoqcRatio, int paqcRatio, string editor, DateTime cdt, DateTime udt)
        {
            //ID = id;
            Family = family;
            OQCRatio = qcRatio;
            EOQCRatio = eoqcRatio;
            PAQCRatio = paqcRatio;
            Editor = editor;
            Cdt = cdt;
            Udt = udt;
        }

        public QCRatio(/*int id, */string family, int qcRatio, int eoqcRatio, int paqcRatio, string editor, DateTime cdt, DateTime udt, int rpaqcRatio)
            : this(family, qcRatio, eoqcRatio, paqcRatio, editor, cdt, udt)
        {
            RPAQCRatio = rpaqcRatio;
        }

        ///// <summary>
        ///// ID
        ///// </summary>
        //public int ID { get; private set; }

        /// <summary>
        /// Family
        /// </summary>
        public string Family { get; private set; }

        /// <summary>
        /// QCRatio
        /// </summary>
        public int OQCRatio { get; private set; }

        /// <summary>
        /// EOQCRatio
        /// </summary>
        public int EOQCRatio { get; private set; }

        /// <summary>
        /// PAQCRatio
        /// </summary>
        public int PAQCRatio { get; private set; }

        public string Editor { get; private set; }

        public DateTime Cdt { get; private set; }

        public DateTime Udt { get; private set; }

		/// <summary>
        /// RPAQCRatio
        /// </summary>
        public int RPAQCRatio { get; private set; }
    }
}