using System;
using System.ComponentModel.DataAnnotations;

namespace Blatt3_Aufgabe4.Models
{
	public class Booking
	{
		[Range(0, 100)]
		[Required]
		public int chargeStatus { get; set; }
		[Range(1, 1000)]
		[Required]
		public int distance { get; set; }
		[DataType(DataType.DateTime)]
		[Required]
		public DateTime startTime { get; set; }
		[DataType(DataType.DateTime)]
		[Required]
		public DateTime endTime { get; set; }

		public Booking()
		{

		}
	}
}