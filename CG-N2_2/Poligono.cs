using System;
using System.Collections.Generic;
using CG_Biblioteca;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using static System.Math;

namespace gcgcg
{
  internal class Poligono : ObjetoAramado
  {

    private Ponto4D verticeSelecionado = null;
    public Ponto4D VerticeSelecionado {
      get { return this.verticeSelecionado; }
    }

    public Poligono(string rotulo, Objeto paiRef) : base(rotulo, paiRef)
    {
    }

    /// <summary>
    /// Seleciona o vertice mais proximo em relação ao ponto passado
    /// </summary>
    /// <param name="ponto">Ponto de referencia</param>
    public void SelecionarVertice(Ponto4D ponto) {
      this.SetVerticeSelecionado(this.GetVerticeMaisProximo(ponto));
    }

    /// <summary>
    /// Encontra o vertice mais proximo em relação ao ponto passado
    /// </summary>
    /// <param name="ponto">Ponto de referencia</param>
    /// <returns>
    /// O vertice mais proximo encontrado
    /// </returns>
    private Ponto4D GetVerticeMaisProximo(Ponto4D ponto) {
      Ponto4D maisProximo = null;
      foreach (Ponto4D vertice in base.pontosLista) {
        if (maisProximo == null || this.GetDistancia(maisProximo, ponto) > this.GetDistancia(vertice, ponto)) {
          maisProximo = vertice;
        }
      }
      return maisProximo;
    }

    /// <summary>
    /// Remove o vertice atualmente selecionado da lista de vertices
    /// </summary>
    public void RemoverVerticeSelecionado() {
      base.pontosLista.RemoveAll(vertice => vertice == this.verticeSelecionado);
      this.verticeSelecionado = null;
    }

    public void SetVerticeSelecionado(Ponto4D vertice) {
      this.verticeSelecionado = vertice;
    }


    /// <summary>
    /// Calcula a distancia entre dois pontos
    /// </summary>
    /// <param name="ponto1">Primeiro ponto de referencia</param>
    /// <param name="ponto2">Segundo ponto de referencia</param>
    /// <returns>
    /// A distancia entre os pontos
    /// </returns>
    private double GetDistancia(Ponto4D ponto1, Ponto4D ponto2)
    {
      return Math.Sqrt(Math.Pow(ponto1.X - ponto2.X, 2) + Math.Pow(ponto1.Y - ponto2.Y, 2));
    }

    /// <summary>
    /// Calcula se o ponto passado está dentro desse objeto
    /// </summary>
    /// <param name="ponto">Ponto de referencia</param>
    /// <returns>
    /// True caso o ponto esteja dentro do objeto
    /// </returns>
    public bool PontoDentroDoObjeto(Ponto4D ponto)
    {
      if (!this.BBox.DentroDaArea(ponto)) {
        return false;
      }
      var scanLine = new ScanLine(ponto.Y);
      Ponto4D p1 = null;
      Ponto4D p2 = null;
      for (int i = 1; i < base.pontosLista.Count; i++)
      {
        p1 = base.pontosLista[i - 1];
        p2 = base.pontosLista[i];
        if (((p1.Y <= scanLine.Y) && (p2.Y > scanLine.Y)) || ((p2.Y <= scanLine.Y) && (p1.Y > scanLine.Y)))
        {
          scanLine.CalcularScanExtrema(p1, p2);
        }
      }
      return scanLine.InteseccoesImparesNaEsquerdaDireita(ponto);
    }

    /// <summary>
    /// Calcula se alguns dos objetos contidos nesse objeto deve ser selecinado de acordo com o ponto passado
    /// </summary>
    /// <param name="ponto">Ponto de referencia</param>
    /// <returns>
    /// O objeto que deve ser selecionado ou null
    /// </returns>
    public ObjetoAramado GetObjetoSelecao(Ponto4D ponto)
    {
      if (this.PontoDentroDoObjeto(ponto))
      {
        return this;
      }
      foreach (Objeto objeto in base.objetosLista) {
        if (objeto is Poligono && ((Poligono) objeto).PontoDentroDoObjeto(ponto))
        {
          return ((Poligono) objeto).GetObjetoSelecao(ponto);
        }
      }
      return null;
    }

  }

}