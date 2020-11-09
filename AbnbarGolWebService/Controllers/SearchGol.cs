using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using AbnbarGolWebService.Models;

namespace AbnbarGolWebService.Controllers
{
    [EnableCors("*", "*", "get")]
    public class SearchController : ApiController
    {
        private readonly SqlConnection _con = new SqlConnection(ConfigurationManager.ConnectionStrings["flower_depot"].ConnectionString);

        [Route("api/search")]
        [HttpGet]
        public IHttpActionResult SearchInGol(string name, string code)
        {
            var str1 = name.Replace("ک", "ك").Replace("ی", "ي").Replace("ة", "ه");
            var str2 = name.Replace("ك", "ک").Replace("ي", "ی").Replace("ه", "ة");
            _con.Open();
            var content = new List<Gol>();
            var sqlDataReader = new SqlCommand("SELECT dbo.flower_colors.flow_color, dbo.flower_colortypes.flow_colortype," +
                                               " flower_entry.id, dbo.flower_formats.flow_format, dbo.flower_customers.customer_name," +
                                               " dbo.flower_companies.company_name , flower_entry.flower_name, flower_entry.flower_code," +
                                               " flower_entry.enter_date , flower_entry.comment FROM dbo.flower_entry INNER JOIN " +
                                               "dbo.flower_colors ON dbo.flower_entry.flower_color = dbo.flower_colors.flowcolor_id" +
                                               " INNER JOIN dbo.flower_colortypes ON dbo.flower_entry.flower_colortype = " +
                                               "dbo.flower_colortypes.colortype_id INNER JOIN dbo.flower_formats ON dbo.flower_entry.flower_format =" +
                                               " dbo.flower_formats.flowformat_id INNER JOIN dbo.flower_customers ON dbo.flower_entry.customer_name =" +
                                               " dbo.flower_customers.customer_id INNER JOIN dbo.flower_companies ON dbo.flower_entry.company_name =" +
                                               " dbo.flower_companies.company_id where flower_entry.flower_name like N'%" + name + "%' " +
                                               "or flower_entry.flower_name like N'%" + str1 + "%' or flower_entry.flower_name like N'%" + str2 + "%' " +
                                               "or flower_entry.flower_code like N'%" + code + "%'", _con).ExecuteReader();
            while (sqlDataReader.Read())
                content.Add(new Gol()
                {
                    Id = Convert.ToInt32(sqlDataReader["id"]),
                    GolName = sqlDataReader["flower_name"].ToString(),
                    Color = sqlDataReader["flow_color"].ToString(),
                    ColorType = sqlDataReader["flow_colortype"].ToString(),
                    Format = sqlDataReader["flow_format"].ToString(),
                    Code = sqlDataReader["flower_code"].ToString(),
                    EnterDate = sqlDataReader["enter_date"].ToString(),
                    Customer = sqlDataReader["customer_name"].ToString(),
                    Company = sqlDataReader["company_name"].ToString(),
                    Comment = sqlDataReader["comment"].ToString()
                });
            _con.Close();
            return Json(content);
        }
    }
}