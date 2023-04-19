using Microsoft.AspNetCore.Mvc;
using CognitiveSearch.UI.Models;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Data;
using System;
using System.Data.SqlClient;

namespace CognitiveSearch.UI.Controllers
{
    public class RequestsController : Controller
    {
        private IConfiguration _configuration { get; set; }
        public RequestsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public IActionResult Index()
        {
            return View();
        }



        [HttpPost]
        public JsonResult GetFOIARequests()
        {
            var results = new List<FOIARequest>();

            SqlConnection conn;
            var connStr = _configuration.GetSection("FOIA-DB-ConnStr")?.Value;

            try
            {
                conn = new SqlConnection(connStr);

                using (SqlDataAdapter da = new SqlDataAdapter())
                {
                    da.SelectCommand = new SqlCommand("dbo.GetFOIARequests", conn);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;

                    DataSet ds = new DataSet();
                    da.Fill(ds, "result_name");

                    DataTable dt = ds.Tables["result_name"];

                    foreach (DataRow row in dt.Rows)
                    {
                        var foiaRequest = new FOIARequest
                        {
                            CaseNumber = row["CaseNumber"].ToString(),
                            CaseType = row["CaseType"].ToString(),
                            RequestorFullName = row["RequestorFullName"].ToString(),
                            RequestorOrganization = row["RequestorOrganization"].ToString(),
                            AssignedOfficerName = row["AssignedOfficerName"].ToString(),

                        };

                        if (row["RequestDate"] != DBNull.Value)
                        {
                            foiaRequest.RequestDate = Convert.ToDateTime(row["RequestDate"]);
                        }

                        results.Add(foiaRequest);
                    }
                }

            }
            catch (Exception e)
            {
                Console.Write(e.Message, e);
            }

            return Json(results);
        }


        public ActionResult CreateRequest()
        {
            
            return View("Details");
        }

        public ActionResult FOIARequest()
        {
            FOIARequest foiaRequest = new FOIARequest();
            return PartialView("_FOIARequest", foiaRequest);
        }
    }
}
