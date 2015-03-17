using System;
using System.Dynamic;
using System.Web.Mvc;
using MLWD.Umbraco.Mvc.Controllers.Base;
using jumps.umbraco.usync;
using jumps.umbraco.usync.Interfaces;

namespace MLWD.Umbraco.Mvc.Controllers.CMS
{
    public class MLWDAdminCMSController : BaseController
    {
        private IuSyncService _uSync { get; set; }

        public MLWDAdminCMSController()
        {
            _uSync = new uSyncService();    
        }

        private dynamic DoRequest(string message, Action action)
        {
            dynamic response = new ExpandoObject();

            response.result = true;
            response.message = message;

            try
            {
                action();
            }
            catch (Exception ex)
            {
                response.result = false;
                response.message = ex.Message;
            }

            return response;
        }

        [HttpPost]
        public ActionResult SyncDataTypes()
        {
            var response = DoRequest("Data types synced successfully", () => _uSync.Sync());

            return Json(response);
        }

        [HttpPost]
        public ActionResult SaveDataTypes()
        {
            var response = DoRequest("Data types saved successfully", () => _uSync.Save());

            return Json(response);
        }
    }
}