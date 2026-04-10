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
int[] x = new int[N];
int[] y = new int[N];
    public Form1()
    {
        InitializeComponent();
        this.Text = "Graph";
        this.Size = new Size (800, 600);
        this.Paint += OnPaint;
        GenerateMatrics();
    }
    void OnPaint (object sender, PaintEventArgs e)
    {
        var g = e.Graphics;

    }
    void GenerateMatrics()
    {
        var rng = new Random(SEED);
        double k = 1.0 - N3 * 0.02 - N4 * 0.005 - 0.25;

        dirMatrix = new int [N, N];
        undirMatrix = new int [N, N];
        
        for (int i = 0; i < N; i++)
        {
            for (int j = 0; j < N; j++)
            {
                double val = rng.NextDouble() * 2 * k;
                dirMatrix[i,j] = val >= 1 ? 1 : 0;
            }
        }
        for (int i = 0; i<N; i++)
        {
           for (int j = 0; j<N; j++)
            {
                if(dirMatrix[i,j] == 1 || dirMatrix[j, i] == 1)
                {
                    undirMatrix[i,j] = undirMatrix[j,i] = 1;
                }
            }
        }
        for (int i = 0; i < N; i++)
        {
        for (int j = 0; j < N; j++)
        {
        Console.Write(dirMatrix[i,j] + " ");
        }
        Console.WriteLine();
        }
    }
}
