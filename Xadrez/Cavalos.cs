﻿using System;
using System.Collections.Generic;
using System.Text;
using tabuleiro;

namespace Xadrez
{
    class Cavalo : Peca
    {
        public Cavalo(Cor cor, Tabuleiro tab)
            : base(cor, tab)
        {

        }

        public override bool[,] MovimentosPossiveis()
        {
            return null;
        }

        public override string ToString()
        {
            return "C";
        }
    }
}
