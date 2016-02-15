using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UTL;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Security.Permissions;

namespace UTL
{
    public class Logon
    {
        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern bool LogonUser(String lpszUsername, String lpszDomain, String lpszPassword,
                      int dwLogonType, int dwLogonProvider, ref IntPtr phToken);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public extern static bool CloseHandle(IntPtr handle);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public extern static bool DuplicateToken(IntPtr ExistingTokenHandle,
            int SECURITY_IMPERSINATION_LEVEL, ref IntPtr DuplicateTokenHandle);

        //private System.Security.Principal.WindowsImpersonationContext newUser;


        public enum SECURITY_IMPERSONATION_LEVEL : int
        {
            SecurityAnonymous = 0,
            SecurityIdentification = 1,
            SecurityImpersonation = 2,
            SecurityDelegation = 3
        }
        static public WindowsImpersonationContext ImpersinateUser(string sUsername, string sDomain, string sPassword, Log log)
        {

            IntPtr pExistingTokenHandle = new IntPtr(0);
            IntPtr pDuplicateTokenHandle = new IntPtr(0);
            pExistingTokenHandle = IntPtr.Zero;
            pDuplicateTokenHandle = IntPtr.Zero;
            try
            {
                //const int LOGON32_PROVIDER_DEFAULT = 0;
                const int LOGON32_PROVIDER_NT = 3;

                //const int LOGON32_LOGON_INTERACTIVE = 2;
                const int LOGON32_LOGON_NEW_CREDENTIAL = 9;
                bool bImpersonated = LogonUser(sUsername, sDomain, sPassword, LOGON32_LOGON_NEW_CREDENTIAL, LOGON32_PROVIDER_NT, ref pExistingTokenHandle);

                if (false == bImpersonated)
                {
                    int nErrorCode = Marshal.GetLastWin32Error();
                    log.write(LogType.Info, 0, "LogOn", "LogTest", "Log On failed with error code" + nErrorCode);
                    return null;
                }
                log.write(LogType.Info, 0, "LogOn", "LogTest", "Before impersomation" + WindowsIdentity.GetCurrent().Name);
                bool bRetVal = DuplicateToken(pExistingTokenHandle, (int)SECURITY_IMPERSONATION_LEVEL.SecurityImpersonation, ref pDuplicateTokenHandle);
                if (false == bRetVal)
                {
                    int nErrorCode = Marshal.GetLastWin32Error();
                    CloseHandle(pExistingTokenHandle);
                    log.write(LogType.Info, 0, "LogOn", "LogTest", "DuplocateToken() failed with error code" + nErrorCode);
                    return null;
                }
                else
                {
                    WindowsIdentity newId = new WindowsIdentity(pDuplicateTokenHandle);
                    WindowsImpersonationContext impersonatedUser = newId.Impersonate();
                    log.write(LogType.Info, 0, "LogOn", "LogSuccess", "After impersonation:" + WindowsIdentity.GetCurrent().Name);
                    return impersonatedUser;
                }

            }
            catch (Exception ex)
            {
                log.write(LogType.Info, 0, "LogOn", "LogFaile", "Logon Fail:" + ex.Message);
            }
            finally
            {
                if (pExistingTokenHandle != IntPtr.Zero)
                    CloseHandle(pExistingTokenHandle);
                if (pDuplicateTokenHandle != IntPtr.Zero)
                    CloseHandle(pDuplicateTokenHandle);
            }

            return null;


        }
        static public void Log_off(WindowsImpersonationContext wic)
        {
            wic.Undo();

        }
    }
}
