// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.Authentication.SimpleWebToken
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using System;
using System.Collections.Generic;
using System.Globalization;

namespace Microsoft.Health.Cloud.Client.Authentication
{
  public class SimpleWebToken
  {
    private const string AudienceTokenName = "Audience";
    private const string IssuerTokenName = "Issuer";
    private const string ExpiresOnTokenName = "ExpiresOn";
    private const string HmacSha256TokenName = "HMACSHA256";

    public SimpleWebToken(string token)
    {
      this.Token = token != null ? token : throw new ArgumentNullException(nameof (token));
      this.Parse();
    }

    public string Audience { get; private set; }

    public IDictionary<string, string> Claims { get; private set; }

    public DateTimeOffset ExpiresOn { get; private set; }

    public string Issuer { get; private set; }

    public string Token { get; private set; }

    public override string ToString() => this.Token;

    public bool IsExpired() => this.IsExpired(TimeSpan.Zero);

    public bool IsExpired(TimeSpan bufferFromExpiry) => DateTimeOffset.UtcNow + bufferFromExpiry > this.ExpiresOn.ToUniversalTime();

    private static DateTimeOffset ToDateTimeFromEpoch(long secondsSince1970) => new DateTimeOffset(1970, 1, 1, 0, 0, 0, 0, TimeSpan.Zero).AddSeconds((double) secondsSince1970);

    private void Parse()
    {
      this.Claims = (IDictionary<string, string>) new Dictionary<string, string>();
      string token = this.Token;
      char[] separator = new char[1]{ '&' };
      foreach (string str1 in token.Split(separator, StringSplitOptions.RemoveEmptyEntries))
      {
        int length = str1.IndexOf('=');
        string str2 = length >= 0 ? Uri.UnescapeDataString(str1.Substring(0, length)) : throw new FormatException(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "Invalid token contains a name/value pair missing an = character: '{0}'", new object[1]
        {
          (object) str1
        }));
        if (!string.Equals(str2, "HMACSHA256", StringComparison.OrdinalIgnoreCase))
        {
          string s = Uri.UnescapeDataString(str1.Substring(length + 1, str1.Length - length - 1));
          if (!(str2 == "Issuer"))
          {
            if (!(str2 == "Audience"))
            {
              if (str2 == "ExpiresOn")
                this.ExpiresOn = SimpleWebToken.ToDateTimeFromEpoch(long.Parse(s, (IFormatProvider) CultureInfo.InvariantCulture));
              else
                this.Claims[str2] = s;
            }
            else
              this.Audience = s;
          }
          else
            this.Issuer = s;
        }
      }
    }
  }
}
