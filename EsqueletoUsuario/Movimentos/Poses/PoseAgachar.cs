using EsqueletoUsuario.FuncoesBasicas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;

namespace EsqueletoUsuario.Movimentos.Poses
{
    public class PoseAgachar : Pose
    {

        public PoseAgachar()
        {
            this.Nome = "PoseAgachar";
            this.QuadroIdentificacao = 5;
        }

        protected override bool PosicaoValida(Skeleton esqueletoUsuario)
        {
            const double ANGULO_ESPERADO = 10;

            Joint quadrilEsquerdo = esqueletoUsuario.Joints[JointType.HipLeft];
            Joint quadrilDireito = esqueletoUsuario.Joints[JointType.HipRight];
            Joint joelhoEsquerdo = esqueletoUsuario.Joints[JointType.KneeLeft];
            Joint joelhoDireito = esqueletoUsuario.Joints[JointType.KneeRight];
            Joint tornozeloEsquerdo = esqueletoUsuario.Joints[JointType.AnkleLeft];
            Joint tornozeloDireito = esqueletoUsuario.Joints[JointType.AnkleRight];

            double resultadoAnguloEsquerdo = Util.CalcularProdutoEscalar(quadrilEsquerdo, joelhoEsquerdo, tornozeloEsquerdo);
            double resultadoAnguloDireito = Util.CalcularProdutoEscalar(quadrilDireito, joelhoDireito, tornozeloDireito);

            bool anguloEsquerdoCorreto = resultadoAnguloEsquerdo > ANGULO_ESPERADO;
            bool anguloDireitoCorreto = resultadoAnguloDireito > ANGULO_ESPERADO;

            return anguloEsquerdoCorreto && anguloDireitoCorreto;
        }
    }
}
