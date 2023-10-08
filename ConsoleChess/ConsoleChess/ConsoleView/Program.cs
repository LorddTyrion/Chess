// See https://aka.ms/new-console-template for more information
using ConsoleChess;
using ConsoleChess.Pieces;
using FrameworkBackend;


Console.WriteLine("SAKK");
Console.WriteLine();
ChessBoard board = new ChessBoard();
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
    ChessMove move = new ChessMove();
    move.InitialX= oldX;
    move.InitialY= oldY;
    move.TargetX= newX;
    move.TargetY= newY;
    move.PromoteTo= promoteTo;
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

    while (!board.Move(move))
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
    //board.boardState.SerializeBoard(@"c:\Save\movie.json");
    //ChessBoardState cb = (ChessBoardState)board.boardState.DeserializeBoard(@"c:\Save\movie.json");
    //Console.WriteLine("ggggggggggggggggg     "+cb.turnOf);
    bw.Draw();
    Console.WriteLine(board.boardState.moves[board.boardState.moves.Count-1]);
    bw.ListPossibleMoves(0, 4);
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
