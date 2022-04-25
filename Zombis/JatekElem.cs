using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Zombis
{
    abstract class JatekElem
    {
        protected double cx, cy, deg;
        protected double r1;
        protected double r2;
        protected double scale;

        const double d= 20.0; //távolság amitől közel van, zombik ennél közelebb nem mennek egymáshoz, játékos sérül ha ilyen közel kerül zombihoz

        protected Geometry alak;
        //protected Brush ecset;
        protected bool aktiv;

        public bool Aktiv { get { return aktiv; } }


        public double Cy
        {
            get { return cy; }

        }

        public double Cx
        {
            get { return cx; }

        }
//        public Brush Ecset {get {return ecset;}}
        public virtual double Sebesseg { get { return 2.0; } }

        public double Rad { get { return deg / 180 * Math.PI; } set { deg = value / Math.PI * 180; } }

        public Geometry Alak
        {
            get
            {
                Geometry copy = alak.Clone();
                TransformGroup tg = new TransformGroup();
                tg.Children.Add(new ScaleTransform(scale, scale));
                tg.Children.Add(new TranslateTransform(cx, cy));
                tg.Children.Add(new RotateTransform(deg + 90, cx, cy));
           
     
       
                copy.Transform = tg;
                return copy.GetFlattenedPathGeometry();
            }
        }

        public JatekElem(double cx, double cy)
        {
            this.cx = cx;
            this.cy = cy;
            scale = 1.0;
        }

        public double Tavolsag(JatekElem e)
        {
            return Math.Sqrt((cx - e.cx) * (cx - e.cx) + (cy - e.cy) * (cy - e.cy));
        }
        public bool KozelVan(JatekElem e)
        {
            return Tavolsag(e) < d;
        }

        public virtual void Mozgat(double dx, double dy)
        {
            cx += dx;
            cy += dy;
            Rad = Math.Atan2(dy,dx);
        }

         
    }

    

    class Jatekos :JatekElem, INotifyPropertyChanged
    {
        
        double hp;
        double maxhp;
        double mp;
        double maxmp;
        double mpregen;
        int xp;
        int maxxp;
        int szint;

        int kitartas;
        int intelligencia;
        int lelekero;

        int tuzlovedek;
        int jegfuvallat;
        int akna;



        int tulpont;
        int varpont;



        public int Pontszam { get { return (szint) * (szint - 1) / 2 * 100 + xp; } }

        public int Tulpont
        {
            get { return tulpont; }
            set { tulpont = value; onPrpChg(); }
        }


        public int Varpont
        {
            get { return varpont; }
            set { varpont = value; onPrpChg(); }
        }

        public int Kitartas
        {
            get { return kitartas; }
            set { kitartas = value; onPrpChg(); UjraSzamol(); }
        }
  

        public int Intelligencia
        {
            get { return intelligencia; }
            set { intelligencia = value; onPrpChg(); UjraSzamol(); }
        }
   

        public int Lelekero
        {
            get { return lelekero; }
            set { lelekero = value; onPrpChg(); UjraSzamol(); }
        }
        public int Tuzlovedek
        {
            get { return tuzlovedek; }
            set { tuzlovedek = value; onPrpChg(); }
        }
        public int Jegfuvallat
        {
            get { return jegfuvallat; }
            set { jegfuvallat = value; onPrpChg(); }
        }
        public int Akna
        {
            get { return akna; }
            set { akna = value; onPrpChg(); }
        }

        


        double aw, ah;

        public int Szint { get { return szint; } }
        public int XP { get { return xp; } set { xp = value; } } 
        public int MaxXP {get {return maxxp;}}
        public double MaxMP { get { return maxmp; } set { maxmp = value; onPrpChg(); } }

        public double HP { get { return hp; } set { hp = value; } }
        public double MP { get { return mp; } set { mp = value; } }
        public double MaxHP { get { return maxhp; } set {maxhp=value; onPrpChg(); }}
        public double MPregen { get { return mpregen; } set { mpregen = value; onPrpChg(); } }
        
        public Jatekos(double cx, double cy, double aw, double ah)
            :base(cx,cy)
        {
            this.aw = aw;
            this.ah = ah;

            r1 = 15.0;
            r2 = 8.0;
            GeometryGroup gg = new GeometryGroup();
            gg.Children.Add(new EllipseGeometry(new Point(0, 0), r1, r2));
            gg.Children.Add(new RectangleGeometry(new Rect(new Point(-r1, -(r1-5)), new Point(-(r1-2), 0))));
            gg.Children.Add(new RectangleGeometry(new Rect(new Point(r1-2, -(r1-5)), new Point(r1, 0))));
            alak = gg;

            aktiv = true;
            szint = 1;
            maxxp = 100;
            xp = 0;

            kitartas = 1;
            intelligencia = 1;
            lelekero = 1;

            tuzlovedek = 1;
            jegfuvallat = 0;
            akna = 0;

            UjraSzamol();
        }
        
        void UjraSzamol()
        {
            MaxHP = (kitartas - 1) * 20 + 100;
            MaxMP = (intelligencia - 1) * 20 + 100;
            MPregen = (lelekero - 1) * 0.8 + 3.0;

            hp = maxhp;
            mp = maxmp;
            
        }

        public bool Serul(double d)
        {
            if (aktiv)
            {
                hp -= d;
                if (hp <= 0)
                {
                    hp = 0;
                    aktiv = false;
                }
            }

            return !aktiv;
        }

        public void SzintetLep()
        {

            szint++;
            maxxp = szint * 100;
            xp = 0;
            tulpont += 2;
            varpont++;
            hp = maxhp;
            mp = maxmp;
        }

        public override void Mozgat(double dx, double dy)
        {
            if(Cx+dx<=aw-r1 && Cx+dx >=r1 && Cy+dy<=ah-r1 && Cy+dy>=r1 )
                base.Mozgat(dx, dy);
        }

        public override double Sebesseg
        {
            get
            {
                return 2.2;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        void onPrpChg([CallerMemberName] string s="")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(s));
        }
    }

    class Zombi : JatekElem
    {
        double maxhp;
        int szint;
        double hp;

        public double Sebzes { get { return (szint + 1)/5.0; } }
        public int Szint { get { return szint; } }

        public Zombi(double cx, double cy, int szint = 1)
            : base(cx, cy)
        {
            r1 = 15.0;
            r2 = 8.0;
            GeometryGroup gg = new GeometryGroup();
            gg.Children.Add(new EllipseGeometry(new Point(0, 0), r1, r2));
            gg.Children.Add(new RectangleGeometry(new Rect(new Point(-r1, -(r1-5)), new Point(-(r1-2), 0))));
            gg.Children.Add(new RectangleGeometry(new Rect(new Point(r1-2, -(r1-5)), new Point(r1, 0))));
            alak = gg;
            aktiv = true;
            this.szint = szint;
            maxhp = 20 + (szint - 1) * 10;
            hp = maxhp;
      
        }

        public bool Serul(double d)
        {
            if (aktiv)
            {
                hp -= d;
                if (hp <= 0)
                {
                    hp = 0;
                    aktiv = false;
                }
            }

            return !aktiv;
        }



    }

    abstract class Varazslat : JatekElem
    {
        protected int szint;
        public Varazslat(double cx, double cy)
            : base(cx, cy) { }

        public abstract bool Eltalal(JatekElem j);
        public abstract double Sebzes { get; }
        public abstract double Koltseg { get; }
        public int Szint { get { return szint; } }

    }

    class Tuzlovedek : Varazslat
    {

        const double R = 5.0;

        
        public override double Koltseg { get { return 1.0+(Szint-1)*0.5; } }
        public override double Sebzes
        {
            get { return 10.0 + (Szint - 1) * 4.0; }
        }
        public override double Sebesseg
        {
            get
            {
                return 8.0;
            }
        }

        public Tuzlovedek(double cx, double cy, double celx, double cely, int szint)
            : base(cx, cy)
        {
            aktiv = true;
            alak = new EllipseGeometry(new Point(0, 0),R, R);
            Rad = Math.Atan2(cely - cy, celx - cx);
            this.szint = szint;

        }

        public override bool Eltalal(JatekElem j)
        {

            if (Aktiv && j.Aktiv && this.KozelVan(j))
            {
                aktiv = false;
                return true;
            }
            return false;
        }

        public virtual bool Kirepul(double w, double h)
        {
            return cx - R > w || cx < -R || cy - R > h || cy < -R;
        }

    }

    class Jegfuvallat :Varazslat
    {
        const double R = 80.0;
        const double alfa = Math.PI/2;
        int fazis;
           public Jegfuvallat(double cx, double cy, double celx, double cely, int szint)
            : base(cx, cy)
        {
            aktiv = true;
            scale=0.0;
       //     alak = new EllipseGeometry(new Point(0, 0),r, r);
            PathFigure pf = new PathFigure();
            pf.StartPoint= new Point(0,0);
           pf.Segments.Add(new LineSegment(new Point(-R*Math.Sin(alfa/2),-R*Math.Cos(alfa/2)),true));
           pf.Segments.Add(new ArcSegment(new Point(R * Math.Sin(alfa / 2), -R * Math.Cos(alfa / 2)), new Size(R, R), alfa/2, false, SweepDirection.Clockwise, true));
           pf.Segments.Add(new LineSegment(new Point(0,0),true));
           alak =  new PathGeometry(new PathFigure[] { pf });
           Rad = Math.Atan2(cely - cy, celx - cx);
           this.szint = szint;

        }

        public void Frissit()
        {
            if (fazis <= 10)
                scale = 0.1 * fazis;
            else if (fazis >= 10 && fazis < 19)
                scale = 1.0;
            else
            {
                scale = 0;
                aktiv = false;
            }
            fazis++;
        }

        public void setScale(double scale)
        {
            this.scale = scale;
        }
        public override bool Eltalal(JatekElem j)
        {
            return Geometry.Combine(Alak, j.Alak, GeometryCombineMode.Intersect, null).GetArea() > 0;
        }

        public override double Sebzes
        {
            get { return (15.0+(Szint -1) * 4.5)/10.0; }
        }

        public override double Koltseg
        {
            get { return 3.0 + (Szint - 1) * 1.5; }
        }
    }

    class Akna : Varazslat
    {
        const double R = 50.0;  //hatósugár
        const double R1 = 12.0;
        const double R2 = 5.0;
        bool aktivalt;
        int fazis;

        public bool Aktivalt 
        { 
            get 
            { 
                return aktivalt; 
            } 
            set 
            { 
                if(!aktivalt && value)
                {

                    alak = new EllipseGeometry(new Point(0, 0), R, R);
                    scale = 0.0;
                }
                aktivalt = value;  
            }
        }
        public override bool Eltalal(JatekElem j)
        {
            return Geometry.Combine(Alak, j.Alak, GeometryCombineMode.Intersect, null).GetArea() > 0;
        }

        public void Frissit()
        {
            if (fazis <= 10)
                scale = 0.1 * fazis;
            else if (fazis >= 10 && fazis < 19)
                scale = 1.0;
            else
            {
                scale = 0;
                aktiv = false;
                aktivalt = false;
            }
            fazis++;
        }
       
        public override double Sebzes
        {
            get { return (25.0+ (Szint-1)*6.0)/10.0; }
        }

        public override double Koltseg
        {
            get { return 6.0 + (Szint - 1) * 2.0; }
        }

         public Akna(double cx, double cy,  int szint)
            : base(cx, cy)
        {
            aktiv = true;
            scale = 1.0;
            alak = new EllipseGeometry(new Point(0, 0), R2, R1);

        }
    }

    abstract class Felveheto : JatekElem
    {
        int ido;
        const int elettartam = 375;

        protected int szint;
        public int Szint { get { return szint; } }
        public abstract void Felvesz(Jatekos j);

        public void Frissit()
        {
            ido++;
            if (ido == elettartam)
                aktiv = false;
        }
        protected double HatasErosseg
        {
            get
            {
                return 50 + (szint-1) * 10;
            }
        }


        public Felveheto(double cx, double cy)
            :base(cx,cy)
        {
            ido = 0;   
        }
    }

    class EleteroItal : Felveheto
    {

        public EleteroItal(double cx, double cy, int szint)
            :base(cx,cy)
        {
            aktiv = true;
            this.szint=szint;
            this.cx = cx;
            this.cy = cy;

            alak = Geometry.Combine(new RectangleGeometry(new Rect(-5, -25, 10, 25)), new EllipseGeometry(new Point(0, 0), 10, 10), GeometryCombineMode.Union, null);
            deg = -90;

        }


        public override void Felvesz(Jatekos j)
        {
            aktiv = false;
            if (j.HP + HatasErosseg >= j.MaxHP)
                j.HP = j.MaxHP;
            else
                j.HP += HatasErosseg;
        }
    }

    class ManaItal :Felveheto
    {
        public override void Felvesz(Jatekos j)
        {
            aktiv = false;
            if (j.MP + HatasErosseg >= j.MaxMP)
                j.MP = j.MaxMP;
            else
                j.MP += HatasErosseg;
        }

        public ManaItal(double cx, double cy, int szint)
            :base(cx,cy)
        {
            aktiv = true;
            this.szint=szint;
            this.cx = cx;
            this.cy = cy;

            alak = Geometry.Combine(new RectangleGeometry(new Rect(-5, -25, 10, 25)), new EllipseGeometry(new Point(0, 0), 10, 10), GeometryCombineMode.Union, null);
            deg = -90;
        }

        


    }
    
}
