using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BloodDonationApp.Helper_Class;
using BloodDonationApp.Models;
using DatabaseLayer;

namespace BloodDonationApp.Controllers
{
    public class BloodBankController : Controller
    {
        OnlineBloodBankDbEntities db = new OnlineBloodBankDbEntities();

        // GET: BloodBank
        public ActionResult BloodBankStock()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }

            int bloodbankID = 0;
            string bloodbankid = Convert.ToString(Session["BloodBankID"]);
            int.TryParse(bloodbankid, out bloodbankID);

            var list = new List<BloodBankStockMV>();
            var stocklist = db.BloodBankStockTables.Where(b => b.BloodBankID == bloodbankID);
            foreach (var stock in stocklist)
            {
                string bloodbank = stock.BloodBanKTable.BloodBankName;
                string bloodgroup = stock.BloodGroupsTable.BloodGroup;
                var bloodBankstockMV = new BloodBankStockMV();
                bloodBankstockMV.BloodBankStockID = stock.BloodBankStockID;
                bloodBankstockMV.BloodBankID = stock.BloodBankID;
                bloodBankstockMV.BloodBank = bloodbank;
                bloodBankstockMV.BloodGroupID = stock.BloodGroupID;
                bloodBankstockMV.BloodGroup = bloodgroup;
                bloodBankstockMV.Status = stock.Status == true ? "Ready For Use" : "Not Ready";
                bloodBankstockMV.Quantity = stock.Quantity;
                bloodBankstockMV.Description = stock.Description;
                list.Add(bloodBankstockMV);
            }
            return View(list);
        }

        public ActionResult AllCampaigns()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            int bloodbankID = 0;
            int.TryParse(Convert.ToString(Session["BloodBankID"]), out bloodbankID);
            var allcampaigns = db.CampaignTables.Where(c => c.BloodBankID == bloodbankID);
            if (allcampaigns.Count() > 0)
            {
                allcampaigns = allcampaigns.OrderByDescending(o => o.CampaignID);
            }
            return View(allcampaigns);
        }

        public ActionResult NewCampaign()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var campaignMV = new CampaignMV();
            return View(campaignMV);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewCampaign(CampaignMV campaignMV)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            int bloodbankID = 0;
            int.TryParse(Convert.ToString(Session["BloodBankID"]), out bloodbankID);
            campaignMV.BloodBankID = bloodbankID;
            if (ModelState.IsValid)
            {
                var campaign = new CampaignTable();
                campaign.BloodBankID = bloodbankID;
                campaign.CampaignDate = campaignMV.CampaignDate;
                campaign.StartTime = campaignMV.StartTime;
                campaign.EndTime = campaignMV.EndTime;
                campaign.Location = campaignMV.Location;
                campaign.CampaignDetails = campaignMV.CampaignDetails;
                campaign.CampaignTitle = campaignMV.CampaignTitle;
                campaign.CampaignPhoto = "~/Content/CampaignPhoto";  //~/Content/CampaignPhoto/C1.png
                db.CampaignTables.Add(campaign);
                db.SaveChanges(); // Campaign ID is generated here

                // Use the entity's CampaignID instead of the model's
                if (campaignMV.CampaignPhotoFile != null)
                {
                    var folder = "~/Content/CampaignPhoto";
                    var file = string.Format("{0}.jpg", campaign.CampaignID); // Fixed line
                    var response = FileHelpers.UploadPhoto(campaignMV.CampaignPhotoFile, folder, file);
                    if (response)
                    {
                        var pic = string.Format("{0}/{1}", folder, file);
                        campaign.CampaignPhoto = pic;
                        db.Entry(campaign).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
                return RedirectToAction("AllCampaigns");
            }

            return View(campaignMV);
        }

        // *************************************
        // ************** EDIT *****************
        // *************************************

        public ActionResult EditCampaign(int id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }

            int bloodbankID = Convert.ToInt32(Session["BloodBankID"]);
            var campaign = db.CampaignTables.FirstOrDefault(c => c.CampaignID == id && c.BloodBankID == bloodbankID);
            
            if (campaign == null)
            {
                return HttpNotFound();
            }

            var model = new CampaignMV
            {
                CampaignID = campaign.CampaignID,
                CampaignTitle = campaign.CampaignTitle,
                CampaignDate = campaign.CampaignDate,
                StartTime = campaign.StartTime,
                EndTime = campaign.EndTime,
                Location = campaign.Location,
                CampaignDetails = campaign.CampaignDetails,
                CampaignPhoto = campaign.CampaignPhoto
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCampaign(CampaignMV model)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }

            if (ModelState.IsValid)
            {
                int bloodbankID = Convert.ToInt32(Session["BloodBankID"]);
                var existingCampaign = db.CampaignTables.FirstOrDefault(c => c.CampaignID == model.CampaignID && c.BloodBankID == bloodbankID);
                
                if (existingCampaign == null)
                {
                    return HttpNotFound();
                }

                existingCampaign.CampaignTitle = model.CampaignTitle;
                existingCampaign.CampaignDate = model.CampaignDate;
                existingCampaign.StartTime = model.StartTime;
                existingCampaign.EndTime = model.EndTime;
                existingCampaign.Location = model.Location;
                existingCampaign.CampaignDetails = model.CampaignDetails;

                if (model.CampaignPhotoFile != null)
                {
                    var folder = "~/Content/CampaignPhoto";
                    var file = $"{existingCampaign.CampaignID}.jpg";
                    var response = FileHelpers.UploadPhoto(model.CampaignPhotoFile, folder, file);
                    
                    if (response)
                    {
                        existingCampaign.CampaignPhoto = $"{folder}/{file}";
                    }
                }

                db.SaveChanges();
                return RedirectToAction("AllCampaigns");
            }

            return View(model);
        }

        // *************************************
        // ************** DELETE ***************
        // *************************************

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteCampaign(int id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }

            int bloodbankID = Convert.ToInt32(Session["BloodBankID"]);
            var campaign = db.CampaignTables.FirstOrDefault(c => c.CampaignID == id && c.BloodBankID == bloodbankID);

            if (campaign != null)
            {
                var relatedEntries = db.BloodBankStockDetailTables.Where(b => b.CampaignID == id).ToList();
                db.BloodBankStockDetailTables.RemoveRange(relatedEntries);
                db.CampaignTables.Remove(campaign);
                db.SaveChanges();
            }

            return RedirectToAction("AllCampaigns");
        }

    }
}