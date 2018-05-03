using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsqueletoUsuario.Movimentos
{
    public class GestoQuadroChave
    {
        public int QuadroLimiteInferior { get; private set; }
        public int QuadroLimiteSuperior { get; private set; }
        public Pose PoseChave
        {
            get => default(Pose);
            private set
            {
            }
        }

        public GestoQuadroChave(Pose poseChave, int limiteInferior, int limiteSuperior)
        {
            this.PoseChave = poseChave;
            this.QuadroLimiteInferior = limiteInferior;
            this.QuadroLimiteSuperior = limiteSuperior;
        }
    }
}
