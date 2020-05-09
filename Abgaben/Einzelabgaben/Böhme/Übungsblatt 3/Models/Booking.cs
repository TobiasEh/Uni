using System;

namespace Blatt3_Aufgabe4.Models
{
	public class Booking
	{
		public int chargeStatus { get; set; }
		public int distance { get; set; }
		public DateTime startTime { get; set; }
		public DateTime endTime { get; set; }

		public Booking()
		{
		}
	}
}