using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zombis
{
    class ViewModel
    {
        static ViewModel instance;

        public Jatekos Jatekos { get; set; }
        public List<Zombi> Zombik { get; set; }
        public List<Tuzlovedek> Lovedekek { get; set;}

        public List<Akna> Aknak { get; set; }
        public Jegfuvallat Jegfuvallat { get; set; }

        public List<Felveheto> Felvehetok { get; set; }


        public static ViewModel Get()
        {
            if (instance == null)
                instance = new ViewModel();
            return instance;
        }


        private ViewModel()
        {
     

        }
    }
}
