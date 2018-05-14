using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsqueletoUsuario.Auxiliar
{
    public class ControleJogador
    {
        public BitArray controle;
        private StatusJogador status;
        public String strStatus;

        private int contFramesMudancaDirecao;
        private int contFramesRepouso;
        private int maxFramesMudaDirecao;
        private int maxFramesRepouso;

        public ControleJogador()
        {                 
            controle = new BitArray(4);
            status = new StatusJogador();

            contFramesMudancaDirecao = 0;
            contFramesRepouso = 0;
            maxFramesMudaDirecao = 20;
            maxFramesRepouso = 20;
        }

        public void AndarFrente()
        {
            if(!controle[(int)Controles.PULAR] && !controle[(int)Controles.AGACHAR])
            {
                if (controle[(int)Controles.ANDART])
                {
                    if (contFramesMudancaDirecao <= maxFramesMudaDirecao)
                    {
                        contFramesMudancaDirecao++;
                    }
                    else
                    {
                        MudarDirecao();
                    }
                }
                else
                {
                    controle[(int)Controles.ANDARF] = true;
                    strStatus = "Andando para frente";
                    Console.WriteLine(strStatus);
                }
            }
        }

        public void AndarTras()
        {
            if (!controle[(int)Controles.PULAR] && !controle[(int)Controles.AGACHAR])
            {
                if ( controle[(int)Controles.ANDARF] )
                {
                    if (contFramesMudancaDirecao < maxFramesMudaDirecao)
                    {
                        contFramesMudancaDirecao++;
                    }
                    else
                    {
                        MudarDirecao();
                    }
                }
                else
                {
                    controle[(int)Controles.ANDART] = true;
                    strStatus = "Andando para trás";
                    Console.WriteLine(strStatus);
                    
                }
            }
        }

        public void Agachar()
        {
            if (!controle[(int)Controles.PULAR])
            {
                controle[(int)Controles.AGACHAR] = true;
                strStatus = "Agachando";
                Console.WriteLine(strStatus);
            }
        }

        public void Pular()
        {
            controle[(int)Controles.PULAR] = true;
            strStatus = "Pulando";
            Console.WriteLine(strStatus);
        }

        private void EnviarControle()
        {
            //enviar this.controle para função de envio pela rede;
        }

        private void MudarDirecao()
        {
            contFramesMudancaDirecao = 0;
            controle[(int)Controles.ANDARF] = !controle[(int)Controles.ANDARF];
            controle[(int)Controles.ANDART] = !controle[(int)Controles.ANDART];
        }
    }
}
