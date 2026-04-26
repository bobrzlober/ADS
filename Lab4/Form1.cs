namespace Lab4;
using System;
using System.Drawing;

public partial class Form1 : Form
{
    const int N = 10;
    const int SEED = 5302;
    const int N3 = 0;
    const int N4 = 2;

    int[,] dirMatrix;
    int[,] undirMatrix;
    int[] coordX = [200, 400, 600, 800, 800, 800, 600, 400, 200, 200];
    int[] coordY = [200, 200, 200, 200, 400, 600, 600, 600, 600, 400];

    public Form1()
    {
        InitializeComponent();
        this.Text = "Graph";
        this.Size = new Size(2560, 1440);
        this.Paint += OnPaint;
        GenerateMatrices();
        PrintVertexDegrees();
        CheckForRegular();
        CheckForSpecialVertices();
    }

    void GenerateMatrices()
    {
        var rng = new Random(SEED);
        double k = 1.0 - N3 * 0.01 - N4 * 0.01 - 0.3;

        dirMatrix = new int[N, N];
        undirMatrix = new int[N, N];

        for (int i = 0; i < N; i++)
            for (int j = 0; j < N; j++)
            {
                double val = rng.NextDouble() * 2.0 * k;
                dirMatrix[i, j] = (val >= 1.0) ? 1 : 0;
            }

        for (int i = 0; i < N; i++)
            for (int j = 0; j < N; j++)
                if (dirMatrix[i, j] == 1 || dirMatrix[j, i] == 1)
                    undirMatrix[i, j] = undirMatrix[j, i] = 1;

        Console.WriteLine("dir:");
        for (int i = 0; i < N; i++)
        {
            for (int j = 0; j < N; j++)
                Console.Write(dirMatrix[i, j] + " ");
            Console.WriteLine();
        }
        Console.WriteLine("undir:");
        for (int i = 0; i < N; i++)
        {
        for (int j = 0; j < N; j++)
            Console.Write(undirMatrix[i, j] + " ");
        Console.WriteLine();
        }
    }
    void PrintVertexDegrees()
    {
        Console.WriteLine("\nthe degree of the vertices\n");
        Console.WriteLine("undir:");
        for (int i = 0; i < N; i++)
        {
            int degree = 0;
            for (int j = 0; j < N; j++)
            {
                degree += undirMatrix[i,j];
            }
            Console.WriteLine($"Vertice {i+1} : degree = {degree} ");
        }
        Console.WriteLine("dir:");
        for (int i = 0; i < N; i++)
        {
            int inDegree = 0;
            int outDegree = 0;
            for (int j = 0; j < N; j++)
            {
                inDegree += dirMatrix[j, i];
                outDegree += dirMatrix[i,j];
            }
            Console.WriteLine($"Vertice {i+1} : inDegree = {inDegree}, outDegree = {outDegree}, total = {inDegree + outDegree}");
        }
    }
    void CheckDirForRegular()
    {
       Console.WriteLine("dirMatrix:");
        int firstInDegree = 0;
        int firstOutDegree = 0;
        for (int k = 0; k < N; k++)
        {
            firstOutDegree += dirMatrix[0,k];
            firstInDegree += dirMatrix[k,0];
        }
        for (int i = 0; i < N; i++)
        {
            int currentOutDegree = 0;
            int currentInDegree = 0;
            for (int j = 0; j < N; j++)
            {
                currentOutDegree += dirMatrix[i,j];
                currentInDegree += dirMatrix[j,i];
            }
            if (currentOutDegree != firstOutDegree || currentInDegree != firstInDegree)
            {
                Console.WriteLine("The DirMatrix is non-Regular");
                return;
            }   
        } 
    }
    void CheckUndirforRegular()
    {
        Console.WriteLine("\nundirMatrix:\n");
        int firstDegree = 0;
        for (int j = 0; j < N; j++)
        {
            firstDegree += undirMatrix[0,j];
        }
        for (int i = 1; i < N; i++)
        {
            int currentDegree = 0;
            for(int j = 0; j < N; j++)
            {
                currentDegree += undirMatrix[i,j];
            } 
            if (currentDegree != firstDegree)
            {
                Console.WriteLine("The undirMatrix is non-Regular");
                return;
            }
            
        }
        Console.WriteLine("The undirMatrix is Regular");
    }
    void CheckForRegular()
    {   
        CheckDirForRegular();
        CheckUndirforRegular();
    }
    void CheckForSpecialVertices()
    {   
        bool found = false;
        Console.WriteLine("UndirMatrix:");
        for(int i = 0; i < N; i++)
        {
            int currentVertice = 0;
            for(int j = 0; j < N; j++)
            {
                currentVertice += undirMatrix[i,j];
            }
            if (currentVertice == 0)
            {
                Console.WriteLine($"Vertice {i+1} is isolated");
                found = true;
            } else if (currentVertice == 1)
            {
                Console.WriteLine($"Vertice {i+1} is hanging");
                found = true;
            }
            if (!found)
            {
                Console.WriteLine("No Hanging or Isolated vortices found;");
            }

        } 
        Console.WriteLine("DirMatrix:");
        for (int k = 0; k < N; k++)
        {
            int currentInVertice = 0;
            int currentOutVertice = 0;
        }

    }
    void OnPaint(object sender, PaintEventArgs e)
    {
        var g = e.Graphics;
        int r = 20;
        int offsetX = 1000;
        int loopSize = 40;
        for (int i = 0; i < N; i++)
        {
            for (int j = i + 1; j < N; j++)
                if (undirMatrix[i, j] == 1)
                    g.DrawLine(Pens.Black, coordX[i], coordY[i], coordX[j], coordY[j]);
            g.FillEllipse(Brushes.White, coordX[i] - r, coordY[i] - r, r * 2, r * 2);
            g.DrawEllipse(Pens.Black, coordX[i] - r, coordY[i] - r, r * 2, r * 2);
            g.DrawString((i + 1).ToString(), this.Font, Brushes.Black, coordX[i] - 5, coordY[i] - 7);
        }
        for (int i = 0; i < N; i++)
        {
            if (dirMatrix[i, i] == 1)
            {
                int x = coordX[i] + offsetX;
                int y = coordY[i];
                int lx = x - loopSize / 2;
                int ly = y - loopSize / 2;
                if (coordY[i] == 200) ly -= r;
                else if (coordY[i] == 600) ly += r;
                else if (coordX[i] == 800) lx += r;
                else if (coordX[i] == 200) lx -= r;

                g.DrawEllipse(Pens.Black, lx, ly, loopSize, loopSize);
            }
        }
        for (int i = 0; i < N; i++)
        {
            for (int j = 0; j < N; j++)
            {
                if (i != j && dirMatrix[i, j] == 1)
                {
                    double angle = Math.Atan2(coordY[j] - coordY[i], coordX[j] - coordX[i]);
                    int startX = coordX[i] + offsetX + (int)(r * Math.Cos(angle));
                    int startY = coordY[i] + (int)(r * Math.Sin(angle));
                    int endX = coordX[j] + offsetX - (int)(r * Math.Cos(angle));
                    int endY = coordY[j] - (int)(r * Math.Sin(angle));
                    g.DrawLine(Pens.Black, startX, startY, endX, endY);
                    int arrowLen = 12;
                    g.DrawLine(Pens.Black, endX, endY, (int)(endX - arrowLen * Math.Cos(angle - 0.4)), (int)(endY - arrowLen * Math.Sin(angle - 0.4)));
                    g.DrawLine(Pens.Black, endX, endY, (int)(endX - arrowLen * Math.Cos(angle + 0.4)), (int)(endY - arrowLen * Math.Sin(angle + 0.4)));
                }
            }
        }
        for (int i = 0; i < N; i++)
        {
            int x = coordX[i] + offsetX;
            int y = coordY[i];
            
            g.FillEllipse(Brushes.White, x - r, y - r, r * 2, r * 2);
            g.DrawEllipse(Pens.Black, x - r, y - r, r * 2, r * 2);
            g.DrawString((i + 1).ToString(), this.Font, Brushes.Black, x - 5, y - 7);
        }
        g.DrawString("Undir graph", this.Font, Brushes.Black, 400, 650);
        g.DrawString("Dir graph", this.Font, Brushes.Black, 1400, 650);
        g.DrawString("A self-loop starting and ending at the same node indicates that the object is directed at itself", this.Font, Brushes.Black, 1000, 700);
    }
}