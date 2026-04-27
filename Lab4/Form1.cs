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
    int[,] dirMatrix1;
    int[,] undirMatrix1;
    int[,] dirMatrix2;
    int[,] undirMatrix2;
    int[] coordX = [200, 400, 600, 800, 800, 800, 600, 400, 200, 200];
    int[] coordY = [200, 200, 200, 200, 400, 600, 600, 600, 600, 400];
    int[] ccordX = [600, 800, 600, 800];
    int[] ccordY = [200, 200, 400, 400];
    List<List<int>> strongComponents;
    int[,] condensationMatrix;
    

    public Form1()
    {
        double k1 = 1 - N3 * 0.01 - N4 * 0.01 - 0.3; 
        double k2 = 1 - N3 * 0.005 - N4 * 0.005 - 0.27; 
        InitializeComponent();
        this.Text = "Graph";
        this.Size = new Size(2560, 1440);
        this.Paint += OnPaint;
        GenerateMatrices(k1);
        dirMatrix1 = dirMatrix;
        undirMatrix1 = undirMatrix;
        PrintVertexDegrees();
        CheckForRegular();
        CheckForSpecialVertices();
        GenerateMatrices(k2);
        dirMatrix2 = dirMatrix;
        undirMatrix2 = undirMatrix;
        PrintVertexDegrees();
        FindPath();
        FindReachabilty();
        StrongConnectivity();

    }
    void GenerateMatrices(double k)
    {
        var rng = new Random(SEED);

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
            } 
            else if (currentVertice == 1)
            {
                Console.WriteLine($"Vertice {i+1} is hanging");
                found = true;
            }
        }
        if (!found)
        {
            Console.WriteLine("No Hanging or Isolated vortices found");
        }  
        Console.WriteLine("DirMatrix:");
        found = false;

        for (int i = 0; i < N; i++)
        {
            int currentInVertice = 0;
            int currentOutVertice = 0;
            for (int j = 0; j < N; j++)
            {
                currentInVertice += dirMatrix[j,i];
                currentOutVertice += dirMatrix[i,j];
            }
            if(currentInVertice == 0)
            {
                Console.WriteLine($"InVertice {i+1} is isolated");
                found = true;
            }
            else if(currentInVertice == 1)
            {
                Console.WriteLine($"InVertice {i+1} is hanging");
                found = true;
            }
            else if(currentOutVertice == 0)
            {
                Console.WriteLine($"OutVertice {i+1} is isolated");
                found = true;
            }
            else if(currentOutVertice == 1)
            {
                Console.WriteLine($"OutVertice {i+1} is hanging");
                found = true;
            }
        }
        if (!found)
        {
            Console.WriteLine("No Hanging or Isolated vortices found");
        }  

    }
    void FindPath()
    {
        var lines = new List<string>();

        lines.Add("All Paths of length(2) :");
        for(int i = 0; i < N; i++)
        {
           for(int j = 0; j < N; j++)
            {
                for(int k = 0; k < N; k++)
                {
                    if(dirMatrix[i,k] == 1 && dirMatrix[k,j] == 1)
                    {
                        lines.Add($"{i+1} -> {k+1} -> {j+1}");
                    }
                }
            } 
        }
        lines.Add("All Paths of length(3) :");
        for(int i = 0; i < N; i++)
        {
           for(int j = 0; j < N; j++)
            {
                for(int k = 0; k < N; k++)
                {
                    for(int l = 0; l < N; l++)
                    {
                        if(dirMatrix[i,k] == 1 && dirMatrix[k,j] == 1)
                        {
                            lines.Add($"{i+1} -> {k+1} -> {l+1} -> {j+1}");
                        }
                    }
                }
            } 
        }
        File.WriteAllLines("paths.txt", lines);
        Console.WriteLine("Paths saved to path.txt");
    }
    int[,] MultiplyMatrix(int[,] A)
    {
        int[,] result = new int[N,N];
        for (int i = 0; i < N; i++)
        {
            for(int j = 0; j < N; j++)
            {
                for(int k = 0; k < N; k++)
                {
                    result[i,j] += A[i,k] * A[k,j];  
                }
            }
        }
        return result;
    }
    int[,] FindReachabilty()
    {
        int[,] result = new int [N,N];
        int[,] current = dirMatrix;
        for (int k = 0; k < N; k++)
        {
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    result[i,j] += current[i,j];
                }
            }
            current = MultiplyMatrix(current);
        }
        for (int i = 0; i < N; i++)
        {
            for (int j = 0; j < N; j++)
            {
                result[i,j] = result[i,j] > 0 ? 1 : 0;
            }
        }
        Console.WriteLine("\nReachability Matrix:");
        for(int i = 0; i < N; i++)
        {
            for(int j = 0; j < N; j++)
            {
                Console.Write(result[i,j] + " ");
            }
            Console.WriteLine();
        }
        return result;

    }
    int[,] FindStrongConnectivity(int[,] A)
    {
        int[,] result = new int[N,N];
        for(int i = 0; i < N; i++)
        {
            for(int j = 0; j < N; j++)
            {
                if(A[i,j] == 1 && A[j,i] == 1)
                {
                    result[i,j] = 1;
                }
            }
        }
        return result;
    }
    void StrongConnectivity()
    {
        var reachability = FindReachabilty();
        var strongConnectivity = FindStrongConnectivity(reachability);
        
        Console.WriteLine("\nStrong Connectivity Matrix: ");
        for (int i = 0; i < N; i++)
        {
            for (int j = 0; j < N; j++)
            {
                Console.Write(strongConnectivity[i,j] + " ");
            }
            Console.WriteLine();
        }       
        strongComponents = FindComponents(strongConnectivity);
        condensationMatrix = BuildCondensationMatrix(strongComponents);        
        Console.WriteLine("Components:");
        for (int i = 0; i < strongComponents.Count; i++)
        {
            Console.Write($"Component {i+1} : {{");
            foreach(var j in strongComponents[i])
            {
                Console.Write($" {j}");
            }
            Console.WriteLine(" }");
        }
        Console.WriteLine("\n CondensationMatrix: ");
        for (int i = 0; i < strongComponents.Count; i++)
        {
            for (int j = 0; j < strongComponents.Count; j++)
            {
                Console.Write(condensationMatrix[i,j] + " ");
            }
            Console.WriteLine();
        }
    }
    List<List<int>> FindComponents(int [,] S)
    {
        var components = new List<List<int>>();
        bool[] visited = new bool[N];
        for(int i = 0; i < N; i++)
        {
            if (!visited[i])
            {
                visited[i] = true;
                var members = new List<int>();
                members.Add(i + 1);
                for (int j = 0; j < N; j++)
                {
                    if(S[i,j] == 1 && !visited[j])
                    {
                        members.Add(j+1);
                        visited[j] = true;
                    }
                }
                components.Add(members);
            }
        }
        return components;
    }
    int[,] BuildCondensationMatrix(List<List<int>> components)
    {
        int k = components.Count;
        int[,] condensation = new int[k, k];
        for (int i = 0; i < k; i++)
        {
            for(int j = 0; j < k; j++)
            {
                if (i != j)
                {
                    foreach (var l in components[i])
                    {
                        foreach (var m in components[j])
                        {
                            if(dirMatrix[l-1, m-1] == 1)
                            {
                                condensation[i,j] = 1;
                            }
                        }
                    }
                }
            }
        }
        return condensation;
    }
    void DrawUndirGraph(Graphics g, int[,] matrix, int offsetX, int offsetY)
    {
        int r = 20;
        for (int i = 0; i < N; i++)
        {
            for (int j = 0; j < N; j++)
            {
                if (matrix[i,j] == 1)
                {
                    g.DrawLine(Pens.Black, coordX[i] + offsetX, coordY[i] + offsetY, coordX[j] + offsetX, coordY[j] + offsetY);
                }
            }
        }
        for(int i = 0; i < N; i++)
        {
            g.FillEllipse(Brushes.White, coordX[i] + offsetX - r, coordY[i] + offsetY - r, r * 2, r * 2);
            g.DrawEllipse(Pens.Black, coordX[i] + offsetX - r, coordY[i] + offsetY - r, r * 2, r * 2);
            g.DrawString((i + 1).ToString(), this.Font, Brushes.Black, coordX[i] + offsetX - 5, coordY[i] + offsetY - 7);
        }
    }
    void DrawDirGraph(Graphics g, int[,] matrix, int offsetX, int offsetY)
    {
        int r = 20;
        int loopSize = 40;
        int bend = 50;
        for ( int i = 0; i < N; i++)
        {
            int lx = coordX[i] + offsetX - loopSize / 2;
            int ly = coordY[i] + offsetY - loopSize / 2;
            if( matrix[i,i] == 1)
            {
                if(coordY[i] == 200)
                {
                    ly -=20;
                } else if(coordY[i] == 600)
                {
                    ly +=20;
                }else if(coordX[i] == 800)
                {
                    lx +=20;
                }else if(coordX[i] == 200)
                {
                    lx -=20;
                }
                g.DrawEllipse(Pens.Black, lx, ly, loopSize, loopSize);
                
            }
            
        }
        for (int i = 0; i < N; i++)
        {
            for (int j = 0; j < N; j++)
            {
                if (i != j && matrix[i, j] == 1)
                {
                    double angle = Math.Atan2(coordY[j] - coordY[i], coordX[j] - coordX[i]);
                    int startX = coordX[i] + offsetX + (int)(r * Math.Cos(angle));
                    int startY = coordY[i] + offsetY + (int)(r * Math.Sin(angle));
                    int endX = coordX[j] + offsetX - (int)(r * Math.Cos(angle));
                    int endY = coordY[j] + offsetY - (int)(r * Math.Sin(angle));
                    int cx = (startX + endX) / 2 + (int)(bend * Math.Sin(angle));
                    int cy = (startY + endY) / 2 + (int)(bend * Math.Cos(angle));
                    g.DrawBezier(Pens.Black, startX, startY, cx, cy, cx, cy, endX, endY);
                    double endAngle = Math.Atan2(endY - cy, endX - cx);
                    int arrowLen = 12;
                    g.DrawLine(Pens.Black, endX, endY, (int)(endX - arrowLen * Math.Cos(endAngle - 0.4)), (int)(endY - arrowLen * Math.Sin(endAngle - 0.4)));
                    g.DrawLine(Pens.Black, endX, endY, (int)(endX - arrowLen * Math.Cos(endAngle + 0.4)), (int)(endY - arrowLen * Math.Sin(endAngle + 0.4)));
                }
            }
        }
        for (int i = 0; i < N; i++)
        {
            g.FillEllipse(Brushes.White, coordX[i] + offsetX - r, coordY[i] + offsetY - r, r * 2, r * 2);
            g.DrawEllipse(Pens.Black, coordX[i] + offsetX - r, coordY[i] + offsetY - r, r * 2, r * 2);
            g.DrawString((i + 1).ToString(), this.Font, Brushes.Black, coordX[i] + offsetX - 5, coordY[i] + offsetY - 7); 
        }
    }
    void DrawCondensationGraph(Graphics g, int[,] matrix, int offsetX, int offsetY)
    {
        int r = 40;
        int loopSize = 40;
        for ( int i = 0; i < strongComponents.Count; i++)
        {
            int lx = ccordX[i] + offsetX - loopSize / 2;
            int ly = ccordY[i] + offsetY - loopSize / 2;
            if( matrix[i,i] == 1)
            {
                if(ccordY[i] == 100)
                {
                    ly -=20;
                } else if(ccordY[i] == 300)
                {
                    ly +=20;
                }
                g.DrawEllipse(Pens.Black, lx, ly, loopSize, loopSize);
                
            }
            
        }
        for (int i = 0; i < strongComponents.Count; i++)
        {
            for (int j = 0; j < strongComponents.Count; j++)
            {
                if (i != j && matrix[i, j] == 1)
                {
                    double angle = Math.Atan2(ccordY[j] - ccordY[i], ccordX[j] - ccordX[i]);
                    int startX = ccordX[i] + offsetX + (int)(r * Math.Cos(angle));
                    int startY = ccordY[i] + offsetY + (int)(r * Math.Sin(angle));
                    int endX = ccordX[j] + offsetX - (int)(r * Math.Cos(angle));
                    int endY = ccordY[j] + offsetY - (int)(r * Math.Sin(angle));
                    g.DrawLine(Pens.Black, startX, startY, endX, endY);
                    int arrowLen = 12;
                    g.DrawLine(Pens.Black, endX, endY, (int)(endX - arrowLen * Math.Cos(angle - 0.4)), (int)(endY - arrowLen * Math.Sin(angle - 0.4)));
                    g.DrawLine(Pens.Black, endX, endY, (int)(endX - arrowLen * Math.Cos(angle + 0.4)), (int)(endY - arrowLen * Math.Sin(angle + 0.4)));
                }
            }
        }
        for (int i = 0; i < strongComponents.Count; i++)
        {
            g.FillEllipse(Brushes.White, ccordX[i] + offsetX - r, ccordY[i] + offsetY - r, r * 2, r * 2);
            g.DrawEllipse(Pens.Black, ccordX[i] + offsetX - r, ccordY[i] + offsetY - r, r * 2, r * 2);
            string label = "{" + string.Join(",", strongComponents[i]) + "}";
            g.DrawString(label, this.Font, Brushes.Black, ccordX[i] + offsetX - 30, ccordY[i] + offsetY - 30); 
        }
    }
    void OnPaint(object sender, PaintEventArgs e)
    {
        var g = e.Graphics;
        DrawUndirGraph(g, undirMatrix1, 0, 0);
        DrawDirGraph(g, dirMatrix1, 1000, 0);
        DrawDirGraph(g, dirMatrix2, 0, 650);
        DrawCondensationGraph(g, condensationMatrix, 1000, 750);

        g.DrawString("First Undir Graph", this.Font, Brushes.Black, 300, 670);
        g.DrawString("First Dir Graph", this.Font, Brushes.Black, 1300, 670);
        g.DrawString("Second Dir Graph", this.Font, Brushes.Black, 300, 750);
        g.DrawString("Condensation Graph", this.Font, Brushes.Black, 1300, 1050);
    }     
}