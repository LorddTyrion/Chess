// See https://aka.ms/new-console-template for more information
using TicTacToe;

Console.WriteLine("Hello, World!");
TicTacToeBoard board = new TicTacToeBoard();
while (board.CheckEndGame() == FrameworkBackend.Color.NONE)
{
    Console.WriteLine("Adj meg egy mezőt!");
    int result=Convert.ToInt32(Console.ReadLine());
    int x = result / 10;
    int y = result % 10;
    TicTacToeMove move = new TicTacToeMove();
    move.X = x;
    move.Y = y;
    board.Move(move);
}
switch (board.CheckEndGame())
{
    case FrameworkBackend.Color.NONE:
        break;        
    case FrameworkBackend.Color.BLACK:
        Console.WriteLine("Második (X) nyert");
        break;
    case FrameworkBackend.Color.WHITE:
        Console.WriteLine("Első (O) nyert");
        break;
    case FrameworkBackend.Color.DRAW:
        Console.WriteLine("Döntetlen");
        break;
     default:
        break;

}
