namespace IMES.Station.Interface.StationIntf
{
    public interface ICNCardReceive
    {
        void CheckPartNo(string partNo);

        void Save(string partNo, string begNo, string endNo, string station, string editor, string pdline, string customer);
    }
}
