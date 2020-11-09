using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AbnbarGolWebService.Models
{
    public class Gol
    {
        public int Id { get; set; }

        public string GolName { get; set; }

        public string Code { get; set; }

        public string Color { get; set; }

        public int ColorId { get; set; }

        public string ColorType { get; set; }

        public int ColorTypeId { get; set; }

        public string Format { get; set; }

        public int FormatId { get; set; }

        public string Customer { get; set; }

        public int CustomerId { get; set; }

        public string Company { get; set; }

        public int CompanyId { get; set; }

        public string EnterDate { get; set; }

        public string Comment { get; set; }
    }
}