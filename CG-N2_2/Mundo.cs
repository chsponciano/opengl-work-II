using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using System.Drawing;
using OpenTK.Input;
using CG_Biblioteca;
using System.Globalization;

namespace gcgcg
{
    class Mundo : GameWindow
    {
        private static Mundo instanciaMundo = null;

        private Mundo(int width, int height) : base(width, height) { }

        public static Mundo GetInstance(int width, int height)
        {
        if (instanciaMundo == null)
            instanciaMundo = new Mundo(width, height);
        return instanciaMundo;
        }

        private Camera camera = new Camera();
        protected List<Objeto> objetosLista = new List<Objeto>();
        private ObjetoAramado objetoSelecionado = null;
        private bool bBoxDesenhar = false;
        private ObjetoAramado objetoNovo = null;
        private String objetoId = "A";
        private bool criandoObjeto = false;
        private bool movendoVertice = false;
        private bool criandoFilho = false;

        protected override void OnLoad(EventArgs e)
        {
        base.OnLoad(e);
        GL.ClearColor(Color.Gray);
        }
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
        base.OnUpdateFrame(e);
        GL.MatrixMode(MatrixMode.Projection);
        GL.LoadIdentity();
        GL.Ortho(camera.xmin, camera.xmax, camera.ymin, camera.ymax, camera.zmin, camera.zmax);
        }
        protected override void OnRenderFrame(FrameEventArgs e)
        {
        base.OnRenderFrame(e);
        GL.Clear(ClearBufferMask.ColorBufferBit);
        GL.MatrixMode(MatrixMode.Modelview);
        GL.LoadIdentity();
        for (var i = 0; i < objetosLista.Count; i++)
            objetosLista[i].Desenhar();
        if (bBoxDesenhar && (objetoSelecionado != null))
            objetoSelecionado.BBox.Desenhar();
        this.SwapBuffers();
        }

        protected override void OnKeyDown(OpenTK.Input.KeyboardKeyEventArgs e)
        {
            // N3-Exe2: usar o arquivo docs/umlClasses.wsd
            // N3-Exe3: usar o arquivo bin/documentação.XML -> ver exemplo CG_Biblioteca/bin/documentação.XML
            if (e.Key == Key.Escape)
                Exit();
            else if (e.Key == Key.E)    // N3-Exe4: ajuda a conferir se os poligonos e vértices estão certos
            {
                for (var i = 0; i < objetosLista.Count; i++)
                {
                objetosLista[i].PontosExibirObjeto();
                }
            }
            else if (e.Key == Key.O)
                bBoxDesenhar = !bBoxDesenhar;   // N3-Exe9: exibe a BBox
            else if (e.Key == Key.M)
                objetoSelecionado.ExibeMatriz();
            else if (e.Key == Key.P)
                objetoSelecionado.PontosExibirObjeto();
            else if (e.Key == Key.I)
                objetoSelecionado.AtribuirIdentidade();
            //FIXME: não está atualizando a BBox com as transformações geométricas
            else if (e.Key == Key.Left)
                objetoSelecionado.TranslacaoXY(-10, 0);
            else if (e.Key == Key.Right)
                objetoSelecionado.TranslacaoXY(10, 0); 
            else if (e.Key == Key.Up)
                objetoSelecionado.TranslacaoXY(0, 10); 
            else if (e.Key == Key.Down)
                objetoSelecionado.TranslacaoXY(0, -10);
            else if (e.Key == Key.PageUp)
                objetoSelecionado.EscalaXY(1.1, 1.1);
            else if (e.Key == Key.PageDown)
                objetoSelecionado.EscalaXY(0.9, 0.9);
            else if (e.Key == Key.Home)
                objetoSelecionado.EscalaXYBBox(0.5);
            else if (e.Key == Key.End)
                objetoSelecionado.EscalaXYBBox(2);  
            else if (e.Key == Key.Number1)
                objetoSelecionado.RotacaoZ(10);
            else if (e.Key == Key.Number2)
                objetoSelecionado.RotacaoZ(-10);
            else if (e.Key == Key.Number3)
                objetoSelecionado.RotacaoZBBox(10); 
            else if (e.Key == Key.Number4)
                objetoSelecionado.RotacaoZBBox(-10);
            else if (e.Key == Key.Enter)
            {
                objetoSelecionado = objetoNovo;
                objetoNovo.PontosRemoverUltimo();   // N3-Exe6: "troque" para deixar o rastro
                objetoNovo = null;
            }
            else if (e.Key == Key.Space)
            {
                if (this.criandoObjeto)
                {
                    this.criandoObjeto = false;
                    if (this.objetoNovo != null)
                    {
                        this.objetoNovo.PontosRemoverUltimo();
                    }
                    this.objetoNovo = null;
                }
                else 
                {
                    this.criandoObjeto = true;
                }
            }
            else if (e.Key == Key.Delete) 
            {
                if (this.objetoSelecionado != null && !this.movendoVertice)
                {
                    if (this.objetoSelecionado.PaiRef != null) {
                        this.objetoSelecionado.PaiRef.RemoverFilho(this.objetoSelecionado);
                    } else {
                        this.objetosLista.Remove(this.objetoSelecionado);
                    }
                    this.objetoSelecionado = null;
                }
                else if (this.objetoSelecionado != null && this.movendoVertice)
                {
                    ((Poligono) this.objetoSelecionado).RemoverVerticeSelecionado();
                    this.movendoVertice = false;
                }
            }
            else if (e.Key == Key.C)
            {
                if (this.objetoSelecionado != null)
                {
                    this.objetoSelecionado.Aberto = !this.objetoSelecionado.Aberto;
                }
            }
            else if (e.Key == Key.F)
            {
                this.criandoFilho = this.objetoSelecionado != null && !this.criandoFilho;
            }
            else if (e.Key == Key.R)
            {
                if (this.objetoSelecionado != null)
                {
                    this.objetoSelecionado.AddRed();
                }
            }
            else if (e.Key == Key.G)
            {
                if (this.objetoSelecionado != null)
                {
                    this.objetoSelecionado.AddGreen();
                }
            }
            else if (e.Key == Key.B)
            {
                if (this.objetoSelecionado != null)
                {
                    this.objetoSelecionado.AddBlue();
                }
            }
        }
        

        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            var mouseX = e.Position.X - 300;
            var mouseY = 300 - e.Position.Y; 
            if (objetoNovo != null)
            {
                objetoNovo.PontosUltimo().X = mouseX;
                objetoNovo.PontosUltimo().Y = mouseY;
            }
            else if (movendoVertice && objetoSelecionado != null && ((Poligono) objetoSelecionado).VerticeSelecionado != null)
            {
                ((Poligono) objetoSelecionado).VerticeSelecionado.X = mouseX;
                ((Poligono) objetoSelecionado).VerticeSelecionado.Y = mouseY;
            }
        }

        protected override void OnMouseDown(OpenTK.Input.MouseButtonEventArgs e) 
        {
            var mouseX = e.Position.X - 300; 
            var mouseY = 300 - e.Position.Y;
            if (this.criandoObjeto && !this.criandoFilho) 
                this.criarObjeto(mouseX, mouseY);
            else if (this.criandoFilho)
                this.criarFilho(mouseX, mouseY);
            else 
                this.selecionarObjeto(mouseX, mouseY);
            
        }

        private void criarObjeto(double mouseX, double mouseY) 
        {
            if (objetoNovo == null)
            {
                objetoNovo = new Poligono(objetoId + 1, null);
                objetosLista.Add(objetoNovo);
                objetoNovo.PontosAdicionar(new Ponto4D(mouseX, mouseY));
                objetoNovo.PontosAdicionar(new Ponto4D(mouseX, mouseY));
            }
            else
                objetoNovo.PontosAdicionar(new Ponto4D(mouseX, mouseY));
        }

        private void criarFilho(double mouseX, double mouseY)
        {
            if (objetoNovo == null)
            {
                objetoNovo = new Poligono(objetoId + 1, objetoSelecionado);
                objetoSelecionado.FilhoAdicionar(objetoNovo);
                objetoNovo.PontosAdicionar(new Ponto4D(mouseX, mouseY));
                objetoNovo.PontosAdicionar(new Ponto4D(mouseX, mouseY));
            }
            else
                objetoNovo.PontosAdicionar(new Ponto4D(mouseX, mouseY));
        }

        private void selecionarObjeto(double mouseX, double mouseY)
        {
            var ponto = new Ponto4D(mouseX, mouseY);
            var dentroDaBBox = this.objetoSelecionado != null && this.objetoSelecionado.BBox.DentroDaArea(ponto);
            if (this.objetoSelecionado != null && this.objetoSelecionado is Poligono && !this.movendoVertice && dentroDaBBox)
            {
                this.movendoVertice = true;
                ((Poligono) this.objetoSelecionado).SelecionarVertice(ponto);
                return;
            }
            else if (this.objetoSelecionado != null && this.objetoSelecionado is Poligono && this.movendoVertice && dentroDaBBox)
            {
                this.movendoVertice = false;
                ((Poligono) this.objetoSelecionado).SetVerticeSelecionado(null);
                return;
            }
            this.objetoSelecionado = null;
            foreach (Objeto objeto in this.objetosLista)
            {
                if (objeto is Poligono) 
                {
                    var aux = ((Poligono) objeto).GetObjetoSelecao(ponto);
                    if (aux != null)
                    {
                        this.objetoSelecionado = aux;
                        break;
                    }
                }
            }
        }

    }

    class Program
    {
        static void Main(string[] args)
        {
        Mundo window = Mundo.GetInstance(600, 600);
        window.Title = "CG-N3";
        window.Run(1.0 / 60.0);
        }
    }
}