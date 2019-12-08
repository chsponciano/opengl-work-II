using System;
using System.Collections.Generic;
using CG_Biblioteca;
using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace gcgcg
{
  internal class BBox
  {
    private double _xMin;
    public double xMin {
      get { return _xMin; }
      set { this._xMin = value; }
    }
    private double _xMax;
    public double xMax {
      get { return _xMax; }
      set { this._xMax = value; }
    }
    private double _yMin;
    public double yMin {
      get { return _yMin; }
      set { this._yMin = value; }
    
    }
    private double _yMax;
    public double yMax {
      get { return _yMax; }
      set { this._yMax = value; }
    }
    public Ponto4D _centro;
    public Ponto4D obterCentro {
      get { return _centro; }
    }
    private Objeto referencia;

    public BBox(Objeto referencia)
    {
      this.referencia = referencia;
    }

    public bool DentroDaArea(Ponto4D ponto) {
      return xMin <= ponto.X && yMax >= ponto.Y && xMax >= ponto.X && yMin <= ponto.Y;
    }

    public void ProcessarCentro() {
      this._centro =  new Ponto4D((xMax + xMin) / 2,(yMax + yMin) / 2);
    }

    /// <summary>
    /// Atribui os valores xMax, xMin, yMax e yMin com o valores X e Y do ponto passado
    /// </summary>
    /// <param name="ponto">Ponto de referencia</param>
    public void Atribuir(Ponto4D ponto) {
      this.xMax = ponto.X;
      this.xMin = ponto.X;
      this.yMax = ponto.Y;
      this.yMin = ponto.Y;
    }

    /// <summary>
    /// Atualiza se necessário os valores xMax, xMin, yMax e yMin com o valores X e Y do ponto passado
    /// </summary>
    /// <param name="ponto">Ponto de referencia</param>
    public void Atualizar(Ponto4D ponto) {
      this.xMax = this.xMax < ponto.X ? ponto.X : this.xMax;
      this.xMin = this.xMin > ponto.X ? ponto.X : this.xMin;
      this.yMax = this.yMax < ponto.Y ? ponto.Y : this.yMax;
      this.yMin = this.yMin > ponto.Y ? ponto.Y : this.yMin;
    }

    public void Desenhar() {
      GL.PushMatrix();   
      GL.MultMatrix(referencia.Matriz.ObterDados());
      GL.LineWidth(2);
      GL.Color3(Color.Yellow);
      GL.Begin(PrimitiveType.LineLoop);
      GL.Vertex2(xMin, yMax);
      GL.Vertex2(xMax, yMax);
      GL.Vertex2(xMax, yMin);
      GL.Vertex2(xMin, yMin);
      GL.Vertex2(xMin, yMax);
      GL.End();

      GL.PushMatrix();   
      GL.MultMatrix(referencia.Matriz.ObterDados());
      GL.PointSize(2);
      GL.Color3(Color.Yellow);
      GL.Begin(PrimitiveType.Points);
      GL.Vertex2(obterCentro.X, obterCentro.Y);
      GL.End();
    }

  }
}