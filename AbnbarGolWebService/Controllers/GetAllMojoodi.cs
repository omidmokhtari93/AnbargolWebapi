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
    public class MojoodiController : ApiController
    {
        private readonly SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["flower_depot"].ConnectionString);

        [Route("api/getall")]
        [HttpGet]
        public IHttpActionResult GetMojoodi(int id)
        {
            var content = new List<MojoodiKol>();
            this.con.Open();
            new SqlCommand("update cutted_and_remain set falleh = j.falleh ,service = j.service from " +
                           "(select item,case when[1] is null then 0 else [1] end as service ,case when[2] is " +
                           "null then 0 else [2] end as falleh from ((select item, arrange_type, (count * tedad)" +
                           " as tedad from new_halfcut inner join new_halfcutRiz on new_halfcut.id = new_halfcutRiz.hid " +
                           "inner join flower_forms_entry on new_halfcut.formid = flower_forms_entry.id where flowid = " +
                           "" + (object)id + ") union all (select fai.item_name as item, ffe.arrange_type, " +
                           "(fai.item_insheet_count * ffe.sheetcount) as tedad from flower_arrange_items as fai " +
                           "inner join flower_forms_entry as ffe on fai.form_id = ffe.id where fai.flower_id " +
                           "= " + (object)id + ")) as src pivot(sum(tedad) for arrange_type in ([1],[2]))as piv)j " +
                           "where j.item = cutted_and_remain.item_name and cutted_and_remain.flower_id = " + (object)id + " insert" +
                           " into cutted_and_remain (flower_id, item_name, service, falleh) select " + (object)id + ", j.item," +
                           " j.service, j.falleh from (select item,case when[1] is null then 0 else [1] end as service ,case " +
                           "when[2] is null then 0 else [2] end as falleh from ((select item, arrange_type, (count * tedad) " +
                           "as tedad from new_halfcut inner join new_halfcutRiz on new_halfcut.id = new_halfcutRiz.hid inner " +
                           "join flower_forms_entry on new_halfcut.formid = flower_forms_entry.id where flowid = " + (object)id + ")" +
                           " union all(select fai.item_name as item, ffe.arrange_type, (fai.item_insheet_count * ffe.sheetcount) as" +
                           " tedad from flower_arrange_items as fai inner join flower_forms_entry as ffe on fai.form_id = ffe.id " +
                           "where fai.flower_id = " + (object)id + ")) as src pivot(sum(tedad) for arrange_type in ([1],[2]))as piv)j" +
                           " where j.item not in(select item_name as item from cutted_and_remain where flower_id = " + (object)id + ") " +
                           "update cutted_and_remain set total = falleh + service + cutted  " +
                           "where flower_id = " + (object)id + " ", this.con).ExecuteNonQuery();
            var sqlDataReader = new SqlCommand("SELECT cutted_and_remain.total, cutted_and_remain.cutted, " +
                                               "cutted_and_remain.falleh, cutted_and_remain.service , " +
                                               "cutted_and_remain.comment, items.item_name FROM cutted_and_remain" +
                                               " INNER JOIN items ON  cutted_and_remain.item_name = items.item_id" +
                                               " WHERE (cutted_and_remain.flower_id = " + (object)id + ")  " +
                                               "ORDER BY items.item_name", this.con).ExecuteReader();
            while (sqlDataReader.Read())
                content.Add(new MojoodiKol()
                {
                    Item = sqlDataReader["item_name"].ToString(),
                    Cutted = Convert.ToInt32(sqlDataReader["cutted"]),
                    Falleh = Convert.ToInt32(sqlDataReader["falleh"]),
                    Service = Convert.ToInt32(sqlDataReader["service"]),
                    Total = Convert.ToInt32(sqlDataReader["total"]),
                    Comment = sqlDataReader["comment"].ToString()
                });
            this.con.Close();
            return Json(content);
        }
    }
}