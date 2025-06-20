using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BloodDonationApp.Models;
using DatabaseLayer;

namespace BloodDonationApp.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        OnlineBloodBankDbEntities db = new OnlineBloodBankDbEntities();
        public ActionResult UserProfile(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var user = db.UserTables.Find(id);
            return View(user);
        }

        public ActionResult EditUserProfile(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
          
            var userprofile = new RegisterationMV();
            var user = db.UserTables.Find(id);

            userprofile.UserTypeID = user.UserTypeID;

            userprofile.User.UserID = user.UserID;
            userprofile.User.UserName = user.UserName;
            userprofile.User.EmailAddress = user.EmailAddress;
            userprofile.User.AccountStatusID = user.AccountStatusID;
            userprofile.User.UserTypeID = user.UserTypeID;
            userprofile.User.Description = user.Description;



            if (user.SeekerTables.Count > 0)
            {
                var seeker = user.SeekerTables.FirstOrDefault();
                userprofile.Seeker.SeekerID = seeker.SeekerID;
                userprofile.Seeker.Fullname = seeker.Fullname;
                userprofile.Seeker.Age = seeker.Age;
                userprofile.Seeker.CityID = seeker.CityID;
                userprofile.Seeker.BloodGroupID = seeker.BloodGroupID;
                userprofile.Seeker.ContactNo = seeker.ContactNo;
                userprofile.Seeker.CNIC = seeker.CNIC;
                userprofile.Seeker.GenderID = seeker.GenderID;
                userprofile.Seeker.Address = seeker.Address;
                userprofile.Seeker.UserID = seeker.UserID;

                userprofile.CityID = seeker.CityID;
                userprofile.ContactNo = seeker.ContactNo;
                userprofile.BloodGroupID = seeker.BloodGroupID;
                userprofile.GenderID = seeker.GenderID;
            }
            else if (user.HospitalTables.Count > 0)
            {
                var hospital = user.HospitalTables.FirstOrDefault();
                userprofile.Hospital.HospitalID = hospital.HospitalID;
                userprofile.Hospital.FullName = hospital.FullName;
                userprofile.Hospital.Address = hospital.Address;
                userprofile.Hospital.PhoneNo = hospital.PhoneNo;
                userprofile.Hospital.Website = hospital.Website;
                userprofile.Hospital.Email = hospital.Email;
                userprofile.Hospital.Location = hospital.Location;
                userprofile.Hospital.CityID = hospital.CityID;
                userprofile.Hospital.UserID = hospital.UserID;

                userprofile.CityID = hospital.CityID;
                userprofile.ContactNo = hospital.PhoneNo;
            }
            else if (user.BloodBanKTables.Count > 0)
            {
                var bloodbank = user.BloodBanKTables.FirstOrDefault();
                userprofile.BloodBank.BloodBankID = bloodbank.BloodBankID;
                userprofile.BloodBank.BloodBankName = bloodbank.BloodBankName;
                userprofile.BloodBank.Address = bloodbank.Address;
                userprofile.BloodBank.PhoneNo = bloodbank.PhoneNo;
                userprofile.BloodBank.Location = bloodbank.Location;
                userprofile.BloodBank.Website = bloodbank.Website;
                userprofile.BloodBank.Email = bloodbank.Email;
                userprofile.BloodBank.CityID = bloodbank.CityID;
                userprofile.BloodBank.UserID = bloodbank.UserID;


                userprofile.CityID = bloodbank.CityID;
                userprofile.ContactNo = bloodbank.PhoneNo;
            }
            else if (user.DonorTables.Count > 0)
            {
                var donor = user.DonorTables.FirstOrDefault();
                userprofile.Donor.DonorID = donor.DonorID;
                userprofile.Donor.FullName = donor.FullName;
                userprofile.Donor.GenderID = donor.GenderID;
                userprofile.Donor.BloodGroupID = donor.BloodGroupID;
                userprofile.Donor.LastDonationDate = donor.LastDonationDate;
                userprofile.Donor.ContactNo = donor.ContactNo;
                userprofile.Donor.CNIC = donor.CNIC;
                userprofile.Donor.Location = donor.Location;
                userprofile.Donor.CityID = donor.CityID;
                userprofile.Donor.UserID = donor.UserID;

                userprofile.CityID = donor.CityID;
                userprofile.ContactNo = donor.ContactNo;
                userprofile.BloodGroupID = donor.BloodGroupID;
                userprofile.GenderID = donor.GenderID;






            }

            ViewBag.CityID = new SelectList(db.CityTables.ToList(), "CityID", "City", userprofile.CityID);
            ViewBag.BloodGroupID = new SelectList(db.BloodGroupsTables.ToList(), "BloodGroupID", "BloodGroup", userprofile.BloodGroupID);
            ViewBag.GenderID = new SelectList(db.GenderTables.ToList(), "GenderID", "Gender", userprofile.GenderID);

            return View(userprofile);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUserProfile(RegisterationMV userprofile)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }

            if (ModelState.IsValid)
            {
                var checkuseremail = db.UserTables.Where(u => u.EmailAddress == userprofile.User.EmailAddress && u.UserID != userprofile.User.UserID).FirstOrDefault();
                if (checkuseremail == null)
                {
                    try
                    {
                        var user = db.UserTables.Find(userprofile.User.UserID);
                        user.EmailAddress = userprofile.User.EmailAddress;
                        db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();

                        if (userprofile.Donor.DonorID > 0)
                        {
                            var donor = db.DonorTables.Find(userprofile.Donor.DonorID);
                            donor.FullName = userprofile.Donor.FullName;
                            donor.BloodGroupID = userprofile.BloodGroupID;
                            donor.GenderID = userprofile.GenderID;
                            donor.ContactNo = userprofile.Donor.ContactNo;
                            donor.CNIC = userprofile.Donor.CNIC;
                            donor.CityID = userprofile.CityID;
                            donor.Location = userprofile.Donor.Location;
                            db.Entry(donor).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                        }
                        else if (userprofile.Seeker.SeekerID > 0)
                        {
                            var seeker = db.SeekerTables.Find(userprofile.Seeker.SeekerID);
                            seeker.Fullname = userprofile.Seeker.Fullname;
                            seeker.Age = userprofile.Seeker.Age;
                            seeker.CityID = userprofile.CityID;  //**//
                            seeker.BloodGroupID = userprofile.BloodGroupID;
                            seeker.ContactNo = userprofile.Seeker.ContactNo;
                            seeker.CNIC = userprofile.Seeker.CNIC;
                            seeker.GenderID = userprofile.GenderID;
                            seeker.Address = userprofile.Seeker.Address;
                            db.Entry(seeker).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();

                        }
                        else if (userprofile.Hospital.HospitalID > 0)
                        {
                            var hospital = db.HospitalTables.Find(userprofile.Hospital.HospitalID);
                            hospital.FullName = userprofile.Hospital.FullName;
                            hospital.Address = userprofile.Hospital.Address;
                            hospital.PhoneNo = userprofile.Hospital.PhoneNo;
                            hospital.Website = userprofile.Hospital.Website;
                            hospital.Email = userprofile.Hospital.Email;
                            hospital.CityID = userprofile.CityID;
                            db.Entry(hospital).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                        }
                        else if (userprofile.BloodBank.BloodBankID > 0)
                        {
                            var bloodbank = db.BloodBanKTables.Find(userprofile.BloodBank.BloodBankID);
                            bloodbank.BloodBankName = userprofile.BloodBank.BloodBankName;
                            bloodbank.Address = userprofile.BloodBank.Address;
                            bloodbank.PhoneNo = userprofile.BloodBank.PhoneNo;
                            bloodbank.Website = userprofile.BloodBank.Website;
                            bloodbank.Email = userprofile.BloodBank.Email;
                            bloodbank.CityID = userprofile.CityID;
                            db.Entry(bloodbank).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();

                        }
                        return RedirectToAction("UserProfile","User", new { id = userprofile.User.UserID });
                    }
                    catch 
                    {

                        ModelState.AddModelError(string.Empty, "Please fill all the required fields");
                    }
                }
                else 
                {
                   ModelState.AddModelError(string.Empty, "Email Address already exists");
                    
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Please fill all the required fields");
            }
            // var user = db.UserTables.Find(id);

            ViewBag.CityID = new SelectList(db.CityTables.ToList(), "CityID", "City", userprofile.CityID);
            ViewBag.BloodGroupID = new SelectList(db.BloodGroupsTables.ToList(), "BloodGroupID", "BloodGroup", userprofile.BloodGroupID);
            ViewBag.GenderID = new SelectList(db.GenderTables.ToList(), "GenderID", "Gender", userprofile.GenderID);
            return View(userprofile);
        }
    }
}