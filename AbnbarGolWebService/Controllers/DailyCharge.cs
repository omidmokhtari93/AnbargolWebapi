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
    public class DailyChargeController : ApiController
    {
        private readonly SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["flower_depot"].ConnectionString);

        [Route("api/getdaily")]
        [HttpGet]
        public IHttpActionResult GetDailyChargeActionResult()
        {
            con.Open();
            var list = new List<Gol>();
            var cmd = new SqlCommand("select * from (SELECT sum(cutted_and_remain.total)as total ,DATEDIFF(DAY , GETDATE() , orders.Created) as days, " +
                                     "flower_colors.flow_color, flower_colortypes.flow_colortype, flower_entry.id, " +
                                     "flower_formats.flow_format, flower_customers.customer_name, flower_companies.company_name " +
                                     ", flower_entry.flower_name, flower_entry.flower_code, flower_entry.enter_date " +
                                     ", flower_entry.comment FROM flower_entry INNER JOIN " +
                                     "flower_colors ON flower_entry.flower_color = flower_colors.flowcolor_id INNER JOIN " +
                                     "flower_colortypes ON flower_entry.flower_colortype = flower_colortypes.colortype_id INNER JOIN " +
                                     "flower_formats ON flower_entry.flower_format = flower_formats.flowformat_id INNER JOIN " +
                                     "flower_customers ON flower_entry.customer_name = flower_customers.customer_id INNER JOIN " +
                                     "flower_companies ON flower_entry.company_name = flower_companies.company_id inner join " +
                                     "cutted_and_remain on flower_entry.id = cutted_and_remain.flower_id inner join " +
                                     "orders on flower_entry.id = orders.flower_id " +
                                     "group by flower_colors.flow_color, flower_colortypes.flow_colortype, flower_entry.id, " +
                                     "flower_formats.flow_format, flower_customers.customer_name, flower_entry.flower_name, " +
                                     "flower_companies.company_name, flower_entry.flower_code, flower_entry.enter_date, " +
                                     "flower_entry.comment, orders.Created)i where i.total > 0 and i.days is not null and i.days = 0", con);
            var rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                list.Add(new Gol()
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
            return Json(list);
        }
    }
}