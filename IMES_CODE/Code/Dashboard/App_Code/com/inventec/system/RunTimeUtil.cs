/*
 * INVENTEC corporation (c)2008 all rights reserved. 
 * Description: convert runtime datetime to defined format
 *                  
 * 
 * Update: 
 * Date         Name            Reason 
 * ========== ================= =====================================
 * 2008-12-24   ZhangXueMin     Create 
 * Known issues:Any restrictions about this file 
 *              1 xxxxxxxx
 *              2 xxxxxxxx
 */

using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;

namespace com.inventec.system
{
    public class RunTimeUtil
    {
        //������Ȼ�r�g���x���죬���磺2008-11-20
        static String getRunTimeDate()
        {
            String time = System.DateTime.Now.ToString("yyyy-MM-dd");
            return time;

        }


        //�·ݾ�̖,��:1
        static int getRunTimeMonth()
        {
            return System.DateTime.Now.Month;

        }

        //���Ⱦ�̖,�磺1
        static int getRunTimeQuarter()
        {
            int month= System.DateTime.Now.Month;
            int quarter=0;
            switch (month)
            {
                case 1: 
                case 2:
                case 3:
                    quarter = 1;
                    break;
                case 4:
                case 5:
                case 6:
                    quarter = 2;
                    break;
                case 7:
                case 8:
                case 9:
                    quarter = 3;
                    break;
                case 10:
                case 11:
                case 12:
                    quarter = 4;
                break;
            }
            return quarter;
        }

        //��Ⱦ�̖����2008
        static int getRunTimeYear()
        {
            return System.DateTime.Now.Year;
        }

        //С�r�����I��ÿС�r�����cֵ�����硯08:00:00��
        public static string getRunTimeHour()
        {
            return System.DateTime.Now.ToString("HH:00:00");
        }

        //�Ô��ֱ�ʾ�ǵڎׂ�С�r��������0��23
        static int getRunTimeSimpleHour()
        {
            return System.DateTime.Now.Hour;

        }

    }
}
