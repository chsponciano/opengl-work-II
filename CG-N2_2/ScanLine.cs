using System;
using System.Collections.Generic;
using CG_Biblioteca;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using static System.Math;

namespace gcgcg
{
  internal class ScanLine
  {

    public double Y;
    private List<Ponto4D> pontos = new List<Ponto4D>();

    public ScanLine(double Y)
    {
      this.Y = Y;
    }

    /// <summary>
    /// Calcula o ponto de interseccao entre um ponto e outro usando o valor de Y contido no estado desse objeto.
    /// Também guarda o ponto encontra numa lista interna
    /// </summary>
    /// <param name="p1">Ponto inicial da linha imaginaria</param>
    /// <param name="p2">Ponto final da linha imaginaria</param>
    /// <returns>
    /// O ponto encontrado
    /// </returns>
    public Ponto4D CalcularScanExtrema(Ponto4D p1, Ponto4D p2)
    {
      var x = p1.X + (p2.X - p1.X) / ((p2.Y - p1.Y)) * (this.Y - p1.Y);
      var ponto = new Ponto4D(Math.Round(x), this.Y);
      var existe = false;
      foreach(Ponto4D pto in this.pontos) {
        if (ponto.X == pto.X && pto.Y == ponto.Y) {
          existe = true;
          break;
        }
      }
      if (!existe) {
        this.pontos.Add(ponto);
      }
      return ponto;
    }

    /// <summary>
    /// Usa uma lista interna para saber se existem um numero de interseccoes impares a esquerda e a direita
    /// a partir de um ponto
    /// </summary>
    /// <param name="ponto">Ponto usado para a validacao</param>
    /// <returns>
    /// True se a condicao for verdadeira
    /// </returns>
    public bool InteseccoesImparesNaEsquerdaDireita(Ponto4D ponto) {
      var contadorDireita = 0;
      var contadorEsquerda = 0;
      foreach (Ponto4D interseccao in this.pontos) {
        if (interseccao.X > ponto.X) {
          contadorEsquerda++;
        } else if (interseccao.X < ponto.X) {
          contadorDireita++;
        }
      }
      return contadorDireita % 2 != 0 && contadorEsquerda % 2 != 0;
    }
  }
}