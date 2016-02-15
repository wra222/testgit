using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using UPH.Entity.Repository.Meta.IMESSKU;
namespace UPH.Interface
{
    public  interface IUPH
    {
        /// <summary>
        /// 获取所有记录
        /// </summary>
        /// <returns></returns>
        IList<ProductUPHInfo> GetAllProductUPHInfo();
        /// <summary>
        /// 根据条件获取
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        IList<ProductUPHInfo> GetProductUPHInfoList( DateTime from, DateTime to);

        /// <summary>
        /// 增加1笔
        /// </summary>
        /// <param name="item"></param>
        void AddProductUPHInfo(ProductUPHInfo item);

       /// <summary>
       /// 删掉 
       /// </summary>
       /// <param name="astType"></param>
       /// <param name="astCode"></param>
        void DelProductUPHInfo(ProductUPHInfo item);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="item"></param>
        /// <param name="astType"></param>
        /// <param name="astCode"></param>
        void UpdateProductUPHInfo(ProductUPHInfo item);

        /// <summary>
        /// check 重复
        /// </summary>
        /// <param name="astType"></param>
        /// <param name="astCode"></param>
        bool CheckDuplicateData(ProductUPHInfo item);

        List<string> GetUPHLine();
        DataTable GetUPH(string line);

        List<string> GetUPHLine(string Process);
        DataTable GetUPH(string line,string Process,DateTime begin,DateTime end);
        DataTable GetUPH_Month(string line, string Process, DateTime begin, DateTime end);

        List<string> GetUPHLine_OutPut(string Process,DateTime DT);
        DataTable GetUPH_OutPut(string line, string Process, DateTime begin);

        List<PdLine> EffPdline(string Process);
        List<Efficiency_Hour> GetEfficiency_Hour(string line, string Process, DateTime begin, DateTime end);
        DataTable GetEfficiency_Hour_Echart(string line, string Process, DateTime begin, DateTime end);

        List<Efficiency_Day> GetEfficiency_Day(string line, string Process, DateTime begin, DateTime end);
        List<Efficiency_Process_Day> GetEfficiency_Process_Day(string Process, DateTime begin, DateTime end);
        List<Efficiency_Process_Hour> GetEfficiency_Process_Hour(string Process, DateTime begin, DateTime end);
        List<ProductQty> GetProductQty();





    }
    /// <summary>
    /// 前台使用
    /// </summary>
    [Serializable]
    public class ProductUPHInfo
    {
       
        public int ID;
        public DateTime Date;
        public string Line;
        public string Lesson;
        public string TimeRange;
        public string Family;
        public Decimal ProductRatio;
    }
    [Serializable]
    public class PdLine
    {
        public string Line;
        public string Description;
        
    }
 


}
