//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DatabaseLayer
{
    using System;
    using System.Collections.Generic;
    
    public partial class CampaignTable
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CampaignTable()
        {
            this.BloodBankStockDetailTables = new HashSet<BloodBankStockDetailTable>();
        }
    
        public int CampaignID { get; set; }
        public int BloodBankID { get; set; }
        public System.DateTime CampaignDate { get; set; }
        public System.TimeSpan StartTime { get; set; }
        public System.TimeSpan EndTime { get; set; }
        public string Location { get; set; }
        public string CampaignDetails { get; set; }
        public string CampaignTitle { get; set; }
        public string CampaignPhoto { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BloodBankStockDetailTable> BloodBankStockDetailTables { get; set; }
        public virtual BloodBanKTable BloodBanKTable { get; set; }
    }
}
