using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BloodDonationApp.Models;
using DatabaseLayer;

namespace BloodDonationApp.Controllers
{
    public class AccountsController : Controller
    {
        // GET: Accounts
        OnlineBloodBankDbEntities db = new OnlineBloodBankDbEntities();
        public ActionResult AllNewUserRequests()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var users = db.UserTables.Where(u => u.AccountStatusID == 1).ToList();

            return View(users);
        }

        public ActionResult UserDetails(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var user = db.UserTables.Find(id);
            return View(user);
        }
        public ActionResult UserApproved(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var user = db.UserTables.Find(id);
            user.AccountStatusID = 2;
            db.Entry(user).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("AllNewUserRequests");
        }

        public ActionResult UserRejected(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var user = db.UserTables.Find(id);
            user.AccountStatusID = 3;
            db.Entry(user).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("AllNewUserRequests");
        }



        public ActionResult AddNewDonorByBloodBank()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }

            var collectbloodMV = new CollectBloodMV();
            ViewBag.CityID = new SelectList(db.CityTables.ToList(), "CityID", "City", "0");
            ViewBag.BloodGroupID = new SelectList(db.BloodGroupsTables.ToList(), "BloodGroupID", "BloodGroup", "0");
            ViewBag.GenderID = new SelectList(db.GenderTables.ToList(), "GenderID", "Gender", "0");
            return View(collectbloodMV);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddNewDonorByBloodBank(CollectBloodMV collectBloodMV)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            int bloodbankID = 0;
            string bloodbankid = Convert.ToString(Session["BloodBankID"]);
            int.TryParse(bloodbankid, out bloodbankID);
            var currentdate = DateTime.Now.Date;
            var currentcompaign = db.CampaignTables.Where(c => c.CampaignDate == currentdate && c.BloodBankID == bloodbankID).FirstOrDefault();
            if (ModelState.IsValid)
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var checkdonor = db.DonorTables.Where(d => d.CNIC.Trim().Replace("-", "") == collectBloodMV.DonorDetails.CNIC.Trim().Replace("-", "")).FirstOrDefault();
                        if (checkdonor == null)
                        {
                            var user = new UserTable();
                            user.UserName = collectBloodMV.DonorDetails.FullName.Trim();
                            user.Password = "12345";
                            user.EmailAddress = collectBloodMV.DonorDetails.EmailAddress;
                            user.AccountStatusID = 2;
                            user.UserTypeID = 2;
                            user.Description = "Add By Blood Bank";
                            db.UserTables.Add(user);
                            db.SaveChanges();

                            var donor = new DonorTable();
                            donor.FullName = collectBloodMV.DonorDetails.FullName;
                            donor.BloodGroupID = collectBloodMV.BloodGroupID;
                            donor.Location = collectBloodMV.DonorDetails.Location;
                            donor.ContactNo = collectBloodMV.DonorDetails.ContactNo;
                            donor.LastDonationDate = DateTime.Now;
                            donor.CNIC = collectBloodMV.DonorDetails.CNIC;
                            donor.GenderID = collectBloodMV.GenderID;
                            donor.CityID = collectBloodMV.CityID;
                            donor.UserID = user.UserID;
                            db.DonorTables.Add(donor);
                            db.SaveChanges();
                            checkdonor = db.DonorTables.Where(d => d.CNIC.Trim().Replace("-", "") == collectBloodMV.DonorDetails.CNIC.Trim().Replace("-", "")).FirstOrDefault();

                        }

                        if ((DateTime.Now - checkdonor.LastDonationDate).TotalDays < 120)
                        {
                            ModelState.AddModelError(String.Empty,"Donor Already Donate has Blood!");
                            transaction.Rollback();
                        }else
                        {
                         var checkbloodgroupstock = db.BloodBankStockTables.Where(s => s.BloodBankID == bloodbankID && s.BloodGroupID == collectBloodMV.BloodGroupID).FirstOrDefault();
                        if (checkbloodgroupstock == null)
                        {
                            var bloodbankstock = new BloodBankStockTable();

                            bloodbankstock.BloodBankID = bloodbankID;
                            bloodbankstock.BloodGroupID = collectBloodMV.BloodGroupID;
                            bloodbankstock.Quantity = 0;
                            bloodbankstock.Status = true;
                            bloodbankstock.Description = "";
                            db.BloodBankStockTables.Add(bloodbankstock);
                            db.SaveChanges();
                            checkbloodgroupstock = db.BloodBankStockTables.Where(s => s.BloodBankID == bloodbankID && s.BloodGroupID == collectBloodMV.BloodGroupID).FirstOrDefault();
                        }
                        checkbloodgroupstock.Quantity += collectBloodMV.Quantity;
                        db.Entry(checkbloodgroupstock).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();

                        var collectblooddetail = new BloodBankStockDetailTable();
                        collectblooddetail.BloodBankStockID = checkbloodgroupstock.BloodBankStockID;
                        collectblooddetail.BloodGroupID = collectBloodMV.BloodGroupID;
                        collectblooddetail.CampaignID = currentcompaign.CampaignID;
                        collectblooddetail.Quantity = collectBloodMV.Quantity;
                        collectblooddetail.DonorID = checkdonor.DonorID;
                        collectblooddetail.DonateDateTime = DateTime.Now;
                        db.BloodBankStockDetailTables.Add(collectblooddetail);
                        db.SaveChanges();

                        checkdonor.LastDonationDate = DateTime.Now;
                        db.Entry(checkdonor).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                        transaction.Commit();
                        return RedirectToAction("BloodBankStock", "BloodBank");
                        }
                    }
                    catch
                    {
                        ModelState.AddModelError(string.Empty, "Plaese Provide Correct Informtion!");
                        transaction.Rollback();
                    }
                }
            }
            else
            { 
                ModelState.AddModelError(string.Empty, "Plaese Provide Donor Full Detais!"); 
            }

            ViewBag.CityID = new SelectList(db.CityTables.ToList(), "CityID", "City", collectBloodMV.CityID);
            ViewBag.BloodGroupID = new SelectList(db.BloodGroupsTables.ToList(), "BloodGroupID", "BloodGroup", collectBloodMV.BloodGroupID);
            ViewBag.GenderID = new SelectList(db.GenderTables.ToList(), "GenderID", "Gender", collectBloodMV.GenderID);
            return View(collectBloodMV);
        }
    }
}