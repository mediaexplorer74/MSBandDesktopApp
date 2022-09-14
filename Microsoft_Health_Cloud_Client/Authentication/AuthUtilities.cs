// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.Authentication.AuthUtilities
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using System;
using System.Net;
using System.Text;
using System.Xml.Linq;

namespace Microsoft.Health.Cloud.Client.Authentication
{
  public static class AuthUtilities
  {
    private const string WrapAccessTokenPrefix = "WRAP access_token=\"";
    private const string WrapAccessTokenPostfix = "\"";

    public static string ParseSecurityToken(string securityToken)
    {
      byte[] bytes = Convert.FromBase64String(XDocument.Parse(WebUtility.HtmlDecode(securityToken)).Root.Value);
      return Encoding.UTF8.GetString(bytes, 0, bytes.Length);
    }

    public static string WrapAccessToken(string accessToken) => "WRAP access_token=\"" + accessToken + "\"";

    public static string UnwrapAccessToken(string headerValue) => headerValue.StartsWith("WRAP access_token=\"") && headerValue.EndsWith("\"") && headerValue.Length >= "WRAP access_token=\"".Length + "\"".Length ? headerValue.Substring("WRAP access_token=\"".Length, headerValue.Length - "WRAP access_token=\"".Length - "\"".Length) : throw new FormatException("WRAP access_token has invalid format: " + headerValue);
  }
}
