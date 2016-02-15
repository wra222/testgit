using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace testWS4
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]    
    public interface IGetModel
    {

        [OperationContract]
        ModelQty[] GetModelQty();        

        // TODO: Add your service operations here
    }


    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    [DataContract]
    public class ModelQty
    {
        [DataMember]
        public int Qty { get; set; }
        [DataMember]
        public DateTime Date { get; set; }       
    }
}
