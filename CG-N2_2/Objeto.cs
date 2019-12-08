using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using CG_Biblioteca;
using System.Drawing;
using System;

namespace gcgcg
{
  internal abstract class Objeto
  {
    protected string rotulo;
    private PrimitiveType primitivaTipo = PrimitiveType.LineLoop;
    private bool _aberto = false;
    public bool Aberto {
      get { return this._aberto; }
      set { 
        this._aberto = value;
        this.primitivaTipo = this._aberto ? PrimitiveType.LineStrip : PrimitiveType.LineLoop;
      }
    }
    private Objeto _paiRef;
    public Objeto PaiRef {
      get { return this._paiRef; }
      set { this._paiRef = value; }
    }
    protected PrimitiveType PrimitivaTipo { get => primitivaTipo; set => primitivaTipo = value; }
    private float primitivaTamanho = 2;
    protected float PrimitivaTamanho { get => primitivaTamanho; set => primitivaTamanho = value; }
    private BBox bBox;
    public BBox BBox { get => bBox; set => bBox = value; }
    protected List<Objeto> objetosLista = new List<Objeto>();

    public Transformacao4D Matriz {
      get { return this._matriz; }
    }
    private Transformacao4D _matriz = new Transformacao4D();
    /// Matrizes temporarias que sempre sao inicializadas com matriz Identidade entao podem ser "static".
    private static Transformacao4D matrizTmpTranslacao = new Transformacao4D();
    private static Transformacao4D matrizTmpTranslacaoInversa = new Transformacao4D();
    private static Transformacao4D matrizTmpEscala = new Transformacao4D();
    private static Transformacao4D matrizTmpRotacao = new Transformacao4D();
    private static Transformacao4D matrizGlobal = new Transformacao4D();

    public Objeto(string rotulo, Objeto paiRef)
    {
      this.rotulo = rotulo;
      this.PaiRef = paiRef;
      this.bBox = new BBox(this);
    }

    public void Desenhar()
    {
      GL.PushMatrix();                                    // N3-Exe14: grafo de cena
      GL.MultMatrix(_matriz.ObterDados());
      DesenharAramado();
      for (var i = 0; i < objetosLista.Count; i++)
      {
        objetosLista[i].Desenhar();
      }
      GL.PopMatrix();                                     // N3-Exe14: grafo de cena
    }
    protected abstract void DesenharAramado();
    public void FilhoAdicionar(Objeto filho)
    {
      this.objetosLista.Add(filho);
    }
    public void FilhoRemover(Objeto filho)
    {
      this.objetosLista.Remove(filho);
    }
    protected abstract void PontosExibir();
    public void PontosExibirObjeto()
    {
      PontosExibir();
    }
    public void ExibeMatriz()
    {
      _matriz.ExibeMatriz();
    }
    public void AtribuirIdentidade()
    {
      _matriz.AtribuirIdentidade();
    }
    public void TranslacaoXY(double tx, double ty)
    {
      Transformacao4D matrizTranslate = new Transformacao4D();
      matrizTranslate.AtribuirTranslacao(tx, ty, 0);
      _matriz = matrizTranslate.MultiplicarMatriz(_matriz);
    }
    public void EscalaXY(double Sx, double Sy)
    {
      Transformacao4D matrizScale = new Transformacao4D();
      matrizScale.AtribuirEscala(Sx, Sy, 1.0);
      _matriz = matrizScale.MultiplicarMatriz(_matriz);
    }

    public void EscalaXYBBox(double escala)
    {
      matrizGlobal.AtribuirIdentidade();
      Ponto4D pontoPivo = bBox.obterCentro;

      matrizTmpTranslacao.AtribuirTranslacao(-pontoPivo.X, -pontoPivo.Y, -pontoPivo.Z); // Inverter sinal
      matrizGlobal = matrizTmpTranslacao.MultiplicarMatriz(matrizGlobal);

      matrizTmpEscala.AtribuirEscala(escala, escala, 1.0);
      matrizGlobal = matrizTmpEscala.MultiplicarMatriz(matrizGlobal);

      matrizTmpTranslacaoInversa.AtribuirTranslacao(pontoPivo.X, pontoPivo.Y, pontoPivo.Z);
      matrizGlobal = matrizTmpTranslacaoInversa.MultiplicarMatriz(matrizGlobal);

      _matriz = _matriz.MultiplicarMatriz(matrizGlobal);
    }
    public void RotacaoZ(double angulo)
    {
      matrizTmpRotacao.AtribuirRotacaoZ(Transformacao4D.DEG_TO_RAD * angulo);
      _matriz = matrizTmpRotacao.MultiplicarMatriz(_matriz);
    }
    public void RotacaoZBBox(double angulo)
    {
      matrizGlobal.AtribuirIdentidade();
      Ponto4D pontoPivo = bBox.obterCentro;

      matrizTmpTranslacao.AtribuirTranslacao(-pontoPivo.X, -pontoPivo.Y, -pontoPivo.Z); // Inverter sinal
      matrizGlobal = matrizTmpTranslacao.MultiplicarMatriz(matrizGlobal);

      matrizTmpRotacao.AtribuirRotacaoZ(Transformacao4D.DEG_TO_RAD * angulo);
      matrizGlobal = matrizTmpRotacao.MultiplicarMatriz(matrizGlobal);

      matrizTmpTranslacaoInversa.AtribuirTranslacao(pontoPivo.X, pontoPivo.Y, pontoPivo.Z);
      matrizGlobal = matrizTmpTranslacaoInversa.MultiplicarMatriz(matrizGlobal);

      _matriz = _matriz.MultiplicarMatriz(matrizGlobal);
    }

    public void RemoverFilho(Objeto objeto)
    {
      this.objetosLista.Remove(objeto);
    }

    public Ponto4D DistanciaParaOCentro(Ponto4D ponto)
    {
      var centro = this.BBox.obterCentro;
      return new Ponto4D(centro.X - ponto.X, centro.Y - ponto.Y);
    }

  }
}