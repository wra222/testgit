using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;


namespace IMES.Maintain.Interface.MaintainIntf
{
    public interface IBTOceanOrder
    {
        //获取所有BTOceanOrder数据
        IList<BTOceanOrder> getAllBTOceanOrder();

        //根据pdline和model获取BTOceanOrder数据
        IList<BTOceanOrder> getListByPdLineAndModel(string pdLine, string model);

        //修改一条BTOceanOrder数据
        void updateBTOceanOrderbyPdlineAndModel(BTOceanOrder obj, string pdLine, string model);

        //添加一条BTOceanOrder数据
        void addBTOceanOrder(BTOceanOrder obj);

        //删除一条BTOceanOrder数据
        void deleteBTOceanOrderByPdlineAndModel(string pdLine, string model);


    }
}
