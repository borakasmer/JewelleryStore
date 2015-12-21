using DAL;
using JewelleryStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JewelleryStore.Models
{
    [AttributeUsageAttribute(AttributeTargets.All, AllowMultiple = true)]
    public class LogAttribute : ActionFilterAttribute
    {
        public LogAttribute(LogType type,bool isBefore)
        {
            LgType = type;
            IsBefore = isBefore;
        }
        public LogType LgType { get; set; }
        public bool IsBefore { get; set; }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            //if (filterContext.HttpContext.Session["UserID"] != null && LgType != LogType.LogOut && LgType != LogType.Log)
            if (filterContext.HttpContext.Session["UserID"] != null && IsBefore==false)
            {
                //if (filterContext.HttpContext.Session["ProductID"] != null && (LgType == LogType.ProductDelete || LgType == LogType.ProductInsert || LgType == LogType.ProductUpdate))
                //if (filterContext.HttpContext.Session["ProductID"] != null && (LgType == LogType.ProductDelete || LgType == LogType.ProductInsert || LgType == LogType.ProductUpdate))
                if (filterContext.HttpContext.Session["ProductID"] != null)
                {
                    using (JewelleryStoreDB dbContext = new JewelleryStoreDB())
                    {
                        tblLog log = new tblLog();
                        log.CreatedDate = DateTime.Now;
                        log.LogType = LgType.ToString();
                        log.UserID = int.Parse(filterContext.HttpContext.Session["UserID"].ToString());
                        log.ProductID = int.Parse(filterContext.HttpContext.Session["ProductID"].ToString());
                        dbContext.tblLog.Add(log);
                        dbContext.SaveChanges();
                        if (filterContext.HttpContext.Session["ProductID"] != null)
                        { filterContext.HttpContext.Session.Remove("ProductID"); }

                    }
                }
                else
                {
                    using (JewelleryStoreDB dbContext = new JewelleryStoreDB())
                    {
                        tblLog log = new tblLog();
                        log.CreatedDate = DateTime.Now;
                        log.LogType = LgType.ToString();
                        log.UserID = int.Parse(filterContext.HttpContext.Session["UserID"].ToString());
                        dbContext.tblLog.Add(log);
                        dbContext.SaveChanges();
                    }
                }
            }
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //if (filterContext.HttpContext.Session["UserID"] != null && (LgType == LogType.LogOut || LgType == LogType.Log))
            if (filterContext.HttpContext.Session["UserID"] != null && IsBefore==true)
            {
                using (JewelleryStoreDB dbContext = new JewelleryStoreDB())
                {
                    tblLog log = new tblLog();
                    log.CreatedDate = DateTime.Now;
                    log.LogType = LgType.ToString();
                    log.UserID = int.Parse(filterContext.HttpContext.Session["UserID"].ToString());
                    dbContext.tblLog.Add(log);
                    dbContext.SaveChanges();
                }
            }
        }
    }
}