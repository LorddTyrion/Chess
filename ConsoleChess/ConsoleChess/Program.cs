// See https://aka.ms/new-console-template for more information
using ConsoleChess;
using ConsoleChess.Pieces;


Console.WriteLine("SAKK");
Console.WriteLine();
Board board = new Board();
BoardView bw=new BoardView(board);

string moveString = "";
bw.Draw();
bool running = true;
while (running)
{
    Console.WriteLine("{0} játékos lép.", board.boardState.moves.Count%2==0? "Fehér":"Fekete");
    moveString = Console.ReadLine();
    char [] moveChars = moveString.ToLower().ToCharArray();
    while (!bw.ValidateInput(moveChars))
    {
        Console.WriteLine("Hibás bemenet, add meg újra!");
        moveChars = Console.ReadLine().ToLower().ToCharArray();
    }
    int oldX=moveChars[1]-'1';
    int oldY= moveChars[0] - 'a';
    int newX= moveChars[3] - '1';
    int newY= moveChars[2] - 'a';
    PieceName promoteTo = PieceName.QUEEN;
    if (moveChars.Length == 5)
    {
        switch (moveChars[4])
        {
            case 'q':
                promoteTo = PieceName.QUEEN;
                break;
            case 'r':
                promoteTo = PieceName.ROOK;
                break;
            case 'n':
                promoteTo = PieceName.KNIGHT;
                break;
            case 'b':
                promoteTo = PieceName.BISHOP;
                break;
            default:
                break;
        }
    }

    while (!board.Move(oldX, oldY, newX, newY, promoteTo))
    {
        Console.WriteLine("Hibás lépés, add meg újra!");
        moveChars = Console.ReadLine().ToLower().ToCharArray();
        while (!bw.ValidateInput(moveChars))
        {
            Console.WriteLine("Hibás bemenet, add meg újra!");
            moveChars = Console.ReadLine().ToLower().ToCharArray();
        }
        oldX = moveChars[1] - '1';
        oldY = moveChars[0] - 'a';
        newX = moveChars[3] - '1';
        newY = moveChars[2] - 'a';
        promoteTo = PieceName.QUEEN;
        if (moveChars.Length == 5)
        {
            switch (moveChars[4])
            {
                case 'q':
                    promoteTo = PieceName.QUEEN;
                    break;
                case 'r':
                    promoteTo = PieceName.ROOK;
                    break;
                case 'n':
                    promoteTo = PieceName.KNIGHT;
                    break;
                case 'b':
                    promoteTo = PieceName.BISHOP;
                    break;
                default:
                    break;
            }
        }
    }
    bw.Draw();
    switch (board.CheckEndGame())
    {
        case Color.WHITE:
            Console.WriteLine("Fehér nyert.");
            running = false;
            break;
        case Color.BLACK:
            Console.WriteLine("Fekete nyert");
            running = false;
            break;
        case Color.DRAW:
            Console.WriteLine("Döntetlen");
            running = false;
            break;
        default:
            break;
    }
    
}







//for (int i = 0; i < 8; i++)
//{
//    for (int j = 0; j < 8; j++)
//    {
//        board.boardState.squares[i, j].Piece = null;
//    }
//}
//board.boardState.WhitePieces.Clear();
//board.boardState.BlackPieces.Clear();

//King k1 = new King();
//k1.IsWhite = false; k1.X = 7; k1.Y = 4;
//board.boardState.squares[7, 4].Piece = k1;
//board.boardState.BlackPieces.Add(k1); board.boardState.BlackKing = k1;

//King k2 = new King();
//k2.IsWhite = true; k2.X = 5; k2.Y = 4;
//board.boardState.squares[5, 4].Piece = k2;
//board.boardState.WhitePieces.Add(k2); board.boardState.WhiteKing = k2;

//Queen q = new Queen();
//q.IsWhite = true; q.X = 6; q.Y = 4;
//board.boardState.squares[6, 4].Piece = q;
//board.boardState.WhitePieces.Add(q);
//board.boardState.turnOf = Color.BLACK;
//bw.Draw();
//bw.ListPossibleMoves(7, 4);
//bool res = board.Move(7, 4, 6, 3);
//bw.Draw();

//King k1 = new King();
//k1.IsWhite = false; k1.X = 7; k1.Y = 4;
//board.boardState.squares[7, 4].Piece = k1;
//board.boardState.BlackPieces.Add(k1); board.boardState.BlackKing = k1;

//King k2 = new King();
//k2.IsWhite = true; k2.X = 0; k2.Y = 4;
//board.boardState.squares[0, 4].Piece = k2;
//board.boardState.WhitePieces.Add(k2); board.boardState.WhiteKing = k2;

//Pawn p = new Pawn();
//p.IsWhite = true; p.X = 6; p.Y = 2;
//board.boardState.squares[6, 2].Piece = p;
//board.boardState.WhitePieces.Add(p);
//bw.Draw();
//bw.ListPossibleMoves(6, 2);
//bool result = board.Move(6, 2, 7, 2);
//bw.Draw();
