using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BloodDonationApp.Models
{
	public class CampaignMV
	{
        public int CampaignID { get; set; }
        [Display(Name = "Campaign Title")]
        [Required(ErrorMessage = "Campaign Title is required")]
        public string CampaignTitle { get; set; }

        [Display(Name = "Campaign Photo")]
        public string CampaignPhoto { get; set; }
        [Display(Name = "Blood Bank")]
        [Required(ErrorMessage = "Blood Bank is required")]
        public int BloodBankID { get; set; }
        [Display(Name = "Campaign Date")]
        [Required(ErrorMessage = "Campaign Date is required")]
        [DataType(DataType.Date)]
        public System.DateTime CampaignDate { get; set; }

        [Display(Name = "Start Time")]
        [Required(ErrorMessage = "Start Time is required")]
       
        public System.TimeSpan StartTime { get; set; }

        [Display(Name = "End Time")]
        [Required(ErrorMessage = "End Time is required")]

        public System.TimeSpan EndTime { get; set; }

        [Display(Name = "Location")]
        [Required(ErrorMessage = "Location is required")]
        public string Location { get; set; }

        [Display(Name = "Campaign Details")]
        [Required(ErrorMessage = "Campaign Details is required")]
        public string CampaignDetails { get; set; }

        [NotMapped]

        public HttpPostedFileBase CampaignPhotoFile { get; set; }


    }
}