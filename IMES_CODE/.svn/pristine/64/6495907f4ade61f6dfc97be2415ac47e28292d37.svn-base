namespace IMES.FisObject.Common.Station
{
    /// <summary>
    /// 机器在各站可能的处理状态
    /// </summary>
    public enum StationStatus
    {
        NULL = -1,
        Fail = 0, //机器在某站处理失败
        Pass = 1, //机器成功完成某站处理
        Processing = 2, //机器正在某站处理，此状态只应用于MVS, PVS等Initial,与Final多站连续且站号相同的情况
        Exemption = 10,  //AFT_MVS,免检
        EPIA = 12,//AFT_MVS,抽中EPIA
        PIA = 15 //AFT_MVS,抽中PIA
    }
}