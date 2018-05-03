using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsqueletoUsuario.Movimentos
{
    public interface IRastreador
    {
        void Rastrear(Skeleton esqueletoUsuario);
        EstadoRastreamento EstadoAtual { get; }
    }
}
