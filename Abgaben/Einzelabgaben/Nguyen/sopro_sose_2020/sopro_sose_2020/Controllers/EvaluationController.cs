using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using sopro_sose_2020.Models;
using sopro_sose_2020.ViewModel.Booking;

namespace sopro_sose_2020.Controllers
{
    public class EvaluationController : Controller
    {
        public List<ConnectorTypeEvaluationViewModel> Evaluation(List<Booking> _bookingList)
        {
            var bookingList = _bookingList;
            List<ConnectorTypeEvaluationViewModel> EvaList = new List<ConnectorTypeEvaluationViewModel>() { };
            int i = 0;
            int ac = 0, bc = 0, cc = 0; // "a-Count" == ac [...]
            foreach (Booking b in bookingList)
            {
                if (b.connectorType == ConnectorType.type_a)
                {
                    ac++;

                }
                else if (b.connectorType == ConnectorType.type_b)
                {
                    bc++;
                }
                else if (b.connectorType == ConnectorType.type_c)
                {
                    cc++;
                }
                i++;
            }


            EvaList.Add(new ConnectorTypeEvaluationViewModel()
            {
                connectorType = ConnectorType.type_a,
                percOfBookingsCT = percCalc(ac, i)

            });
            EvaList.Add(new ConnectorTypeEvaluationViewModel()
            {
                connectorType = ConnectorType.type_b,
                percOfBookingsCT = percCalc(bc, i)

            });
            EvaList.Add(new ConnectorTypeEvaluationViewModel()
            {
                connectorType = ConnectorType.type_c,
                percOfBookingsCT = percCalc(cc, i)

            });
            
            return EvaList;
        }
        public double percCalc(int a, int b)
        {
            return Math.Round(((double)a / b) * 100, 2);
        }
    }
}