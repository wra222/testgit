/*
 * INVENTEC corporation ?2011 all rights reserved. 
 * Description:Interface for UnpackDNByCarton
 * UC:CI-MES12-SPEC-PAK-UC Unpack.doc –2013/02/19            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* 2013-02-19   Benson  Chen   
* Known issues:
* TODO：
* UC 具体业务：
* UC 具体业务：
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Station.Interface.CommonIntf;
using IMES.DataModel;

namespace IMES.Station.Interface.BSamIntf
{
    
    /// <summary>
    /// IUnpackDNByCarton
    /// </summary>
    public interface IUnpackDNByCarton
    {
        /// <summary>
        /// 返回ArrayList
        /// </summary>
        /// <param name="input">input</param>
        /// <returns>ArrayList</returns>
        ArrayList GetDNListByInput(string sn);

        void Unpack(string proid, string line, string editor, string station, string customer);
    
    }
   

}