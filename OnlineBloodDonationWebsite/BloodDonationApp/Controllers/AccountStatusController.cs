using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BloodDonationApp.Models;
using DatabaseLayer;

namespace BloodDonationApp.Controllers
{
    public class AccountStatusController : Controller
    {
        OnlineBloodBankDbEntities DB = new OnlineBloodBankDbEntities();
        public ActionResult AllAccountStatus()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var accountstatuses = DB.AccountStatusTables.ToList();
            var listaccountstatuses = new List<AccountStatusMV>();
            foreach (var accountstatus in accountstatuses)
            {
                var addaccountstatusmv = new AccountStatusMV();
                addaccountstatusmv.AccountStatusID = accountstatus.AccountStatusID;
                addaccountstatusmv.AccountStatus = accountstatus.AccountStatus;
                listaccountstatuses.Add(addaccountstatusmv);
            }
            return View(listaccountstatuses);
        }

        public ActionResult Create()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var accountstatus = new AccountStatusMV();
            return View(accountstatus);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AccountStatusMV accountStatusMV)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            if (ModelState.IsValid)
            {
                var checkaccountstatus = DB.AccountStatusTables.Where(b => b.AccountStatus == accountStatusMV.AccountStatus).FirstOrDefault();
                if (checkaccountstatus == null)
                {
                    var accountstatusesTable = new AccountStatusTable();
                    accountstatusesTable.AccountStatusID = accountStatusMV.AccountStatusID;
                    accountstatusesTable.AccountStatus = accountStatusMV.AccountStatus;
                    DB.AccountStatusTables.Add(accountstatusesTable);
                    DB.SaveChanges();
                    return RedirectToAction("AllAccountStatus");
                }
                else
                {
                    ModelState.AddModelError("AccountStatus", "Already Exists");
                }
            }
            return View(accountStatusMV);
        }

        public ActionResult Edit(int id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var accountstatus = DB.AccountStatusTables.Find(id);
            if (accountstatus == null)
            {
                return HttpNotFound();
            }
            var accountStatusMV = new AccountStatusMV();
            accountStatusMV.AccountStatusID = accountstatus.AccountStatusID;
            accountStatusMV.AccountStatus = accountstatus.AccountStatus;

            return View(accountStatusMV);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AccountStatusMV accountStatusMV)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            if (ModelState.IsValid)
            {
                var checkaccountstatus = DB.AccountStatusTables.Where(b => b.AccountStatus == accountStatusMV.AccountStatus && b.AccountStatusID != accountStatusMV.AccountStatusID).FirstOrDefault();
                if (checkaccountstatus == null)
                {
                    var accountstatusesTable = new AccountStatusTable();
                    accountstatusesTable.AccountStatusID = accountStatusMV.AccountStatusID;
                    accountstatusesTable.AccountStatus = accountStatusMV.AccountStatus;
                    DB.Entry(accountstatusesTable).State = EntityState.Modified;
                    DB.SaveChanges();
                    return RedirectToAction("AllAccountStatus");
                }
                else
                {
                    ModelState.AddModelError("AccountStatus", "Already Exists");
                }
            }
            return View(accountStatusMV);
        }

        public ActionResult Delete(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var accountstatus = DB.AccountStatusTables.Find(id);
            if (accountstatus == null)
            {
                return HttpNotFound();
            }
            var accountStatusMV = new AccountStatusMV();
            accountStatusMV.AccountStatusID = accountstatus.AccountStatusID;
            accountStatusMV.AccountStatus = accountstatus.AccountStatus;

            return View(accountStatusMV);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirm(int id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var accountstatus = DB.AccountStatusTables.Find(id);
            DB.AccountStatusTables.Remove(accountstatus);
            DB.SaveChanges();
            return RedirectToAction("AllAccountStatus");
        }
    }
}  
