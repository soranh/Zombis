using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace Zombis
{

    class Felulet : FrameworkElement
    {
        ViewModel vm;
        DispatcherTimer dt;
        bool W, S, A, D;
        double mx, my;
        int zombiSzam;
        int zombiMax;
        static Random rnd = new Random();
        bool vege;
        int szamlalo;

        int kivalasztottVarazslat;
      


        static Zombi ujZombi(int szint=1)
        {
            return new Zombi(rnd.Next(50, 750), rnd.Next(50, 550),szint);
        }
        

        public Felulet()
        {
            vm = ViewModel.Get();
            vm.Zombik = new List<Zombi>();
            vm.Lovedekek = new List<Tuzlovedek>();
            vm.Aknak = new List<Akna>();
            vm.Felvehetok = new List<Felveheto>();
            Loaded += Felulet_Loaded;
            kivalasztottVarazslat = 1;

        }

        void dt_Tick(object sender, EventArgs e)
        {
            if(W && !S && !A && !D)
            {
                vm.Jatekos.Mozgat(0, -vm.Jatekos.Sebesseg);            
            }
            else if(A && !D && !W && !S)
            {      
                vm.Jatekos.Mozgat(-vm.Jatekos.Sebesseg,0);
            }
            else if(S && !W && !A && !D)
            {
                vm.Jatekos.Mozgat(0, vm.Jatekos.Sebesseg);
            }
            else if(D && !A && !W && !S)
            {
                vm.Jatekos.Mozgat(vm.Jatekos.Sebesseg,0);
            }

            else if (W && A && !D && !S)
            {
                vm.Jatekos.Mozgat(-vm.Jatekos.Sebesseg / Math.Sqrt(2), -vm.Jatekos.Sebesseg / Math.Sqrt(2));
            }
            else if (W && D && !A && !S)
            {
                vm.Jatekos.Mozgat(vm.Jatekos.Sebesseg / Math.Sqrt(2), -vm.Jatekos.Sebesseg / Math.Sqrt(2));
            }
            else if (A && S && !W && !D)
            {
                vm.Jatekos.Mozgat(-vm.Jatekos.Sebesseg / Math.Sqrt(2), vm.Jatekos.Sebesseg / Math.Sqrt(2));
            }
            else if (S && D && !W && !A)
            {
                vm.Jatekos.Mozgat(vm.Jatekos.Sebesseg / Math.Sqrt(2), vm.Jatekos.Sebesseg / Math.Sqrt(2));
            }

            if(rnd.Next(0,100)==0)
            {
                Felveheto f;

                do
                {
                    if (rnd.Next(0, 2) == 1)
                        f=new EleteroItal(rnd.Next(50, 750), rnd.Next(50, 550), vm.Jatekos.Szint);
                    else
                        f=new ManaItal(rnd.Next(50, 750), rnd.Next(50, 550), vm.Jatekos.Szint);
                } while (f.KozelVan(vm.Jatekos));
                vm.Felvehetok.Add(f);
            }

            foreach (Felveheto f in vm.Felvehetok)
            {
                if (Geometry.Combine(f.Alak, vm.Jatekos.Alak, GeometryCombineMode.Intersect, null).GetArea() > 0)
                    f.Felvesz(vm.Jatekos);
                f.Frissit();
            }

            foreach (Tuzlovedek l in vm.Lovedekek)
            {
                l.Mozgat(Math.Cos(l.Rad) * l.Sebesseg, Math.Sin(l.Rad) * l.Sebesseg);
                foreach(Zombi z in vm.Zombik)
                    if(l.Eltalal(z))
                    {
                        z.Serul(10);
                    }
            }

            foreach (Akna a in vm.Aknak)
            {
                if(!a.Aktivalt && a.Tavolsag(vm.Jatekos) > 40)
                     foreach (Zombi z in vm.Zombik)
                     {
                         if (z.Tavolsag(a) < 25)
                         {
                             a.Aktivalt = true;
                             break;
                         }
                     }

                if (a.Aktivalt)
                {
                    foreach (Zombi z in vm.Zombik)
                    {
                        if (a.Eltalal(z))
                            z.Serul(a.Sebzes);

                    }    
                    a.Frissit();
                }

            }
       
            if (vm.Jegfuvallat!=null && vm.Jegfuvallat.Aktiv)
            {
                vm.Jegfuvallat.Frissit();
                foreach(Zombi z in vm.Zombik)
                {
                    if (vm.Jegfuvallat.Eltalal(z))
                        z.Serul(vm.Jegfuvallat.Sebzes);
                }
            }
            for (int i = 0; i < vm.Lovedekek.Count;i++ )
                if (vm.Lovedekek.ElementAt(i).Kirepul(ActualWidth,ActualHeight) || !vm.Lovedekek.ElementAt(i).Aktiv )
                    vm.Lovedekek.Remove(vm.Lovedekek.ElementAt(i));
            for (int i = 0; i < vm.Aknak.Count; i++)
                if (!vm.Aknak.ElementAt(i).Aktiv)
                    vm.Aknak.Remove(vm.Aknak.ElementAt(i));
            for (int i = 0; i < vm.Felvehetok.Count; i++)
                if (!vm.Felvehetok.ElementAt(i).Aktiv)
                    vm.Felvehetok.Remove(vm.Felvehetok.ElementAt(i));

            for (int i = 0; i < vm.Zombik.Count; i++)
                if (!vm.Zombik.ElementAt(i).Aktiv)
                {
                    vm.Jatekos.XP += vm.Zombik.ElementAt(i).Szint*2;
                    if (vm.Jatekos.XP >= vm.Jatekos.MaxXP)
                    {
                        vm.Jatekos.SzintetLep();
                 
                        zombiMax++;
                    }
                    vm.Zombik.Remove(vm.Zombik.ElementAt(i));
                    zombiSzam--;
                    
                }

       


           while (zombiSzam < zombiMax)
            {
                    Zombi z;
                    do
                    {
                        z = ujZombi(rnd.Next(1, vm.Jatekos.Szint + 1));
                    }
                    while (z.KozelVan(vm.Jatekos));

                    zombiSzam++;
                    vm.Zombik.Add(z);
            }

            foreach (Zombi z in vm.Zombik)
            {
                z.Rad = Math.Atan2(vm.Jatekos.Cy - z.Cy, vm.Jatekos.Cx - z.Cx);
                if (!z.KozelVan(vm.Jatekos))
                    z.Mozgat(z.Sebesseg * Math.Cos(z.Rad), z.Sebesseg * Math.Sin(z.Rad));
                else if (vm.Jatekos.Serul(z.Sebzes))
                {
                    vege = true;
                    dt.IsEnabled = false;
                    
                }
                    
            }

           
            vm.Jatekos.MP += vm.Jatekos.MPregen / 25.0;
            if (vm.Jatekos.MP >= vm.Jatekos.MaxMP)
                vm.Jatekos.MP = vm.Jatekos.MaxMP;

            InvalidateVisual();
        }

        void Felulet_Loaded(object sender, RoutedEventArgs e)
        {
            (this.Parent as Window).KeyDown += Felulet_KeyDown;
            (this.Parent as Window).KeyUp += Felulet_KeyUp;
            (this.Parent as Window).MouseDown += Felulet_MouseDown;
            (this.Parent as Window).MouseMove += Felulet_MouseMove;

            vm.Jatekos = new Jatekos(100, 100, ActualWidth, ActualHeight);
           

            while (zombiSzam < zombiMax)
            {
                zombiSzam++;
                vm.Zombik.Add(ujZombi());
            }
            dt = new DispatcherTimer();
            dt.Interval = new TimeSpan(0, 0, 0, 0, 40);
            dt.Tick += dt_Tick;
            dt.Start();
            W = S = A = D = false;
            zombiSzam = 0;
            zombiMax = 10;

           

            
        }

        void Felulet_MouseMove(object sender, MouseEventArgs e)
        {
            mx = e.GetPosition(this).X;
            my = e.GetPosition(this).Y;
        }

        void Felulet_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            
            if (kivalasztottVarazslat == 1)
            {
                Tuzlovedek l = new Tuzlovedek(vm.Jatekos.Cx, vm.Jatekos.Cy, mx, my, vm.Jatekos.Tuzlovedek);
                if (vm.Jatekos.MP - l.Koltseg >= 0)
                {
                    vm.Jatekos.MP -= l.Koltseg;
                    vm.Lovedekek.Add(l);
                }
            }
            else if (kivalasztottVarazslat ==2)
            {
                Jegfuvallat j = new Jegfuvallat(vm.Jatekos.Cx, vm.Jatekos.Cy, mx, my, vm.Jatekos.Jegfuvallat);
                if (vm.Jatekos.MP - j.Koltseg >= 0)
                {
                    vm.Jatekos.MP -= j.Koltseg;
                    vm.Jegfuvallat = j;
                }
            }
            else if (kivalasztottVarazslat ==3)
            {
                Akna a = new Akna(vm.Jatekos.Cx, vm.Jatekos.Cy, vm.Jatekos.Akna);
                if(vm.Jatekos.MP - a.Koltseg>=0)
                {
                    bool vanKozel = false;
                    foreach (Akna a2 in vm.Aknak)
                        if (a.KozelVan(a2))
                        {
                            vanKozel = true;
                            break;
                        }
                    if (!vanKozel)
                    {
                        vm.Jatekos.MP -= a.Koltseg;
                        vm.Aknak.Add(a);
                    }
                }
            }
        }

        void Felulet_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.W || e.Key == Key.Up)
                W = false;
            if (e.Key == Key.S || e.Key == Key.Down)
                S = false;
            if (e.Key == Key.A || e.Key == Key.Left)
                A = false;
            if (e.Key == Key.D || e.Key == Key.Right)
                D = false;
        }

        void Felulet_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.W || e.Key== Key.Up)
                W = true;
            if (e.Key == Key.S || e.Key== Key.Down)
                S = true;
            if (e.Key == Key.A || e.Key== Key.Left)
                A = true;
            if (e.Key == Key.D || e.Key==Key.Right)
                D = true;

            if(e.Key == Key.Space)
            {
                dt.IsEnabled = false;
                (new Karakterlap()).ShowDialog();
                dt.IsEnabled = true;
            }
            if (e.Key == Key.D1)
                kivalasztottVarazslat = 1;
            if (e.Key == Key.D2 && vm.Jatekos.Jegfuvallat>0)
                kivalasztottVarazslat = 2;
            if (e.Key == Key.D3 && vm.Jatekos.Akna>0)
                kivalasztottVarazslat = 3;
            if (e.Key == Key.K)
                vm.Jatekos.XP = vm.Jatekos.MaxXP - 1;
        }

        void DrawString(DrawingContext drawingContext, string s, int fontSize, Point position)
        {
            FormattedText ft = new FormattedText(s , System.Globalization.CultureInfo.CurrentCulture, System.Windows.FlowDirection.LeftToRight,
                new Typeface(new FontFamily("Arial"), FontStyles.Normal, FontWeights.Bold, FontStretches.Normal), fontSize, Brushes.White);

            drawingContext.DrawGeometry(Brushes.Black, new Pen(), ft.BuildGeometry(position));
        }

        void DrawUI( DrawingContext drawingContext)
        {
            drawingContext.DrawRectangle(Brushes.Red, new Pen(Brushes.Black, 1), new Rect(40, 15, vm.Jatekos.HP / vm.Jatekos.MaxHP * 200, 10));
            drawingContext.DrawRectangle(Brushes.Blue, new Pen(Brushes.Black, 1), new Rect(40, 28, vm.Jatekos.MP / vm.Jatekos.MaxMP * 200, 10));
            drawingContext.DrawRectangle(Brushes.Yellow, new Pen(Brushes.Black, 1), new Rect(40, 40, vm.Jatekos.XP * 1.0 / vm.Jatekos.MaxXP * 200, 10));

            DrawString(drawingContext,"Élet",12,new Point(5,15));
            DrawString(drawingContext, (int)vm.Jatekos.HP+"/"+(int)vm.Jatekos.MaxHP, 12, new Point(120, 13));

            DrawString(drawingContext, "Mana", 12, new Point(5, 28));
            DrawString(drawingContext, (int)vm.Jatekos.MP + "/" + (int)vm.Jatekos.MaxMP, 12, new Point(120, 26));
            DrawString(drawingContext, "XP", 12, new Point(5, 40));
            DrawString(drawingContext, vm.Jatekos.XP + "/" + vm.Jatekos.MaxXP, 12, new Point(120, 38));

            DrawString(drawingContext, "Pontszám: " + vm.Jatekos.Pontszam, 12, new Point(700, 10));

            DrawString(drawingContext, "Varázslatok", 12, new Point(10, 495)); 


            drawingContext.DrawRectangle(Brushes.LightGray, new Pen(Brushes.Black, 1), new Rect(10, 510, 32, 32));
            drawingContext.DrawGeometry(Brushes.Red, new Pen(Brushes.Yellow, 2), new Tuzlovedek(26, 526, 0, 0, 1).Alak);
            drawingContext.DrawRectangle(Brushes.LightGray, new Pen(Brushes.Black, 1), new Rect(42, 510, 32, 32));

            if (vm.Jatekos.Jegfuvallat > 0)
            {
                Jegfuvallat j = new Jegfuvallat(58, 542, 58, 510, 1);
                j.setScale(0.3);
                drawingContext.DrawGeometry(Brushes.Aquamarine, new Pen(), j.Alak);
            }
            drawingContext.DrawRectangle(Brushes.LightGray, new Pen(Brushes.Black, 1), new Rect(74, 510, 32, 32));
            if (vm.Jatekos.Akna > 0)
            {
                drawingContext.DrawGeometry(Brushes.Black, new Pen(Brushes.Brown, 2), new Akna(90, 526, 1).Alak);
            }

            drawingContext.DrawRectangle(Brushes.Transparent,new Pen(Brushes.Blue,3),new Rect(10+(kivalasztottVarazslat-1)*32,510,32,32));

            if (vm.Jatekos.Tulpont != 0 ||vm.Jatekos.Varpont !=0)
                DrawString(drawingContext, "Vannak elosztható tulajdonság vagy varázslat pontjaid! Space lenyomásával bejön a karakterlap.", 14, new Point(50, 544));
         
       
        }


        protected override void OnRender(DrawingContext drawingContext)        
        {
            //háttér kirajzolása
            drawingContext.DrawRectangle(new SolidColorBrush(Color.FromRgb(190,255,80)), new Pen(Brushes.Black, 0), new Rect(0, 0, ActualWidth, ActualHeight));

            if (this.IsLoaded)
            {
                foreach (Zombi z in vm.Zombik)
                    drawingContext.DrawGeometry(Brushes.DarkGray, new Pen(Brushes.Black, 2), z.Alak);
                foreach (Tuzlovedek l in vm.Lovedekek)
                    drawingContext.DrawGeometry(Brushes.Red, new Pen(Brushes.Yellow, 2), l.Alak);
                foreach (Akna a in vm.Aknak)
                {
                    if(!a.Aktivalt)
                    drawingContext.DrawGeometry(Brushes.Black, new Pen(Brushes.Brown, 2), a.Alak);
                    else
                     drawingContext.DrawGeometry(Brushes.Red, new Pen(Brushes.Yellow, 2), a.Alak);

                }

                foreach(Felveheto f in vm.Felvehetok)
                {
                    if (f is EleteroItal)
                    {
                        drawingContext.DrawGeometry(Brushes.Red, new Pen(Brushes.Black, 2), f.Alak);
                    }
                    else if (f is ManaItal)
                    {
                        drawingContext.DrawGeometry(Brushes.Blue, new Pen(Brushes.Black, 2), f.Alak);
                    }
                }

                if(vm.Jegfuvallat!= null && vm.Jegfuvallat.Aktiv)
                    drawingContext.DrawGeometry(Brushes.Aquamarine, new Pen(), vm.Jegfuvallat.Alak);

            
                drawingContext.DrawGeometry(Brushes.Blue, new Pen(Brushes.Black, 2), vm.Jatekos.Alak);


                DrawUI(drawingContext);
            }
 
            if(vege)
            {
                DrawString(drawingContext, "NyammNyamm! Zombikaja!", 50, new Point(50, 250));
            }
        }
    }
}
