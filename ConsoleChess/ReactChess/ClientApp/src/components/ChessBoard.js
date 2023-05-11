import React, { Component } from 'react';
import { HubConnectionBuilder } from '@microsoft/signalr';
import authService from './api-authorization/AuthorizeService'
import { HttpTransportType } from '@microsoft/signalr';
import { LogLevel } from '@microsoft/signalr';
import { Clock } from 'chess-clock'


import './styles.css';

const fischer = Clock.getConfig('Fischer Rapid 1|5')
const updateInterval = 500
const callback = console.info

/*const clock = new Clock({
  ...fischer,
  updateInterval,
  callback,
})*/

var gameConnection = new HubConnectionBuilder()
    .withUrl('https://localhost:7073/chesshub', {
        accessTokenFactory: () => {
            return authService.getAccessToken();
        }, transport: HttpTransportType.WebSockets, skipNegotiation: true
    })
    .configureLogging(LogLevel.Information)
    .build();

export class ChessBoard extends Component {
    static displayName = ChessBoard.name;

    constructor(props) {
        super(props);
        this.state = {
            board: [], players: [], loading: true, started: false, joined: false, isWhite: true, turnOf: true, duringMove: false, prevx: 0, prevy: 0, promoteTo: 1, winner: 3,
            possibleMoves: [], prevMoves: [], promotionVisible: false, ownuser: "", otheruser: "", clock: new Clock({
                ...fischer,
                updateInterval,
                callback,
            }),
            whitepoints: 0, blackpoints: 0
        };

        gameConnection.on('AddToGame', (playerlist) => {
            this.setState({ players: playerlist })

            this.forceUpdate()
        })

        gameConnection.on('SetColor', (isWhite, username) => {
            this.setState({ isWhite: isWhite, ownuser: username })
            this.forceUpdate()
        })

        gameConnection.on('GameCreated', (board) => {
            this.setState({
                board: board, loading: false, started: true
            })
            for (let i = 0; i < this.state.players.length; i++) {
                if (this.state.players[i] !== this.state.ownuser) {
                    this.setState({ otheruser: this.state.players[i] })
                    break;
                }

            }
            console.log("Game started")
            this.forceUpdate()

        })
        gameConnection.on('RefreshBoard', (board, success) => {
            this.setState({ board: board })

            if (!success) console.log("Rossz lépés")
            else {
                if (this.state.turnOf) this.state.clock.push(0);
                else this.state.clock.push(1);
                this.setState({ turnOf: !this.state.turnOf })
            }
            this.forceUpdate()
        })
        gameConnection.on('GetPossibleMoves', (moves) => {
            this.setState({ possibleMoves: moves })
            this.forceUpdate()
        })
        gameConnection.on('PreviousMoves', (moves) => {
            this.setState({ prevMoves: moves })
            this.forceUpdate()
        })
        gameConnection.on('RefreshPoints', (white, black) => {
            this.setState({ whitepoints: white, blackpoints: black })
            this.forceUpdate()
        })

        gameConnection.on('GameEnds', (result) => {
            this.setState({ winner: result })
            this.forceUpdate()
        })

        gameConnection.onclose(async () => {
            await this.startconnection();
        })

        this.startconnection();
    }
    clockCallback() {
        console.log("called")
        if (this.state.clock.state.status === "done") {
            console.log("DONE")
        }

    }

    componentDidMount() {
        this.interval = setInterval(() => this.setState({ time: Date.now() }), 1000);


        this.setState({
            clock: new Clock({
                ...fischer,
                updateInterval,
                callback: async (state) => {
                    if (state.status === "done") {
                        if (this.state.turnOf && this.state.isWhite) {
                            console.log("black wins")
                            await gameConnection.invoke('LoseGame');
                        }
                        else if (!this.state.turnOf && !this.state.isWhite) {
                            console.log("white wins")
                            await gameConnection.invoke('LoseGame');
                        }
                    }
                }
            })
        })
    }
    componentWillUnmount() {
        clearInterval(this.interval);
    }

    renderJoin(players) {
        return (
            <div>
                <button onClick={this.onJoinGame}>Join game</button>
                <table>
                    <thead>
                        <tr>
                            <th>Players</th>
                        </tr>
                    </thead>
                    <tbody>
                        {players.map(players =>
                            <tr key={Math.random() * 1000000 + 1}>
                                <td>{players}</td>
                            </tr>
                        )}
                    </tbody>
                </table>

            </div>)

    }
    renderMoves() {
        const moves=[];
        for (let i = 0; i < this.state.prevMoves.length; i++) {
            let movestring = this.moveToString(this.state.prevMoves[i]);
            moves.push(movestring)
        }

        const rows=[];
        for (let i = 0; i < Math.ceil(this.state.prevMoves.length/2); i++) {
            const cols=[];
            for(let j=0; j<2; j++){
                if(i*2+j>=moves.length){
                    cols.push(<td ></td>)
                }
                else cols.push(<td >{moves[i*2+j]}</td>)
            }
            
            rows.push(<tr>{cols}</tr>)

        }
        
        return (
            <div>
                <strong>Moves</strong>
                <table >
                    <tbody>
                        {rows}
                    </tbody>
                </table>

            </div>)
    }
    moveToString(move) {
        if (move.piece === 0 && move.targetY - move.initialY === 2) {
            if (move.isCheck) return "O-O+"
            return "O-O";
        }
        else if (move.piece === 0 && move.initialY - move.targetY === 2) {
            if (move.isCheck) return "O-O-O+"
            return "O-O-O";
        }

        let initial = "";
        switch (move.piece) {
            case 0: initial += "K"; break;
            case 1: initial += "Q"; break;
            case 2: initial += "N"; break;
            case 3: initial += "B"; break;
            case 4: initial += "R"; break;
            default: break;
        }
        initial += this.colToChar(move.initialY);

        initial += (move.initialX + 1);

        if (move.isCapture) initial += "x";

        initial += this.colToChar(move.targetY)

        initial += (move.targetX + 1);


        switch (move.promoteTo) {
            case 1: initial += "=Q"; break;
            case 4: initial += "=R"; break;
            case 3: initial += "=B"; break;
            case 2: initial += "=N"; break;
            default: break;
        }
        if (move.isCheck) initial += "+";
        return initial;
    }
    colToChar(col) {
        switch (col) {
            case 0: return "a";
            case 1: return "b";
            case 2: return "c";
            case 3: return "d";
            case 4: return "e";
            case 5: return "f";
            case 6: return "g";
            case 7: return "h";
            default: break;
        }
    }

    renderBlack() {
        let whiteminutes = Math.floor(this.state.clock.state.remainingTime[0] / 60000).toLocaleString('en-US', { minimumIntegerDigits: 2, useGrouping: false });
        let blackminutes = Math.floor(this.state.clock.state.remainingTime[1] / 60000).toLocaleString('en-US', { minimumIntegerDigits: 2, useGrouping: false });
        let whiteseconds = Math.floor((this.state.clock.state.remainingTime[0] % 60000) / 1000).toLocaleString('en-US', { minimumIntegerDigits: 2, useGrouping: false });
        let blackseconds = Math.floor((this.state.clock.state.remainingTime[1] % 60000) / 1000).toLocaleString('en-US', { minimumIntegerDigits: 2, useGrouping: false });
        const rows = [];
        for (let i = 0; i < 8; i++) {

            const cols = [];
            for (let j = 0; j < 8; j++) {
                var ishighlighted = false;

                for (let p = 0; p < this.state.possibleMoves.length; p++) {
                    if (this.state.possibleMoves[p].targetX === i && this.state.possibleMoves[p].targetY === j) {
                        ishighlighted = true;
                    }
                }
                if (ishighlighted && this.state.turnOf === this.state.isWhite && this.state.duringMove) {
                    cols.push(<td><button className='chess' style={this.returnColor((i + j) % 2 === 0, true)} onClick={() => this.onClick(i, j)}>{this.returnText(i, j)}&#xFE0E;</button></td>);
                }
                else {
                    cols.push(<td><button className='chess' style={this.returnColor((i + j) % 2 === 0, false)} onClick={() => this.onClick(i, j)}>{this.returnText(i, j)}&#xFE0E;</button></td>);
                }
            }
            rows.push(<tr>{cols}</tr>)
        }
        return (
            <div>
                <strong>{whiteminutes}:{whiteseconds}</strong>
                <strong>   {this.state.otheruser}</strong>
                <strong>   {this.state.whitepoints>this.state.blackpoints? ("+")+(this.state.whitepoints-this.state.blackpoints): ""}</strong>
                <table style={this.setBackground()} className='table' aria-labelledby="tabelLabel">
                    <tbody>
                        {rows}
                    </tbody>
                </table>
                <strong>{blackminutes}:{blackseconds}</strong>
                <strong>   {this.state.ownuser}</strong>
                <strong>   {this.state.whitepoints<this.state.blackpoints? ("+")+(this.state.blackpoints-this.state.whitepoints): ""}</strong>
                <button onClick={() => this.onResign()}>Resign</button>
            </div>)


    }
    renderWhite() {
        let whiteminutes = Math.floor(this.state.clock.state.remainingTime[0] / 60000).toLocaleString('en-US', { minimumIntegerDigits: 2, useGrouping: false });
        let blackminutes = Math.floor(this.state.clock.state.remainingTime[1] / 60000).toLocaleString('en-US', { minimumIntegerDigits: 2, useGrouping: false });
        let whiteseconds = Math.floor((this.state.clock.state.remainingTime[0] % 60000) / 1000).toLocaleString('en-US', { minimumIntegerDigits: 2, useGrouping: false });
        let blackseconds = Math.floor((this.state.clock.state.remainingTime[1] % 60000) / 1000).toLocaleString('en-US', { minimumIntegerDigits: 2, useGrouping: false });
        const rows = [];
        for (let i = 7; i >= 0; i--) {

            const cols = [];
            for (let j = 0; j < 8; j++) {
                var ishighlighted = false;

                for (let p = 0; p < this.state.possibleMoves.length; p++) {
                    if (this.state.possibleMoves[p].targetX === i && this.state.possibleMoves[p].targetY === j) {
                        ishighlighted = true;
                    }
                }
                if (ishighlighted && this.state.turnOf === this.state.isWhite && this.state.duringMove) {
                    cols.push(<td><button className='chess' style={this.returnColor((i + j) % 2 === 0, true)} onClick={() => this.onClick(i, j)}>{this.returnText(i, j)}&#xFE0E;</button></td>);
                }
                else {
                    cols.push(<td><button className='chess' style={this.returnColor((i + j) % 2 === 0, false)} onClick={() => this.onClick(i, j)}>{this.returnText(i, j)}&#xFE0E;</button></td>);
                }
            }
            rows.push(<tr>{cols}</tr>)
        }
        return (
            <div>

                <strong>{blackminutes}:{blackseconds}</strong>
                <strong>   {this.state.otheruser}</strong>
                <strong>   {this.state.whitepoints<this.state.blackpoints? ("+")+(this.state.blackpoints-this.state.whitepoints): ""}</strong>
                <table style={this.setBackground()} className='table' aria-labelledby="tabelLabel">
                    <tbody>
                        {rows}
                    </tbody>
                </table>
                <strong>{whiteminutes}:{whiteseconds}</strong>
                <strong>   {this.state.ownuser}</strong>
                <strong>   {this.state.whitepoints>this.state.blackpoints? ("+")+(this.state.whitepoints-this.state.blackpoints): ""}</strong>
                <button onClick={() => this.onResign()}>Resign</button>
            </div>)


    }

    renderPromoteSelection() {
        
        return (
            <div onChange={this.handleChange}>
                <input type="radio" value="Queen" name="promote" /> Queen
                <input type="radio" value="Knight" name="promote" /> Knight
                <input type="radio" value="Bishop" name="promote" /> Bishop
                <input type="radio" value="Rook" name="promote" /> Rook
            </div>
        );
    }

    returnColor(white, highlighted) {
        if (!highlighted) {
            if (white === true) {
                return { backgroundColor: 'white', color: 'black', width: '100px', height: '100px', border: 'none', borderRadius: '0px 0px 0px 0px', borderWidth: '0px', textAlign: 'center', bottom: '0', fontSize: '80px' };
            }
            return { backgroundColor: 'darkgreen', color: 'black', width: '100px', height: '100px', border: 'none', borderRadius: '0px 0px 0px 0px', borderWidth: '0px', textAlign: 'center', bottom: '0', fontSize: '80px' };
        }
        else {
            if (white === true) {
                return { backgroundColor: 'lightgrey', color: 'black', width: '100px', height: '100px', border: 'none', borderRadius: '0px 0px 0px 0px', borderWidth: '0px', textAlign: 'center', bottom: '0', fontSize: '80px' };
            }
            return { backgroundColor: 'limegreen', color: 'black', width: '100px', height: '100px', border: 'none', borderRadius: '0px 0px 0px 0px', borderWidth: '0px', textAlign: 'center', bottom: '0', fontSize: '80px' };
        }
    }

    handleChange = (e) => {
        switch (e.target.value) {
            case "Queen":
                this.setState({ promoteTo: 1 })
                break;
            case "Knight":
                this.setState({ promoteTo: 2 })
                break;
            case "Bishop":
                this.setState({ promoteTo: 3 })
                break;
            case "Rook":
                this.setState({ promoteTo: 4 })
                break;
            default:
                break;
        }

    }


    setBackground() {
        return ({
            height: 'fit-content',
            width: 'fit-content',
            backgroundSize: 'cover',
            backgroundPosition: 'center',
            padding: '100px',
            borderCollapse: 'collapse'
        })
    }

    returnText(x, y) {
        if (this.state.board[8 * x + y].piece == null) return ""
        switch (this.state.board[8 * x + y].piece.pieceName) {
            case 0:
                return this.state.board[8 * x + y].piece.isWhite ? "♔" : "♚"
            case 1:
                return this.state.board[8 * x + y].piece.isWhite ? "♕" : "♛"
            case 2:
                return this.state.board[8 * x + y].piece.isWhite ? "♘" : "♞"
            case 3:
                return this.state.board[8 * x + y].piece.isWhite ? "♗" : "♝"
            case 4:
                return this.state.board[8 * x + y].piece.isWhite ? "♖" : "♜"
            case 5:
                return this.state.board[8 * x + y].piece.isWhite ? "♙" : "♟"
            default:
                break;
        }
    }

    render() {

        let join = this.state.started ? <div></div> : this.renderJoin(this.state.players)
        let content = <div></div>
        if (this.state.isWhite) {
            content = this.state.loading ? <div></div> : this.renderWhite()
        }
        else {
            content = this.state.loading ? <div></div> : this.renderBlack()
        }
        let promote = !this.state.promotionVisible ? <div></div> : this.renderPromoteSelection()
        let win = <div></div>

        switch (this.state.winner) {
            case 0:
                win = <p>White wins!</p>
                break;
            case 1:
                win = <p>Black wins!</p>
                break;
            case 2:
                win = <p>Draw!</p>
                break;
            default:
                break;

        }
        let moves = this.state.loading ? <div></div> : this.renderMoves();

        let all = !this.state.started ? <div>{join}</div> :
            <div className='flexing'>
                {content}
                {moves}
                {promote}
            </div>
        if (this.state.winner <= 2) all = win


        return (
            <div>
                {all}
            </div>

        );
    }

    async startconnection() {

        try {
            await gameConnection.start();
            console.log("SignalR (game) Connected.");
            //await gameConnection.invoke('GameStarted');

        } catch (err) {
            console.log("Start hiba:" + err);
            setTimeout(this.start, 50000);
        }
    }
    onClick = async (x, y) => {
        console.log(x + " " + y + " meg lett nyomva")
        if (this.state.board[8 * x + y].piece == null && !this.state.duringMove) return
        if (this.state.turnOf !== this.state.isWhite) return
        if (!this.state.duringMove) {
            this.setState({ duringMove: true, prevx: x, prevy: y })
            if (this.state.isWhite && x === 6 && this.state.board[8 * x + y].piece.pieceName === 5 && this.state.board[8 * x + y].piece.isWhite) this.setState({ promotionVisible: true })
            else if (!this.state.isWhite && x === 1 && this.state.board[8 * x + y].piece.pieceName === 5 && !this.state.board[8 * x + y].piece.isWhite) this.setState({ promotionVisible: true })
            await gameConnection.invoke('PossibleMoves', x, y);
        }
        else {
            this.setState({ duringMove: false, possibleMoves: [], promotionVisible: false });
            await gameConnection.invoke('MakeMove', this.state.prevx, this.state.prevy, x, y, this.state.promoteTo);
        }
    }
    onResign = async () => {
        await gameConnection.invoke('LoseGame');
    }
    onJoinGame = async (e) => {
        e.preventDefault()
        if (!this.state.joined) {
            this.setState({ joined: false })
            await gameConnection.invoke('EnterGame')
        }

    }
    onStartGame = async (e) => {
        e.preventDefault();
        try {
            if (this.state.players.length === 2) {
                this.setState({ started: true })

                await gameConnection.invoke('GameStarted');

            }
            else {
                alert("Not enough players")
            }
        } catch (err) {
            console.error(err)
        }

    }
}