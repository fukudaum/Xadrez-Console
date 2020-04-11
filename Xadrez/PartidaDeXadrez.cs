using System;
using System.Collections.Generic;
using System.Text;
using tabuleiro;

namespace Xadrez
{
    class PartidaDeXadrez
    {
        public Tabuleiro tab { get; private set; }
        public int turno { get; private set; }
        public Cor jogadorAtual { get; private set; }
        public bool partidaTerminada { get; private set; }

        public PartidaDeXadrez()
        {
            tab = new Tabuleiro(8, 8);
            turno = 1;
            jogadorAtual = Cor.Branca;
            ColocarPecas();
        }

        public void ValidarPosicaoOrigem(Posicao pos)
        {
            if (tab.peca(pos) == null)
                throw new TabuleiroException("Nao existe peca na posicao de origem escolhida!");
            if (jogadorAtual != tab.peca(pos).Cor)
                throw new TabuleiroException("A peca de origem nao e sua!");
            if (!tab.peca(pos).ExisteMovimentosPossiveis())
                throw new TabuleiroException("Nao existe movimentos possiveis para essa peca!");

        }

        public void ValidarPosicaoDestino(Posicao origem, Posicao destino)
        {
            if (!tab.peca(origem).PodeMoverPara(destino))
                throw new TabuleiroException("Posicao de destino invalida!");

        }

        public void ExecutaMovimento(Posicao origem, Posicao destino)
        {
            Peca p = tab.retirarPeca(origem);
            p.IncrementarMovimentos();
            Peca pecaCapturada = tab.retirarPeca(destino);
            tab.colocarPeca(p, destino);
        }

        public void RealizaJogada(Posicao origem, Posicao destino)
        {
            ExecutaMovimento(origem, destino);
            turno++;
            MudaJogador();

        }

        public void MudaJogador()
        {
            if (jogadorAtual == Cor.Branca)
                jogadorAtual = Cor.Preta;
            else
                jogadorAtual = Cor.Branca;

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

    }
}
