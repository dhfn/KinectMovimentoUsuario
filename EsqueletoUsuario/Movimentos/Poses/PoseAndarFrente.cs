using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EsqueletoUsuario.FuncoesBasicas;
using Microsoft.Kinect;

namespace EsqueletoUsuario.Movimentos.Poses
{
    class PoseAndarFrente : Pose
    {
        public PoseAndarFrente() {
            this.Nome = "PoseAndarFrente";
            this.QuadroIdentificacao = 1;
        }

        protected override bool PosicaoValida(Skeleton esqueletoUsuario)
        {
            double anguloEspinhaEsperado = 10;
            double anguloPernaDeApoioEsperado = 5;
            double anguloPernaDeMovimentoEsperado = 20;

            Joint quadril = esqueletoUsuario.Joints[JointType.HipCenter];
            Joint espinha = esqueletoUsuario.Joints[JointType.Spine];
            Joint ombro = esqueletoUsuario.Joints[JointType.ShoulderCenter];

            Joint peEsquerdo = esqueletoUsuario.Joints[JointType.AnkleLeft];
            Joint joelhoEsquerdo = esqueletoUsuario.Joints[JointType.KneeLeft];
            Joint quadrilEsquerdo = esqueletoUsuario.Joints[JointType.HipLeft];

            Joint peDireito = esqueletoUsuario.Joints[JointType.AnkleRight];
            Joint joelhoDireito = esqueletoUsuario.Joints[JointType.KneeRight];
            Joint quadrilDireito = esqueletoUsuario.Joints[JointType.HipRight];

            double anguloEspinha = Util.CalcularProdutoEscalar(quadril, espinha, ombro);
            double anguloPernaEsquerda = Util.CalcularProdutoEscalar(peEsquerdo, joelhoEsquerdo, quadrilEsquerdo);
            double anguloPernaDireita = Util.CalcularProdutoEscalar(peDireito, joelhoDireito, quadrilDireito);

            bool validaPasso = false;

            if (anguloEspinha > anguloEspinhaEsperado)
                if (anguloPernaEsquerda < anguloPernaDeApoioEsperado && anguloPernaDireita > anguloPernaDeMovimentoEsperado)
                    validaPasso = true;
                else if (anguloPernaDireita < anguloPernaDeApoioEsperado && anguloPernaEsquerda > anguloPernaDeMovimentoEsperado)
                    validaPasso = true;

            return validaPasso;
        }
    }
}
