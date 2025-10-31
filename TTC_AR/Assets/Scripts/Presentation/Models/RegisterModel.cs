using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine.Scripting;
#nullable enable

[Preserve]

[Serializable]
public class RegisterModel
{
    public string email { get; set; } = string.Empty;
    public string password { get; set; } = string.Empty;
    public string confirmPassword { get; set; } = string.Empty;

    public RegisterModel(string email, string password, string confirmPassword)
    {
        this.email = email;
        this.password = password;
        this.confirmPassword = confirmPassword;
    }
}