using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Script.Serialization;
using AbnbarGolWebService.Models;

namespace AbnbarGolWebService.Controllers
{
    [EnableCors("*", "*", "get")]
    public class GetGolsByFormatController : ApiController
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["flower_depot"].ConnectionString);
        [Route("api/golbyformat")]
        [HttpGet]
        public IHttpActionResult GetGolByCustomerActionResult(int format)
        {
            con.Open();
            var gols = new List<Gol>();
            var cmd = new SqlCommand("SELECT dbo.flower_colors.flow_color, dbo.flower_colortypes.flow_colortype, flower_entry.id, " +
                                     "dbo.flower_formats.flow_format, dbo.flower_customers.customer_name, dbo.flower_companies.company_name " +
                                     ", flower_entry.flower_name, flower_entry.flower_code, flower_entry.enter_date " +
                                     ", flower_entry.comment FROM dbo.flower_entry INNER JOIN " +
                                     "dbo.flower_colors ON dbo.flower_entry.flower_color = dbo.flower_colors.flowcolor_id INNER JOIN " +
                                     "dbo.flower_colortypes ON dbo.flower_entry.flower_colortype = dbo.flower_colortypes.colortype_id INNER JOIN " +
                                     "dbo.flower_formats ON dbo.flower_entry.flower_format = dbo.flower_formats.flowformat_id INNER JOIN " +
                                     "dbo.flower_customers ON dbo.flower_entry.customer_name = dbo.flower_customers.customer_id INNER JOIN " +
                                     "dbo.flower_companies ON dbo.flower_entry.company_name = dbo.flower_companies.company_id " +
                                     "where flower_entry.flower_format = " + format + " ", con);
            var rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                gols.Add(new Gol()
                {
                    Id = Convert.ToInt32(rd["id"]),
                    GolName = rd["flower_name"].ToString(),
                    Color = rd["flow_color"].ToString(),
                    ColorType = rd["flow_colortype"].ToString(),
                    Format = rd["flow_format"].ToString(),
                    Code = rd["flower_code"].ToString(),
                    EnterDate = rd["enter_date"].ToString(),
                    Customer = rd["customer_name"].ToString(),
                    Company = rd["company_name"].ToString(),
                    Comment = rd["comment"].ToString()
                });
            }
            con.Close();
            return Json(gols);
        }

    }
}