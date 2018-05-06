using EsqueletoUsuario.FuncoesBasicas;
using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Kinect.Toolkit.Controls;
using EsqueletoUsuario.Auxiliar;
using EsqueletoUsuario.Movimentos;
using EsqueletoUsuario.Movimentos.Poses;

namespace EsqueletoUsuario
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        KinectSensor kinect;
        List<IRastreador> rastreadores;
        ControleJogador controle;

        double alturaChao;

        public MainWindow()
        {
            InitializeComponent();
            InicializarSeletor();
            InicializarRastreadores();
            controle = new ControleJogador();
        }

        private void InicializarSeletor()
        {
            InicializadorKinect inicializador = new InicializadorKinect();
            inicializador.MetodoInicializadorKinect = InicializarKinect;
            sensorChooserUi.KinectSensorChooser = inicializador.SeletorKinect;
        }

        private void InicializarKinect(KinectSensor kinectSensor)
        {
            kinect = kinectSensor;
            //slider.Value = kinect.ElevationAngle;

            kinect.DepthStream.Enable();
            kinect.SkeletonStream.Enable();
            kinect.ColorStream.Enable();
            kinect.AllFramesReady += kinect_AllFramesReady;
        }

        private void InicializarRastreadores()
        {
            rastreadores = new List<IRastreador>();

            Rastreador<PoseAgachar> rastreadorPoseAgachar = new Rastreador<PoseAgachar>();
            rastreadorPoseAgachar.MovimentoIdentificado += PoseAgacharIdentificada;

            //Rastreador<PoseAndar> rastreadorPoseAndar = new Rastreador<PoseAndar>();
            //rastreadorPoseAndar.MovimentoIdentificado += PoseAndarIdentificada;
            //rastreadorPoseAndar.MovimentoEmProgresso += PoseAndarEmProgresso;

            Rastreador<PoseAndarFrente> rastreadorPoseAndarFrente = new Rastreador<PoseAndarFrente>();
            rastreadorPoseAndarFrente.MovimentoIdentificado += PoseAndarFrenteIdentificada;
            rastreadorPoseAndarFrente.MovimentoEmProgresso += PoseAndarFrenteEmProgresso;

            Rastreador<PoseAndarTras> rastreadorPoseAndarTras = new Rastreador<PoseAndarTras>();
            rastreadorPoseAndarTras.MovimentoIdentificado += PoseAndarTrasIdentificada;
            rastreadorPoseAndarTras.MovimentoEmProgresso += PoseAndarTrasEmProgresso;

            Rastreador<PoseT> rastreadorPoseT = new Rastreador<PoseT>();
            rastreadorPoseT.MovimentoIdentificado += PoseTIdentificada;
            rastreadorPoseT.MovimentoEmProgresso += PoseTEmProgresso;

            rastreadores.Add(rastreadorPoseAgachar);
            rastreadores.Add(rastreadorPoseAndarFrente);
            rastreadores.Add(rastreadorPoseAndarTras);
            rastreadores.Add(rastreadorPoseT);
        }

        private void PoseAndarTrasIdentificada(object sender, EventArgs e)
        {
            txtControle.Text = "Andando para trás";
        }

        private void PoseAndarTrasEmProgresso(object sender, EventArgs e)
        {
            txtControle.Text = "";
        }

        private void PoseTEmProgresso(object sender, EventArgs e)
        {
            PoseT pose = sender as PoseT;

            Rectangle retangulo = new Rectangle();
            retangulo.Width = canvasKinect.ActualWidth;
            retangulo.Height = 20;
            retangulo.Fill = Brushes.Black;

            Rectangle poseRetangulo = new Rectangle();
            poseRetangulo.Width = canvasKinect.ActualWidth * pose.PercentualProgresso / 100;
            poseRetangulo.Height = 20;
            poseRetangulo.Fill = Brushes.BlueViolet;

            canvasKinect.Children.Add(retangulo);
            canvasKinect.Children.Add(poseRetangulo);
        }

        private void PoseTIdentificada(object sender, EventArgs e)
        {
            Rastreador<PosePulo> rastreadorPosePulo = new Rastreador<PosePulo>();
            rastreadorPosePulo.MovimentoIdentificado += PosePuloIdentificada;

            rastreadores.Add(rastreadorPosePulo);
        }

        private void PoseAndarFrenteEmProgresso(object sender, EventArgs e)
        {
            txtControle.Text = "";
        }

        private void PoseAndarFrenteIdentificada(object sender, EventArgs e)
        {
            controle.Andar();
            txtControle.Text = "Andando para frente";
            
        }

        private void PosePuloIdentificada(object sender, EventArgs e)
        {
            controle.Pular();
            txtControle.Text = "Pulando";
        }

        private void PoseAgacharIdentificada(object sender, EventArgs e)
        {
            controle.Agachar();
            txtControle.Text = "Agachando";
        }

        private void kinect_AllFramesReady(object sender, AllFramesReadyEventArgs e)
        {
            byte[] imagem = ObterImagemSensorRGB(e.OpenColorImageFrame());

            if (chkEscalaCinza.IsChecked.HasValue && chkEscalaCinza.IsChecked.Value)
                ReconhecerDistancia(e.OpenDepthImageFrame(), imagem, 2000);

            if (imagem != null)
                canvasKinect.Background = new ImageBrush(BitmapSource.Create(kinect.ColorStream.FrameWidth, kinect.ColorStream.FrameHeight,
                                    96, 96, PixelFormats.Bgr32, null, imagem,
                                    kinect.ColorStream.FrameWidth * kinect.ColorStream.FrameBytesPerPixel));

            canvasKinect.Children.Clear();

            FuncoesEsqueletoUsuario(e.OpenSkeletonFrame());
        }

        private void FuncoesEsqueletoUsuario(SkeletonFrame quadro)
        {
            if (quadro == null) return;

            using (quadro)
            {
                Skeleton esqueletoUsuario = quadro.ObterEsqueletoUsuario();
                //foreach (IRastreador rastreador in rastreadores)
                //    rastreador.Rastrear(esqueletoUsuario);
                for(int i = 0; i < rastreadores.Count; i++)
                {
                    rastreadores.ElementAt(i).Rastrear(esqueletoUsuario);
                }

                if (chkEsqueleto.IsChecked.HasValue && chkEsqueleto.IsChecked.Value)
                    quadro.DesenharEsqueletoUsuario(kinect, canvasKinect);
            }
        }

        private byte[] ObterImagemSensorRGB(ColorImageFrame quadro)
        {
            if (quadro == null) return null;

            using (quadro)
            {
                byte[] bytesImagem = new byte[quadro.PixelDataLength];
                quadro.CopyPixelDataTo(bytesImagem);

                return bytesImagem;
            }
        }

        private void ReconhecerDistancia(DepthImageFrame quadro, byte[] bytesImagem, int distanciaMaxima)
        {
            if (quadro == null || bytesImagem == null) return;

            using (quadro)
            {
                DepthImagePixel[] imagemProfundidade = new DepthImagePixel[quadro.PixelDataLength];
                quadro.CopyDepthImagePixelDataTo(imagemProfundidade);

                DepthImagePoint[] pontosImagemProfundidade = new DepthImagePoint[640 * 480];
                kinect.CoordinateMapper.MapColorFrameToDepthFrame(kinect.ColorStream.Format, kinect.DepthStream.Format, imagemProfundidade, pontosImagemProfundidade);

                for (int i = 0; i < pontosImagemProfundidade.Length; i++)
                {
                    var point = pontosImagemProfundidade[i];
                    if (point.Depth < distanciaMaxima && KinectSensor.IsKnownPoint(point))
                    {
                        var pixelDataIndex = i * 4;

                        byte maiorValorCor = Math.Max(bytesImagem[pixelDataIndex], Math.Max(bytesImagem[pixelDataIndex + 1], bytesImagem[pixelDataIndex + 2]));

                        bytesImagem[pixelDataIndex] = maiorValorCor;
                        bytesImagem[pixelDataIndex + 1] = maiorValorCor;
                        bytesImagem[pixelDataIndex + 2] = maiorValorCor;
                    }
                }
            }
        }

        private void slider_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            kinect.ElevationAngle = Convert.ToInt32(slider.Value);
        }
    }
}
