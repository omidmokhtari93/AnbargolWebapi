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
    public class GetControlsController : ApiController
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["flower_depot"].ConnectionString);
        [Route("api/controls")]
        [HttpGet]
        public IHttpActionResult GetCotnrolsActionResult()
        {
            con.Open();
            var list = new List<Controls>();
            var cm = new SqlCommand("" +
                                    //"select item_id as Value ,item_name as name FROM items where item_id <> 0 order by item_name " +
                                    "select flowformat_id as Value, flow_format as name from flower_formats " +
                                    //"select customer_id as Value, customer_name as name from flower_customers " +
                                    //"select company_id  as Value, company_name as name from flower_companies" +
                                    "", con);
            var r = cm.ExecuteReader();
            var items = new Controls
            {
                Name = "قالب ها"
            };
            while (r.Read())
            {
                items.Items.Add(new Item()
                {
                    Value = Convert.ToInt32(r["Value"]),
                    Text = r["name"].ToString()
                });
            }
            //list.Add(items);
            //r.NextResult();
            //items = new Controls
            //{
            //    Name = "قالب ها"
            //};
            //while (r.Read())
            //{
            //    items.Items.Add(new Item()
            //    {
            //        Value = Convert.ToInt32(r["Value"]),
            //        Text = r["name"].ToString()
            //    });
            //}
            //list.Add(items);
            //r.NextResult();
            //items = new Controls
            //{
            //    Name = "مشتریان"
            //};
            //while (r.Read())
            //{
            //    items.Items.Add(new Item()
            //    {
            //        Value = Convert.ToInt32(r["Value"]),
            //        Text = r["name"].ToString()
            //    });
            //}
            //list.Add(items);
            //r.NextResult();
            //items = new Controls
            //{
            //    Name = "شرکت ها"
            //};
            //while (r.Read())
            //{
            //    items.Items.Add(new Item()
            //    {
            //        Value = Convert.ToInt32(r["Value"]),
            //        Text = r["name"].ToString()
            //    });
            //}
            //list.Add(items);
            con.Close();
            return Json(items);
        }
    }
}