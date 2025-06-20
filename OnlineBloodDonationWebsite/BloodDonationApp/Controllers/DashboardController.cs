using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BloodDonationApp.Models;
using DatabaseLayer;
// Ensure you have the correct namespace for the service

namespace BloodDonationApp.Controllers
{
    public class DashboardController : Controller
    {
        OnlineBloodBankDbEntities db = new OnlineBloodBankDbEntities();


        // GET: Dashboard
        public ActionResult Donor()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }

            int UserTypeID = 0;
            int AcceptedTypeID = 0;
            int AcceptedByID = 0;
            int.TryParse(Convert.ToString(Session["UserTypeID"]), out UserTypeID);

            if (UserTypeID == 2) // Donor
            {
                AcceptedTypeID = 1;
                int.TryParse(Convert.ToString(Session["DonorID"]), out AcceptedByID);
            }
            else if (UserTypeID == 3) // Seeker
            {

                int.TryParse(Convert.ToString(Session["SeekerID"]), out AcceptedByID);
            }
            else if (UserTypeID == 4) // Hospital
            {

                int.TryParse(Convert.ToString(Session["HospitalID"]), out AcceptedByID);

            }
            else if (UserTypeID == 5) // Blood Bank
            {
                AcceptedTypeID = 2;
                int.TryParse(Convert.ToString(Session["BloodBankID"]), out AcceptedByID);
            }
            var requests = db.RequestTables.Where(r => r.AcceptedID == AcceptedByID && r.AcceptedTypeID == AcceptedTypeID).ToList();
            var list = new List<RequestListMV>();
            foreach (var request in requests)
            {
                var addrequest = new RequestListMV();
                addrequest.RequestID = request.RequestID;
                addrequest.RequestDate = request.RequestDate.ToString("dd MMMM, yyyy");
                addrequest.RequestByID = request.RequestByID;
                addrequest.AcceptedID = request.AcceptedID;

                if (request.RequestTypeID == 1)//Seeker
                {
                    var getseeker = db.SeekerTables.Find(request.RequestByID);
                    addrequest.RequestBy = getseeker.Fullname;
                    addrequest.ContactNo = getseeker.ContactNo;
                    addrequest.Address = getseeker.Address;
                }
                else if (request.RequestTypeID == 2)//Hospital
                {
                    var gethospital = db.HospitalTables.Find(request.RequestByID);
                    addrequest.RequestBy = gethospital.FullName;
                    addrequest.ContactNo = gethospital.PhoneNo;
                    addrequest.Address = gethospital.Address;
                }
                else if (request.RequestTypeID == 3)//Blood Bank
                {
                    var getbloodbank = db.BloodBanKTables.Find(request.RequestByID);
                    addrequest.RequestBy = getbloodbank.BloodBankName;
                    addrequest.ContactNo = getbloodbank.PhoneNo;
                    addrequest.Address = getbloodbank.Address;
                }

                addrequest.AcceptedTypeID = request.AcceptedTypeID;
                addrequest.AcceptedType = request.AcceptedTypeTable.AcceptedType;
                addrequest.RequiredBloodGroupID = request.RequiredBloodGroupID;
                var bloodgroup = db.BloodGroupsTables.Find(addrequest.RequiredBloodGroupID);
                addrequest.BloodGroup = bloodgroup.BloodGroup;
                addrequest.RequestTypeID = request.RequestID;
                addrequest.RequestType = request.RequestTypeTable.RequestType;
                addrequest.RequestStatus = request.RequestStatusTable.RequestStatus;
                addrequest.RequestStatusID = request.RequestStatusID;
                addrequest.ExpectedDate = request.ExpectedDate.ToString("dd MMMM, yyyy");
                addrequest.RequestDetails = request.RequestDetails;
                list.Add(addrequest);
            }

            return View(list);
        }


        public ActionResult Seeker()
        {

            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }

            int UserTypeID = 0;
            int RequestTypeID = 0;
            int RequestByID = 0;
            int.TryParse(Convert.ToString(Session["UserTypeID"]), out UserTypeID);

            if (UserTypeID == 2) // Donor
            {
                int.TryParse(Convert.ToString(Session["DonorID"]), out RequestByID);
            }
            else if (UserTypeID == 3) // Seeker
            {
                RequestTypeID = 1;
                int.TryParse(Convert.ToString(Session["SeekerID"]), out RequestByID);
            }
            else if (UserTypeID == 4) // Hospital
            {
                RequestTypeID = 2;
                int.TryParse(Convert.ToString(Session["HospitalID"]), out RequestByID);

            }
            else if (UserTypeID == 5) // Blood Bank
            {
                RequestTypeID = 3;
                int.TryParse(Convert.ToString(Session["BloodBankID"]), out RequestByID);
            }
            var requests = db.RequestTables.Where(r => r.RequestByID == RequestByID && r.RequestTypeID == RequestTypeID).ToList();
            var list = new List<RequestListMV>();
            foreach (var request in requests)
            {
                var addrequest = new RequestListMV();
                addrequest.RequestID = request.RequestID;
                addrequest.RequestDate = request.RequestDate.ToString("dd MMMM, yyyy");
                addrequest.RequestByID = request.RequestByID;
                addrequest.AcceptedID = request.AcceptedID;

                if (request.AcceptedTypeID == 1)//Donor
                {
                    var getdonor = db.DonorTables.Find(request.AcceptedID);
                    addrequest.AcceptedFullName = getdonor.FullName;
                    addrequest.ContactNo = getdonor.ContactNo;
                    addrequest.Address = getdonor.Location;
                }
                else if (request.AcceptedTypeID == 2)//Blood Bank
                {
                    var getbloodbank = db.BloodBanKTables.Find(request.AcceptedID);
                    addrequest.AcceptedFullName = getbloodbank.BloodBankName;
                    addrequest.ContactNo = getbloodbank.PhoneNo;
                    addrequest.Address = getbloodbank.Address;
                }

                addrequest.AcceptedTypeID = request.AcceptedTypeID;
                addrequest.AcceptedType = request.AcceptedTypeTable.AcceptedType;
                addrequest.RequiredBloodGroupID = request.RequiredBloodGroupID;
                var bloodgroup = db.BloodGroupsTables.Find(addrequest.RequiredBloodGroupID);
                addrequest.BloodGroup = bloodgroup.BloodGroup;
                addrequest.RequestTypeID = request.RequestID;
                addrequest.RequestType = request.RequestTypeTable.RequestType;
                addrequest.RequestStatus = request.RequestStatusTable.RequestStatus;
                addrequest.RequestStatusID = request.RequestStatusID;
                addrequest.ExpectedDate = request.ExpectedDate.ToString("dd MMMM, yyyy");
                addrequest.RequestDetails = request.RequestDetails;
                list.Add(addrequest);
            }

            return View(list);
        }


        public ActionResult Hospital()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }

            int UserTypeID = 0;
            int RequestTypeID = 0;
            int RequestByID = 0;
            int.TryParse(Convert.ToString(Session["UserTypeID"]), out UserTypeID);

            if (UserTypeID == 2) // Donor
            {
                int.TryParse(Convert.ToString(Session["DonorID"]), out RequestByID);
            }
            else if (UserTypeID == 3) // Seeker
            {
                RequestTypeID = 1;
                int.TryParse(Convert.ToString(Session["SeekerID"]), out RequestByID);
            }
            else if (UserTypeID == 4) // Hospital
            {
                RequestTypeID = 2;
                int.TryParse(Convert.ToString(Session["HospitalID"]), out RequestByID);

            }
            else if (UserTypeID == 5) // Blood Bank
            {
                RequestTypeID = 3;
                int.TryParse(Convert.ToString(Session["BloodBankID"]), out RequestByID);
            }
            var requests = db.RequestTables.Where(r => r.RequestByID == RequestByID && r.RequestTypeID == RequestTypeID).ToList();
            var list = new List<RequestListMV>();
            foreach (var request in requests)
            {
                var addrequest = new RequestListMV();
                addrequest.RequestID = request.RequestID;
                addrequest.RequestDate = request.RequestDate.ToString("dd MMMM, yyyy");
                addrequest.RequestByID = request.RequestByID;
                addrequest.AcceptedID = request.AcceptedID;

                if (request.AcceptedTypeID == 1)//Donor
                {
                    var getdonor = db.DonorTables.Find(request.AcceptedID);
                    addrequest.AcceptedFullName = getdonor.FullName;
                    addrequest.ContactNo = getdonor.ContactNo;
                    addrequest.Address = getdonor.Location;
                }
                else if (request.AcceptedTypeID == 2)//Blood Bank
                {
                    var getbloodbank = db.BloodBanKTables.Find(request.AcceptedID);
                    addrequest.AcceptedFullName = getbloodbank.BloodBankName;
                    addrequest.ContactNo = getbloodbank.PhoneNo;
                    addrequest.Address = getbloodbank.Address;
                }

                addrequest.AcceptedTypeID = request.AcceptedTypeID;
                addrequest.AcceptedType = request.AcceptedTypeTable.AcceptedType;
                addrequest.RequiredBloodGroupID = request.RequiredBloodGroupID;
                var bloodgroup = db.BloodGroupsTables.Find(addrequest.RequiredBloodGroupID);
                addrequest.BloodGroup = bloodgroup.BloodGroup;
                addrequest.RequestTypeID = request.RequestID;
                addrequest.RequestType = request.RequestTypeTable.RequestType;
                addrequest.RequestStatus = request.RequestStatusTable.RequestStatus;
                addrequest.RequestStatusID = request.RequestStatusID;
                addrequest.ExpectedDate = request.ExpectedDate.ToString("dd MMMM, yyyy");
                addrequest.RequestDetails = request.RequestDetails;
                list.Add(addrequest);
            }

            return View(list);
        }

        public ActionResult BloodBank()
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
    }
}
