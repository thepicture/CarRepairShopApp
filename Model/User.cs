//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CarRepairShopApp.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class User
    {
        public int USER_ID { get; set; }
        public string USER_LOGIN { get; set; }
        public string USER_PASSWORD { get; set; }
        public string USER_NAME { get; set; }
        public int ROLE_ID { get; set; }
        public byte[] USER_PHOTO { get; set; }
    
        public virtual Role Role { get; set; }
    }
}
