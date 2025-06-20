using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BloodDonationApp.Models;
using DatabaseLayer;

namespace BloodDonationApp.Controllers
{
    public class HomeController : Controller
    {
        OnlineBloodBankDbEntities db = new OnlineBloodBankDbEntities();
        public ActionResult AllCampaigns()
        {

            var date = DateTime.Now.Date;
            var allcampaigns = db.CampaignTables.Where(c => c.CampaignDate >= date).ToList();
            return View(allcampaigns);
        }
        public ActionResult MainHome()
        {
            var message = ViewData["Message"] == null ? "Welcome To Online Blood Donation " : ViewData["Message"];
            ViewData["Message"] = message;

            var date = DateTime.Now.Date;
            var allcampaigns = db.CampaignTables.Where(c => c.CampaignDate >= date).ToList();
            ViewBag.AllCampaigns = allcampaigns;

            var registeration = new RegisterationMV();
            ViewBag.UserTypeID = new SelectList(db.UserTypeTables.Where(ut => ut.UserTypeID > 1).ToList(), "UserTypeID", "UserType", "0");
            ViewBag.CityID = new SelectList(db.CityTables.ToList(), "CityID", "City", "0");
            return View(registeration);
        }

        public ActionResult Login()
        {
            var userMV = new UserMV();
            return View(userMV);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserMV userMV)
        {
            if (ModelState.IsValid)
            {
                var user = db.UserTables.Where(u => u.UserName == userMV.UserName && u.Password == userMV.Password).FirstOrDefault();
                if (user != null)
                {
                    if (user.AccountStatusID == 1)
                    {
                        ModelState.AddModelError(string.Empty, "Please Wait,Your Account is Under Review!");

                    }
                    else if (user.AccountStatusID == 3)
                    {
                        ModelState.AddModelError(string.Empty, "Your Account is Rejected For More Details Contact Us.");
                    }
                    else if (user.AccountStatusID == 4)
                    {
                        ModelState.AddModelError(string.Empty, "Your Account is Suspended! For More Details Contact Us.");
                    }
                    else if (user.AccountStatusID == 2)
                    {
                        Session["UserID"] = user.UserID;
                        Session["UserName"] = user.UserName;
                        Session["Password"] = user.Password;
                        Session["EmailAddress"] = user.EmailAddress;
                        Session["AccountStatusID"] = user.AccountStatusID;
                        Session["AccountStatus"] = user.AccountStatusTable.AccountStatus;
                        Session["UserTypeID"] = user.UserTypeID;
                        Session["UserType"] = user.UserTypeTable.UserType;
                        Session["Description"] = user.Description;

                        if (user.UserTypeID == 1) // Admin 
                        {
                            return RedirectToAction("AllNewUserRequests", "Accounts");
                        }
                        else if (user.UserTypeID == 2) // Donor
                        {
                            var donor = db.DonorTables.Where(d => d.UserID == user.UserID).FirstOrDefault();
                            if (donor != null)
                            {
                                Session["DonorID"] = donor.DonorID;
                                Session["FullName"] = donor.FullName;
                                Session["GenderID"] = donor.GenderID;
                                Session["BloodGroupID"] = donor.BloodGroupID;
                                Session["BloodGroup"] = donor.BloodGroupsTable.BloodGroup;
                                Session["LastDonationDate"] = donor.LastDonationDate;
                                Session["ContactNo"] = donor.ContactNo;
                                Session["CNIC"] = donor.CNIC;
                                Session["Location"] = donor.Location;
                                Session["CityID"] = donor.CityID;
                                Session["City"] = donor.CityTable.City;
                                return RedirectToAction("Donor", "Dashboard");
                            }
                            else
                            {

                                ModelState.AddModelError(string.Empty, "Invalid Username or Password");
                            }
                        }
                        else if (user.UserTypeID == 3) // Seeker
                        {
                            var seeker = db.SeekerTables.Where(s => s.UserID == user.UserID).FirstOrDefault();
                            if (seeker != null)
                            {
                                Session["SeekerID"] = seeker.SeekerID;
                                Session["FullName"] = seeker.Fullname;
                                Session["Age"] = seeker.Age;
                                Session["CityID"] = seeker.CityID;
                                Session["City"] = seeker.CityTable.City;
                                Session["BloodGroupID"] = seeker.BloodGroupID;
                                Session["BloodGroup"] = seeker.BloodGroupsTable.BloodGroup;
                                Session["ContactNo"] = seeker.ContactNo;
                                Session["CNIC"] = seeker.CNIC;
                                Session["GenderID"] = seeker.GenderID;
                                Session["GenderID"] = seeker.GenderTable.Gender;
                                Session["RegestrationDate"] = seeker.RegestrationDate;
                                Session["Location"] = seeker.Address;
                                return RedirectToAction("Seeker", "Dashboard");

                            }
                            else
                            {
                                ModelState.AddModelError(string.Empty, "Invalid Username or Password");
                            }
                        }
                        else if (user.UserTypeID == 4) // Hospital
                        {
                            var hospital = db.HospitalTables.Where(h => h.UserID == user.UserID).FirstOrDefault();
                            if (hospital != null)
                            {
                                Session["HospitalID"] = hospital.HospitalID;
                                Session["FullName"] = hospital.FullName;
                                Session["Address"] = hospital.Address;
                                Session["PhoneNo"] = hospital.PhoneNo;
                                Session["Website"] = hospital.Website;
                                Session["Email"] = hospital.Email;
                                Session["Location"] = hospital.Location;
                                Session["CityID"] = hospital.CityID;
                                Session["City"] = hospital.CityTable.City;
                                return RedirectToAction("Hospital", "Dashboard");
                            }
                            else
                            {
                                ModelState.AddModelError(string.Empty, "Invalid Username or Password");
                            }
                        }
                        else if (user.UserTypeID == 5) // Blood Bank
                        {
                            var bloodBank = db.BloodBanKTables.Where(b => b.UserID == user.UserID).FirstOrDefault(); //bloodbank
                            if (bloodBank != null)
                            {
                                Session["BloodBankID"] = bloodBank.BloodBankID;
                                Session["BloodBankName"] = bloodBank.BloodBankName;
                                Session["Address"] = bloodBank.Address;
                                Session["PhoneNo"] = bloodBank.PhoneNo;
                                Session["Location"] = bloodBank.Location;
                                Session["Website"] = bloodBank.Website;
                                Session["Email"] = bloodBank.Email;
                                Session["CityID"] = bloodBank.CityID;
                                Session["City"] = bloodBank.CityTable.City;
                                return RedirectToAction("BloodBank", "Dashboard");
                            }
                            else
                            {
                                ModelState.AddModelError(string.Empty, "Invalid Username or Password");
                            }
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Invalid Username or Password");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid Username or Password");
                    }

                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid Username or Password");
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Please Provide Username and Password!");
            }
            ClearSession();
            return View(userMV);
        }

        private void ClearSession()
        {
            Session["UserID"] = string.Empty;
            Session["UserName"] = string.Empty;
            Session["Password"] = string.Empty;
            Session["EmailAddress"] = string.Empty;
            Session["AccountStatusID"] = string.Empty;
            Session["AccountStatus"] = string.Empty;
            Session["UserTypeID"] = string.Empty;
            Session["UserType"] = string.Empty;
            Session["Description"] = string.Empty;
        }


        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(string email)
        {
            var user = db.UserTables.FirstOrDefault(u => u.EmailAddress == email);
            if (user != null)
            {
                // Generate a reset token
                var token = Guid.NewGuid().ToString();
                user.ResetPasswordToken = token;
                user.ResetPasswordTokenExpiry = DateTime.Now.AddHours(1); // Token valid for 1 hour
                db.SaveChanges();

                // Send email with reset link
                var resetLink = Url.Action("ResetPassword", "Home", new { token = token }, protocol: Request.Url.Scheme);
                var emailBody = $"Please reset your password by clicking <a href='{resetLink}'>here</a>.";

                try
                {
                    SendEmail(user.EmailAddress, "Password Reset Request", emailBody);
                    ViewBag.Message = "Password reset link has been sent to your email.";
                }
                catch (Exception ex)
                {
                    // Log the exception
                    System.Diagnostics.Debug.WriteLine("Email sending failed: " + ex.Message);
                    ModelState.AddModelError(string.Empty, "Failed to send email. Please try again later.");
                }

                return View();
            }
            else
            {
                ModelState.AddModelError(string.Empty, "No account found with this email address.");
                return View();
            }
        }

        // GET: ResetPassword (Displays the reset password form)
        public ActionResult ResetPassword(string token)
        {
            var user = db.UserTables.FirstOrDefault(u => u.ResetPasswordToken == token && u.ResetPasswordTokenExpiry > DateTime.Now);
            if (user == null)
            {
                ViewBag.ErrorMessage = "Invalid or expired token.";
                return View("Error");
            }

            ViewBag.Token = token;
            return View();
        }

        // POST: ResetPassword (Handles the form submission)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(string token, string newPassword, string confirmPassword)
        {
            var user = db.UserTables.FirstOrDefault(u => u.ResetPasswordToken == token && u.ResetPasswordTokenExpiry > DateTime.Now);
            if (user != null)
            {
                if (newPassword == confirmPassword)
                {
                    // Update password
                    user.Password = newPassword;
                    user.ResetPasswordToken = null;
                    user.ResetPasswordTokenExpiry = null;
                    db.SaveChanges();

                    ViewBag.Message = "Your password has been reset successfully.";
                    return View("Login");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "The new password and confirmation password do not match.");
                    return View();
                }
            }
            else
            {
                ViewBag.ErrorMessage = "Invalid or expired token.";
                return View("Error");
            }
        }

        private void SendEmail(string toEmail, string subject, string body)
        {
            try
            {
                using (var smtpClient = new SmtpClient())
                {
                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress("bloodbond247@gmail.com"), // Use the email from web.config
                        Subject = subject,
                        Body = body,
                        IsBodyHtml = true
                    };
                    mailMessage.To.Add(toEmail);

                    smtpClient.Send(mailMessage);
                }
            }
            catch (SmtpException ex)
            {
                // Log the SMTP-specific error
                System.Diagnostics.Debug.WriteLine("SMTP Error: " + ex.Message);
                System.Diagnostics.Debug.WriteLine("SMTP Status Code: " + ex.StatusCode);
                throw new Exception("SMTP Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                // Log the general error
                System.Diagnostics.Debug.WriteLine("Failed to send email: " + ex.Message);
                throw new Exception("Failed to send email: " + ex.Message);
            }
        }

        public ActionResult Logout()
        {
            ClearSession();
            return RedirectToAction("MainHome");
        }

        public ActionResult AboutUs()
        {
            return View();
        }

        public ActionResult FAQ()
        {
            return View();
        }
        public ActionResult Contact()
        {
            return View();
        }


    }
}