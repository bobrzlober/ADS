namespace Lab3;
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
    }

    void GenerateMatrices()
    {
        var rng = new Random(SEED);
        double k = 1.0 - N3 * 0.02 - N4 * 0.005 - 0.25;

        dirMatrix = new int[N, N];
        undirMatrix = new int[N, N];

        for (int i = 0; i < N; i++)
            for (int j = 0; j < N; j++)
            {
                double val = rng.NextDouble() * 2.0 * k;
                dirMatrix[i, j] = val >= 1.0 ? 1 : 0;
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
    }

    void OnPaint(object sender, PaintEventArgs e)
    {
        var g = e.Graphics;
        int r = 20;
        int offsetX = 1000;
        for(int i = 0; i<N; i++)
        {
            for(int j = 0; j<N; j++)
            {
                if(undirMatrix[i, j] == 1)
                {
                    g.DrawLine(Pens.Black, coordX[i], coordY[i], coordX[j], coordY[j]);
                }
            }
        }
        for(int i = 0; i<N; i++)
        {
            g.FillEllipse(Brushes.White, coordX[i] - r, coordY[i] - r, r*2, r*2);
            g.DrawEllipse(Pens.Black, coordX[i] - r, coordY[i] - r, r*2, r*2);
            g.DrawString((i + 1).ToString(), this.Font, Brushes.Black, coordX[i] - 5, coordY[i] - 7);
        }
        for (int i = 0; i<N; i++)
        {
            for (int j = 0; j<N; j++)
            {
                if(dirMatrix[i, j] == 1)
                {
                    int MidX = (coordX[i] + coordX[j]) / 2 + offsetX;
                    int MidY = (coordY[i] + coordY[j]) / 2;
                    double angle = Math.Atan2(coordY[j] - coordY[i], coordX[j] - coordX[i]);
                    int arrowSize = 15;
                    g.DrawLine(Pens.Black, coordX[i] + offsetX, coordY[i], coordX[j] + offsetX, coordY[j]);
                    int ax1 = (int)(MidX - arrowSize * Math.Cos(angle - 0.5));
                    int ay1 = (int)(MidY - arrowSize * Math.Sin(angle - 0.5));
                    int ax2 = (int)(MidX - arrowSize * Math.Cos(angle + 0.5));
                    int ay2 = (int)(MidY - arrowSize * Math.Sin(angle + 0.5));
                    g.DrawLine(Pens.DarkRed, MidX, MidY, ax1, ay1);
                    g.DrawLine(Pens.DarkRed, MidX, MidY, ax2, ay2);
                }
            }
        }
        for (int i = 0; i<N; i++)
        {
            g.FillEllipse(Brushes.White, coordX[i] + offsetX - r, coordY[i] - r, r*2, r*2);
            g.DrawEllipse(Pens.Black, coordX[i] + offsetX- r, coordY[i] - r, r*2, r*2);
            g.DrawString((i + 1).ToString(), this.Font, Brushes.Black, coordX[i] + offsetX - 5, coordY[i] - 7);
        }
    }
}