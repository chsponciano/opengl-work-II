using System;
using System.Collections.Generic;
using CG_Biblioteca;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using OpenTK;

namespace gcgcg
{
  internal class ObjetoAramado : Objeto
  {
    private Cor PrimitivaCor;
    protected List<Ponto4D> pontosLista = new List<Ponto4D>();

    public ObjetoAramado(string rotulo, Objeto paiRef) : base(rotulo, paiRef)
    {
      this.PrimitivaCor = new Cor(0, 0, 0);
    }

    protected override void DesenharAramado()
    {
      GL.LineWidth(base.PrimitivaTamanho);
      GL.Color3(this.PrimitivaCor.Red, this.PrimitivaCor.Green, this.PrimitivaCor.Blue);
      GL.Begin(base.PrimitivaTipo);
      foreach (Ponto4D pto in pontosLista)
      {
        GL.Vertex2(pto.X, pto.Y);
      }
      GL.End();
    }

    public void PontosAdicionar(Ponto4D pto)
    {
      pontosLista.Add(pto);
      if (pontosLista.Count.Equals(1))
        base.BBox.Atribuir(pto);
      else
        base.BBox.Atualizar(pto);
      base.BBox.ProcessarCentro();
    }

    public void PontosRemoverUltimo()
    {
      pontosLista.RemoveAt(pontosLista.Count - 1);
    }

    protected void PontosRemoverTodos()
    {
      pontosLista.Clear();
    }

    public Ponto4D PontosUltimo()
    {
      return pontosLista[pontosLista.Count - 1];
    }

    protected override void PontosExibir()
    {
      Console.WriteLine("__ Objeto: " + base.rotulo);
      for (var i = 0; i < pontosLista.Count; i++)
      {
        Console.WriteLine("P" + i + "[" + pontosLista[i].X + "," + pontosLista[i].Y + "," + pontosLista[i].Z + "," + pontosLista[i].W + "]");
      }
    }

    /// <summary>
    /// Aumenta o valor da cor Vermelha
    /// </summary>
    public void AddRed()
    {
      if (this.PrimitivaCor.Red == 1.2f) 
      {
        this.PrimitivaCor.Red = 0f;
      }
      else 
      {
        this.PrimitivaCor.Red += 0.0047f;
      }
    }

    /// <summary>
    /// Aumenta o valor da cor Verde
    /// </summary>
    public void AddGreen()
    {
      if (this.PrimitivaCor.Green == 1.2f) 
      {
        this.PrimitivaCor.Green = 0f;
      }
      else 
      {
        this.PrimitivaCor.Green += 0.0047f;
      }
    }

    /// <summary>
    /// Aumenta o valor da cor Azul
    /// </summary>
    public void AddBlue()
    {
      if (this.PrimitivaCor.Blue == 1.2f) 
      {
        this.PrimitivaCor.Blue = 0f;
      }
      else 
      {
        this.PrimitivaCor.Blue += 0.0047f;
      }
    }

  }
}