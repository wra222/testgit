namespace IMES.FisObject.Common.Station
{
    /// <summary>
    /// �����ڸ�վ���ܵĴ���״̬
    /// </summary>
    public enum StationStatus
    {
        NULL = -1,
        Fail = 0, //������ĳվ����ʧ��
        Pass = 1, //�����ɹ����ĳվ����
        Processing = 2, //��������ĳվ������״ֻ̬Ӧ����MVS, PVS��Initial,��Final��վ������վ����ͬ�����
        Exemption = 10,  //AFT_MVS,���
        EPIA = 12,//AFT_MVS,����EPIA
        PIA = 15 //AFT_MVS,����PIA
    }
}