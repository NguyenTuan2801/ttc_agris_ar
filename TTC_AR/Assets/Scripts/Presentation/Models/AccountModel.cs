using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine.Scripting;
#nullable enable

[Preserve]

[Serializable]
public class AccountModel
{
  public string email { get; set; } = string.Empty;
  public string password { get; set; } = string.Empty;
  public AccountModel(string email, string password)
  {
    this.email = email;
    this.password = password;
  }
}


