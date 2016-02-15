using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UTL;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Security.Permissions;
using log4net;
using System.Reflection;


namespace UTL
{
    public class Logon
    {

        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern bool LogonUser(String lpszUsername, String lpszDomain, String lpszPassword,
                      int dwLogonType, int dwLogonProvider,ref IntPtr phToken);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public extern static bool CloseHandle(IntPtr handle);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public extern static bool DuplicateToken(IntPtr ExistingTokenHandle,
            int SECURITY_IMPERSINATION_LEVEL, ref IntPtr DuplicateTokenHandle);

        //private System.Security.Principal.WindowsImpersonationContext newUser;

        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public enum SECURITY_IMPERSONATION_LEVEL : int
        {
            SecurityAnonymous = 0,
            SecurityIdentification = 1,
            SecurityImpersonation = 2,
            SecurityDelegation = 3
        }

        public enum LogonType
        {
            /// <summary>
            /// This logon type is intended for users who will be interactively using the computer, such as a user being logged on  
            /// by a terminal server, remote shell, or similar process.
            /// This logon type has the additional expense of caching logon information for disconnected operations; 
            /// therefore, it is inappropriate for some client/server applications,
            /// such as a mail server.
            /// </summary>
            LOGON32_LOGON_INTERACTIVE = 2,

            /// <summary>
            /// This logon type is intended for high performance servers to authenticate plaintext passwords.

            /// The LogonUser function does not cache credentials for this logon type.
            /// </summary>
            LOGON32_LOGON_NETWORK = 3,

            /// <summary>
            /// This logon type is intended for batch servers, where processes may be executing on behalf of a user without 
            /// their direct intervention. This type is also for higher performance servers that process many plaintext
            /// authentication attempts at a time, such as mail or Web servers. 
            /// The LogonUser function does not cache credentials for this logon type.
            /// </summary>
            LOGON32_LOGON_BATCH = 4,

            /// <summary>
            /// Indicates a service-type logon. The account provided must have the service privilege enabled. 
            /// </summary>
            LOGON32_LOGON_SERVICE = 5,

            /// <summary>
            /// This logon type is for GINA DLLs that log on users who will be interactively using the computer. 
            /// This logon type can generate a unique audit record that shows when the workstation was unlocked. 
            /// </summary>
            LOGON32_LOGON_UNLOCK = 7,

            /// <summary>
            /// This logon type preserves the name and password in the authentication package, which allows the server to make 
            /// connections to other network servers while impersonating the client. A server can accept plaintext credentials 
            /// from a client, call LogonUser, verify that the user can access the system across the network, and still 
            /// communicate with other servers.
            /// NOTE: Windows NT:  This value is not supported. 
            /// </summary>
            LOGON32_LOGON_NETWORK_CLEARTEXT = 8,

            /// <summary>
            /// This logon type allows the caller to clone its current token and specify new credentials for outbound connections.
            /// The new logon session has the same local identifier but uses different credentials for other network connections. 
            /// NOTE: This logon type is supported only by the LOGON32_PROVIDER_WINNT50 logon provider.
            /// NOTE: Windows NT:  This value is not supported. 
            /// </summary>
            LOGON32_LOGON_NEW_CREDENTIALS = 9,
        }

        public enum LogonProvider
        {
            /// <summary>
            /// Use the standard logon provider for the system. 
            /// The default security provider is negotiate, unless you pass NULL for the domain name and the user name 
            /// is not in UPN format. In this case, the default provider is NTLM. 
            /// NOTE: Windows 2000/NT:   The default security provider is NTLM.
            /// </summary>
            LOGON32_PROVIDER_DEFAULT = 0,
            LOGON32_PROVIDER_WINNT35 = 1,
            LOGON32_PROVIDER_WINNT40 = 2,
            LOGON32_PROVIDER_WINNT50 = 3
        }

        [PermissionSetAttribute(SecurityAction.Demand, Name = "FullTrust")]
        static public WindowsImpersonationContext ImpersinateUser(string sUsername, 
                                                                                                            string sDomain, 
                                                                                                             string sPassword)
        {

            IntPtr pExistingTokenHandle = new IntPtr(0);
            IntPtr pDuplicateTokenHandle = new IntPtr(0);
            pExistingTokenHandle = IntPtr.Zero;
            pDuplicateTokenHandle = IntPtr.Zero;
            string methodName = MethodBase.GetCurrentMethod().Name;
            logger.InfoFormat("BEGIN: {0}()", methodName);
            try
            {
                //const int LOGON32_PROVIDER_DEFAULT = 0;
                //const int LOGON32_PROVIDER_NT = 3;

                //const int LOGON32_LOGON_INTERACTIVE = 2;
                //const int LOGON32_LOGON_NEW_CREDENTIAL = 9;
               
                bool bImpersonated = LogonUser(sUsername, 
                                                                       sDomain, 
                                                                       sPassword, 
                                                                       //LOGON32_LOGON_NEW_CREDENTIAL, 
                                                                       (int)LogonType.LOGON32_LOGON_NEW_CREDENTIALS,
                                                                       //LOGON32_PROVIDER_NT,
                                                                       (int)LogonProvider.LOGON32_PROVIDER_WINNT50,
                                                                       ref pExistingTokenHandle);
                logger.DebugFormat("{0}.LogonUser(): Username:{1}, Domain:{2}, Password:{3}", methodName, sUsername, sDomain, sPassword);
                if (false == bImpersonated)
                {
                    int nErrorCode = Marshal.GetLastWin32Error();
                    //log.write(LogType.Info, 0, "LogOn", "LogTest", "Log On failed with error code" + nErrorCode);
                  
                    logger.InfoFormat("Log On failed with error code: {0}  {1} ", nErrorCode.ToString(), (new System.ComponentModel.Win32Exception(nErrorCode)).Message);
                    return null;
                }
                //log.write(LogType.Info, 0, "LogOn", "LogTest", "Before impersomation" + WindowsIdentity.GetCurrent().Name);
                logger.DebugFormat("Before impersomation: {0}", WindowsIdentity.GetCurrent().Name);
                bool bRetVal = DuplicateToken(pExistingTokenHandle, 
                                                                  (int)SECURITY_IMPERSONATION_LEVEL.SecurityImpersonation, 
                                                                  ref pDuplicateTokenHandle);
                logger.DebugFormat("{0}.DuplicateToken()", methodName);
                if (false == bRetVal)
                {
                    int nErrorCode = Marshal.GetLastWin32Error();
                    CloseHandle(pExistingTokenHandle);
                    //log.write(LogType.Info, 0, "LogOn", "LogTest", "DuplocateToken() failed with error code" + nErrorCode);
                    logger.InfoFormat("DuplocateToken() failed with error code: {0} {1}", nErrorCode.ToString(), (new System.ComponentModel.Win32Exception(nErrorCode)).Message);
                    return null;
                }
                else
                {
                    //WindowsIdentity newId = new WindowsIdentity(pDuplicateTokenHandle);
                    logger.DebugFormat("BEGIN: {0}.WindowsIdentity.Impersonate ", methodName);
                    WindowsImpersonationContext impersonatedUser = WindowsIdentity.Impersonate(pDuplicateTokenHandle);     //newId.Impersonate();
                    logger.DebugFormat("END: {0}.WindowsIdentity.Impersonate ", methodName);
                    //log.write(LogType.Info, 0, "LogOn", "LogSuccess", "After impersonation:" + WindowsIdentity.GetCurrent().Name);
                    logger.InfoFormat("LogOn Success:SECURITY_IMPERSONATION_LEVEL:{0},User:{1}", WindowsIdentity.GetCurrent().ImpersonationLevel, WindowsIdentity.GetCurrent().Name);
                    return impersonatedUser;
                }

            }
            catch (Exception ex)
            {
                //log.write(LogType.Info, 0, "LogOn", "LogFaile", "Logon Fail:" + ex.Message);
                logger.Error(methodName, ex);
                return null;
            }
            finally
            {
                if (pExistingTokenHandle != IntPtr.Zero)
                    CloseHandle(pExistingTokenHandle);
                if (pDuplicateTokenHandle != IntPtr.Zero)
                    CloseHandle(pDuplicateTokenHandle);
                logger.InfoFormat("END: {0}()", methodName);
            }

            


        }

        [PermissionSetAttribute(SecurityAction.Demand, Name = "FullTrust")]
        static public void Log_off(WindowsImpersonationContext wic)
        {
            wic.Undo();

        }
    }
}
