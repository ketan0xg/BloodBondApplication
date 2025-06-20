using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BloodDonationApp.Models;
using DatabaseLayer;

namespace BloodDonationApp.Controllers
{
    public class RegistrationController : Controller
    {
        OnlineBloodBankDbEntities db = new OnlineBloodBankDbEntities();
        static RegisterationMV registerationmv;

        // GET: Registration

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SelectUser(RegisterationMV registerationMV)
        {
            registerationmv = registerationMV;

            if (registerationMV.UserTypeID == 2)
            {
                return RedirectToAction("DonorUser");
            }
            else if (registerationMV.UserTypeID == 3)
            {
                return RedirectToAction("SeekerUser");
            }
            else if (registerationMV.UserTypeID == 4)
            {
                return RedirectToAction("HopitalUser");
            }
            else if (registerationMV.UserTypeID == 5)
            {
                return RedirectToAction("BloodBankUser");
            }
            else
            {
                return RedirectToAction("MainHome", "Home");
            }


        }

        public ActionResult HopitalUser()
        {
            ViewBag.CityID = new SelectList(db.CityTables.ToList(), "CityID", "City", registerationmv.CityID);
            return View(registerationmv);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult HopitalUser(RegisterationMV registerationMV)
        {
            if (ModelState.IsValid)
            {
                var checktitle = db.HospitalTables.Where(h=>h.FullName == registerationMV.Hospital.FullName.Trim()).FirstOrDefault();
                if (checktitle == null)
                {
                    using (var transaction = db.Database.BeginTransaction())
                    {
                        try
                        {
                            var user = new UserTable();
                            user.UserName = registerationMV.User.UserName;
                            user.Password = registerationMV.User.Password;
                            user.EmailAddress = registerationMV.User.EmailAddress;
                            user.AccountStatusID = 1;
                            user.UserTypeID = registerationMV.UserTypeID;
                            user.Description = registerationMV.User.Description;
                            db.UserTables.Add(user);
                            db.SaveChanges();

                            var hospital = new HospitalTable();
                            hospital.FullName = registerationMV.Hospital.FullName;
                            hospital.Address = registerationMV.Hospital.Address;
                            hospital.PhoneNo = registerationMV.Hospital.PhoneNo;
                            hospital.Website = registerationMV.Hospital.Website;
                            hospital.Email = registerationMV.Hospital.Email;
                            hospital.Location = registerationMV.Hospital.Address;
                            hospital.CityID = registerationMV.CityID;
                            hospital.UserID = user.UserID;
                            db.HospitalTables.Add(hospital);
                            db.SaveChanges();
                            transaction.Commit();
                            ViewData["Message"] = "Thank You For Registration , Your Query Will be Review Shortly!";
                            return RedirectToAction("MainHome", "Home");
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
                    ModelState.AddModelError(string.Empty, "Hospital Already Registered!");
                }
            }
            ViewBag.CityID = new SelectList(db.CityTables.ToList(), "CityID", "City", registerationMV.CityID);
            return View(registerationMV);
        }


        public ActionResult DonorUser()
        {
            //ViewBag.UserTypeID = new SelectList(db.UserTypeTables.Where(ut => ut.UserTypeID > 1).ToList(), "UserTypeID", "UserType", registerationmv.UserTypeID);
            ViewBag.CityID = new SelectList(db.CityTables.ToList(), "CityID", "City", registerationmv.CityID);
            ViewBag.BloodGroupID = new SelectList(db.BloodGroupsTables.ToList(), "BloodGroupID", "BloodGroup", "0");
            ViewBag.GenderID = new SelectList(db.GenderTables.ToList(), "GenderID", "Gender", "0");
            return View(registerationmv);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DonorUser(RegisterationMV registerationMV)
        {

            if (ModelState.IsValid)
            {
                var checktitle = db.DonorTables.Where(h => h.FullName == registerationMV.Donor.FullName.Trim()&& h.CNIC == registerationMV.Donor.CNIC).FirstOrDefault();
                if (checktitle == null)
                {
                    using (var transaction = db.Database.BeginTransaction())
                    {
                        try
                        {
                            var user = new UserTable();
                            user.UserName = registerationMV.User.UserName;
                            user.Password = registerationMV.User.Password;
                            user.EmailAddress = registerationMV.User.EmailAddress;
                            user.AccountStatusID = 1;
                            user.UserTypeID = registerationMV.UserTypeID;
                            user.Description = registerationMV.User.Description;
                            db.UserTables.Add(user);
                            db.SaveChanges();

                            var donor = new DonorTable();
                            donor.FullName = registerationMV.Donor.FullName;
                            donor.BloodGroupID = registerationMV.BloodGroupID;
                            donor.Location = registerationMV.Donor.Location;
                            donor.ContactNo = registerationMV.Donor.ContactNo;
                            donor.LastDonationDate = registerationMV.Donor.LastDonationDate;
                            donor.CNIC = registerationMV.Donor.CNIC;
                            donor.GenderID = registerationMV.GenderID;
                            donor.CityID = registerationMV.CityID;
                            donor.UserID = user.UserID;
                            db.DonorTables.Add(donor);
                            db.SaveChanges();
                            transaction.Commit();
                            ViewData["Message"] = "Thank You For Registration , Your Query Will be Review Shortly!";
                            return RedirectToAction("MainHome", "Home");
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
                    ModelState.AddModelError(string.Empty, "Donor Already Registered!");
                }
            }
            ViewBag.GenderID = new SelectList(db.GenderTables.ToList(), "GenderID", "Gender", registerationMV.GenderID);
            ViewBag.BloodGroupID = new SelectList(db.BloodGroupsTables.ToList(), "BloodGroupID", "BloodGroup", registerationMV.BloodGroupID);
            ViewBag.CityID = new SelectList(db.CityTables.ToList(), "CityID", "City", registerationMV.CityID);
            return View(registerationMV);
        }
        


        public ActionResult BloodBankUser()
        {


            ViewBag.CityID = new SelectList(db.CityTables.ToList(), "CityID", "City", registerationmv.CityID);
            return View(registerationmv);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BloodBankUser(RegisterationMV registerationMV)
        {
            if (ModelState.IsValid)
            {
                var checktitle = db.BloodBanKTables.Where(h => h.BloodBankName == registerationMV.BloodBank.BloodBankName.Trim() && h.PhoneNo == registerationMV.BloodBank.PhoneNo).FirstOrDefault();
                if (checktitle == null)
                {
                    using (var transaction = db.Database.BeginTransaction())
                    {
                        try
                        {
                            var user = new UserTable();
                            user.UserName = registerationMV.User.UserName;
                            user.Password = registerationMV.User.Password;
                            user.EmailAddress = registerationMV.User.EmailAddress;
                            user.AccountStatusID = 1;
                            user.UserTypeID = registerationMV.UserTypeID;
                            user.Description = registerationMV.User.Description;
                            db.UserTables.Add(user);
                            db.SaveChanges();

                            var bloodbank = new BloodBanKTable();
                            bloodbank.BloodBankName = registerationMV.BloodBank.BloodBankName;
                            bloodbank.Address = registerationMV.BloodBank.Location;
                            bloodbank.Location = registerationMV.BloodBank.Location;
                            bloodbank.PhoneNo = registerationMV.BloodBank.PhoneNo;
                            bloodbank.Website = registerationMV.BloodBank.Website;
                            bloodbank.CityID = registerationMV.CityID;
                            bloodbank.UserID = user.UserID;
                            bloodbank.Email = registerationMV.BloodBank.Email;
                            db.BloodBanKTables.Add(bloodbank);
                            db.SaveChanges();
                            transaction.Commit();
                            ViewData["Message"] = "Thank You For Registration , Your Query Will be Review Shortly!";
                            return RedirectToAction("MainHome", "Home");
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
                    ModelState.AddModelError(string.Empty, "Blood Bank Already Registered!");
                }
            }
            ViewBag.CityID = new SelectList(db.CityTables.ToList(), "CityID", "City", registerationMV.CityID);
            return View(registerationMV);
        }

        public ActionResult SeekerUser()
        {
            ViewBag.CityID = new SelectList(db.CityTables.ToList(), "CityID", "City",registerationmv.CityID);
            ViewBag.BloodGroupID = new SelectList(db.BloodGroupsTables.ToList(), "BloodGroupID", "BloodGroup", registerationmv.BloodGroupID);
            ViewBag.GenderID = new SelectList(db.GenderTables.ToList(), "GenderID", "Gender", registerationmv.GenderID);
            return View(registerationmv);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SeekerUser(RegisterationMV registerationMV)
        {

            if (ModelState.IsValid)
            {
                var checktitle = db.SeekerTables.Where(h => h.Fullname == registerationMV.Seeker.Fullname.Trim() && h.CNIC == registerationMV.Seeker.CNIC).FirstOrDefault();
                if (checktitle == null)
                {
                    using (var transaction = db.Database.BeginTransaction())
                    {
                        try
                        {
                            var user = new UserTable();
                            user.UserName = registerationMV.User.UserName;
                            user.Password = registerationMV.User.Password;
                            user.EmailAddress = registerationMV.User.EmailAddress;
                            user.AccountStatusID = 1;
                            user.UserTypeID = registerationMV.UserTypeID;
                            user.Description = registerationMV.User.Description;
                            db.UserTables.Add(user);
                            db.SaveChanges();

                            var seeker = new SeekerTable();
                            seeker.Fullname = registerationMV.Seeker.Fullname;
                            seeker.Age = registerationMV.Seeker.Age;
                            seeker.BloodGroupID = registerationMV.BloodGroupID;
                            seeker.Address = registerationMV.Seeker.Address;
                            seeker.ContactNo = registerationMV.Seeker.ContactNo;
                            seeker.RegestrationDate = DateTime.Now;
                            seeker.CNIC = registerationMV.Seeker.CNIC;
                            seeker.GenderID = registerationMV.GenderID;
                            seeker.CityID = registerationMV.CityID;
                            seeker.UserID = user.UserID;
                            db.SeekerTables.Add(seeker);
                            db.SaveChanges();
                            transaction.Commit();
                            ViewData["Message"] = "Thank You For Registration , Your Query Will be Review Shortly!";
                            return RedirectToAction("MainHome", "Home");
                        }
                        catch (Exception ex)
                        {
                            ModelState.AddModelError(string.Empty, "Plaese Provide Correct Informtion!" + ex.Message);
                            transaction.Rollback();
                        }
                    }

                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Seeker Already Registered!");
                }
            }
            ViewBag.BloodGroupID = new SelectList(db.BloodGroupsTables.ToList(), "BloodGroupID", "BloodGroup", registerationMV.BloodGroupID);
            ViewBag.CityID = new SelectList(db.CityTables.ToList(), "CityID", "City", registerationMV.CityID);
            ViewBag.GenderID = new SelectList(db.GenderTables.ToList(), "GenderID", "Gender", registerationMV.GenderID);
            return View(registerationMV);
            
        }
    }
}





