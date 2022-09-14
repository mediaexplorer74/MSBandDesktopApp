// Decompiled with JetBrains decompiler
// Type: DesktopSyncApp.LoginResponse
// Assembly: Microsoft Band Sync, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 85967930-2DEF-43AB-AC73-6FA058C5AE66
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\MicrosoftBandSync.exe

using System;
using System.Net;

namespace DesktopSyncApp
{
  public class LoginResponse
  {
    public static LoginResponse Create(Uri stopUri)
    {
      LoginResponse loginResponse = new LoginResponse();
      string str1 = WebUtility.UrlDecode(stopUri.Fragment.TrimStart('#'));
      char[] chArray = new char[1]{ '&' };
      foreach (string str2 in str1.Split(chArray))
      {
        char[] separator = new char[1]{ '=' };
        string[] strArray = str2.Split(separator, 2);
        if (strArray.Length == 2)
        {
          string lower = strArray[0].ToLower();
          if (!(lower == "access_token"))
          {
            if (!(lower == "refresh_token"))
            {
              if (!(lower == "expires_in"))
              {
                if (!(lower == "scope"))
                {
                  if (lower == "error")
                    loginResponse.Error = strArray[1].Trim();
                }
                else
                  loginResponse.Scope = strArray[1].Trim();
              }
              else
              {
                try
                {
                  loginResponse.ExpiresIn = Convert.ToInt32(strArray[1].Trim());
                }
                catch (Exception ex)
                {
                  throw new Exception("Unable to convert refresh_token field to Int32", ex);
                }
              }
            }
            else
              loginResponse.RefreshToken = strArray[1].Trim();
          }
          else
            loginResponse.AccessToken = strArray[1].Trim();
        }
      }
      return loginResponse;
    }

    public string AccessToken { get; private set; }

    public string RefreshToken { get; private set; }

    public int ExpiresIn { get; private set; }

    public string Scope { get; private set; }

    public string Error { get; private set; }
  }
}
