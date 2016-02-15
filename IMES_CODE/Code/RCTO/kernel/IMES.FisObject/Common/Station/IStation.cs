using System;
using IMES.Infrastructure.FisObjectBase;

namespace IMES.FisObject.Common.Station
{
    ///<summary>
    /// Station�࣬���ڱ�ʾ���ߵĲ���վ
    ///</summary>
    public interface IStation : IAggregateRoot
    {
        /// <summary>
        /// Station����
        /// </summary>
        string StationId { get; set; }

        /// <summary>
        /// StationType
        /// </summary>
        StationType StationType { get; set; }

        /// <summary>
        /// ��վ�����Ķ�������0:δ���� 1:MB; 2:Product;
        /// </summary>
        int OperationObject { get; set; }

        /// <summary>
        /// Station����
        /// </summary>
        string Descr { get; set; }

        /// <summary>
        /// ά����Ա
        /// </summary>
        string Editor { get; set; }

        /// <summary>
        /// ����ʱ��
        /// </summary>
        DateTime Cdt { get; set; }

        /// <summary>
        /// ����ʱ��
        /// </summary>
        DateTime Udt { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        string Name { get; set; }
    }
}