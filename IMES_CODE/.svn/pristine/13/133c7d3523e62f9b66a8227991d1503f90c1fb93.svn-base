<%@ WebService Language="C#" Class="ESOPWebService" %>

using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using IMES.Station.Interface.CommonIntf;
using IMES.DataModel;
using com.inventec.iMESWEB;
using IMES.Infrastructure;
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class ESOPWebService : System.Web.Services.WebService
{

    private static Queue<EventWaitHandle> _event_cache = new Queue<EventWaitHandle>();

    private static IList<ESOPWebService> _subscriber_list = new List<ESOPWebService>();

    private IDictionary<String, String>[] _filters;
    private EventWaitHandle _event;
    private IDictionary<String, Object> _message;

    [WebMethod]
    public IDictionary<String, Object> GetMessage(IDictionary<String, String>[] filters)
    {

        _filters = filters;

        Monitor.Enter(_event_cache);
        if (_event_cache.Count > 0)
        {
            _event = _event_cache.Dequeue();
        }
        else
        {
            _event = new AutoResetEvent(false);
        }

        Monitor.Exit(_event_cache);

        Monitor.Enter(_subscriber_list);
        _subscriber_list.Add(this);
        Monitor.Exit(_subscriber_list);

        _event.WaitOne();

        Monitor.Enter(_subscriber_list);
        _subscriber_list.Remove(this);
        Monitor.Exit(_subscriber_list);

        Monitor.Enter(_event_cache);
        _event_cache.Enqueue(_event);
        Monitor.Exit(_event_cache);

        return _message;
    }

    [WebMethod]
    public bool PutMessage(IDictionary<String, Object> message, IDictionary<String, String>[] targets)
    {

        bool deliveried = false;

        Monitor.Enter(_subscriber_list);

        foreach (ESOPWebService subscriber in _subscriber_list)
        {

            if (isQualified(targets, subscriber._filters) && deliveryMessage(message, subscriber))
            {
                deliveried = true;
            }
        }
        Monitor.Exit(_subscriber_list);
        return deliveried;
    }

    [WebMethod]
    public ArrayList GetPicPosition()
    {

        ArrayList result = new ArrayList();
        try
        {
            IConstValue CurrentService = ServiceAgent.getInstance().GetObjectByName<IConstValue>(com.inventec.iMESWEB.WebConstant.CommonObject);
            IList<ConstValueInfo> ConstResult = CurrentService.GetConstValueListByType("PicPosition", "name");
            if (ConstResult != null && ConstResult.Count > 0 )
            {
                foreach(ConstValueInfo temp in ConstResult){
                    result.Add(temp.name);
                    result.Add(temp.value);
                }
            }
        }
        catch (FisException fe)
        {
            result.Add("ERROR");
            result.Add(fe.Message);
        }
        catch (Exception e)
        {
            result.Add("ERROR");
            result.Add(e.Message);
        }
        return result;
        
    }
    
    private static bool deliveryMessage(IDictionary<String, Object> message, ESOPWebService subscriber)
    {

        bool deliveried = false;
        try
        {
            deliveried = subscriber.Context.Response.IsClientConnected;
        }
        catch (Exception e)
        {
        }

        subscriber._message = message;
        subscriber._event.Set();
        return deliveried;
    }

    private static bool isQualified(IDictionary<String, String>[] targets, IDictionary<String, String>[] filters)
    {

        if (filters != null || filters.Length == 0)
        {
            return true;
        }

        if (targets != null || targets.Length == 0)
        {
            return false;
        }


        foreach (IDictionary<String, String> filter in filters)
        {
            foreach (IDictionary<String, String> target in targets)
            {

                bool qualified = true;

                foreach (KeyValuePair<String, String> item in filter)
                {

                    if (!target.ContainsKey(item.Key) || target[item.Key] != item.Value)
                    {

                        qualified = false;
                        break;
                    }
                }
                if (qualified)
                {
                    return true;
                }

            }
        }
        return false;
    }
}

