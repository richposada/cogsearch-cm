using Microsoft.AspNetCore.Mvc;
using CognitiveSearch.UI.Models;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Data;
using System;
using System.Data.SqlClient;
using System.Linq;
using CognitiveSearch.UI.Models;



namespace CognitiveSearch.UI.Controllers
{
    public class FOIAController : Controller
    {
        private IConfiguration _configuration { get; set; }
        private DocumentSearchClient _docSearch { get; set; }
        private string _configurationError { get; set; }

        public FOIAController(IConfiguration configuration)
        {
            _configuration = configuration;
            InitializeDocSearch();
        }

        private void InitializeDocSearch()
        {
            try
            {
                _docSearch = DocumentSearchClient.Instance(_configuration);
            }
            catch (Exception e)
            {
                _configurationError = $"The application settings are possibly incorrect. The server responded with this message: " + e.Message.ToString();
            }
        }

        public bool CheckDocSearchInitialized()
        {
            if (_docSearch == null)
            {
                ViewBag.Style = "alert-warning";
                ViewBag.Message = _configurationError;
                return false;
            }

            return true;
        }


        public IActionResult Index()
        {
            return View();
        }

        public ActionResult Search()
        {
            return PartialView("_Search");
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


        public IActionResult SearchIndex([FromQuery] string q, [FromQuery] string facets = "", [FromQuery] int page = 1)
        {
            // Split the facets.
            //  Expected format: &facets=key1_val1,key1_val2,key2_val1
            var searchFacets = facets
                // Split by individual keys
                .Split(",", StringSplitOptions.RemoveEmptyEntries)
                // Split key/values
                .Select(f => f.Split("_", StringSplitOptions.RemoveEmptyEntries))
                // Group by keys
                .GroupBy(f => f[0])
                // Select grouped key/values into SearchFacet array
                .Select(g => new SearchFacet { Key = g.Key, Value = g.Select(f => f[1]).ToArray() })
                .ToArray();

            var viewModel = SearchView(new SearchOptions
            {
                q = q,
                searchFacets = searchFacets,
                currentPage = page
            });

            return PartialView("_SearchResults", viewModel);
        }

        public class SearchOptions
        {
            public string q { get; set; }
            public SearchFacet[] searchFacets { get; set; }
            public int currentPage { get; set; }
            public string polygonString { get; set; }
        }

        [HttpPost]
        public SearchResultViewModel SearchView([FromForm] SearchOptions searchParams)
        {
            if (searchParams.q == null)
                searchParams.q = "*";
            if (searchParams.searchFacets == null)
                searchParams.searchFacets = new SearchFacet[0];
            if (searchParams.currentPage == 0)
                searchParams.currentPage = 1;

            string searchidId = null;

            if (CheckDocSearchInitialized())
                searchidId = _docSearch.GetSearchId().ToString();

            var viewModel = new SearchResultViewModel
            {
                documentResult = _docSearch.GetDocuments(searchParams.q, searchParams.searchFacets, searchParams.currentPage, searchParams.polygonString),
                query = searchParams.q,
                selectedFacets = searchParams.searchFacets,
                currentPage = searchParams.currentPage,
                searchId = searchidId ?? null,
                applicationInstrumentationKey = _configuration.GetSection("InstrumentationKey")?.Value,
                searchServiceName = _configuration.GetSection("SearchServiceName")?.Value,
                indexName = _configuration.GetSection("SearchIndexName")?.Value,
                facetableFields = _docSearch.Model.Facets.Select(k => k.Name).ToArray()
            };
            return viewModel;
        }

        [HttpPost]
        public IActionResult GetDocumentById(string id = "")
        {
            var result = _docSearch.GetDocumentById(id);

            return new JsonResult(result);
        }

        
    }
}
