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
        this.state = { board: [], loading: true, isWhite: true, turnOf: true, duringMove: false, prevx: 0, prevy: 0, promoteTo: 1, winner: 3 };

        gameConnection.on('GameCreated', (board) => {
            this.setState({
                board: board, loading: false
            })
            console.log("Game started")
            this.forceUpdate()

        })
        gameConnection.on('RefreshBoard', (board, success) => {
            this.setState({ board: board })
            console.log(this.state.board)
            if (!success) console.log("Rossz lépés")
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

    renderWhite() {
        const rows = [];
        for (let i = 0; i < 8; i++) {

            const cols = [];
            for (let j = 0; j < 8; j++) {
                cols.push(<td><button style={this.returnColor((i + j) % 2 === 0)} onClick={() => this.onClick(i, j)}>{this.returnText(i, j)}</button></td>);
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
            return { backgroundColor: 'white', color: 'black', width: '100px', height: '100px', border: 'none', borderRadius: '0px 0px 0px 0px', borderWidth: '0px', textAlign: 'center', bottom: '0', fontSize: '26px' };
        }
        return { backgroundColor: 'black', color: 'white', width: '100px', height: '100px', border: 'none', borderRadius: '0px 0px 0px 0px', borderWidth: '0px', textAlign: 'center', bottom: '0', fontSize: '26px' };
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
                return this.state.board[8 * x + y].piece.isWhite ? "WK" : "BK"
            case 1:
                return this.state.board[8 * x + y].piece.isWhite ? "WQ" : "BQ"
            case 2:
                return this.state.board[8 * x + y].piece.isWhite ? "WN" : "BN"
            case 3:
                return this.state.board[8 * x + y].piece.isWhite ? "WB" : "BB"
            case 4:
                return this.state.board[8 * x + y].piece.isWhite ? "WR" : "BR"
            case 5:
                return this.state.board[8 * x + y].piece.isWhite ? "WP" : "BP"
            default:
                break;
        }
    }

    render() {
        let content = this.state.loading ? <div></div> : this.renderWhite()
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
        return (
            <div>
                {content}
                {promote}
                {win}
            </div>

        );
    }

    async startconnection() {

        try {
            await gameConnection.start();
            console.log("SignalR (game) Connected.");
            await gameConnection.invoke('GameStarted');

        } catch (err) {
            console.log("Start hiba:" + err);
            setTimeout(this.start, 50000);
        }
    }
    onClick = async (x, y) => {
        console.log(x + " " + y + " meg lett nyomva")
        if (this.state.board[8 * x + y].piece == null && !this.state.duringMove) return
        if (!this.state.duringMove) {
            this.setState({ duringMove: true, prevx: x, prevy: y })
        }
        else {
            this.setState({ duringMove: false });
            await gameConnection.invoke('MakeMove', this.state.prevx, this.state.prevy, x, y, this.state.promoteTo);
        }
    }
}