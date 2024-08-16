using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RS_bot.Subcommittees
{
    abstract internal class SubComittes
    {
        private string name;
        private string informationAboutWork;
        private string timeWork;
        private string adress;
        private double longitude;
        private double latitude;
        private List<string> listWithInformation;

        abstract public string Name { get; set; }
        abstract public string InformationAboutWork { get; set; }
        abstract public string TimeWork { get; set; }
        abstract public string Adress { get; set; }
        abstract public string Longitude { get; set; }
        abstract public string Latitude { get; set; }
        abstract public List<string> ListWithInfomration { get;}

        internal SubComittes(List<string> information)
        {
            listWithInformation = information;
        }
        internal SubComittes(double longitude, double latitude)
        {
            this.longitude = longitude;
            this.latitude = latitude;
        }

        abstract internal Task MapPointAsync(double longidue, double latitude);
        abstract internal Task BaseInformationAsync(List<string> information);       
    }
}
