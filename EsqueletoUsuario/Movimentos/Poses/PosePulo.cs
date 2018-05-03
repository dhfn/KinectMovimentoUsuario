using EsqueletoUsuario.FuncoesBasicas;
using EsqueletoUsuario.Movimentos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;

namespace EsqueletoUsuario.Movimentos.Poses
{
    public class PosePulo : Pose
    {
        private double alturaChao;
        public bool novaAlturaChao;

        public PosePulo()
        {
            this.Nome = "PosePulo";
            this.QuadroIdentificacao = 1;
        }

        protected override bool PosicaoValida(Skeleton esqueletoUsuario)
        {
            if (novaAlturaChao)
            {
                alturaChao = esqueletoUsuario.Joints[JointType.FootLeft].Position.Y;
                novaAlturaChao = !novaAlturaChao;
            }

            Joint peEsquerdo = esqueletoUsuario.Joints[JointType.FootLeft];
            Joint peDireito = esqueletoUsuario.Joints[JointType.FootRight];
            double alturaCorreta = 0.7;

            //Console.WriteLine("pé esquerdo: ("+peEsquerdo.Position.X+", "+ peEsquerdo.Position.Y + ", "+ peEsquerdo.Position.Z + ")  "+ "pé direito: (" + peDireito.Position.X + ", " + peDireito.Position.Y + ", " + peDireito.Position.Z + ")");
            bool peEsquerdoAlturaCorreta = peEsquerdo.Position.Y > alturaCorreta + alturaChao;
            bool peDireitoAlturaCorreta = peDireito.Position.Y > alturaCorreta + alturaChao;

            return peEsquerdoAlturaCorreta && peDireitoAlturaCorreta;
        }
    }
}
