using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroPOS.Model
{
	public class boleto
	{
		public bool failed { get; set; }
        public  string failedMsg { get; set; }
		public string digitable { get; set; }
		public string boleto_company { get; set; }

		public double discount_value { get; set; }

		public string doc_payer { get; set; }

		public string due_date { get; set; }

		public double original_value { get; set; }

		public string payer { get; set; }
		public string receiver { get; set; }
		public double total_additional { get; set; }
		public double total_discount { get; set; }
		public double total_updated { get; set; }
		public int transaction_id { get; set; }
		public int type { get; set; }
	
		public double value { get; set; }
	
		


		public override string ToString()
		{
			return JsonConvert.SerializeObject(this, Formatting.Indented);
		}
	}
}
