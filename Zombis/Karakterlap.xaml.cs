using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Zombis
{
    /// <summary>
    /// Interaction logic for Karakterlap.xaml
    /// </summary>
    public partial class Karakterlap : Window
    {
        ViewModel vm;
        public Karakterlap()
        {
            InitializeComponent();
            vm = ViewModel.Get();
            this.DataContext = vm.Jatekos;
        }

        private void KitartasBtn(object sender, RoutedEventArgs e)
        {
            if (vm.Jatekos.Tulpont>0)
            {
                vm.Jatekos.Tulpont--;
                vm.Jatekos.Kitartas++;
            }

        }

        private void IntelligenciaBtn(object sender, RoutedEventArgs e)
        {
            if (vm.Jatekos.Tulpont > 0)
            {
                vm.Jatekos.Tulpont--;
                vm.Jatekos.Intelligencia++;
            }


        }

        private void LelekeroBtn(object sender, RoutedEventArgs e)
        {
            if (vm.Jatekos.Tulpont > 0)
            {
                vm.Jatekos.Tulpont--;
                vm.Jatekos.Lelekero++;
            }

        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Source is Label && (e.Source as Label).Content is string)
            {
                string s = (e.Source as Label).Content as string;
                if (s.StartsWith("Kitartás"))
                {
                    uzenetBlock.Text = "A kitartás a maximum életerőt növeli: minden kitartás pont +" + 20 + " életerőt ad";
                }
                if (s.StartsWith("Intelligencia"))
                {
                    uzenetBlock.Text = "Az intelligencia  a maximum manát növeli: minden intelligencia pont +" + 20 + " manát ad";
                }
                if (s.StartsWith("Lélekerő"))
                {
                    uzenetBlock.Text = "A lélekerő a mana regenerációt növeli: minden lélekerő pont +" + 0.8 + " ponttal növeli a másodpercenkénti regenerációt.";
                }

                if(s.StartsWith("Tűzlövedék"))
                {
                    uzenetBlock.Text = "Tűzlövedék: ez a varázslat egyenes vonalban kilő egy lövedéket az egér kurzor irányába, az első eltalált zombinak sérülést okoz, és megsemmisül";
                }
                if(s.StartsWith("Jégfuvallat"))
                {
                    uzenetBlock.Text = "Jégfuvallat: ez a játékos poziciójából induló az egérkurzor irányába körcikk alakban terjedő területre ható varázslat ";
                }
                if(s.StartsWith("Mágikus akna"))
                {
                    uzenetBlock.Text = "Mágikus akna: ezzel a varázslattal mágikus aknát lehet lehelyezni a játékos poziciójára. Akkor aktiválódnak, amikor a játékos a hatósugarán kívül van és zombi van a közelben. Ekkor területre ható robbanást idéz elő.";
                }

            }
            else
                uzenetBlock.Text = "";

   

        }

        private void TuzlovedekBtn(object sender, RoutedEventArgs e)
        {
            if (vm.Jatekos.Varpont > 0)
            {
                vm.Jatekos.Varpont--;
                vm.Jatekos.Tuzlovedek++;
            }
        }

        private void JegfuvallatBtn(object sender, RoutedEventArgs e)
        {
            if (vm.Jatekos.Varpont > 0)
            {
                vm.Jatekos.Varpont--;
                vm.Jatekos.Jegfuvallat++;
            }
        }

        private void AknaBtn(object sender, RoutedEventArgs e)
        {
            if (vm.Jatekos.Varpont > 0)
            {
                vm.Jatekos.Varpont--;
                vm.Jatekos.Akna++;
            }
        }
    }
}
