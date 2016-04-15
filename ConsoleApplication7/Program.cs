using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication7
{
    class Program
    {
        static int n = 100;
        static int m = 100;
        static int p = 50;//временной слой


        static double pi = 3.1415926535897932384626433832795028841971693993751058209749445923078164062862089986280348253421170679821;


        static double a = pi / 2.0, T = 0.5, b = pi / 2.0;
       static int var = 1;
    // Точное решение
    static double U(double x, double y, double t)
    {
        if (var == 1)
            return Math.Exp(-2.0 * t) * Math.Cos(x) * Math.Cos(y);//точное решение
        else
            return 0;
    }
    // начальное условие для начала работы метода
    static double Uvol(double x, double y)
    {
        return U(x,y,0);
    }
    /////////////////////////////////////////
    static double M1(double t, double y)
    {
        return U(0,y,t);
    }

    static double M2(double y, double t)
    {
	    return U(a,y,t);
    }
    static double M3(double t, double x)
    {
        return U(x, 0, t);
    }

    static double M4(double t, double x)
    {
        return U(x, a, t);
    }

        static void Main(string[] args)
        {
            
           // setlocale(LC_ALL, "Russian");
	       // cout << "Введите:\n 1" << endl;
	       // cin >> var;
          // var = Convert.ToInt32(Console.ReadKey());
	        if (var == 1) 
	        { 
		        a = pi / 2.0;
		        b = pi / 2.0;
		        T = 0.5; 
	        }



	        double he = a / m, 
			        ye = b / n, 
			        Te = T / p;
	        //double L = 1.0 / (he*he);
	        //double G = Te / (ye*ye);
	        double A = Te,
		        B = 2.0 * Te + he*he,
		        C = Te;
	        //cout << "A=" << A << endl;
	        //cout << "B=" << B << endl;
	        //cout << "C=" << C << endl;
	        double[] x = new double[m];
	        for (int i = 0; i < m; i++)
	        {
		        x[i] = 0.0 + i*he;
	        }
	        double[] y = new double[n];
	        for (int j = 0; j < n; j++)
	        {
		        y[j] = 0.0 + j*he;
	        }
	        double[] t = new double[p];
	        for (int q = 0; q < p; q++)
	        {
		        t[q] = 0.0 + Te*q;
	        } // узлы по y x t


	        double[] alpha = new double[n];
	        double[] beta = new double[n];
	        double[,,] u = new double[m,n,p];
	        //double u2[m + 1][p + 1];
	        ///////////

	        ////////////////////////
	        /*for (int i = 0; i < m; i++)
	        {
		        for (int j = 0; j < n; j++)
		        {
			        u[i,j] = Uvol(x[i], y[j]);

		        }// условие в начальный момент времени
	        }*/
            u[0, 0, 0] = M1(0, 0);
            u[m - 1, n - 1, 0] = 0;
	
	        for (int j = 0; j < p; j++)
	        {
                for (int q = 0; q < n; q++)
                {

                    //cout << "alpha=" << alpha[1] << endl;
                    //cout << "beta=" << beta[1] << endl;

                    for (int i = 0; i < m; i++)
                    {
                        alpha[0] = 0.0;
                        beta[0] = M1(t[j], y[q]);
                        if (i != m - 1)
                        {
                            alpha[i + 1] = B / (C - (A * alpha[i]));
                            beta[i + 1] = ((-U(q, i, j) * he * he) - A * beta[i]) / (A * alpha[i] - C);
                        }
                        //cout << "alpha=" << alpha[i + 1] << endl;
                        //cout << "beta=" << beta[i + 1] << endl;

                    }
                }
                


                u[m - 1, n - 1,j] = 0.0; // ????
              //  for(int ss = 0; ss < m; ss++)
                  //  u[0, 0] = M1(t,u[ss,0]);
                    for (int i = m-1; i > 0; i--)
                    {
                        for (int q1 = n-1; q1 >= 0; q1--)
                        {
                            u[i, q1,j] = alpha[i] * U(i,q1,j) + beta[i];
                        }
                    }
                    u[0, 0, j] = U(0,0,j);
                    
                // ЗДЕСЬ ОСТАНОВИЛИСЬ
		        ////////////////////////////////




                    for (int q = 0; q < m; q++)
                    {
                        alpha[0] = 0.0;
                        beta[0] = M3(t[j], x[q]);

                        for (int i = 0; i < n; i++)
                        {
                            if (i != n - 1)
                            {
                                alpha[i + 1] = B / (C - (A * alpha[i]));
                                beta[i + 1] = ((-u[q,i,j] * he * he) - A * beta[i]) / (A * alpha[i] - C);
                            }

                        }
                    }

                    u[m - 1, n - 1,j] = 0.0; // ????
			        for (int i = n - 1; i>=0; i--)
			        {
                        for (int q1 = m - 1; q1 > 0; q1--)
                        {
                            u[i, q1,j] = alpha[i] * u[i, q1,j] + beta[i];
                        }
			        }
                    u[0, 0, j] = U(0, 0, j);
                    
		        
	        }
	        ///////////////выводы
	        if (var == 1)
	        {
		        double max = -1;
		        //cout << "Точка     Приближенное     Точное      Погрешность" << endl;
                Console.WriteLine("Точка     Приближенное     Точное      Погрешность");
		        for (int i = 0; i < m; i++)
		        {

			        for (int j = 0; j < n; j++)
			        {
                        Console.WriteLine(x[i] + " " + y[j] + " " + u[i,j,p-1] + " " + U(x[i], y[j], t[p-1]) + " " + Math.Abs(u[i,j,p-1] - U(x[i],y[j],t[p-1])));

				       // cout << setw(13) << x[i] << setw(13) << y[j] << setw(13) << u[i][j] << setw(14) << U(x[i], y[j], t[p]) << setw(14) << abs(u[i][j] - U(x[i],y[j],t[p])) << endl;

				        //cout << u[i][j];
				        //cout << t[i];
				    //    cout << endl;

                        






			        }
		        }
                Console.ReadKey();


		     //   cout << "Программа завершила работу." << endl;
		      //  system("pause");
	        }

                }
            }
}
