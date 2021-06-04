using System;

public static class ServerLocalSimulator
{
    public static float InternetVelocityMB;
    
    public static float PercentFails;

    public static void StartServer(float pInternetVelocityMB, float pPercentFails)
    {
        InternetVelocityMB = pInternetVelocityMB;
        PercentFails = pPercentFails;
    }

    public static void Login(string pNickName, string pPwd, Action pOnSuccess, Action pOnFail)
    {
        
    } 
}
