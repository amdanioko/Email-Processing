using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OrderProcessing.Lib.Activites
{
    public class ExtractDateFromMailMessageActivity : CodeActivity
    {
        static readonly Regex rxDate = new Regex(@"(\d+) (\w{3}) (\d{4}) (\d+):(\d+):(\d+)", RegexOptions.Compiled);
        static readonly List<string> months = new List<string> { "Jan","Feb","Mar", "Apr","May","Jun",
            "Jul","Aug","Sep","Oct","Nov","Dec" };
        public static DateTime ParseDate(string sDate)
        {
            //11 Mar 2019 10:14:46 -0400
            Match match = rxDate.Match(sDate);
            int day = int.Parse(match.Groups[1].Value),
                year = int.Parse(match.Groups[3].Value),
                hour = int.Parse(match.Groups[4].Value),
                min = int.Parse(match.Groups[5].Value),
                sec = int.Parse(match.Groups[6].Value),
                month = 1 + months.IndexOf(match.Groups[2].Value);
            return new DateTime(year, month, day, hour, min, sec);
        }


        [Category("Input")]
        [RequiredArgument]
        public InArgument<MailMessage> Email { get; set; }

        [Category("Output")]
        public OutArgument<DateTime> EmailDate { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            string sDate = Email.Get(context).Headers["Date"];
            EmailDate.Set(context, ParseDate(sDate));
        }
    }
}
