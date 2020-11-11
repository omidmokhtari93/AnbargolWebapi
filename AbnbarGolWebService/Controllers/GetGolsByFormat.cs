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
        private readonly SqlConnection _con = new SqlConnection(ConfigurationManager.ConnectionStrings["flower_depot"].ConnectionString);
        [Route("api/golbyformat")]
        [HttpGet]
        public IHttpActionResult GetGolByCustomerActionResult(int format)
        {
            _con.Open();
            var gols = new List<Gol>();
            var cmd = new SqlCommand("select flower_colors.flow_color, flower_colortypes.flow_colortype, flower_entry.id, " +
                                             "flower_formats.flow_format, flower_customers.customer_name, flower_companies.company_name " +
                                             ", flower_entry.flower_name, flower_entry.flower_code, flower_entry.enter_date " +
                                             ", flower_entry.comment FROM flower_entry INNER JOIN " +
                                             "flower_colors ON flower_entry.flower_color = flower_colors.flowcolor_id INNER JOIN " +
                                             "flower_colortypes ON flower_entry.flower_colortype = flower_colortypes.colortype_id INNER JOIN " +
                                             "flower_formats ON flower_entry.flower_format = flower_formats.flowformat_id INNER JOIN " +
                                             "flower_customers ON flower_entry.customer_name = flower_customers.customer_id INNER JOIN " +
                                             "flower_companies ON flower_entry.company_name = flower_companies.company_id " +
                                             "where flower_entry.id in (select j.id from(select sum(i.mojoodi) as mojoodi, i.id " +
                                             "from(select flower_entry.id, sum(new_halfcut.tedad) as mojoodi from " +
                                             "flower_entry inner join new_halfcut on flower_entry.id = new_halfcut.flowid " +
                                             "where flower_entry.flower_format = @format group by flower_entry.id union all " +
                                             "select flower_entry.id, sum(cutted_and_remain.cutted) as mojoodi from flower_entry " +
                                             "inner join cutted_and_remain on flower_entry.id = cutted_and_remain.flower_id " +
                                             "where flower_entry.flower_format = @format group by flower_entry.id union all " +
                                             "select flower_entry.id, SUM(flower_forms_entry.sheetcount) as mojoodi from flower_entry " +
                                             "inner join flower_forms_entry on flower_entry.id = flower_forms_entry.flower_id " +
                                             "where flower_entry.flower_format = @format group by flower_entry.id)i group by i.id)j where j.mojoodi > 0)", _con);
            cmd.Parameters.AddWithValue("@format", format);
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
            _con.Close();
            return Json(gols);
        }

    }
}