using System;
using System.Collections.Generic;
using System.Text;
using tabuleiro;

namespace Xadrez
{
    class PartidaDeXadrez
    {
        private Tabuleiro tab;
        private int turno;
        private Cor jogadorAtual;
        public bool partidaTerminada;

        public PartidaDeXadrez()
        {
            tab = new Tabuleiro(8, 8);
            turno = 1;
            jogadorAtual = Cor.Branca;
            ColocarPecas();
        }

        public void ExecutaMovimento(Posicao origem, Posicao destino)
        {
            Peca p = tab.retirarPeca(origem);
            p.IncrementarMovimentos();
            Peca pecaCapturada = tab.retirarPeca(destino);
            tab.colocarPeca(p, destino);
        }

        private void ColocarPecas()
        {
            tab.colocarPeca(new Torre(Cor.Preta, tab), new PosicaoXadrez('a', 8).ToPosicao());
            tab.colocarPeca(new Cavalo(Cor.Preta, tab), new PosicaoXadrez('b', 8).ToPosicao());
            tab.colocarPeca(new Bispo(Cor.Preta, tab), new PosicaoXadrez('c', 8).ToPosicao());
            tab.colocarPeca(new Rei(Cor.Preta, tab), new PosicaoXadrez('d', 8).ToPosicao());
            tab.colocarPeca(new Rainha(Cor.Preta, tab), new PosicaoXadrez('e', 8).ToPosicao());
            tab.colocarPeca(new Bispo(Cor.Preta, tab), new PosicaoXadrez('f', 8).ToPosicao());
            tab.colocarPeca(new Cavalo(Cor.Preta, tab), new PosicaoXadrez('g', 8).ToPosicao());
            tab.colocarPeca(new Torre(Cor.Preta, tab), new PosicaoXadrez('h', 8).ToPosicao());

            tab.colocarPeca(new Torre(Cor.Branca, tab), new PosicaoXadrez('a', 1).ToPosicao());
            tab.colocarPeca(new Cavalo(Cor.Branca, tab), new PosicaoXadrez('b', 1).ToPosicao());
            tab.colocarPeca(new Bispo(Cor.Branca, tab), new PosicaoXadrez('c', 1).ToPosicao());
            tab.colocarPeca(new Rei(Cor.Branca, tab), new PosicaoXadrez('d', 1).ToPosicao());
            tab.colocarPeca(new Rainha(Cor.Branca, tab), new PosicaoXadrez('e', 1).ToPosicao());
            tab.colocarPeca(new Bispo(Cor.Branca, tab), new PosicaoXadrez('f', 1).ToPosicao());
            tab.colocarPeca(new Cavalo(Cor.Branca, tab), new PosicaoXadrez('g', 1).ToPosicao());
            tab.colocarPeca(new Torre(Cor.Branca, tab), new PosicaoXadrez('h', 1).ToPosicao());
        }

        public Tabuleiro GetTab()
        {
            return tab;
        }
    }
}
