//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DoNotHarm
{
    using System;
    using System.Collections.Generic;
    
    public partial class Client
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int CountryId { get; set; }
        public string EIN { get; set; }
        public string Email { get; set; }
        public System.Guid Guid { get; set; }
        public System.DateTime Birthdate { get; set; }
        public int InsuranceId { get; set; }
        public string Ipadress { get; set; }
        public string Login { get; set; }
        public string PassportNumber { get; set; }
        public string PassportSeria { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public string SocialSecNumber { get; set; }
        public int SocialTypeId { get; set; }
        public string Ua { get; set; }
    
        public virtual Countri Countri { get; set; }
        public virtual Insurance Insurance { get; set; }
        public virtual SocialType SocialType { get; set; }
    }
}
