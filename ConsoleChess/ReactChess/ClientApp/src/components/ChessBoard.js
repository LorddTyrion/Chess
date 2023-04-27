import React, { Component } from 'react';
import { HubConnectionBuilder } from '@microsoft/signalr';
import authService from './api-authorization/AuthorizeService'
import { HttpTransportType } from '@microsoft/signalr';
import { LogLevel } from '@microsoft/signalr';

import './styles.css';

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
        this.state = { board: [], players: [], loading: true, started: false, joined: false, isWhite: true, turnOf: true, duringMove: false, prevx: 0, prevy: 0, promoteTo: 1, winner: 3 };

        gameConnection.on('AddToGame', (playerlist) =>{
            this.setState({players: playerlist})
            
            this.forceUpdate()
        })

        gameConnection.on('SetColor', (isWhite)=>{
            this.setState({isWhite: isWhite})
            this.forceUpdate()
        })

        gameConnection.on('GameCreated', (board) => {
            this.setState({
                board: board, loading: false, started:true
            })
            console.log("Game started")
            this.forceUpdate()

        })
        gameConnection.on('RefreshBoard', (board, success) => {
            this.setState({ board: board })
            console.log(this.state.board)
            if (!success) console.log("Rossz lépés")
            else this.setState({turnOf: !this.state.turnOf})
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

    componentDidMount() {

    }

    renderJoin(players){
        return (
            <div>
                <button  onClick={this.onJoinGame}>Join game</button>                
                <table className='table table-striped' aria-labelledby="tabelLabel">
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

    renderBlack() {
        const rows = [];
        for (let i = 0; i < 8; i++) {

            const cols = [];
            for (let j = 0; j < 8; j++) {
                cols.push(<td><button className='chess' style={this.returnColor((i + j) % 2 === 0)} onClick={() => this.onClick(i, j)}>{this.returnText(i, j)}&#xFE0E;</button></td>);
            }
            rows.push(<tr>{cols}</tr>)
        }
        return (
            <div>
                <table style={this.setBackground()} className='table' aria-labelledby="tabelLabel">
                    <tbody>
                        {rows}
                    </tbody>
                </table>
            </div>)


    }
    renderWhite() {
        const rows = [];
        for (let i = 7; i >= 0; i--) {

            const cols = [];
            for (let j = 0; j < 8; j++) {
                cols.push(<td><button className='chess' style={this.returnColor((i + j) % 2 === 0)} onClick={() => this.onClick(i, j)}>{this.returnText(i, j)}&#xFE0E;</button></td>);
            }
            rows.push(<tr>{cols}</tr>)
        }
        return (
            <div>
                <table style={this.setBackground()} className='table' aria-labelledby="tabelLabel">
                    <tbody>
                        {rows}
                    </tbody>
                </table>
            </div>)


    }

    renderPromoteSelection() {
        console.log(this.state.promoteTo)
        return (
            <div onChange={this.handleChange}>
                <input type="radio" value="Queen" name="promote" /> Queen
                <input type="radio" value="Knight" name="promote" /> Knight
                <input type="radio" value="Bishop" name="promote" /> Bishop
                <input type="radio" value="Rook" name="promote" /> Rook
            </div>
        );
    }

    returnColor(white) {
        if (white === true) {
            return { backgroundColor: 'white', color: 'black', width: '100px', height: '100px', border: 'none', borderRadius: '0px 0px 0px 0px', borderWidth: '0px', textAlign: 'center', bottom: '0', fontSize: '80px' };
        }
        return { backgroundColor: 'green', color: 'black', width: '100px', height: '100px', border: 'none', borderRadius: '0px 0px 0px 0px', borderWidth: '0px', textAlign: 'center', bottom: '0', fontSize: '80px' };
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
        let join=this.state.started?<div></div> : this.renderJoin(this.state.players)
        console.log(this.state.started)
        let content = <div></div>
        if(this.state.isWhite){
            content=this.state.loading ? <div></div> : this.renderWhite()
        }
        else{
            content=this.state.loading ? <div></div> : this.renderBlack()
        }
        let promote = this.renderPromoteSelection()
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
        let all=!this.state.started ? <div>{join}</div>:<div>
                {content}
                {promote}
            </div>
        if(this.state.winner<=2) all=win

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
        if(this.state.turnOf !== this.state.isWhite) return
        if (!this.state.duringMove) {
            this.setState({ duringMove: true, prevx: x, prevy: y })
        }
        else {
            this.setState({ duringMove: false });
            await gameConnection.invoke('MakeMove', this.state.prevx, this.state.prevy, x, y, this.state.promoteTo);
        }
    }
    onJoinGame = async (e)=>{
        e.preventDefault()
        if(!this.state.joined){
            this.setState({joined: false})
            await gameConnection.invoke('EnterGame')
        }
        
    }
    onStartGame=async (e)=>{
        e.preventDefault();
        try {
            if (this.state.players.length===2) {
                this.setState({started: true})

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