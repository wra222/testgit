using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMES.DataModel
{
    /// <summary>
    /// Attribute Information
    /// </summary>
    [Serializable]
    public class AttributeInfo
    {
        /// <summary>
        /// Name
        /// </summary>
        public string Name {get;set;}

        /// <summary>
        /// Value
        /// </summary>
        public string Value {get;set;}
        
    }
}
