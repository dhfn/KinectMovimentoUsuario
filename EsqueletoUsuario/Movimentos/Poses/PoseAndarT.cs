using EsqueletoUsuario.FuncoesBasicas;
using EsqueletoUsuario.Movimentos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;
using System.Windows;

namespace EsqueletoUsuario.Movimentos.Poses
{
    public class PoseAndarT : Pose
    {
        private static Skeleton esqueletoAnterior = null;

        public PoseAndarT()
        {
            this.Nome = "PosePulo";
            this.QuadroIdentificacao = 1;
        }

        protected override bool PosicaoValida(Skeleton esqueletoUsuario)
        {
            Joint peEsquerdo = esqueletoUsuario.Joints[JointType.FootLeft];
            Joint peDireito = esqueletoUsuario.Joints[JointType.FootRight];
            double raio = 0.01;

            if (esqueletoAnterior == null)
            {
                esqueletoAnterior = esqueletoUsuario;
                return false;
            }

            Joint peEsquerdoAnterior = esqueletoAnterior.Joints[JointType.FootLeft];
            Joint peDireitoAnterior = esqueletoAnterior.Joints[JointType.FootRight];

            Vector centroEsquerdo = new Vector(peEsquerdoAnterior.Position.X, peEsquerdoAnterior.Position.Z);
            Vector centroDireito = new Vector(peDireitoAnterior.Position.X, peDireitoAnterior.Position.Z);

            Vector pontoEsquero = new Vector(peEsquerdo.Position.X, peEsquerdo.Position.Z);
            Vector pontoDireito = new Vector(peDireito.Position.X, peDireito.Position.Z);

            bool foraDaCircunferenciaEsquerdo = Util.ForaDaCircunferencia(centroEsquerdo, pontoEsquero, raio);
            bool foraDaCircunferenciaDireito = Util.ForaDaCircunferencia(centroDireito, pontoDireito, raio);

            bool direcaoCertaEsquerdo = false;
            bool direcaoCertaDireiro = false;

            if (foraDaCircunferenciaEsquerdo)
            {
                direcaoCertaEsquerdo = DetectaDireção(peDireito, peEsquerdo, esqueletoUsuario);
            }
            if (foraDaCircunferenciaDireito)
            {
                direcaoCertaDireiro = DetectaDireção(peEsquerdo, peDireito, esqueletoUsuario);
            }

            // captura o esquelo atual para futura comparação
            esqueletoAnterior = esqueletoUsuario;

            return (foraDaCircunferenciaEsquerdo || foraDaCircunferenciaDireito) && (direcaoCertaEsquerdo || direcaoCertaDireiro);
        }

        private bool DetectaDireção(Joint peOrigem, Joint pe, Skeleton esqueleto)
        {
            Vector4 orientacao = esqueleto.BoneOrientations[JointType.HipCenter].AbsoluteRotation.Quaternion;
            Joint central = esqueleto.Joints[JointType.HipCenter];

            Vector orientacaoDoCorpo = new Vector(orientacao.X, orientacao.Z);
            Vector posicaoPeOrigem = new Vector(peOrigem.Position.X, peOrigem.Position.Z);
            Vector posicaoPe = new Vector(pe.Position.X, pe.Position.Z);

            double anguloDoPasso = Util.CalcularProdutoEscalar(orientacaoDoCorpo, posicaoPeOrigem, posicaoPe);

            Console.WriteLine("Angulo do passo: " + anguloDoPasso);

            return anguloDoPasso > 90;
        }
    }
}
