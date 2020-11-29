using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace surveychallenge.ViewModel
{
    public class Submit
    {
        public int SurveyId { get; set; }
        public List<int> SelectedOptions { get; set; }
    }
}