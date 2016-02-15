/*
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;

namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICheckCartonCTForRCTO
    {
        
        /// <summary>
        /// 刷CartonSN，获取该CartonSN下结合的所有机器MBCT2
        /// </summary>
        /// <param name="CartonSN"></param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        ArrayList InputSN(string CartonSN, string line, string editor, string station, string customer);


        /// <summary>
        /// check MBCT
        /// </summary>
        /// <param name="CartonSN"></param>
        /// <param name="mbct"></param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        bool checkMBCT(string CartonSN, string mbct, string line, string editor, string station, string customer);


        /// <summary>
        /// 结束
        /// 
        /// </summary>
        /// <param name="CartonSN"></param>
		/// <param name="editor"></param>
        /// <param name="station"></param>
        void save(string CartonSN, string line, string editor, string station);

        
    }
}
